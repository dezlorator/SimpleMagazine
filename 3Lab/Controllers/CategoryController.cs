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
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region private

        private readonly ICategoryRepository _repository;

        #endregion

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> List()
        {
            return Ok(_repository.Categories);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromForm]CategoryViewModel categoryModel)
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
                    return BadRequest("Parent is null");
                }

                parent.Children.Add(category);

                _repository.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("GetEdit")]
        public async Task<ActionResult> Edit([FromForm]int categoryId)
        {
            return Ok(_repository.Categories.FirstOrDefault(c => c.ID == categoryId));
        }

        [HttpPost("Edit")]
        public async Task<ActionResult> Edit([FromForm]CategoryNode category, [FromForm]int id)
        {
            if (ModelState.IsValid)
            {
                var cat = _repository.Categories.FirstOrDefault(c => c.ID == id);
                cat.Name = category.Name;

                _repository.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("List");
            }
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromForm]int categoryId)
        {
            DeleteChildren(categoryId);

            _repository.DeleteCategory(categoryId);

            return Ok();
        }

        public void DeleteChildren([FromForm]int categoryId)
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