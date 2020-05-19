using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetStore.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Введите название товара")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите описание товара")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Введите положительную цену")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Укажите категорию")]
        public CategoryNode Category { get; set; }
        public string ImageId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [NotMapped]
        public bool IsInStock { get; set; } = false;
    }
}
