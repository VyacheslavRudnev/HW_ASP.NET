using Microsoft.AspNetCore.Mvc;
using WebApplication01.Data;
using WebApplication01.Data.Entities;
using WebApplication01.Models.Category;

namespace WebApplication01.Controllers
{
    
    public class MainController : Controller
    {
		private readonly AppBimbaDbContext _dbContext;
        //Зберігає різну інфорацію про MVC проект
        private readonly IWebHostEnvironment _environment;   
        //DI - dependency injection
        public MainController(AppBimbaDbContext context,
            IWebHostEnvironment environment)
		{
			_dbContext = context;
            _environment = environment;
        }

		//метод у контролері називається Action - дія
		public IActionResult Index()
        {
			var model = _dbContext.Categories.ToList();
			return View(model);
        }
		[HttpGet]//відображення сторінки для перегляду
        public IActionResult Create()
        {
            //Ми повертає View - пусту, яка відобраєате сторінку де потрібно ввести дані для категорії
            return View();
        }
        [HttpPost]//прийняття даних з форми
        public IActionResult Create(CategoryCreateViewModel model)
        {
            var entity = new CategoryEntity();

            //Збереження в базу даних
            var dirName = "uploading";
            var dirSave = Path.Combine(_environment.WebRootPath, dirName);
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }
            if (model.Photo != null)
            {
                //унікальне значення для імені файлу, яке ніколи не повториться
                string fileName = Guid.NewGuid().ToString();
                var ext = Path.GetExtension(model.Photo.FileName);
                fileName += ext;
                var saveFile = Path.Combine(dirSave, fileName);
                using (var stream = new FileStream(saveFile, FileMode.Create))
                {
                    model.Photo.CopyTo(stream);
                }
                entity.Image = fileName;
            }
            entity.Name = model.Name;
            entity.Description = model.Description;
            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();
            //переходимо по списку всіх категорій, тобто визиваємо метод index для нашого контролера
            return Redirect("/");
           
        }
    }
}
