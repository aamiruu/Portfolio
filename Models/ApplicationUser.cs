using Microsoft.AspNetCore.Identity;

namespace Portfolio.Models
{
    // مدل کاربر برای سیستم Identity
    public class ApplicationUser : IdentityUser
    {
        // می‌تونی فیلدهای دلخواه خودت رو اضافه کنی، مثلاً:
        public string? DisplayName { get; set; }
    }
}
