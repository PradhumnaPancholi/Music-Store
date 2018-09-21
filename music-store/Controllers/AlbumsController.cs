using music_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace music_store.Controllers
{
    public class AlbumsController : Controller
    {
        // GET: Albums
        public ActionResult Index()
        {
            //create a mocklist of type album//
            var albums = new List<Album>(); 

            for (int i=1; i <= 10; i++)
            {
                albums.Add(new Album { Title = "Album " + i.ToString() });
            }

            //add albums to ViewBag for display//
            ViewBag.albums = albums;
            return View();
        }
    }
}