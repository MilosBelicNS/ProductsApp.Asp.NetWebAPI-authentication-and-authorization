using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductApp.Asp.NetWebApi.Controllers;
using ProductApp.Asp.NetWebApi.Interface;
using ProductApp.Asp.NetWebApi.Models;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Linq;

namespace ProductsAppUnitTests.UnitTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetById(42)).Returns(new Product { Id = 42 });

            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetById(42);
            var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(42, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetById(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange 
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetById(10)).Returns(new Product { Id = 10 });
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(10, new Product { Id = 9, Name = "Product2" });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Product { Id = 10, Name = "Product2" });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(10, createdResult.RouteValues["id"]);
        }

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<Product> products = new List<Product>();
            products.Add(new Product { Id = 1, Name = "Product1" });
            products.Add(new Product { Id = 2, Name = "Product2" });

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(products.AsEnumerable());
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IEnumerable<Product> result = controller.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products.Count, result.ToList().Count);
            Assert.AreEqual(products.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(products.ElementAt(1), result.ElementAt(1));
        }
    }
}
