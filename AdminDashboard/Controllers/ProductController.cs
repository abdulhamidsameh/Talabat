using AdminDashboard.Helpers;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Product;
using Talabat.Core.Specifications;
using Talabat.Core.UnitOfWork.Contract;

namespace AdminDashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new BaseSpecifications<Product>();
            spec.Includes.Add(P => P.Brand);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);

            return View(mappedProduct);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (model.Image is not null)
            {
                var imageName = PictureSettings.UploadFile(model.Image, "products");
                model.PictureUrl = imageName;
            }
            else
            {
                model.PictureUrl = "images/products/AngularBlueBoots.jpeg";
            }


            var mapProduct = _mapper.Map<ProductViewModel, Product>(model);
            _unitOfWork.Repository<Product>().Add(mapProduct);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
