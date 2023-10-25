using DevOfWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevOfWebApp.Components
{
	[ViewComponent]
	public class ПреподавателиDetails : ViewComponent
	{
		public IViewComponentResult Invoke(LecturerVM lecturer)
		{
			return View();
		}
	}
}
