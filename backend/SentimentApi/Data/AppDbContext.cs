using Microsoft.EntityFrameworkCore;
using backend.SentimentApi.Models;

namespace backend.SentimentApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Message> Messages { get; set; } // <-- EKLENDİ
    }
}
