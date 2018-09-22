using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerConnection.SQLServerConnection
{
    class SQLServerConnectionService
    {

        SqlConnection _sqlConnection;
        SqlDataReader _sqlDataReader;
        string _connectionString ;

        public SQLServerConnectionService()
        {
            StreamReader sr = new StreamReader("connectionStream.txt");
            string connectionStream = sr.ReadLine();
            _connectionString = connectionStream;
            sr.Close();
        }
        public void CreateInstance(string CreateInstanceCommand)
        {

            Console.WriteLine(_connectionString);
            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                
                using (SqlCommand sqlCommand = new SqlCommand(CreateInstanceCommand, _sqlConnection))
                {
                    try
                    {
                        sqlCommand.CommandTimeout = 0;

                        Console.WriteLine("SQL Command Start");

                        sqlCommand.ExecuteNonQuery();
                    }
                    catch(Exception AddDataException)
                    {
                        //Console.WriteLine(AddDataException);

                        if (AddDataException.Message.Contains("違反 PRIMARY KEY 條件約束 "))
                            return;

                        Console.WriteLine(CreateInstanceCommand);
                        throw (new Exception(AddDataException.ToString()));
                    }
                }

                _sqlConnection.Close();
            }
        }

        public void GetInstance(string GetInstanceCommand)
        {

            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(GetInstanceCommand, _sqlConnection))
                {
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while ((sqlDataReader.Read()))
                        {

                            if (!sqlDataReader[0].Equals(DBNull.Value))
                            {


                                Console.WriteLine(sqlDataReader[0].ToString() + "\t" + sqlDataReader[1].ToString() + "\t" + sqlDataReader[2].ToString() + "\t" + sqlDataReader[3].ToString());

                            }

                        }
                    }
                }

                _sqlConnection.Close();
            }
        }
    }
}
