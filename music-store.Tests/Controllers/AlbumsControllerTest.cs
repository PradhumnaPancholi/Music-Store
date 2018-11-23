using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using music_store.Controllers;

namespace music_store.Tests.Controllers
{
    [TestClass]
    public class AlbumsControllerTest
    {
        [TestMethod]
        public void IndexLoadsView()
        {
            //arrange//
            AlbumsController controller = new AlbumsController();

            //act//
            ViewResult result = controller.Index() as ViewResult;

            //assert//
            Assert.IsNotNull(result);
        }
    }
}
