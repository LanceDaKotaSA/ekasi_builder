using EkasiBuilders.Models;
using EkasiBuilders.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EkasiBuilders.Controllers
{
    public class ProductListController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductList
        public ActionResult Index(string categoryName, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Price" : "";
            ViewBag.DateSortParm = sortOrder == "Quantity" ? "" : "Quantity";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //search by category
            var categoryList = new List<string>();

            var categoryQry = from cat in db.Categories
                              orderby cat.CategoryName
                              select cat.CategoryName;

            categoryList.AddRange(categoryQry.Distinct());
            ViewBag.itemCategory = new SelectList(categoryList);

            //search by product name
            var products = from s in db.Products
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString)
                                       );
            }

            //search by category
            if (!String.IsNullOrEmpty(categoryName))
            {
                products = products.Where(x => x.Category.CategoryName == categoryName);
            }

            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "Price":
                    products = products.OrderByDescending(s => s.SellingPrice);
                    break;
                case "Quantity":
                    products = products.OrderBy(s => s.Quantity);
                    break;

                default:  // Name ascending 
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Details(int? id)
        {
            Product answer = db.Products.Find(id);
            var model = new ProductDetailsViewModel
            {
                ProductName = answer.ProductName,
                Price = answer.SellingPrice,
                Quantity = answer.Quantity,
                Description = answer.Description
            };

            return View(model);
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product product = db.Products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);
        }

        public async Task<ActionResult> RenderImage(int id)
        {
            Product product = await db.Products.FindAsync(id);

            byte[] photoBack = product.InternalImage;

            return File(photoBack, "image/png");
        }
    }
}