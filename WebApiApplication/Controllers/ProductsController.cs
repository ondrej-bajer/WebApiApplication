using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.DTOs;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAll()
            => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<ProductDto> GetById(int id)
        {
            var product = _service.GetById(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPatch("{id:int}/description")]
        public IActionResult UpdateDescription(int id, [FromBody] UpdateProductDescriptionRequest request)
        {
            var ok = _service.UpdateDescription(id, request.Description);
            return ok ? NoContent() : NotFound();
        }
    }
}
