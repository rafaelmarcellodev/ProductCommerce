namespace ProductAPI.Data.Dtos.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
