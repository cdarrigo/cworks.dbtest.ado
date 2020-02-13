using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace cworks.DbTest.ADO
{
   

    public abstract partial class DbTestSetup
    {

        [Collection(DbTestConstants.CollectionName)]
        public abstract class DbTest : DbTestBase
        {
            protected DbTest(ITestOutputHelper outputHelper, DbTestFixture testFixture) : base(outputHelper, testFixture)
            {
            }

            protected override SqlRequest OnArrange(IDbTestRunnerContext context, IDisposable dbHandle, ITestOutputHelper testOutputHelper)
            {
                return Arrange(context, ToSqlConnection(dbHandle), testOutputHelper);
            }

            protected override SqlRequest OnAct(IDbTestRunnerContext context, IDisposable dbHandle, ITestOutputHelper testOutputHelper)
            {
                return Act(context, ToSqlConnection(dbHandle), testOutputHelper);
            }

            protected override void OnAssert(DataTable data, IDisposable dbHandle, DataTable[] allData, ITestOutputHelper testOutputHelper)
            {
                AssertState(data, ToSqlConnection(dbHandle), allData, testOutputHelper);
            }

            private SqlConnection ToSqlConnection(IDisposable dbHandle)
            {
                return (SqlConnection) dbHandle;
            }

            protected override IDisposable ProduceDbHandle()
            {
                return GetDbConnection();
            }

            /// <summary>
            /// Return the sql text to execute as part of the ARRANGE phase of the test.
            /// Or you may modify the data using the provided connection
            /// </summary>
            /// <returns>the sql text to execute</returns>

            protected abstract SqlRequest Arrange(IDbTestRunnerContext context, SqlConnection connection, ITestOutputHelper testOutputHelper);



            /// <summary>
            /// Return the sql text to execute as part of the ACT phase of the test.
            /// Or you may modify the data using the provided connection
            /// </summary>
            /// <returns>the sql text to execute</returns>
            protected abstract SqlRequest Act(IDbTestRunnerContext context, SqlConnection connection, ITestOutputHelper testOutputHelper);

            /// <summary>
            /// Write your assert statements here.
            /// You can reference the data directly via the provided connection or examine the data tables populated from the ACT method.
            /// </summary>
            /// <param name="data">First data table read</param>
            /// <param name="connection">Sql Connection to the test db</param>
            /// <param name="allData">All the data table read</param>
            /// <param name="testOutputHelper">Test Output Helper. Use this to write messages to the test output.</param>
            protected abstract void AssertState(DataTable data, SqlConnection connection, DataTable[] allData, ITestOutputHelper testOutputHelper);

            private SqlConnection sqlConnection;

            protected override SqlConnection GetDbConnection()
            {
                if (sqlConnection == null)
                {
                    sqlConnection = new SqlConnection(this.TestFixture.Context.ConnectionString);
                }

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                return sqlConnection;

            }
        }
    }
}
