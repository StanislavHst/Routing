using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    [Route("products")]
    [Route("items")]
    public class ProductsController : Controller
    {
        private readonly List<Product> products;

        public ProductsController(Data data)
        {
            products = data.Products;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index(int? filterId, string filtername)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            List<Product> result = new List<Product>();
            foreach (Product product in products)
            {
                bool matchesId = filterId == null || product.Id == filterId;
                bool matchesName = string.IsNullOrEmpty(filtername)
                    || product.Name.Contains(filtername, StringComparison.OrdinalIgnoreCase);

                if (matchesId && matchesName)
                {
                    result.Add(product);
                }
            }
            return View(result);
        }

        private Product FindProduct(int id)
        {
            foreach (Product product in products)
            {
                if (product.Id == id)
                {
                    return product;
                }
            }
            return null;
        }

        [HttpGet("{id:int}")]
        [HttpGet("view/{id:int}")]
        public IActionResult Details(int id)
        {
            Product product = FindProduct(id);
            if (product == null)
            {
                return RedirectToAction("ProductNotFound");
            }
            return View(product);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            Product product = FindProduct(id);
            if (product == null)
            {
                return RedirectToAction("ProductNotFound");
            }
            return View(product);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Product product)
        {
            Product existing = FindProduct(id);
            if (existing == null)
            {
                return RedirectToAction("ProductNotFound");
            }

            product.Id = id;
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            existing.Name = product.Name;
            existing.Description = product.Description;
            return RedirectToAction("Index");
        }

        [HttpGet("create")]
        [HttpGet("new")]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost("create")]
        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            int nextId = 1;
            foreach (Product item in products)
            {
                if (item.Id >= nextId)
                {
                    nextId = item.Id + 1;
                }
            }
            product.Id = nextId;
            products.Add(product);
            return RedirectToAction("Index");
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            Product product = FindProduct(id);
            if (product == null)
            {
                return RedirectToAction("ProductNotFound");
            }
            products.Remove(product);
            return RedirectToAction("Index");
        }

        [HttpGet("not-found")]
        public IActionResult ProductNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
