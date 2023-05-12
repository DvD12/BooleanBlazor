using ServiceStack.DataAnnotations;

namespace BlazorPrimaApplicazione.Data
{
    public class Pizza
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
