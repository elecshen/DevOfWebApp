using Microsoft.AspNetCore.Mvc;
using ЛР1.Models;

namespace ЛР1.Controllers
{
	public class LB1Controller : Controller
	{
		public IActionResult FirstViewMethod()
		{
			List<string> list = FirstModel.GetVegetablesList();
			return View(list);
		}

		public IActionResult SecondViewMethod()
		{
			return View(FirstModel.GetVegetablesList().OrderBy(x => x).ToList());
		}

		public IActionResult ThirdViewMethod()
		{
			return View(FirstModel.GetStudents().OrderByDescending(x => x.Score).ToList());
		}
	}
}
