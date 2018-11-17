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
            Cart cart = new Cart
            {
                CartId = CurrentCartId,
                AlbumId = AlbumId,
                Count = 1,
                DateCreated = DateTime.Now
            };

            db.Carts.Add(cart);
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
    }
}