using Microsoft.EntityFrameworkCore;
using WebApplication01.Data;
using WebApplication01.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppBimbaDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionDB")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");

using (var serviceScope = app.Services.CreateScope())
{
	var context = serviceScope.ServiceProvider.GetService<AppBimbaDbContext>();

	// Apply migrations if they are not applied
	context.Database.Migrate(); //автоматичний запуск міграцій на БД, якщо їх там немає

	if (!context.Categories.Any())
	{
		var kovbasa = new CategoryEntity
		{
			Name = "Ковбаси",
			Image = "https://rivnepost.rv.ua/img/650/korisnoi-kovbasi-ne-buvae-hastroenterolohi-nazvali_20240612_4163.jpg",
			Description = "Тим часом відмовлятися від ковбаси повністю не обов’язково. " +
			"Важливо пам’ятати, що це делікатес, який можна вживати не більше 50 грамів на день."
		};

		var cheese = new CategoryEntity
		{
			Name = "Сири",
			Image = "https://www.vsesmak.com.ua/sites/default/files/styles/large/public/field/image/syrnaya_gora_5330_1900_21.jpg?itok=RPUrRskl",
			Description = "Cир – один з найчастіших гостей на нашому столі. " +
			"Адже це і смачно, і корисно, і доступно. Не можна сказати, що увесь, " +
			"що продається на прилавках супермаркетів твердий сир – неякісний."
		};

		var bread = new CategoryEntity
		{
			Name = "Хліб",
			Image = "https://tvoemisto.tv/media/gallery/full/7/7/77777777777777777_e26fc.png",
			Description = "У сегменті ринку «здорового харчування» існують сорти хліба, " +
			"які майже не сприяють набору зайвої ваги – наприклад, цільнозерновий хліб."
		};

		context.Categories.Add(kovbasa);
		context.Categories.Add(cheese);
		context.Categories.Add(bread);
		context.SaveChanges();
	}
}

app.Run();
