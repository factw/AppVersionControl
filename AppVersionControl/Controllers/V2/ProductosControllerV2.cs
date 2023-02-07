using AppVersionControl.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AppVersionControl.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductosControllerV2 : ControllerBase
    {
        private const string URL = "https://fakestoreapi.com/products";
        private readonly HttpClient _httpClient;

        public ProductosControllerV2(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [MapToApiVersion("2.0")]
        [HttpGet(Name = "ProductoWithGUID")]

        public async Task<IActionResult> GetProductsAsync()
        {
            _httpClient.DefaultRequestHeaders.Clear();

            var prodStream = await _httpClient.GetStreamAsync(URL);

            var productoDto = await JsonSerializer.DeserializeAsync<List<ProductoV2>>(prodStream);

            foreach(var item in productoDto) 
            {
                item.Internalld = Guid.NewGuid();
            }

            return Ok(productoDto);

        }
    }
}
