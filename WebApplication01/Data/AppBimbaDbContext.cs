using Microsoft.EntityFrameworkCore;
using WebApplication01.Data.Entities;

namespace WebApplication01.Data;

public class AppBimbaDbContext : DbContext //Клас AppBimbaDbContext наслідує клас DbContext з простору імен Microsoft.EntityFrameworkCore.
                                           //Цей клас відповідає за взаємодію з базою даних.
{
    public AppBimbaDbContext(DbContextOptions<AppBimbaDbContext> options) //Конструктор класу AppBimbaDbContext приймає параметр options типу DbContextOptions<AppBimbaDbContext>.
                                                                          //Цей параметр передається в базовий клас DbContext.
        : base(options) { }

	public DbSet<CategoryEntity> Categories { get; set; } //Властивість Categories представляє таблицю категорій в базі даних.
                                                          //Ця властивість використовується для взаємодії з таблицею категорій в базі даних.
}
