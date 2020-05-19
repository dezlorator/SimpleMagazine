using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    public class CategoryController : Controller
    {
        #region private

        private readonly ICategoryRepository _repository;

        #endregion

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public ViewResult List()
        {
            ViewBag.Current = "Categories";

            return View(_repository.Categories);
        }

        public ViewResult Create(int parentId)
        {
            ViewBag.Current = "Categories";

            return View(new CategoryViewModel
            {
                ParentID = parentId
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create(CategoryViewModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var category = new CategoryNode
                {
                    Name = categoryModel.Name,
                };

                var parent = _repository.Categories.FirstOrDefault(c => c.ID == categoryModel.ParentID);

                if (parent == null)
                {
                    TempData["message"] = $"Ошибка";
                    return RedirectToAction("List");
                }

                parent.Children.Add(category);

                _repository.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                TempData["message"] = $"Ошибка";
                return RedirectToAction("List");
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult Edit(int categoryId)
        {
            ViewBag.Current = "Categories";

            return View(_repository.Categories.FirstOrDefault(c => c.ID == categoryId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Edit(CategoryNode category, int id)
        {
            if (ModelState.IsValid)
            {
                var cat = _repository.Categories.FirstOrDefault(c => c.ID == id);
                cat.Name = category.Name;

                _repository.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                TempData["message"] = $"Ошибка";

                return RedirectToAction("List");
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Delete(int categoryId)
        {
            DeleteChildren(categoryId);

            _repository.DeleteCategory(categoryId);

            return RedirectToAction("List");
        }

        public void DeleteChildren(int categoryId)
        {
            var category = _repository.Categories.FirstOrDefault(c => c.ID == categoryId);

            if (category != null)
            {
                foreach (var c in category.Children)
                {
                    DeleteChildren(c.ID);
                    _repository.DeleteCategory(categoryId);
                }
            }
        }
    }
}