using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using WebApplication01.interfaces;

namespace WebApplication01.Services;

public class ImageWorker : IImageWorker //Реалізація інтерфейсу IImageWorker
{
    private readonly IWebHostEnvironment _environment;  //Зберігає інформацію про середовище хостингу, де виконується MVC-додаток,
                                                        //і використовується для доступу до різних параметрів проекту,
                                                        //таких як шляхи до файлів або конфігураційні налаштування середовища
                                                        //(наприклад, "Development", "Production").
    private const string dirName = "uploading";         //Назва папки, де зберігаються зображення
    private int[] sizes = [50, 150, 300, 600, 1200];    //Масив розмірів зображень
    public ImageWorker(IWebHostEnvironment environment) //Конструктор класу ImageWorker
    {
        _environment = environment;
    }
    public string Save(string url)          //метод для збереження зображення з інтернету
    {
        try
        {
            using (HttpClient client = new HttpClient())    //створення клієнта HttpClient
            {
                // Send a GET request to the image URL
                HttpResponseMessage response = client.GetAsync(url).Result;//відправляємо запит GET на вказаний URL

                // Check if the response status code indicates success (e.g., 200 OK)
                if (response.IsSuccessStatusCode)//перевірка, чи відповідь успішна
                {
                    // Read the image bytes from the response content
                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result; //читаємо байти зображення з відповіді
                    return CompresImage(imageBytes);    //викликаємо метод CompresImage для стискання зображення
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve image. Status code: {response.StatusCode}");    //виводимо повідомлення про помилку
                    return String.Empty;    //повертаємо порожній рядок
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return String.Empty;
        }
    }

    /// <summary>
    /// Стискаємо фото
    /// </summary>
    /// <param name="bytes">Набір байтів фото</param>
    /// <returns>Повертаємо назву збереженого фото</returns>
    private string CompresImage(byte[] bytes)   //метод для стискання зображення
    {
        string imageName = Guid.NewGuid().ToString() + ".webp"; //створюємо унікальне ім'я для зображення

        var dirSave = Path.Combine(_environment.WebRootPath, dirName);  //Створюємо повний шлях до папки, де буде зберігатися зображення
        if (!Directory.Exists(dirSave)) //Перевірка, чи існує папка uploading. Якщо ні — вона створюється.
        {
            Directory.CreateDirectory(dirSave);
        }


        foreach (int size in sizes) // Для кожного розміру зображення
        {
            var path = Path.Combine(dirSave, $"{size}_{imageName}");    //Створюємо повний шлях до зображення
            using (var image = Image.Load(bytes))   //Завантажуємо зображення з набору байтів
            {
                image.Mutate(x => x.Resize(new ResizeOptions    //Змінюємо розмір зображення
                {
                    Size = new Size(size, size),   //Розмір зображення
                    Mode = ResizeMode.Max  //Режим зміни розміру
                }));
                image.SaveAsWebp(path); //Зберігаємо зображення
            }
        }

        return imageName;   //Повертаємо назву збереженого зображення
    }

    public void Delete(string fileName) //метод для видалення зображення
    {
        foreach (int size in sizes) // Для кожного розміру зображення
        {
            var fileSave = Path.Combine(_environment.WebRootPath, dirName, $"{size}_{fileName}");   //Створюємо повний шлях до зображення
            if (File.Exists(fileSave))  //Перевірка, чи існує файл
                File.Delete(fileSave);  //Видаляємо файл
        }
    }

    public string Save(IFormFile file)  //метод для збереження зображення з форми
    {
        try
        {
            using (var memoryStream = new MemoryStream())   //створення потоку пам'яті
            {
                file.CopyTo(memoryStream);  //копіюємо файл у потік пам'яті
                byte[] imageBytes = memoryStream.ToArray(); //читаємо байти зображення з потоку пам'яті
                return CompresImage(imageBytes);    //викликаємо метод CompresImage для стискання зображення
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return String.Empty;
        }
    }
}