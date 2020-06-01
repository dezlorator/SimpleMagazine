using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class CategoryNode
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsRoot { get; set; } = false;
        public IList<CategoryNode> Children { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
