using System.ComponentModel.DataAnnotations;

namespace ProductsApp.Model.Request
{
    public class CategoryRequest : BaseRequest
    {
        public int Id { get; set; }

        public int? ParentCategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
