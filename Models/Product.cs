using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EkasiBuilders.Models
{
    [Bind(Exclude = "ProductId")]
    public class Product
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please enter product name.")]
        [Display(Name = "Product Name")]
        [Remote("Product Exists", "Products", ErrorMessage = "Error, product already exists")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter selling price.")]
        [Display(Name = "Selling Price")]
        [DataType(DataType.Currency)]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Please enter cost price.")]
        [Display(Name = "Cost Price")]
        [DataType(DataType.Currency)]
        public decimal CostPrice { get; set; }

        [Required(ErrorMessage = "Please enter product quantity.")]
        [Display(Name = "Quantity")]
        public int Quantity{ get; set; }

        [Required(ErrorMessage = "Please enter description.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Picture")]
        public byte [] InternalImage { get; set; }

        [Display(Name = "Local file")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    InternalImage = target.ToArray();
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
   
            }
        }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}