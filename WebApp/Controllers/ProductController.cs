using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public ActionResult<int> AddProduct(string name, string description, decimal price)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                if (storageContext.Products.Any(p => p.Name == name))
                    return StatusCode(409);

                var product = new Product() { Name = name, Description = description, Price = price };
                storageContext.Products.Add(product);
                storageContext.SaveChanges();
                return Ok(product.Id);
            }
        }
        [HttpGet("get_all_products")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            using (StorageContext storageContext = new StorageContext())
            {
                var list = storageContext.Products.Select(p => new Product { Name = p.Name, Description = p.Description, Price = p.Price }).ToList();
                return Ok(list);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                var product = storageContext.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                    return NotFound();

                storageContext.Products.Remove(product);
                storageContext.SaveChanges();
                return Ok();
            }
        }

        [HttpPatch("{id}")]
        public ActionResult ChangePrice(int id, decimal newPrice)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                var product = storageContext.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                    return NotFound();

                product.Price = newPrice;
                storageContext.SaveChanges();
                return Ok();
            }
        }
    }
}