namespace Portfolio.Models
{
    public class UserPortfolio
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // شناسه کاربر
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } // لینک تصویر در صورت آپلود در فضای ابری

        // ✅ اضافه کن این خط رو:
        public string? ImagePath { get; set; } // مسیر فیزیکی فایل تصویر در سرور

        public string Link { get; set; }
    }
}
