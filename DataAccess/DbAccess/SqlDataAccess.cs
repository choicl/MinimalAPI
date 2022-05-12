using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DbAccess
{
    /// <summary>
    /// Provides access to database using Dapper.
    /// </summary>
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        //load data from stored procedure
        //connecting sql-executing query-returning IEnumerable
        public async Task<IEnumerable<T>> LoadData<T, U>(
            string storedProcedure,
            U parameters,
            string connectionId = "Default")
        {
            //creating a connection to the database
            //"using" suppose to close a connection to database
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

            //async talk to out connection(sql server)
            //force it to execute stored procedure passed in method
            //commandtype means it's not just the text we're executing(stored procedure)
            return await connection.QueryAsync<T>(storedProcedure,
                                                  parameters,
                                                  commandType: CommandType.StoredProcedure);
        }

        //it's also loading data,but without returning the value 
        public async Task SaveData<T>(
            string storedProcedure,
            T parameters,
            string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

            await connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
