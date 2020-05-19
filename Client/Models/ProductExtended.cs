using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetStore.Models;

namespace PetStore.Models
{
    public class ProductExtended
    {
        [BindNever]
        public int ID { get; set; }
        public Product Product { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public string LongDescription { get; set; }
        public string Manufacturer { get; set; }
        public string OriginCountry { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public int ProductIdentifier { get; set; }
    }
}
