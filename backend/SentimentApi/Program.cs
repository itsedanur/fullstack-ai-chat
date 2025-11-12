using Microsoft.EntityFrameworkCore;
using backend.SentimentApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers ve API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ SQLite veritabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=app.db"
    )
);

// ✅ CORS yapılandırması (Render + localhost)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            // 💻 Lokal geliştirme ortamları
            "http://localhost:3000",
            "http://localhost:3001",
            "http://localhost:3002",

            // 🌐 Frontend Render URL'leri (React)
            "https://fullstack-ai-chat-1-eaa8.onrender.com",
            "https://fullstack-ai-chat-frontend.onrender.com",
            "https://fullstack-ai-chat-six.vercel.app",

            // 🌐 Backend Render URL'si (API)
            "https://fullstack-ai-chat-s4e1.onrender.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// ✅ Middleware sırası çok önemli
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // opsiyonel ama Render bazen SSL’i kendi ekler
app.UseRouting();

// ✅ CORS middleware aktif
app.UseCors("AllowFrontend");

app.UseAuthorization();

// ✅ Test endpoint
app.MapGet("/", () => "🚀 Sentiment API is running on Render!");

// ✅ Controller endpoint’leri
app.MapControllers();

// ✅ Veritabanı migrationlarını otomatik uygula
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ✅ Uygulamayı çalıştır
app.Run();
