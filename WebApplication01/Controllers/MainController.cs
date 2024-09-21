using Microsoft.AspNetCore.Mvc;
using WebApplication01.Data;
using WebApplication01.Data.Entities;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryEntity category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }
    }

}
