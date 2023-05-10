using ServiceStack.OrmLite;
using System.Data;

namespace BlazorPrimaApplicazione.Data
{
    public static class OrmHelper
    {
        public static string ConnectionString { get; private set; } = "Data Source=localhost;Initial Catalog=PizzeriaOrmLite;Integrated Security=True";
        public static IOrmLiteDialectProvider Dialect { get; set; } = SqlServerDialect.Provider;

        private static OrmLiteConnectionFactory ConnectionFactory { get; set; }
        public static IDbConnection OpenConnection()
        {
            if (ConnectionFactory == null)
                ConnectionFactory = new OrmLiteConnectionFactory(ConnectionString, Dialect);

            return ConnectionFactory.Open();
        }

        public static void CreateMissingColumns<T>(this IDbConnection db) where T : new()
        {
            var model = ModelDefinition<T>.Definition;
            // just create the table if it doesn't already exist
            if (db.TableExists(model.ModelName) == false)
            {
                try
                {
                    db.CreateTable<T>(overwrite: false);
                }
                catch (Exception ex)
                {
                    var cause = db.GetLastSql();
                    throw ex;
                }
                return;
            }

            // find each of the missing fields      
            var columns = db.GetColumnNames(model.ModelName);
            var missing = ModelDefinition<T>.Definition.FieldDefinitions
                .Where(field => columns.Any(x => x.ColumnName == field.FieldName) == false)
                .ToList();
            // add a new column for each missing field
            db.SetCommandTimeout(0);
            foreach (var field in missing)
            {
                try
                {
                    if (field.FieldType == typeof(DateTime))
                        field.DefaultValue = "'1900-01-01 00:00:01'";
                    else if (field.FieldType == typeof(TimeSpan))
                        field.DefaultValue = "0";
                    else if (field.FieldType == typeof(Boolean))
                        field.DefaultValue = "0";
                    else if (field.FieldType.IsEnum)
                        field.DefaultValue = $"'{field.DefaultValue}'";
                    else if (field.DefaultValue == null)
                            field.DefaultValue = field.FieldTypeDefaultValue?.ToString().ToUpper();
                    db.AddColumn(typeof(T), field);
                }
                catch (Exception e)
                {
                    db.SetCommandTimeout(60);
                    Console.WriteLine(db.GetLastSql());
                    throw;
                }
            }
            db.SetCommandTimeout(60);
        }
        private static List<DataColumn> GetColumnNames(this IDbConnection db, string tableName)
        {
            var columns = new List<DataColumn>();
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = GetCommandText(tableName);
                var tbl = new DataTable();
                tbl.Load(cmd.ExecuteReader());
                for (int i = 0; i < tbl.Columns.Count; i++)
                {
                    columns.Add(tbl.Columns[i]);
                }
            }
            return columns;
        }
        private static string GetCommandText(string tableName)
        {        
            return string.Format("select top 1 * from {0}", tableName);
        }
    }
}
