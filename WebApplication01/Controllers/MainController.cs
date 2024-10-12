using Microsoft.AspNetCore.Mvc;
using WebApplication01.Data;
using WebApplication01.Data.Entities;
using WebApplication01.interfaces;
using WebApplication01.Models.Category;

namespace WebApplication01.Controllers
{

    public class MainController : Controller
    {
        private readonly AppBimbaDbContext _dbContext;
        private readonly IImageWorker _imageWorker;
        //Зберігає різну інформацію про MVC проект
        private readonly IWebHostEnvironment _environment;
        //DI - Depencecy Injection
        public MainController(AppBimbaDbContext context,
            IWebHostEnvironment environment, IImageWorker imageWorker)
        {
            _dbContext = context;
            _environment = environment;
            _imageWorker = imageWorker;
        }

        //метод у контролері називаться - action - дія
        public IActionResult Index()
        {
            var model = _dbContext.Categories.ToList();
            return View(model);
        }

        [HttpGet] //це означає, що буде відображатися сторінки для перегляду
        public IActionResult Create()
        {
            //Ми повертає View - пусту, яка відобраєате сторінку де потрібно ввести дані для категорії
            return View();
        }

        [HttpPost] //це означає, що ми отримуємо дані із форми від клієнта
        public IActionResult Create(CategoryCreateViewModel model)
        {
            var entity = new CategoryEntity();
            //Збережння в Базу даних інформації
            var dirName = "uploading";
            var dirSave = Path.Combine(_environment.WebRootPath, dirName);
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }
            if (model.Photo != null)
            {
                //унікальне значенн, яке ніколи не повториться
                //string fileName = Guid.NewGuid().ToString();
                //var ext = Path.GetExtension(model.Photo.FileName);
                //fileName += ext;
                //var saveFile = Path.Combine(dirSave, fileName);
                //using (var stream = new FileStream(saveFile, FileMode.Create)) 
                //    model.Photo.CopyTo(stream);
                entity.Image = _imageWorker.Save(model.Photo);
            }
            entity.Name = model.Name;
            entity.Description = model.Description;
            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();
            //Переходимо до списку усіх категорій, тобото визиваємо метод Index нашого контролера
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var imageBaseName = category.Image;  // Базова назва зображення, без розміру
            var sizes = new[] { 50, 150, 300, 600, 1200 };
            var imagePaths = new List<string>();

            foreach (var size in sizes)
            {
                var imagePath = $"/uploading/{size}_{imageBaseName}";
                imagePaths.Add(imagePath);
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

        //[HttpPost]
        //public IActionResult Edit(CategoryEditViewModel model)
        //{
        //    var entity = _dbContext.Categories.FirstOrDefault(c => c.Id == model.Id);
        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }

        //    entity.Name = model.Name;
        //    entity.Description = model.Description;

        //    if (model.Photo != null)
        //    {
        //        var dirName = "uploading";
        //        var dirSave = Path.Combine(_environment.WebRootPath, dirName);
        //        if (!Directory.Exists(dirSave))
        //        {
        //            Directory.CreateDirectory(dirSave);
        //        }

        //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
        //        var saveFile = Path.Combine(dirSave, fileName);
        //        using (var stream = new FileStream(saveFile, FileMode.Create))
        //        {
        //            model.Photo.CopyTo(stream);
        //        }

        //        // Видалення старого зображення
        //        if (!string.IsNullOrEmpty(entity.Image))
        //        {
        //            var oldImagePath = Path.Combine(_environment.WebRootPath, "uploading", entity.Image);
        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }
        //        }

        //        entity.Image = fileName;
        //    }

        //    _dbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model, IFormFile? Photo)
        {
            if (!ModelState.IsValid)
            {
                // Якщо дані не валідні, перезавантажуємо сторінку з помилками
                return View(model);
            }

            // Отримуємо категорію з бази даних
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == model.Id);
            if (category == null)
            {
                return NotFound();
            }

            // Оновлюємо назву та опис категорії
            category.Name = model.Name;
            category.Description = model.Description;

            // Якщо завантажено нове фото
            if (Photo != null && Photo.Length > 0)
            {
                // Видаляємо старі зображення
                if (!string.IsNullOrEmpty(category.Image))
                {
                    _imageWorker.Delete(category.Image);
                }

                // Зберігаємо нове зображення і отримуємо нову назву файлу
                var imageName = _imageWorker.Save(Photo);
                if (!string.IsNullOrEmpty(imageName))
                {
                    category.Image = imageName;  // Зберігаємо нову назву файлу у базі даних
                }
            }

            // Зберігаємо зміни в базі даних
            _dbContext.SaveChanges();

            // Перенаправляємо на сторінку з переліком категорій або іншу сторінку
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.Image))
            {
                _imageWorker.Delete(category.Image);
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return Json(new { text = "Ми його видалили" }); // Вертаю об'єкт у відповідь
        }
    }
}
