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
                "https://fullstack-ai-chat-six.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ✅ Middleware sırası çok önemli!
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); // (isteğe bağlı ama önerilir)
app.UseRouting();          // CORS öncesinde olmalı

app.UseCors("AllowFrontend"); // ✅ TAM BURADA OLMALI

app.UseAuthorization();

// ✅ Ana sayfa testi
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
