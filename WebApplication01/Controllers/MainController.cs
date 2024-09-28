using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryEditViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ExistingImage = category.Image
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            var entity = _dbContext.Categories.FirstOrDefault(c => c.Id == model.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;

            if (model.Photo != null)
            {
                var dirName = "uploading";
                var dirSave = Path.Combine(_environment.WebRootPath, dirName);
                if (!Directory.Exists(dirSave))
                {
                    Directory.CreateDirectory(dirSave);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                var saveFile = Path.Combine(dirSave, fileName);
                using (var stream = new FileStream(saveFile, FileMode.Create))
                {
                    model.Photo.CopyTo(stream);
                }

                // Видалення старого зображення
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, "uploading", entity.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                entity.Image = fileName;
            }

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            var entity = new CategoryEntity();

            var dirName = "uploading";
            var dirSave = Path.Combine(_environment.WebRootPath, dirName);
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }

            if (model.Photo != null)
            {
                // Унікальне ім'я файлу без розширення
                string fileName = Guid.NewGuid().ToString();
                var ext = Path.GetExtension(model.Photo.FileName);
                var baseFileName = fileName;

                using (var stream = new MemoryStream())
                {
                    model.Photo.CopyTo(stream);
                    stream.Position = 0;

                    using (var image = SixLabors.ImageSharp.Image.Load(stream))
                    {
                        var sizes = new[] { 150, 300, 600, 1200 };

                        foreach (var size in sizes)
                        {
                            var resizedImage = image.Clone(ctx => ctx.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions
                            {
                                Mode = SixLabors.ImageSharp.Processing.ResizeMode.Max,
                                Size = new SixLabors.ImageSharp.Size(size, size)  
                            }));

                            // Унікальні імена для різних розмірів
                            var resizedFileName = $"{baseFileName}-{size}.webp";
                            var resizedFilePath = Path.Combine(dirSave, resizedFileName);

                            // Збереження зображення у форматі WebP
                            using (var output = System.IO.File.Create(resizedFilePath))
                            {
                                resizedImage.SaveAsWebp(output);
                            }

                            // Для картки обираємо зображення розміром 300
                            if (size == 300)
                            {
                                entity.Image = resizedFileName;  // Зберігаємо шлях до зображення 300px для картки
                            }
                        }
                    }
                }
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();

            return Redirect("/");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Console.WriteLine($"Запит на видалення категорії з id: {id}");
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                var imagePath = Path.Combine(_environment.WebRootPath, "uploading", category.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    Console.WriteLine($"Зображення {category.Image} було видалено з серверу.");
                }
                else
                {
                    Console.WriteLine($"Зображення {category.Image} не знайдено.");
                }

                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
              
                Console.WriteLine($"Категорія з id {id} була видалена.");
            }
            else
            {
                Console.WriteLine($"Категорія з id {id} не знайдена.");
            }
            return RedirectToAction("Index");
        }
    }
}
