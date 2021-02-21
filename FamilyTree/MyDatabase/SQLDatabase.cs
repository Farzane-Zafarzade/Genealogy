namespace Genealogy.MyDatabase
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System;
    using System.Collections;

    internal class SQLDatabase
    {
        public static string databaseName { get; set; } = "mmmmmmmmmmm";

        public static string connectionStr { get; set; } = @"Data Source =.\SQLExpress;Integrated Security = true; database={0}";


        public void CreateDatabase()
        {
            string commandStr = string.Format("CREATE DATABASE {0};", databaseName);
            string connectionString = string.Format(connectionStr, "");
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(commandStr, cnn);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                }
            }
        }


        public static void ExecuteSQL(string commandString)
        {
            string connectionString = string.Format(connectionStr, databaseName);
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(commandString, cnn);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                }
            }
        }



        public static void ExecuteSQL(string commandString, string[] paramNames, ArrayList paramValues)
        {
            string connectionString = string.Format(connectionStr, databaseName);
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                using (SqlCommand command = new SqlCommand(commandString, cnn))
                {
                    for (int i = 0; i < paramNames.Length; i++)
                    {
                        command.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                    }
                    command.ExecuteNonQuery();

                }
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                }
            }
        }


        public DataTable Read(string sqlCommand, string[] paramNames, ArrayList paramValues)
        {
            var dt = new DataTable();
            string connectionString = string.Format(connectionStr, databaseName);
            using (var cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                using (var command = new SqlCommand(sqlCommand, cnn))
                {
                    for (int i = 0; i < paramNames.Length; i++)
                    {
                        command.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                    }
                    command.ExecuteNonQuery();

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
                cnn.Close();
            }
            return dt;
        }



        public bool DoesDatabaseExists(string databaseName)
        {
            SqlDataReader rdr = null;
            string conString = string.Format(connectionStr, "");
            var conn = new SqlConnection(conString);
            try
            {
                // Open the connection
                conn.Open();

                string sqlString = string.Format(@"SELECT name FROM sys.databases WHERE name='{0}';", databaseName);
                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // Check
                if (rdr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public bool TableExists(string tableName)
        {
            SqlDataReader rdr = null;
            string conString = string.Format(connectionStr, databaseName);
            var conn = new SqlConnection(conString);
            try
            {
                // Open the connection
                conn.Open();

                string sqlString = string.Format(@"SELECT name FROM sys.tables WHERE name='{0}';", tableName);
                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // Check
                if (rdr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }


    }
}
