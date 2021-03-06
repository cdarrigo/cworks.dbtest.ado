﻿using System.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace cworks.DbTest.ADO
{
    public abstract partial class DbTestSetup
    {

        [Collection(DbTestConstants.CollectionName)]
        public abstract class StoredProcedureContractDbTest:ContractDbTestBase
        {
            protected StoredProcedureContractDbTest(ITestOutputHelper outputHelper, DbTestFixture testFixture) : base(outputHelper, testFixture)
            {
            }

            protected override SqlRequest Act(IDbTestRunnerContext context, SqlConnection connection, ITestOutputHelper testOutputHelper)
            {
                var parameters = this.GetParametersForDbObject(this.StoredProcedureName, context);
                return this.ExecuteStoredProcedure(this.DbObjectName, this.SchemaName, parameters);
            }

            protected abstract string StoredProcedureName { get; }
            protected override string DbObjectName => StoredProcedureName;
            protected override string DbObjectType => "Stored Procedure";
        }
    }
}
