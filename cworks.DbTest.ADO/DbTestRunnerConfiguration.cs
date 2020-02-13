using System;
using Microsoft.Extensions.Configuration;

namespace cworks.DbTest.ADO
{
    public class DbTestRunnerConfiguration:DbTestRunnerConfigurationBase
    {
        public Func<string,bool, bool> InitializeDbSchemaAndData { get; private set; }
     

        public void SetDbInitializeFunction(Func<string,bool, bool> func)
        {
            this.InitializeDbSchemaAndData = func;
        }

        public DbTestRunnerConfiguration(string server) : base(server)
        {
        }

        public DbTestRunnerConfiguration(string server, string username, string password) : base(server, username, password)
        {
        }

        public DbTestRunnerConfiguration()
        {
            
        }

        public override void Validate()
        {
            base.Validate();
            if (this.InitializeDbSchemaAndData == null) 
                throw new DbTestSetupException("InitializeDbSchemaAndData function is not set.");
        }

        public static DbTestRunnerConfiguration ProduceDefaultConfiguration(IConfiguration systemConfig, Func<string,bool, bool> initializeDbFunc)
        {
            var config= ProduceTestConfiguration<DbTestRunnerConfiguration>(systemConfig);
            config.SetDbInitializeFunction(initializeDbFunc);
            return config;
        }
        

    }
}
