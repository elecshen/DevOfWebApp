using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevOfWebApp.Models;
using DevOfWebApp.Models.Entities;
using DevOfWebApp.Models.ViewModels;

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

        public IActionResult Create()
		{
			using var context = new LocalDBContext();
			ViewData["IdИнститута"] = new SelectList(context.Институтыs.ToList(), "IdИнститута", "Наименование");
			ViewData["КодУчёногоЗвания"] = new SelectList(context.УчёноеЗваниеs.ToList(), "КодУчёногоЗвания", "Наименование");
			return View();
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(Guid id, LecturerVM lecturer)
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
