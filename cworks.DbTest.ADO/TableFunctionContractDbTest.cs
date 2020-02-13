using System.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace cworks.DbTest.ADO
{
    public abstract partial class DbTestSetup
    {

        [Collection(DbTestConstants.CollectionName)]
        public abstract class TableFunctionContractDbTest: ContractDbTestBase
        {
            protected TableFunctionContractDbTest(ITestOutputHelper outputHelper, DbTestFixture testFixture) : base(outputHelper, testFixture)
            {
            }

            protected override SqlRequest Act(IDbTestRunnerContext context, SqlConnection connection, ITestOutputHelper testOutputHelper)
            {
                var functionParameters = GetParametersForDbObject(this.FunctionName, context );
                return this.InvokeTableFunction(this.FunctionName, this.SchemaName, functionParameters);
            }

            protected abstract string FunctionName { get; }
            protected override string DbObjectName => FunctionName;
            protected override string DbObjectType => "Function";

        }

    }
}