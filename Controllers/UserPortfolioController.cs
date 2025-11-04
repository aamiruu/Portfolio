using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;
using System.Security.Claims;

namespace Portfolio.Controllers
{
    [Authorize]
    public class UserPortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserPortfolioController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: UserPortfolio
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var portfolios = await _context.UserPortfolios
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return View(portfolios);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserPortfolio model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.UserId = userId;

                // ✅ ذخیره تصویر در wwwroot/uploads
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadDir = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    model.ImagePath = "/uploads/" + fileName;
                }

                _context.UserPortfolios.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var portfolio = await _context.UserPortfolios.FindAsync(id);
            if (portfolio == null)
                return NotFound();

            return View(portfolio);
        }
    }
}
