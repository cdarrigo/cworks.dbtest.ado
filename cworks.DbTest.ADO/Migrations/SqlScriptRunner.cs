using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace cworks.DbTest.ADO.Migrations
{
    public static class SqlScriptRunner
    {
        public class RunScriptResult
        {
            public string[] Errors { get; set; } = new string[] { };
            public bool WasSuccessful => !Errors.Any();
        }
        public  static RunScriptResult RunScript(string script, string connectionString)
        {
            var runErrors = new List<string>();

            using (TextReader reader = new StringReader(script))
            {
                TSqlParser parser = new TSql110Parser(true);
                TSqlFragment fragment = parser.Parse(reader, out var errors);
                if (errors != null && errors.Count > 0)
                {
                    foreach (ParseError error in errors)
                    {
                        runErrors.Add($"Line: {error.Line}, Column: {error.Column}: {error.Message}");

                    }

                    return new RunScriptResult {Errors = runErrors.ToArray()};
                }

                SqlScriptGenerator sqlScriptGenerator = new Sql110ScriptGenerator();
                if (!(fragment is TSqlScript sqlScript))
                {
                    sqlScriptGenerator.GenerateScript(fragment, out var sql);
                    if (!ExecuteSql(sql, connectionString, runErrors))
                    {
                        return new RunScriptResult {Errors = runErrors.ToArray()};
                    }
                }
                else
                {
                    foreach (var sqlBatch in sqlScript.Batches)
                    {
                        sqlScriptGenerator.GenerateScript(sqlBatch, out var sql);
                        if (!ExecuteSql(sql, connectionString, runErrors))
                            return new RunScriptResult {Errors = runErrors.ToArray()};
                    }
                }
            }

            return new RunScriptResult();
        }

        private static bool ExecuteSql(string sql, string connectionString, List<string>  errors)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        connection.Open();
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                errors.Add($"Error executing sql: {sql}. Error: {e}");
                return false;
            }

            return true;
        }
    }
}
