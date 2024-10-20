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
    //DI - Depencecy Injection
    public ProductsController(AppBimbaDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
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


}
