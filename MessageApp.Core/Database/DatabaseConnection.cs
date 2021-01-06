using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ContactSystem.Core.Database
{
    public class DatabaseConnection
    {
        private string connectionString = @"Data Source=DESKTOP-6O7E1SM; Database=ContactSystem; User Id=ContactApplication; Password=ContactApplication; Trusted_Connection=True";

        public SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public void CloseConnection(SqlConnection conn)
        {
            // if the connection is open, close the connection
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
