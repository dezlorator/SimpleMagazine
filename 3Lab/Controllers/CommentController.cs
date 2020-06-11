using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningEngine.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region private

        private readonly ICommentRepository _commentRepository;
        private readonly IProductExtendedRepository _productExtendedRepository;
        private ApplicationDbContext _context;
        // private int PageSize = 4;

        #endregion

        public CommentController(ICommentRepository commentRepository, IProductExtendedRepository productExtendedRepository,
                    ApplicationDbContext context)
        {
            _commentRepository = commentRepository;
            _productExtendedRepository = productExtendedRepository;
            _context = context;
    }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromForm]CommentViewModel commentModel)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if(role.CanAddComments == false)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Message = commentModel.Message,
                    Rating = commentModel.Rating,
                    Time = DateTime.Now,
                    UserName = this.GetUserName()
                };

                _commentRepository.SaveComment(comment);

                _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Product.ID == commentModel.ProductId)
                    .Comments.Add(comment);
                _productExtendedRepository.SaveChanges();

                return Ok(commentModel.ReturnUrl);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("GetModel")]
        public async Task<ActionResult> Edit([FromForm]int commentId, [FromForm]string returnUrl)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanModerateComments == false)
            {
                return Forbid();
            }

            var comment = _commentRepository.Сomment.FirstOrDefault(p => p.ID == commentId);
            var productId = _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Comments.Any(c => c.ID == commentId)).Product.ID;

            var result = new CommentViewModel
            {
                ID = comment.ID,
                Message = comment.Message,
                ProductId = productId,
                UserName = comment.UserName,
                Rating = comment.Rating,
                Time = comment.Time,
                ReturnUrl = returnUrl
            };

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit([FromForm]CommentViewModel commentModel, [FromForm]int id)
        {
            if (this.GetUserName() != commentModel.UserName)
            {
                return BadRequest(commentModel.ReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var repositoryComment = _productExtendedRepository.ProductsExtended
                    .FirstOrDefault(p => p.Product.ID == commentModel.ProductId)
                    .Comments.FirstOrDefault(p => p.ID == id);

                repositoryComment.Message = commentModel.Message;
                repositoryComment.Rating = commentModel.Rating;
                repositoryComment.Time = DateTime.Now;

                _productExtendedRepository.SaveChanges();

                return Ok(commentModel.ReturnUrl);
            }
            else
            {
                return BadRequest(commentModel.ReturnUrl);
            }
        }

        [HttpPost("Delete")]
        public IActionResult Delete([FromForm]int commentId, [FromForm]string returnUrl)
        {
            var productId = _productExtendedRepository.ProductsExtended
                .FirstOrDefault(p => p.Comments.Any(c => c.ID == commentId)).Product.ID;
            var comment = _commentRepository.DeleteComment(commentId);
            _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Product.ID == productId)
                .Comments.Remove(comment);

            if (this.GetUserName() != comment.UserName || !User.IsInRole("Admin"))
            {
                return BadRequest(returnUrl);
            }

            return Ok(returnUrl);
        }
    }
}