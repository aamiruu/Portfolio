using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Services
{
    // سرویس ساده برای ذخیره و دریافت پیام‌های تماس
    public class ContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        // افزودن پیام جدید به دیتابیس
        public async Task AddMessageAsync(ContactMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            message.CreatedAt = DateTime.UtcNow;
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        // گرفتن همه پیام‌ها (برای پنل مدیریت یا نمایش)
        public async Task<List<ContactMessage>> GetAllAsync()
        {
            return await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // گرفتن یک پیام بر اساس id
        public async Task<ContactMessage?> GetByIdAsync(int id)
        {
            return await _context.ContactMessages.FindAsync(id);
        }
    }
}
