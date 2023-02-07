using AppVersionControl.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AppVersionControl.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private const string ApiURL = "https://fakestoreapi.com/products";
        private readonly HttpClient _httpClient;

        public ProductosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [MapToApiVersion("1.0")]
        [HttpGet(Name = "ProductoWithRating")]
        public async Task<IActionResult> GetProductsAsync()
        {
            _httpClient.DefaultRequestHeaders.Clear();

            var prodStream = await _httpClient.GetStreamAsync(ApiURL);

            var productoDto = await JsonSerializer.DeserializeAsync<List<Producto>>(prodStream);

            return Ok(productoDto);

        }

    }
}
