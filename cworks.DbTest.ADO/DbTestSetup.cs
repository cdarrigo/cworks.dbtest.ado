using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace cworks.DbTest.ADO
{
    public abstract partial class DbTestSetup:DbTest.DbTestSetup
    {
        /// <summary>
        /// Create connections and commands with the specified connection string
        /// to create whatever db objects and seed data is required before
        /// running any tests
        /// </summary>
        /// <param name="connectionString">connection string to test db</param>
        /// <param name="isNewDb">True if this is a new db we just created, otherwise false</param>
        /// <returns>true if the setup succeeded and its safe to run the Db Tests, otherwise false</returns>
        protected abstract bool InitializeDbSchemaAndData(string connectionString, bool isNewDb);

        //
        // ADO
        //


        protected override IDbTestRunnerConfiguration ProduceDefaultConfiguration(IConfiguration systemConfig)
        {
            var config = DbTestRunnerConfiguration.ProduceDefaultConfiguration(systemConfig, InitializeDbSchemaAndData);
            return config;
        }

        protected override void AfterDbTestsConfigured(IDbTestRunnerConfiguration config)
        {
            if (config is DbTestRunnerConfiguration adoConfig)
            {
                if (adoConfig.InitializeDbSchemaAndData == null)
                {
                    adoConfig.SetDbInitializeFunction(InitializeDbSchemaAndData);
                }
            }
        }

        protected override IDbScaffolder ProduceScaffolder()
        {
            return new AdoSqlServerDbScaffolder();
        }

        public override void TearDownDatabase(IDbTestRunnerContext context, IDbTestRunnerConfiguration config, bool allTestsWereSuccessful)
        {
            var sqlConnection = new SqlConnection(context.ConnectionString);
            SqlConnection.ClearPool(sqlConnection);

            if ((allTestsWereSuccessful && config.DropDatabaseOnSuccess) ||
                (!allTestsWereSuccessful && config.DropDatabaseOnFailure))
            {
                context.DbScaffolder.DropDatabase(context, config);
            }
        }
    }
}