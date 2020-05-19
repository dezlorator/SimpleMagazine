using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetStore.Models
{
    public class Order
    {
        #region properties

        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [BindNever]
        public bool Shipped { get; set; }
        [BindNever]
        public bool Canceled { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ФИО")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите улицу")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите номер дома")]
        public string House { get; set; }

        public string Room { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите область")]
        public string State { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите почтовый индекс")]
        [Range(0, 99999, ErrorMessage = "Недопустимый индекс")]
        public int Zip { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите страну")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; } 

        #endregion
    }
}
