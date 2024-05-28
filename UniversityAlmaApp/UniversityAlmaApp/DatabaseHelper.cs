using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UniversityAlmaApp
{
    internal class DatabaseHelper
    {
        private static string connectionString = @"Server=tcp:mednat.ieeta.pt\SQLSERVER,8101;Database=p11g8;User Id=p11g8;Password=@ILtq123;";

        public static int ExecuteScalar(string storedProcedure, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static bool ExecuteLogin(string storedProcedure, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    return result != DBNull.Value && result != null;
                }
            }
        }

        public static int RegisterUser(string storedProcedure, SqlParameter[] parameters, out string message)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (parameters != null )
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    // Output params
                    SqlParameter userIdParam = new SqlParameter("@UserId", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    SqlParameter returnCodeParam = new SqlParameter("@ReturnCode", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(userIdParam);
                    cmd.Parameters.Add(returnCodeParam);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    int userId = userIdParam.Value != DBNull.Value ? (int)userIdParam.Value : -1;
                    int returnCode = (int)returnCodeParam.Value;

                    //Check result and set the message
                    switch (returnCode)
                    {
                        case 0:
                            message = "Registration successful!";
                            return userId;
                        case -1:
                            message = "Username already exists.";
                            return -1;
                        case -2:
                            message = "Email already exists.";
                            return -2;
                        default:
                            message = "An unknown error occured.";
                            return -3;
                    }
                }
            }
        }
    }
}
