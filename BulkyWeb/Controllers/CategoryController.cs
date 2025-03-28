using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using SD7501Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        //CREATE BUTTON
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                TempData["error"] = "Error";
                ModelState.AddModelError("Name", "The Display Order cannot exactly match with the name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created succesfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        //EDIT BUTTON
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);

            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated succesfully";
                return RedirectToAction("Index");
            }
            return View();

        }


        //EDIT BUTTON
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);

            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            return View(categoryFromDb);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted succesfully";
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
