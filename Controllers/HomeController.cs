using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Portfolio.Services;
using Portfolio.Models;
using Microsoft.AspNetCore.Authorization;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly ContactService _contactService;

        public HomeController(ProjectService projectService, ContactService contactService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }

        // صفحه اصلی — لیست پروژه‌ها (از ProjectService استفاده می‌کند)
        public IActionResult Index()
        {
            // اگر ProjectService متد async داشت، این خط را به صورت await _projectService.GetAllProjectsAsync() تغییر بده
            var projects = _projectService.GetAllProjects();
            return View(projects);
        }

        // صفحه نمایش همه پروژه‌ها (متمایز از Index در صورت نیاز)
        public IActionResult Projects()
        {
            var projects = _projectService.GetAllProjects();
            return View(projects);
        }

        // GET: نمایش فرم تماس
        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactMessage());
        }

        // POST: ارسال پیام تماس
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                // اگر اعتبارسنجی شکست خورد، فرم را دوباره نمایش بده
                return View(model);
            }

            // مقداردهی CreatedAt اگر مدل این فیلد را دارد
            try
            {
                if (model.CreatedAt == default)
                {
                    model.CreatedAt = DateTime.UtcNow;
                }

                // ذخیره پیام از طریق ContactService (async)
                await _contactService.AddMessageAsync(model);

                // پیام موفقیت — می‌تونی از ViewBag یا TempData استفاده کنی
                TempData["ContactSuccess"] = "پیام شما با موفقیت ارسال شد. متشکریم!";
                return RedirectToAction(nameof(Contact));
            }
            catch (Exception ex)
            {
                // لاگ کردن خطا (در صورت داشتن logger اینجا لاگ کن)
                ModelState.AddModelError(string.Empty, "خطا در ارسال پیام. لطفا بعداً تلاش کنید.");
                return View(model);
            }
        }

        // اگر بخواهی صفحه درباره ما یا سایر صفحات هم اینجا اضافه کن
        public IActionResult About()
        {
            ViewData["Message"] = "درباره من";
            return View();
        }

        // مثال: صفحه‌ای که فقط کاربران لاگین شده می‌توانند پروژه‌های خود را ببینند
        [Authorize]
        public IActionResult MyProjects()
        {
            // اگر ProjectService متدی برای گرفتن پروژه‌ها بر اساس userId دارد بهتر است از آن استفاده کنی.
            // در غیر این صورت باید ProjectService یا DbContext را طوری توسعه دهی که پروژه‌های کاربر جاری را برگرداند.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // اگر ProjectService تابعی شبیه GetProjectsByUserId(userId) دارد از آن استفاده کن؛ در غیر این صورت این مثال فرضی است:
            if (_projectService is IProjectUserProvider provider)
            {
                // اگر اینترفیس IProjectUserProvider را برای اینکار گذاشتی
                var projects = provider.GetProjectsByUserId(userId);
                return View(projects);
            }

            // fallback: همه پروژه‌ها را نشان بده (یا خالی)
            var all = _projectService.GetAllProjects();
            return View(all);
        }
    }

    // --- توضیح کوتاه: اگر ProjectService قابلیت گرفتن پروژه‌های یک کاربر را ندارد،
    // می‌توانی این اینترفیس را در سرویس پیاده‌سازی و سپس ثبتش کنی:
    public interface IProjectUserProvider
    {
        List<Project> GetProjectsByUserId(string userId);
    }
}
