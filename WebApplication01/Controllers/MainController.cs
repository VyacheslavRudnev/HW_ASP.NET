using Microsoft.AspNetCore.Mvc;
using WebApplication01.Data;

namespace WebApplication01.Controllers
{
    
    public class MainController : Controller
    {
		private readonly AppBimbaDbContext _dbContext;
		
		//DI - dependency injection
		public MainController(AppBimbaDbContext context)
		{
			_dbContext = context;
		}

		//метод у контролері називається Action - дія
		public IActionResult Index()
        {
			var model = _dbContext.Categories.ToList();
			return View(model);
        }
    }
}
