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

// ✅ CORS yapılandırması
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001",
                "http://localhost:3002",
                "https://fullstack-ai-chat-six.vercel.app" // 🔥 Vercel domaini eklendi
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ✅ CORS aktif et
app.UseCors("AllowFrontend");

// ✅ Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// ✅ Ana sayfa için test endpoint (Render'da görünür olacak)
app.MapGet("/", () => "🚀 Sentiment API is running on Render!");

// ✅ Controller endpoint’leri
app.MapControllers();

// ✅ Veritabanı migrationlarını otomatik uygula (Render için)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ✅ Uygulamayı çalıştır
app.Run();
