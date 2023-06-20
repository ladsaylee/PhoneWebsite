using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    //the brackets are attributes
    [ApiController] //converts the class to api controller
    [Route("api/v1/[controller]")] // routes by this 
    public class CatalogController:ControllerBase
    {
        public readonly IProductRepository productRepository;
        public readonly ILogger<CatalogController> logger;
        public CatalogController(IProductRepository _productRepository, ILogger<CatalogController> _logger)
        {
            productRepository = _productRepository;
            logger = _logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() // IAction is used to return the response type if ok, not ok, nofound etc
        {
           var product = await productRepository.GetProducts();
            return Ok(product);
        }

        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await productRepository.GetProduct(id);
            if (product == null)
            {
                logger.LogError($"Product with id :{id},not found");
                return NotFound();
            }
                
           
            return Ok(product);
        }


        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var product = await productRepository.GetProductByName(name);
            if (product == null)
            {
                logger.LogError($"Product with id :{name},not found");
                return NotFound();
            }


            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string catgeory)
        {
            var product = await productRepository.GetProductByCategory(catgeory);
            if (product == null)
            {
                logger.LogError($"Product with id :{catgeory},not found");
                return NotFound();
            }


            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>>CreateProduct([FromBody]Product product)
        {
            await productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
           return  Ok(await productRepository.UpdateProduct(product));        
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            return Ok(await productRepository.DeleteProduct(id));
        }





    }
}
