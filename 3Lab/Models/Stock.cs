using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public Product Product { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Введите положительную цену")]
        public int Quantity { get; set; }
    }
}
