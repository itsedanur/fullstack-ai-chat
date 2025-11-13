using Microsoft.EntityFrameworkCore;
using backend.SentimentApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db")
);

// ✅ CORS ayarı
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:3001",
            "http://localhost:3002",
            "https://fullstack-ai-chat-six.vercel.app",
            "https://frontend-drjhpu6xc-itsedaas-projects.vercel.app",
            "https://frontend-omega-woad.vercel.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
     




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// ✅ CORS middleware doğru sırada olmalı
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapGet("/", () => "✅ Sentiment API is running on Render!");
app.MapControllers();

// ✅ DB migration (opsiyonel)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
