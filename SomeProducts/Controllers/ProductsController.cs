using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SomeProducts.Models;
using Microsoft.AspNet.Identity;

namespace SomeProducts.Controllers
{
    public class ProductsController : ApiController
    {
        //[Authorize]
        private ProductsContext db = new ProductsContext();

        // GET: api/Products/ForCurrentUser
        [Authorize]
        [Route("api/Products/ForCurrentUser")]
        public IQueryable<Product> GetProductsForCurrentUser()
        {
            string userId = User.Identity.GetUserId();
            return db.Products.Where(product => product.UserId == userId);
        }

        // GET: api/Products
        [Authorize]
        public IQueryable<Product> GetProducts()
        {
            string userId = User.Identity.GetUserId();
            return db.Products;
        }

        // GET: api/Products/5
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();

            if (userId != product.UserId)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.Identity.GetUserId();
            product.UserId = userId;

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            string userId = User.Identity.GetUserId();
            if (userId != product.UserId)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}