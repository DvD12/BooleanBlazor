namespace BlazorPrimaApplicazione.Data
{
    public class PizzaManager
    {
        private static List<Pizza> PizzaList { get; set; } = new List<Pizza>()
        {
            new Pizza()
            {
                Name = "Pizza 1",
                Description = "pizza",
                Price = 5
            },
            new Pizza()
            {
                Name = "Pizza 2",
                Description = "pizza pizza",
                Price = 6
            }
        };

        public static List<Pizza> GetPizzas()
        {
            return PizzaList;
        }
    }
}
