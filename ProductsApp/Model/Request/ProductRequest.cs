namespace ProductsApp.Model.Request
{
    public class ProductRequest : BaseRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int? CategoryId { get; set; }
    }
}
