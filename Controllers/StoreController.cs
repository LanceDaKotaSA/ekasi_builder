using EkasiBuilders.Models;
using EkasiBuilders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EkasiBuilders.Controllers
{
    public class StoreController : Controller
    {
        ApplicationDbContext appDb = new ApplicationDbContext();

        // GET: Store
        public ActionResult Index()
        {
            var categories = appDb.Categories.ToList();

            return View(categories);
        }

        //
        // GET: /Store/Browse?category=Hose Reel
        public ActionResult Browse(string category)
        {
            // Retrieve Category and its Associated Products from database
            var categoryModel = appDb.Categories.Include("Products")
                .Single(c => c.CategoryName == category);

            return View(categoryModel);
        }

        ////// GET: Store/Details/5
        public ActionResult Details(int id)
        {
            var category = appDb.Products.Find(id);

            return View(category);
        }

        //
        // GET: /Store/CategoryMenu
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = appDb.Categories.ToList();

            return PartialView(categories);
        }

        //[HttpPost]
        //public ActionResult AddToCart(int id)
        //{
        //    var addedItem = appDb.Products.Single(i => i.ProductId == id);

        //    var cart = ShoppingCart.GetCart(this.HttpContext);

        //    int count = cart.AddToCart(addedItem);

        //    var results = new ShoppingCartRemoveViewModel
        //    {
        //        Message = Server.HtmlEncode(addedItem.ProductName) +
        //            " has been added to your shopping card",
        //        CartTotal = cart.GetTotal(),
        //        CartCount = cart.GetCount(),
        //        ItemCount = count,
        //        DeleteId = id
        //    };
        //    return Json(results);
        //}


        public async Task<ActionResult> RenderImage(int id)
        {
            Product product = await appDb.Products.FindAsync(id);

            byte[] photoBack = product.InternalImage;

            return File(photoBack, "image/png");
        }
    }
}