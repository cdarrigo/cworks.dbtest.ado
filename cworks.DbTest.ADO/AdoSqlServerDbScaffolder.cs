
namespace cworks.DbTest.ADO
{ 
    public class AdoSqlServerDbScaffolder:SqlServerDbScaffolder
    {
        protected override void InitializeSchemaAndData(IDbTestRunnerConfiguration config, IDbTestRunnerContext context, DbInitializationResult result)
        {
            if (!(config is DbTestRunnerConfiguration adoConfig))
            {
                result.IsSuccessful = false;
                result.Logs.Add($"Failed to initialize schema and database.  Configuration is not of the expected type. Expected: {typeof(DbTestRunnerConfiguration).FullName} actual: {config.GetType().FullName}");
                return;
            }

            var connStr = this.ProduceConnectionString(config, context.DbName);
            if (string.IsNullOrEmpty(connStr))
            {
                result.IsSuccessful = false;
                result.Logs.Add("Failed to initialize schema and database.  Configuration string is empty or null.");
                return;
            }

            result.Logs.Add($"Attempting to initialize DB Schema and Data using connection string: {connStr}");
            result.IsSuccessful = adoConfig.InitializeDbSchemaAndData(connStr,result.WasDatabaseCreated);

        }
    }
}
