using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication01.Data;
using WebApplication01.Data.Entities;
using WebApplication01.interfaces;
using WebApplication01.Models.Category;

namespace WebApplication01.Controllers;

//Controller - відповідає за обробку запитів, що надходять від користувача, і виконання відповідних дій
public class MainController : Controller
{
    private readonly AppBimbaDbContext _dbContext; //Зберігає контекст бази даних
    private readonly IImageWorker _imageWorker; //Це інтерфейс відповідає за обробку зображень.
    private readonly IWebHostEnvironment _environment; //Зберігає інформацію про середовище хостингу, де виконується MVC-додаток,
                                                       //і використовується для доступу до різних параметрів проекту,
                                                       //таких як шляхи до файлів або конфігураційні налаштування середовища
                                                       //(наприклад, "Development", "Production").
    public MainController(AppBimbaDbContext context, //конструктор контролера
        IWebHostEnvironment environment, IImageWorker imageWorker)
    {
        _dbContext = context;
        _environment = environment;
        _imageWorker = imageWorker;
    }

    //метод у контролері називаться - action - дія
    //Цей метод відповідає за відображення списку всіх категорій 
    public IActionResult Index()
    {
        var model = _dbContext.Categories.ToList(); //Це звернення до таблиці або набору даних Categories у базі даних через контекст
                                                    //AppBimbaDbContext.Categories представляє колекцію категорій у базі даних.
                                                    //Метод ToList() перетворює результати запиту в базу даних на список(List).
                                                    //Це означає, що всі категорії з бази даних завантажуються в пам'ять і зберігаються у змінній model.
        return View(model); //Передача змінної model у метод View означає,
                            //що цей список категорій буде переданий до представлення (View),
                            //яке відповідає цьому методу Index.
    }

    [HttpGet] //Атрибут [HttpGet] вказує на те, що цей метод буде відповідати на HTTP-запити типу GET.
              //Запит GET зазвичай використовується для отримання інформації або відображення сторінки, яка не змінює дані на сервері.
    public IActionResult Create() //Цей метод відповідає за відображення сторінки для створення нової категорії
    {
        //Ми повертає View - пусту, яка відобраєате сторінку де потрібно ввести дані для категорії
        return View();
    }

    [HttpPost] //Атрибут казує, що цей метод буде обробляти HTTP POST-запити.
               //POST-запити використовуються для надсилання даних з форми на сервер для збереження або оновлення інформації.
    public IActionResult Create(CategoryCreateViewModel model) //Метод приймає параметр model, який є екземпляром класу CategoryCreateViewModel.
                                                               //Це модель подання, яка містить дані, введені користувачем у форму (назва, опис категорії, завантажене зображення).
                                                               //IActionResult, означає, що цей метод поверне результат дії(в даному випадку — перенаправлення на іншу сторінку).
    {
        var entity = new CategoryEntity(); //Створюється новий екземпляр класу CategoryEntity, який буде представляти нову категорію,
                                           //що зберігатиметься в базі даних.
        var dirName = "uploading"; //Встановлюється назва папки, куди буде зберігатися завантажене зображення.
        var dirSave = Path.Combine(_environment.WebRootPath, dirName); //Створюється повний шлях до папки, де буде зберігатися зображення.
        if (!Directory.Exists(dirSave)) //Перевірка, чи існує папка uploading. Якщо ні — вона створюється.
        {
            Directory.CreateDirectory(dirSave);
        }
        if (model.Photo != null) //Якщо користувач завантажив фото (це поле не порожнє)                                
        {
            entity.Image = _imageWorker.Save(model.Photo);//метод викликає об'єкт _imageWorker для збереження зображення на сервері,
                                                          //і шлях до цього зображення зберігається у властивості entity.Image.
        }
        entity.Name = model.Name; //Назва категорії, введена користувачем у формі, присвоюється властивості Name нового об'єкта entity.
        entity.Description = model.Description; //Опис категорії, введений користувачем у формі, присвоюється властивості Description нового об'єкта entity.
        _dbContext.Categories.Add(entity); //Новий об'єкт entity додається до таблиці Categories у базі даних.
        _dbContext.SaveChanges(); //Зберігаємо зміни в базі даних.
        
        return Redirect("/");//Переходимо до списку усіх категорій, тобото визиваємо метод Index нашого контролера
    }

    [HttpGet]
    public IActionResult Edit(int id)//Метод приймає параметр id, який є ідентифікатором категорії, яку користувач хоче відредагувати.
    {
        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);   //Отримуємо категорію з бази даних за її ідентифікатором.
        if (category == null) //якщо категорія не знайдена
        {
            return NotFound();//Повертаємо статус 404 (Not Found).
        }

        var imageBaseName = category.Image;  // Базова назва зображення, без розміру
        var sizes = new[] { 50, 150, 300, 600, 1200 }; // Масив розмірів зображень
        var imagePaths = new List<string>(); // Список шляхів до зображень

        foreach (var size in sizes) // Для кожного розміру зображення
        {
            var imagePath = $"/uploading/{size}_{imageBaseName}"; // Формуємо шлях до зображення
            imagePaths.Add(imagePath); // Додаємо шлях до списку
        }

        var model = new CategoryEditViewModel // Створюємо модель представлення
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ExistingImage = category.Image
        };
        return View(model);//Передаємо модель представлення у метод View, який відображає сторінку для редагування категорії.
    }

    [HttpPost]
    public IActionResult Edit(CategoryEditViewModel model, IFormFile? Photo) //Метод приймає параметри: model — модель представлення, яка містить дані про категорію,
                                                                             //яку користувач хоче відредагувати, і Photo — зображення, яке користувач може завантажити.
    {
        if (!ModelState.IsValid)//Перевірка валідності даних, які користувач ввів у форму
        {
            // Якщо дані не валідні, перезавантажуємо сторінку з помилками
            return View(model);
        }

        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == model.Id);//Отримуємо категорію з бази даних за її ідентифікатором.
        if (category == null) // Якщо категорія не знайдена
        {
            return NotFound(); // Повертаємо статус 404 (Not Found)
        }

        category.Name = model.Name; // Оновлюємо назву категорії
        category.Description = model.Description; // Оновлюємо опис категорії

        // Якщо завантажено нове фото
        if (Photo != null && Photo.Length > 0)//Перевірка, чи користувач завантажив нове зображення
        {
            // Видаляємо старі зображення
            if (!string.IsNullOrEmpty(category.Image))//Якщо категорія має зображення
            {
                _imageWorker.Delete(category.Image); // Видаляємо старе зображення
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
    public IActionResult Delete(int id) //Метод видалення категорії приймає параметр id, який є ідентифікатором категорії, яку потрібно видалити.
    {
        var category = _dbContext.Categories.Find(id);//Отримуємо категорію з бази даних за її ідентифікатором.
        if (category == null)   //  Якщо категорія не знайдена в базі даних, повертаємо статус 404 (Not Found).
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(category.Image)) // Якщо категорія має зображення
        {
            _imageWorker.Delete(category.Image); // Видаляємо зображення з сервера
        }
        _dbContext.Categories.Remove(category); // Видаляємо категорію з бази даних
        _dbContext.SaveChanges(); //    Зберігаємо зміни в базі даних

        return Json(new { text = "Ми його видалили" }); // Вертаю об'єкт у відповідь
    }
}
