using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products); // Renvoie 200 OK
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var id = await _service.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id }); // Renvoie 201 Created avec l'URL du produit
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProductDto dto)
        {
            await _service.UpdateProductAsync(id, dto);
            return NoContent(); // Renvoie 204 No Content (standard pour Update)
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, PatchProductDto dto)
        {
            await _service.PatchProductAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/promotion")]
        public async Task<IActionResult> ApplyPromotion(Guid id, [FromQuery] decimal percentage)
        {
            await _service.ApplyPromotionAsync(id, percentage);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] bool activate)
        {
            await _service.ChangeStatusAsync(id, activate);
            return NoContent();
        }
    }
}
