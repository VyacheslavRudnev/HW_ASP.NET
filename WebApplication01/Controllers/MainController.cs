using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using WebApplication01.Data;
using WebApplication01.Data.Entities;
using WebApplication01.Interfaces;
using WebApplication01.Models.Category;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication01.Controllers;


public class MainController : Controller
{
    private readonly AppBimbaDbContext _dbContext;
    private readonly IImageWorker _imageWorker;
    //Зберігає різну інформацію про MVC проект
    private readonly IWebHostEnvironment _environment;
    private readonly IMapper _mapper;
    //DI - Depencecy Injection
    public MainController(AppBimbaDbContext context,
        IWebHostEnvironment environment, IImageWorker imageWorker, 
        IMapper mapper)
    {
        _dbContext = context;
        _environment = environment;
        _imageWorker = imageWorker;
        _mapper = mapper;
    }

    //метод у контролері називаться - action - дія
    public IActionResult Index()
    {
        List<CategoryItemViewModel> model = _dbContext.Categories
            .ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
            .ToList();
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
        var entity = _mapper.Map<CategoryEntity>(model);
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
