using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data.Dtos.Product;
using ProductAPI.Data.Models;
using ProductAPI.Data.Repository;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ExceptionHandlingService _exceptionService;

        public ProductController(IProductRepository productRepository, IMapper mapper, ExceptionHandlingService exceptionService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _exceptionService = exceptionService;
        }

        /// <summary>
        /// Adiciona um novo produto.
        /// </summary>
        /// <remarks>
        /// Exemplo de corpo de solicitação JSON:
        ///
        ///     POST /api/Product
        ///     {
        ///         "name": "Nome do produto",
        ///         "stock": "Estoque do produto",
        ///         "price": "Preço do produto"
        ///     }
        ///
        /// </remarks>
        /// <param name="productDto">Informações necessários para criação de um novo produto.</param>
        /// <returns>Produto adicionado.</returns>
        [HttpPost]
        public async Task<ActionResult<CreateProductDto>> AddProduct(CreateProductDto productDto)
        {
            ValidationsErrors(productDto);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Product product = _mapper.Map<Product>(productDto);

                await _productRepository.AddProduct(product);

                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _exceptionService.HandleException(ex));
            }
        }

        /// <summary>
        /// Obtém uma lista de produtos com opção de ordenação.
        /// </summary>
        /// <param name="orderBy">Coluna para ordenar a lista solicitada [id, name, price].</param>
        /// <param name="ascending">Tipo de ordenação Ascendente(ascending = true) ou Descendente(ascending = false).</param>
        /// <returns>Lista de produtos.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ReadProductDto>>> GetProducts([FromQuery] string orderBy = "id",
            [FromQuery] bool ascending = true)
        {
            try
            {
                List<Product> products = await _productRepository.GetProducts();
                products = OrderByAndAscending(orderBy, ascending, products);

                return Ok(_mapper.Map<List<ReadProductDto>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _exceptionService.HandleException(ex));
            }
        }

        /// <summary>
        /// Obtém um produto pelo seu ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>O produto correspondente ao ID fornecido.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadProductDto>> GetProductById(int id)
        {
            Product? product = await _productRepository.GetProductById(id);

            ReadProductDto productDto = _mapper.Map<ReadProductDto>(product);

            if (productDto == null)
                return NotFound();
            return Ok(productDto);
        }

        /// <summary>
        /// Obtém uma lista de produtos pelo seu nome.
        /// </summary>
        /// <param name="name">Nome do produto.</param>
        /// <returns>O produto correspondente ao nome fornecido.</returns>
        [HttpGet("search/{name}")]
        public async Task<ActionResult<List<ReadProductDto>>> GetProductByName(string name)
        {
            try
            {
                List<Product> products = await _productRepository.GetProductByName(name);

                List<ReadProductDto> productsDto = _mapper.Map<List<ReadProductDto>>(products);

                if (productsDto == null)
                    return NotFound();
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _exceptionService.HandleException(ex));
            }
        }

        /// <summary>
        /// Atualiza um determinado produto por ID.
        /// </summary>
        /// <remarks>
        /// Exemplo de corpo de solicitação JSON:
        ///
        ///     PUT /api/Product
        ///     {
        ///         "name": "Nome do produto",
        ///         "stock": "Estoque do produto",
        ///         "price": "Preço do produto"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID do produto.</param>
        /// <param name="productDto">Informações necessários para atualização de um produto.</param>
        /// <returns>204 Produto atualizado com sucesso</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, CreateProductDto productDto)
        {
            ValidationsErrors(productDto);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product == null)
                    return NotFound();

                Product productUpdate = _mapper.Map(productDto, product);

                await _productRepository.UpdateProduct(productUpdate);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, _exceptionService.HandleException(ex));
            }
        }

        /// <summary>
        /// Ecluír um determinado produto por ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>204 Produto excluído com sucesso</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productRepository.DeleteProduct(id);

                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, _exceptionService.HandleException(ex));
            }
        }

        private void ValidationsErrors(CreateProductDto productDto)
        {
            if (String.IsNullOrEmpty(productDto.Name))
                ModelState.AddModelError(nameof(productDto.Name), "O nome não pode ficar em branco!");

            if (productDto.Price < 0)
                ModelState.AddModelError(nameof(productDto.Price), "O preço não pode ser menor que zero!");

            if (productDto.Stock < 0)
                ModelState.AddModelError(nameof(productDto.Stock), "O estoque não pode ser menor que zero!");

        }

        private static List<Product> OrderByAndAscending(string orderBy, bool ascending, List<Product> products)
        {
            if (products != null)
            {
                if (ascending)
                {
                    switch (orderBy.ToLower())
                    {
                        case "name":
                            products = products.OrderBy(p => p.Name).ToList();
                            break;
                        case "price":
                            products = products.OrderBy(p => p.Price).ToList();
                            break;
                        default:
                            products = products.OrderBy(p => p.Id).ToList();
                            break;
                    }
                }
                else
                {
                    switch (orderBy.ToLower())
                    {
                        case "name":
                            products = products.OrderByDescending(p => p.Name).ToList();
                            break;
                        case "price":
                            products = products.OrderByDescending(p => p.Price).ToList();
                            break;
                        default:
                            products = products.OrderByDescending(p => p.Id).ToList();
                            break;
                    }
                }

                return products;
            }

            return new List<Product>();
        }
    }
}
