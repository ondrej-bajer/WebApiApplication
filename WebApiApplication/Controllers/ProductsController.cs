using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.DTOs;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public ActionResult<IEnumerable<ProductDto>> GetAllV1()
            => Ok(_service.GetAll());

        [HttpGet]
        [MapToApiVersion("2.0")]
        public ActionResult<PagedResponse<ProductDto>> GetAllV2([FromQuery] PaginationQuery query)
            => Ok(_service.GetPaged(query.Page, query.PageSize));


        [HttpGet("{id:int}")]
        [MapToApiVersion("1.0")]
        public ActionResult<ProductDto> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive integer.");

            var product = _service.GetById(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPatch("{id:int}/description")]
        [Consumes("application/json")]
        [MapToApiVersion("1.0")]
        public IActionResult UpdateDescription(int id, [FromBody] UpdateProductDescriptionRequest request)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive integer.");

            var ok = _service.UpdateDescription(id, request.Description);
            return ok ? NoContent() : NotFound();
        }
    }
}
