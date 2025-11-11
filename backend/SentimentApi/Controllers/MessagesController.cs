using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend.SentimentApi.Data;
using backend.SentimentApi.Models;

namespace backend.SentimentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _http;

        public MessagesController(AppDbContext context, IHttpClientFactory? factory = null)
        {
            _context = context;
            _http = factory?.CreateClient() ?? new HttpClient();

            // ✅ Yeni Hugging Face Router (eski API kapandı)
            _http.BaseAddress = new Uri("https://router.huggingface.co/hf-inference/");

            // ✅ Token sabit ekliyoruz (geliştirme için)
            var token = Environment.GetEnvironmentVariable("HF_TOKEN");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // ✅ Request (gelen)
        public class InMessageDto
        {
            public string? Text { get; set; }
        }

        // ✅ Response (giden)
        public class OutMessageDto
        {
            public string Text { get; set; } = "";
            public string Sentiment { get; set; } = "UNKNOWN";
        }

        // ✅ Mesaj gönder ve analiz et (POST)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InMessageDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest(new { error = "Text boş olamaz." });

            try
            {
                // ✅ HF modeli (Router uyumlu)
                var modelId = "cardiffnlp/twitter-roberta-base-sentiment-latest";

                var payload = JsonSerializer.Serialize(new { inputs = dto.Text });
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var resp = await _http.PostAsync($"models/{modelId}", content);
                var respBody = await resp.Content.ReadAsStringAsync();

                Console.WriteLine($"HF status: {(int)resp.StatusCode} {resp.ReasonPhrase}");
                Console.WriteLine($"HF body: {respBody}");

                if (!resp.IsSuccessStatusCode)
                    return StatusCode(502, new { error = "Upstream model error", detail = respBody });

                string sentiment = "UNKNOWN";
                try
                {
                    using var doc = JsonDocument.Parse(respBody);

                    // ✅ [[{label,score},...]] veya [{label,score},...] olabilir
                    var root = doc.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        if (root[0].ValueKind == JsonValueKind.Array)
                            root = root[0];

                        if (root.GetArrayLength() > 0)
                        {
                            var first = root[0];
                            if (first.TryGetProperty("label", out var lbl))
                                sentiment = lbl.GetString() ?? "UNKNOWN";
                            if (first.TryGetProperty("score", out var sc))
                                sentiment += $" ({sc.GetRawText()})";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("[DEBUG] Parse error: " + ex.Message);
                }

                // ✅ DB'ye kaydet
                var message = new Message
                {
                    Text = dto.Text,
                    Sentiment = sentiment,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // ✅ Cevap döndür
                return Ok(new OutMessageDto
                {
                    Text = dto.Text,
                    Sentiment = sentiment
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[POST ERROR] " + ex.ToString());
                return StatusCode(502, new { error = "Exception", detail = ex.Message });
            }
        }

        // ✅ Tüm mesaj geçmişini getir (GET)
        [HttpGet]
        public IActionResult Get()
        {
            var messages = _context.Messages
                .OrderByDescending(m => m.CreatedAt)
                .Take(50) // son 50 mesaj
                .Select(m => new OutMessageDto
                {
                    Text = m.Text,
                    Sentiment = m.Sentiment
                })
                .ToList();

            return Ok(messages);
        }
    }
}
