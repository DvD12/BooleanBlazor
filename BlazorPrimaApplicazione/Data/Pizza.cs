using ServiceStack.DataAnnotations;

namespace BlazorPrimaApplicazione.Data
{
    public class Pizza
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
