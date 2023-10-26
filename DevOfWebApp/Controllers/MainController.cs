using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevOfWebApp.Models;
using DevOfWebApp.Models.Entities;
using DevOfWebApp.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DevOfWebApp.Controllers
{
    public class MainController : Controller
    {
        public MainController()
        {
        }

        public async Task<IActionResult> Index()
        {
            using var context = new LocalDBContext();
			var IndexOutputList = context.Преподавателиs.Include(п => п.IdИнститутаNavigation).Include(п => п.КодУчёногоЗванияNavigation);
            return View(await IndexOutputList.ToListAsync());
        }

        public string GeneratePassHash(string pass, Guid? salt)
        {
            salt ??= Guid.NewGuid();
            return string.Format("Pass: {0}\nSalt: {1}\nPassHash: {2}", pass, salt.ToString(), ReturnPassHash(pass, salt.Value));
        }

        private string ReturnPassHash(string pass, Guid salt)
        {
            var s = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(pass + salt.ToString()));
            StringBuilder stringBuilder = new();
            foreach (var d in s)
                stringBuilder.Append(d.ToString("x2"));
            return stringBuilder.ToString();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserVM userVM, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                using var context = new LocalDBContext();
                User? user = context.Users.Where(u => u.Login == userVM.Login).Include(r => r.Role).FirstOrDefault();
                if (user is not null && user.PasswordHash == ReturnPassHash(userVM.Password, user.Salt))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                    };
                    ClaimsIdentity id = new(
                        claims, 
                        "ApplicationCookie", 
                        ClaimsIdentity.DefaultNameClaimType, 
                        ClaimsIdentity.DefaultRoleClaimType
                    );
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                    return Redirect(returnUrl??"/");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(userVM);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
		{
			using var context = new LocalDBContext();
			if (id == null || context.Преподавателиs == null)
            {
                return NotFound();
            }

            var преподаватели = await context.Преподавателиs
                .Include(п => п.IdИнститутаNavigation)
                .Include(п => п.КодУчёногоЗванияNavigation)
                .FirstOrDefaultAsync(m => m.IdПреподавателя == id);
            if (преподаватели == null)
            {
                return NotFound();
            }

            return View(преподаватели);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
		{
			using var context = new LocalDBContext();
			ViewData["IdИнститута"] = new SelectList(context.Институтыs.ToList(), "IdИнститута", "Наименование");
			ViewData["КодУчёногоЗвания"] = new SelectList(context.УчёноеЗваниеs.ToList(), "КодУчёногоЗвания", "Наименование");
			return View();
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LecturerVM lecturer)
		{
			using var context = new LocalDBContext();
			if (ModelState.IsValid)
            {
                var преподаватели = lecturer.GetПреподаватели();
                преподаватели.IdПреподавателя = Guid.NewGuid();
                context.Add(преподаватели);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
			}
			ViewData["IdИнститута"] = new SelectList(context.Институтыs.ToList(), "IdИнститута", "Наименование", lecturer.IdИнститута);
			ViewData["КодУчёногоЗвания"] = new SelectList(context.УчёноеЗваниеs.ToList(), "КодУчёногоЗвания", "Наименование", lecturer.КодУчёногоЗвания);
			return View(lecturer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
		{
			using var context = new LocalDBContext();
			if (id == null || context.Преподавателиs == null)
            {
                return NotFound();
            }

            var преподаватели = await context.Преподавателиs
                .Include(p => p.IdИнститутаNavigation)
                .Include(p => p.КодУчёногоЗванияNavigation)
                .Where(p => p.IdПреподавателя == id)
                .FirstOrDefaultAsync();
            if (преподаватели == null)
            {
                return NotFound();
            }
            ViewData["IdИнститута"] = new SelectList(context.Институтыs.ToList(), "IdИнститута", "Наименование", преподаватели.IdИнститута);
            ViewData["КодУчёногоЗвания"] = new SelectList(context.УчёноеЗваниеs.ToList(), "КодУчёногоЗвания", "Наименование", преподаватели.КодУчёногоЗвания);
            return View(new LecturerVM(преподаватели));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id, LecturerVM lecturer)
		{
			using var context = new LocalDBContext();
            if (id != lecturer.IdПреподавателя)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(lecturer.GetПреподаватели());
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
			}
			ViewData["IdИнститута"] = new SelectList(context.Институтыs.ToList(), "IdИнститута", "Наименование", lecturer.IdИнститута);
			ViewData["КодУчёногоЗвания"] = new SelectList(context.УчёноеЗваниеs.ToList(), "КодУчёногоЗвания", "Наименование", lecturer.КодУчёногоЗвания);
			return View(lecturer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
		{
			using var context = new LocalDBContext();
			if (id == null || context.Преподавателиs == null)
            {
                return NotFound();
            }

            var преподаватели = await context.Преподавателиs
                .Include(п => п.IdИнститутаNavigation)
                .Include(п => п.КодУчёногоЗванияNavigation)
                .FirstOrDefaultAsync(m => m.IdПреподавателя == id);
            if (преподаватели == null)
            {
                return NotFound();
            }

            return View(преподаватели);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			using var context = new LocalDBContext();
			if (context.Преподавателиs == null)
            {
                return Problem("Entity set 'LocalDBContext.Преподавателиs'  is null.");
            }
            var преподаватели = await context.Преподавателиs.FindAsync(id);
            if (преподаватели != null)
            {
                context.Преподавателиs.Remove(преподаватели);
            }
            
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
