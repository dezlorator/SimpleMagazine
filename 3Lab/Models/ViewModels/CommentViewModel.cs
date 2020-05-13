using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetStore.Models;

namespace PetStore.Models.ViewModels
{
    public class CommentViewModel
    {
        //public IEnumerable<Сomment> Comments { get; set; }
        //public PagingInfo PagingInfo { get; set; }
        [BindNever]
        public int ID { get; set; }
        [Required(ErrorMessage = "Введите комментарий")]
        public string Message { get; set; }
        [Range(0, 10, ErrorMessage = "Недопустимый рейтинг")]
        public int Rating { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string ReturnUrl { get; set; }
    }
}
