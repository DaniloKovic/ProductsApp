namespace ProductsApp.Model.Response
{
    public class CategoryResponse : BaseResponse
    {
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }

        public string Name { get; set; }
        public string? ParentCategoryName { get; set; }

        public List<ProductResponse> Products { get; set; }
    }
}
