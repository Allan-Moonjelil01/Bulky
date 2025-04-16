using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD7501Bulky.DataAccess.Repository.IRepository;
using SD7501Bulky.Models;
using SD7501Bulky.Models.ViewModels;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
       
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
     

            return View(objProductList);
        }

        ////CREATE BUTTON
        //public IActionResult Create()
        //{
        //    //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //    //{
        //    //    Text = u.Name,
        //    //    Value = u.Id.ToString()
        //    //});
        //    //ViewBag.CategoryList = CategoryList;
        //    //ViewData["CategoryList"]= CategoryList;
        //    ProductVM productVM = new()
        //    {
        //        CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        }),
        //        Product = new Product()
        //    };
        //    return View(productVM);
        //}

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
                if (id == null || id == 0)
            {
                //create
                return View(productVM);
            } else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            String tempMsg = "";
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    tempMsg = "Product created succesfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    tempMsg = "Product updated succesfully";
                }
                //_unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                //TempData["success"] = "Product created succesfully";
                TempData["success"] = tempMsg;
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        //[HttpPost]
        //public IActionResult Create(ProductVM productVM)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(productVM.Product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created succesfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //        Text = u.Name,
        //        Value = u.Id.ToString()
        //        });
        //        return View(productVM);
        //    }
                
        //}

        ////EDIT BUTTON
        //public IActionResult Edit(int? id)
        //{
        //    if(id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    if(productFromDb == null)
        //    {
        //        return NotFound();
        //    }


        //    //Product? ProductFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);

        //    //Product? ProductFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated succesfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}


        //EDIT BUTTON
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            //Product? ProductFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);

            //Product? ProductFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            return View(productFromDb);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted succesfully";
            return RedirectToAction("Index");
        }




        public string GetAllCategories()
        {
            return "Return All Categories";
        }

        public string GetCategoriesByName(string name)
        {
            return $"Return All Categories :{name}";
        }


    }
}
