using music_store.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace fri_pm_music_store.Controllers
{
    public class StoreController : Controller
    {
        // db connection - this is done automatically in scaffolded controllers
        private MusicStoreModel db = new MusicStoreModel();

        // GET: Store
        public ActionResult Index()
        {
            var genres = db.Genres.OrderBy(g => g.Name).ToList();
            return View(genres);
        }

        // GET: Store/Albums?genre=Name
        public ActionResult Albums(string genre)
        {
            // get albums for selected genre
            var albums = db.Albums.Where(a => a.Genre.Name == genre).ToList();

            // load view and pass the album list for display
            return View(albums);
        } 

        // GET: Store/Product
        public ActionResult Product(string ProductName)
        {
            ViewBag.ProductName = ProductName;
            return View();
        }

        //GET: Store/Add to cart method//
        public ActionResult AddToCart(int AlbumId)
        {
            //to create unique cart id//
            String CurrentCartId;
            GetCartId();
            CurrentCartId = Session["CartId"].ToString();
            //create and save item into cart//

            //to check if item already exists in cart//
            Cart cart = db.Carts.SingleOrDefault(c => c.AlbumId == AlbumId && c.CartId == CurrentCartId);

            if(cart  == null)
            {
                cart = new Cart
                {
                    CartId = CurrentCartId,
                    AlbumId = AlbumId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                db.Carts.Add(cart);
            }
            else
            {
                //increament count by 1//
                cart.Count++;
            }
       
            db.SaveChanges();
            //redirect to cart page//
            return RedirectToAction("ShoppingCart");
        }

        private void GetCartId()
        {
            //to check if it's first item in cart
            if (Session["CartId"] == null)
            {
                //if user is logged in//
                if (User.Identity.IsAuthenticated)
                {
                    Session["CartId"] = User.Identity.Name;
                }
                else
                {
                    //random unique string for CartId//
                    Session["CartId"] = Guid.NewGuid().ToString();
                }
            }
        }

        public ActionResult ShoppingCart()
        {
            //check or generate session cartid//
            GetCartId();
            //select current user's cart//
            string CurrentCardId = Session["CartID"].ToString();
            var CartItems = db.Carts.Where(c => c.CartId == CurrentCardId).ToList();

            return View(CartItems);
        }

        //GET : /Store/RemoveFromCart/:id
        public ActionResult RemoveFromCart(int id)
        {
            //find and delete item from cart//
            Cart CartItem = db.Carts.SingleOrDefault(c => c.RecordId == id);
            db.Carts.Remove(CartItem);
            db.SaveChanges();

            //reload the page//
            return RedirectToAction("ShoppingCart");
        }

        //GET : /Store/Chckout/
        [Authorize]
        public ActionResult Checkout()
        {
            //migrte cart items if user was not logged in while adding items to cart//
            MigrateCart();
            return View();
        }

        //POST : /Store/Chckout/
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(FormCollection values)
        {
            //create a new order and populate it from form values//
            Order order = new Order();
            TryUpdateModel(order);
 
            //autopopulate the fields from user information//
            order.Username = User.Identity.Name;
            order.Email = User.Identity.Name;
            order.OrderDate = DateTime.Now;
            //for running total//
            var CartItems = db.Carts.Where(c => c.CartId == User.Identity.Name);
            Decimal OrderTotal = (from c in CartItems
                                  select (int)c.Count * c.Album.Price).Sum();
            order.Total = OrderTotal;

            //save order//
            db.Orders.Add(order);
            db.SaveChanges();

            //save each item to order details table//
            foreach(Cart item in CartItems)
            {
                OrderDetail od = new OrderDetail
                {
                    OrderId = order.OrderId,
                    AlbumId = item.AlbumId,
                    Quantity = item.Count,
                    UnitPrice = item.Album.Price
                };

                db.OrderDetails.Add(od);
            }
            //save all order items// 
            db.SaveChanges();

            //show the confirmation page//
            return RedirectToAction("Orders/Details", new {id = order.OrderId});
        }

        private void MigrateCart()
        {
            //attach anonymous cart items to users cart after login in
            if (!String.IsNullOrEmpty(Session["CartId"].ToString()) && User.Identity.IsAuthenticated) 
            {
                if(Session["CartID"].ToString() != User.Identity.Name)
                {
                    //get cartitems with random id//
                    String CureentCartId = Session["CartId"].ToString();
                    var CartItems = db.Carts.Where(c => c.CartId == CureentCartId);

                    foreach (Cart item in CartItems)
                    {
                        item.CartId = User.Identity.Name;
                    }

                    db.SaveChanges();

                    //update session var to username//
                    Session["CartId"] = User.Identity.Name;
                }
            }
        }

    }
}