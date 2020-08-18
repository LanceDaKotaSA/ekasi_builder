using EkasiBuilders.Models;
using EkasiBuilders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EkasiBuilders.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext appDb = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            var addedItem = appDb.Products.Single(i => i.ProductId == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            int count = cart.AddToCart(addedItem);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(addedItem.ProductName) +
                    " has been added to your shopping card",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = count,
                DeleteId = id
            };
            return Json(results);
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string itemName = appDb.Products
                .Single(p => p.ProductId == id).ProductName;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = "One(1) " + Server.HtmlEncode(itemName) +
                        " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);

        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }
    }
}