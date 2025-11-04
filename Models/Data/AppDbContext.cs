using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

namespace Portfolio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // جدول پروژه‌ها
        public DbSet<Project> Projects { get; set; }

        // جدول پیام‌های تماس
        public DbSet<ContactMessage> ContactMessages { get; set; }

        // ✅ جدول پورتفولیوهای کاربران (اضافه کن!)
        public DbSet<UserPortfolio> UserPortfolios { get; set; }
    }
}
