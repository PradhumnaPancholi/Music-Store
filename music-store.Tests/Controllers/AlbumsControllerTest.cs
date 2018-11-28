using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using music_store.Controllers;
using music_store.Models;

namespace music_store.Tests.Controllers
{
    [TestClass]
    public class AlbumsControllerTest
    {
        AlbumsController controller;
        Mock<IAlbumsMock> mock;
        List<Album> albums;

        [TestInitialize]
        public void TestInitialize()
        {
            //creates a new mock-data object to holf fafe data //
            mock = new Mock<IAlbumsMock> ();

            //populate mock list/
            albums = new List<Album>
            {
                new Album { AlbumId = 100, Title = "Test 1 ", Price = 6.99m, Artist = new Artist {
                        ArtistId = 1000, Name = "Singer 1"
                    }
                },
                new Album { AlbumId = 101, Title = "Test 2 ", Price = 7.99m, Artist = new Artist {
                        ArtistId = 1007, Name = "Singer 2"
                    }
                }
            };

            //pass mock lst to albums controller//
            //albums.OrderBy(a => a.Artist.Name).ThenBy(a => a.Title);

            mock.Setup(m => m.Albums).Returns(albums.AsQueryable());
            controller = new AlbumsController(mock.Object);

        }

        [TestMethod]
        public void IndexLoadsView()
        {
            //arrange//
            //AlbumsController controller = new AlbumsController(); --moved to TestInitilize to keep code DRY 
            //act//
            ViewResult result = controller.Index() as ViewResult;

            //assert//
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]            
        public void IndexReturnsAlbums()
        {
            //act //
            var result = (List<Album>)((ViewResult)controller.Index()).Model;

            //assert//
            CollectionAssert.AreEqual(albums, result);
        }

        //Tests for GET: Albums/Details

        [TestMethod]
        public void DetailsNoIdLoadsError()
        {
            //act//
            ViewResult result = (ViewResult)controller.Details(null);

            //assert//
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DetailsInvalidIdLoadsError()
        {
            //act//
            ViewResult result = (ViewResult)controller.Details(10000);

            //assert//
            Assert.AreEqual("Error", result.ViewName);

        }

    }
}
