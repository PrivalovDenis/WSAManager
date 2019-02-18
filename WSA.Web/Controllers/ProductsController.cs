using AutoMapper;
using WSAManager.Core.Entities;
using WSAManager.Core.Services;
using WSAManager.Dto.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Swashbuckle;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http.Description;
using System.Net;

namespace WSAManager.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService service)
        {
            _productService = service;
        }

        /// <summary>
        /// Get Products List
        /// </summary>
        /// <returns>Returns List typeof(ProductDto)</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();

                var productsDto = new List<ProductDto>();

                Mapper.Map(products, productsDto);

                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                //TODO: Log something here
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get product By Product Id
        /// </summary>
        /// <param name="id">Product Identifier</param>
        /// <returns>Retruns instance typeof(ProductDto)</returns>
        [Route("ById/{id:int}")]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            try
            {
                Product prod = await _productService.GetByIdAsync(id);

                if (prod == null)
                {
                    return NotFound();
                }

                var productDto = new ProductDto();

                Mapper.Map(prod, productDto);

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                //TODO: Log something here
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update exiting Product By Id
        /// </summary>
        /// <param name="id">Product Identifier</param>
        /// <param name="productDto">Product entity</param>
        /// <returns>Returns instance typeof(ProductDto)</returns>
        [HttpPut]
        public async Task<IHttpActionResult> PutProduct(int id, Product productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productDto.Id)
            {
                return BadRequest();
            }

            try
            {
                Product product = await _productService.GetByIdAsync(id);

                product.Name = productDto.Name;

                await _productService.UpdateAsync(product);
              
                return Ok(productDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create new Product
        /// </summary>
        /// <param name="productDto">Product entity</param>
        /// <returns>Returns instance typeof(ProductDto)</returns>
        [HttpPost]
        public async Task<IHttpActionResult> PostProduct(Product productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try {
                Product prod = new Product();

                prod.Name = productDto.Name;

                prod = await _productService.AddAsync(prod);

                productDto.Id = prod.Id;

                return CreatedAtRoute("ApiRoute", new { id = productDto.Id }, productDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something her...
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete product By id
        /// </summary>
        /// <param name="id">Propduct Identifier</param>
        /// <returns>Returns 200 if Deleted</returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            try
            {
                Product prod = await _productService.GetByIdAsync(id);
                if (prod == null)
                {
                    return NotFound();
                }

                await _productService.DeleteAsync(prod);

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something here..
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsProductExists(int id)
        {
            return _productService.GetByIdAsync(id) != null;
        }
        
    }
}