using ProductApp.Asp.NetWebApi.Interface;
using ProductApp.Asp.NetWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProductApp.Asp.NetWebApi.Controllers
{
    public class ProductsController : ApiController
    {

        public IProductRepository _repository { get; set; }


        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }


        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetById(int id)
        {
            var product = _repository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Delete(int id)
        {
            var product = _repository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            _repository.Delete(product);

            return Ok();
        }

        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Create(product);

            return CreatedAtRoute("DefaultApi", new { Id = product.Id }, product);
        }

        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Put(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product.Id != id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(product);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(product);
        }
    }
}
