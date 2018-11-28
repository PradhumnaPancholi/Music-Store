using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using music_store.Models;

namespace music_store.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AlbumsController : Controller
    {
        //private MusicStoreModel db = new MusicStoreModel();

        // GET: Albums

        //mock db connection//
        private IAlbumsMock db;

        //default controller-uses live db//
        public AlbumsController()
        {
            this.db = new EFAlbums();
        }

        //uses mock data//
        public AlbumsController(IAlbumsMock mock)
        {
            this.db = mock;
        }

        public ActionResult Index()
        {
            var albums = db.Albums.Include(a => a.Artist).Include(a => a.Genre);
            //return View(albums.ToList());
            return View("Index", albums.ToList());
        }


        // GET: Albums/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //scafolded code//
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //to work with mock
                return View("Error");

            }
            //old scafollede method , doesn't work with mock 
            //Album album = db.Albums.Find(id);

            Album album = db.Albums.SingleOrDefault(a => a.AlbumId  == id);
            if (album == null)
            {
                //scafolded code//
                //return HttpNotFound();

                //to work with mock
                return View("Error");
            }
            return View(album);
        }

        //// GET: Albums/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ArtistId = new SelectList(db.Artists.OrderBy(a => a.Name), "ArtistId", "Name");
        //    ViewBag.GenreId = new SelectList(db.Genres.OrderBy(g => g.Name), "GenreId", "Name");
        //    return View();
        //}

        //// POST: Albums/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //uploading the image// 
        //        if(Request.Files.Count > 0 )
        //        {
        //            var file = Request.Files[0];

        //            if(file.FileName != null && file.ContentLength > 0 )
        //            {
        //                //get dynamic file path//
        //                String path = Server.MapPath("~/Content/Images") + file.FileName;
        //                file.SaveAs(path);

        //                album.AlbumArtUrl = "/Content/Images" + file.FileName;
        //            }
        //        }
        //        db.Albums.Add(album);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
        //    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
        //    return View(album);
        //}

        //// GET: Albums/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Album album = db.Albums.Find(id);
        //    if (album == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
        //    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
        //    return View(album);
        //}

        //// POST: Albums/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //uploading the image// 
        //        if (Request.Files.Count > 0)
        //        {
        //            var file = Request.Files[0];

        //            if (file.FileName != null && file.ContentLength > 0)
        //            {
        //                //get dynamic file path//
        //                String path = Server.MapPath("~/Content/Images") + file.FileName;
        //                file.SaveAs(path);

        //                album.AlbumArtUrl = "/Content/Images" + file.FileName;
        //            }
        //        }
        //        db.Entry(album).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
        //    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
        //    return View(album);
        //}

        //// GET: Albums/Delete/5   
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Album album = db.Albums.Find(id);
        //    if (album == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(album);
        //}

        //// POST: Albums/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Album album = db.Albums.Find(id);
        //    db.Albums.Remove(album);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
