using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication01.Data;
using WebApplication01.Interfaces;
using WebApplication01.Models.Category;
using WebApplication01.Models.Product;


namespace WebApplication01.Controllers;

public class ProductsController : Controller
{
    private readonly AppBimbaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IImageWorker _imageWorker;
    //DI - Depencecy Injection
    public ProductsController(AppBimbaDbContext context, IMapper mapper, IImageWorker imageWorker)
    {
        _dbContext = context;
        _mapper = mapper;
        _imageWorker = imageWorker;
    }
    public IActionResult Index()
    {
        List<ProductItemViewModel> model = _dbContext.Products
            .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
            .ToList();
        return View(model);
    }
    public IActionResult Info(int id)
    {
        var product = _dbContext.Products
            .Include(p => p.ProductImages)
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductItemViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Images = product.ProductImages.Select(img => img.Image).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var product = _dbContext.Products.Include(x => x.ProductImages).SingleOrDefault(x => x.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        if (product.ProductImages != null)
        {
            foreach (var productImage in product.ProductImages)
            {
                _imageWorker.Delete(productImage.Image);
                _dbContext.ProductImages.Remove(productImage);

            }
        }
        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();

        return Json(new { text = "Ми його видалили" }); // Вертаю об'єкт у відповідь
    }


}
