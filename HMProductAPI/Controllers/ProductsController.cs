using Application.Common.ViewModel;
using Application.Product.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HMProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductCRUDOperations _productsCRUDOperations;

        public ProductsController(IProductCRUDOperations productsCRUDOperations) 
        {
            _productsCRUDOperations = productsCRUDOperations;
        }
        [Authorize]
        [HttpGet("getProduct")]
        public IActionResult GetProduct(Guid productId)
        {
            try
            {
                var product = _productsCRUDOperations.GetProduct(productId);
                if (string.IsNullOrEmpty(product))
                    return StatusCode(StatusCodes.Status404NotFound,"No Product available.");
                return Ok(product);
            }
            catch (Exception ex)
            {
                //_logger.Log(LogLevel.Error, ex.Message, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost("createProduct")]
        public IActionResult CreateProduct(ProductVM productVM)
        {
            try
            {
                string oid = ClaimsPrincipal.Current.FindFirst("oid")?.Value;
                productVM.createdBy = oid;
                var status = _productsCRUDOperations.AddProduct(productVM);
                if (status.statusCode == HttpStatusCode.Created)
                    return Ok(status);
                else
                {
                    if (status.statusCode == HttpStatusCode.BadRequest)
                        return StatusCode(StatusCodes.Status400BadRequest, status);
                    if (status.statusCode == HttpStatusCode.NotAcceptable)
                        return StatusCode(StatusCodes.Status406NotAcceptable,status);
                    else
                        return StatusCode(StatusCodes.Status400BadRequest,status);
                    
                }
            }
            catch (Exception ex)
            {
                //_logger.Log(LogLevel.Error, ex.Message, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
