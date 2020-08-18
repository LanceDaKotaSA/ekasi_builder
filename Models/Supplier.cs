using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EkasiBuilders.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Please enter supplier name.")]
        [Display(Name = "Supplier Name")]
        [Remote("SupplierExists", "Suppliers", ErrorMessage = "Error, Supplier already exists.")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Please enter telephone number.")]
        [Display(Name = "Telephone number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Please enter email address.")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter street name.")]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Please enter city.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter postal code")]
        [Display(Name = "Postal Code")]
        [MaxLength(4)]
        [MinLength(4)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please enter contact person")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Please enter contact telephone.")]
        [Display(Name = "Contact person telephone number")]
        [DataType(DataType.PhoneNumber)]
        public string PersonTelephone { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}