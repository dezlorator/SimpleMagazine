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
    public class CommentController : Controller
    {
        #region private

        private readonly ICommentRepository _commentRepository;
        private readonly IProductExtendedRepository _productExtendedRepository;
       // private int PageSize = 4;

        #endregion

        public CommentController(ICommentRepository commentRepository, IProductExtendedRepository productExtendedRepository)
        {
            _commentRepository = commentRepository;
            _productExtendedRepository = productExtendedRepository;
        }

        //public ViewResult GetByProductId(int id, int commentPage = 1)
        //{
        //    var comments = _commentRepository.Сomment.Where(p => p.Product.ID == id);

        //    if(comments.Count() == 0)
        //    {
        //        TempData["message_search"] = $"Поиск не дал результатов";
        //    }

        //    var paging = new PagingInfo
        //    {
        //        CurrentPage = commentPage,
        //        ItemsPerPage = PageSize,
        //        TotalItems = comments.Count()
        //    };

        //    var commentViewModel = new CommentViewModel()
        //    {
        //        Comments = comments,
        //        PagingInfo = paging
        //    };

        //    return View(commentViewModel);
        //}

        public ViewResult Create(int productId, string returnUrl) => View(new CommentViewModel
        {
            ProductId = productId,
            ReturnUrl = returnUrl
        });

        [HttpPost]
        public IActionResult Create(CommentViewModel commentModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { commentModel.ReturnUrl });
            }

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Message = commentModel.Message,
                    Rating = commentModel.Rating,
                    Time = DateTime.Now,
                    UserName = User.Identity.Name
                };

                _commentRepository.SaveComment(comment);

                _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Product.ID == commentModel.ProductId)
                    .Comments.Add(comment);
                _productExtendedRepository.SaveChanges();

                return Redirect(commentModel.ReturnUrl);
            }
            else
            {
                TempData["message"] = $"Ошибка";
                return Redirect(commentModel.ReturnUrl);
            }
        }

        public ViewResult Edit(int commentId, string returnUrl)
        {
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

            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(CommentViewModel commentModel, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { commentModel.ReturnUrl });
            }

            if (User.Identity.Name != commentModel.UserName || !User.IsInRole("Admin"))
            {
                TempData["message"] = $"Пользователь не имеет права редактировать комментарий";

                return Redirect(commentModel.ReturnUrl);
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

                return Redirect(commentModel.ReturnUrl);
            }
            else
            {
                TempData["message"] = $"Ошибка";
                return Redirect(commentModel.ReturnUrl);
            }
        }

        public IActionResult Delete(int commentId, string returnUrl)
        {
            var productId = _productExtendedRepository.ProductsExtended
                .FirstOrDefault(p => p.Comments.Any(c => c.ID == commentId)).Product.ID;
            var comment = _commentRepository.DeleteComment(commentId);
            _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Product.ID == productId)
                .Comments.Remove(comment);

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            if (User.Identity.Name != comment.UserName || !User.IsInRole("Admin"))
            {
                TempData["message"] = $"Пользователь не имеет права удалять комментарий";

                return Redirect(returnUrl);
            }

            TempData["message"] = $"Комментарий удален";

            return Redirect(returnUrl);
        }
    }
}