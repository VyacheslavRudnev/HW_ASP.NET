using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
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
}
