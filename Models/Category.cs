using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EkasiBuilders.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter your category name.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}