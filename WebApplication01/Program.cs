using Microsoft.EntityFrameworkCore;
using WebApplication01.Data;
using WebApplication01.Data.Entities;
using WebApplication01.interfaces;
using WebApplication01.Services;

var builder = WebApplication.CreateBuilder(args);	// створення об'єкта WebApplicationBuilder

// Add services to the container.
builder.Services.AddDbContext<AppBimbaDbContext>(options =>     // реєстрація контексту бази даних
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionDB")));	// встановлення з'єднання з БД

builder.Services.AddControllersWithViews();		// реєстрація контролерів та представлень
builder.Services.AddScoped<IImageWorker, ImageWorker>();	// реєстрація сервісу для роботи з зображеннями

var app = builder.Build();	// створення об'єкта WebApplication

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())	// якщо програма не знаходиться в режимі розробки
{
    app.UseExceptionHandler("/Home/Error");	// встановлення обробника виключень
}
app.UseStaticFiles();	// встановлення статичних файлів

app.UseRouting();	// встановлення маршрутизації

app.UseAuthorization();	// встановлення авторизації

app.MapControllerRoute(	// встановлення маршруту контролера
    name: "default",	// назва маршруту
    pattern: "{controller=Main}/{action=Index}/{id?}");	// шаблон маршруту

using (var serviceScope = app.Services.CreateScope())	//створення області сервісів
{
	var context = serviceScope.ServiceProvider.GetService<AppBimbaDbContext>(); // отримання контексту бази даних
    var imageWorker = serviceScope.ServiceProvider.GetService<IImageWorker>();  // отримання сервісу для роботи з зображеннями

    // Apply migrations if they are not applied
    context.Database.Migrate(); //автоматичний запуск міграцій на БД, якщо їх там немає

	if (!context.Categories.Any())  // якщо в таблиці категорій немає записів
    {
		var imageName = imageWorker.Save("https://rivnepost.rv.ua/img/650/korisnoi-kovbasi-ne-buvae-hastroenterolohi-nazvali_20240612_4163.jpg");	// збереження зображення
        var kovbasa = new CategoryEntity    // створення нової категорії
        {
			Name = "Ковбаси",   // назва категорії
            Image = imageName,  // зображення
            Description = "Тим часом відмовлятися від ковбаси повністю не обов’язково. " +  // опис категорії
            "Важливо пам’ятати, що це делікатес, який можна вживати не більше 50 грамів на день."
		};

        imageName = imageWorker.Save("https://www.vsesmak.com.ua/sites/default/files/styles/large/public/field/image/syrnaya_gora_5330_1900_21.jpg?itok=RPUrRskl");
        var cheese = new CategoryEntity
		{
			Name = "Сири",
			Image = imageName,
			Description = "Cир – один з найчастіших гостей на нашому столі. " +
			"Адже це і смачно, і корисно, і доступно. Не можна сказати, що увесь, " +
			"що продається на прилавках супермаркетів твердий сир – неякісний."
		};

        imageName = imageWorker.Save("https://tvoemisto.tv/media/gallery/full/7/7/77777777777777777_e26fc.png");
        var bread = new CategoryEntity
		{
			Name = "Хліб",
			Image = imageName,
			Description = "У сегменті ринку «здорового харчування» існують сорти хліба, " +
			"які майже не сприяють набору зайвої ваги – наприклад, цільнозерновий хліб."
		};

		context.Categories.Add(kovbasa);    // додавання категорії kovbasa до БД
        context.Categories.Add(cheese);     // додавання категорії cheese до БД
        context.Categories.Add(bread);      // додавання категорії bread до БД
        context.SaveChanges();  // збереження змін
    }
}

app.Run();  //  запуск програми
