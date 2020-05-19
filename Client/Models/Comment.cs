using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Comment
    {
        [BindNever]
        public int ID { get; set; }
        [Required(ErrorMessage = "Введите комментарий")]
        public string Message { get; set; }
        [Range(0, 10, ErrorMessage = "Недопустимый рейтинг")]
        public int Rating { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
    }
}
