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
	context.Database.Migrate(); //������������ ������ ������� �� ��, ���� �� ��� ����

	if (!context.Categories.Any())
	{
		var kovbasa = new CategoryEntity
		{
			Name = "�������",
			Image = "https://rivnepost.rv.ua/img/650/korisnoi-kovbasi-ne-buvae-hastroenterolohi-nazvali_20240612_4163.jpg",
			Description = "��� ����� ����������� �� ������� ������� �� ����������. " +
			"������� ��������, �� �� ��������, ���� ����� ������� �� ����� 50 ����� �� ����."
		};

		var cheese = new CategoryEntity
		{
			Name = "����",
			Image = "https://www.vsesmak.com.ua/sites/default/files/styles/large/public/field/image/syrnaya_gora_5330_1900_21.jpg?itok=RPUrRskl",
			Description = "C�� � ���� � ���������� ������ �� ������ ����. " +
			"���� �� � ������, � �������, � ��������. �� ����� �������, �� �����, " +
			"�� ��������� �� ��������� ������������ ������� ��� � ��������."
		};

		var bread = new CategoryEntity
		{
			Name = "���",
			Image = "https://tvoemisto.tv/media/gallery/full/7/7/77777777777777777_e26fc.png",
			Description = "� ������� ����� ���������� ����������� ������� ����� ����, " +
			"�� ����� �� �������� ������ ����� ���� � ���������, �������������� ���."
		};

		context.Categories.Add(kovbasa);
		context.Categories.Add(cheese);
		context.Categories.Add(bread);
		context.SaveChanges();
	}
}

app.Run();
