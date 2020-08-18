using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EkasiBuilders.Models
{
    public partial class ShoppingCart
    {
        ApplicationDbContext appDb = new ApplicationDbContext();

        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(Product product)
        {
            var cartItem = appDb.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                appDb.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
            appDb.SaveChanges();

            return cartItem.Count;
        }

        public int RemoveFromCart(int id)
        {

            var cartItem = appDb.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    appDb.Carts.Remove(cartItem);
                }
                appDb.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = appDb.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                appDb.Carts.Remove(cartItem);
            }
            appDb.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return appDb.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in appDb.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in appDb.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Product.SellingPrice).Sum();

            return total ?? decimal.Zero;
        }

        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetail>();

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.SellingPrice,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Product.SellingPrice);
                order.OrderDetails.Add(orderDetail);
                appDb.OrderDetails.Add(orderDetail);
            }

            order.Total = orderTotal;

            appDb.SaveChanges();

            EmptyCart();

            return order;
        }

        //We are using httpContextBase to allow access to cookies
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = appDb.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            appDb.SaveChanges();
        }
    }
}