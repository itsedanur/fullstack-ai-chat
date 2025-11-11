using Microsoft.EntityFrameworkCore;
using backend.SentimentApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers ve API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ SQLite veritabanı bağlantısı (appsettings.json -> "Default")
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=app.db"
    )
);

// ✅ CORS (frontend için izinler)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001",
                "http://localhost:3002"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    // Geniş izinli test politikası (isteğe bağlı)
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ✅ CORS aktif (önce)
app.UseCors("AllowFrontend");

// ✅ Swagger sadece development ortamında
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authorization (şimdilik gerek yok ama hazır)
app.UseAuthorization();

// ✅ Controller endpoint’lerini bağla
app.MapControllers();

// ✅ Uygulamayı çalıştır
app.Run();
