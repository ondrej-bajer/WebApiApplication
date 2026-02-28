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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllV1(CancellationToken ct)
        {
            return Ok(await _service.GetAllAsync(ct));
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<ProductDto>))]
        [Produces("application/json")]
        public async Task<ActionResult<PagedResponse<ProductDto>>> GetAllV2([FromQuery] PaginationQuery query, CancellationToken ct)
        {
            return Ok(await _service.GetPagedAsync(query.Page, query.PageSize, ct));
        }

        [HttpGet("{id:int}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive integer.");

            var product = await _service.GetByIdAsync(id, ct);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPatch("{id:int}/description")]
        [Consumes("application/json")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] UpdateProductDescriptionRequest request, CancellationToken ct)
        {
            await _service.UpdateDescriptionAsync(id, request.Description, ct);
            return NoContent();
        }
    }
}
