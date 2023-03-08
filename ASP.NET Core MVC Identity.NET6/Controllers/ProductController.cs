using ASP.NET_Core_Identity_Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Identity_Demo.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;
        
        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        //[Route("Product/Test/{name?}")]
        //public IActionResult Test(string name)
        //{
        //    var p = new Product();
        //    p.Name = name;

        //    return View(p);
        //}

        public IActionResult Index(string searchString)
        {
            var products = _repo.GetAllProducts();

            if (!string.IsNullOrEmpty(searchString))
            {
                products.Where(x => x.Name.Contains(searchString));
            }
            return View(products);
        }

        
        public IActionResult ViewProduct(int id)
        {
            var product = _repo.GetProduct(id);

            return View(product);
        }

        //[Authorize]
        public IActionResult UpdateProduct(int id)
        {
            Product prod = _repo.GetProduct(id);

            if (prod == null)
            {
                return View("ProductNotFound");
            }

            return View(prod);
        }
        
        //[Authorize]
        public IActionResult UpdateProductToDatabase(Product product)
        {
            _repo.UpdateProduct(product);

            return RedirectToAction("ViewProduct", new { id = product.ProductID });
        }

        //[Authorize]
        public IActionResult InsertProduct()
        {
            var prod = _repo.AssignCategory();

            return View(prod);
        }

        //[Authorize]
        public IActionResult InsertProductToDatabase(Product productToInsert)
        {
            _repo.InsertProduct(productToInsert);

            return RedirectToAction("Index");
        }

        //[Authorize]
        public IActionResult DeleteProduct(Product product)
        {
            _repo.DeleteProduct(product);

            return RedirectToAction("Index");
        }



    }
}
