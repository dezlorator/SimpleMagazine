using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetStore.Models;

namespace PetStore.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsRoot { get; set; } = false;
        public IList<CategoryNode> Children { get; set; }
        public int ParentID { get; set; }
    }
}
