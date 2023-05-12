using ServiceStack.OrmLite;

namespace BlazorPrimaApplicazione.Data
{
    public class PizzaManager
    {
        public static List<Pizza> GetPizzas()
        {
            using var db = OrmHelper.OpenConnection();
            return db.Select<Pizza>();
        }
    }
}
