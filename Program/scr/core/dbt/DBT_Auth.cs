using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Auth
    {
        public int EmployeeID;
        public string Login;
        public string PasswordHash;
        public int AccessLevel;


        public static List<DBT_Auth> GetAll()
        {
            var objs = new List<DBT_Auth>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Auth";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Auth();

                                obj.EmployeeID = reader.GetInt32(0);
                                obj.Login = reader.GetString(1);
                                obj.PasswordHash = reader.GetString(2);
                                obj.AccessLevel = reader.GetInt32(3);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Auth GetById(int id)
        {
            var obj = new DBT_Auth();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Auth WHERE EmployeeID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.EmployeeID = reader.GetInt32(0);
                                obj.Login = reader.GetString(1);
                                obj.PasswordHash = reader.GetString(2);
                                obj.AccessLevel = reader.GetInt32(3);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Auth obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Auth VALUES (@Login, @PasswordHash, @AccessLevel);";
                        query.Parameters.AddWithValue("@Login", obj.Login);
                        query.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
                        query.Parameters.AddWithValue("@AccessLevel", obj.AccessLevel);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Auth obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Auth SET Login = @Login, PasswordHash = @PasswordHash, AccessLevel = @AccessLevel WHERE EmployeeID = @id;";
                        query.Parameters.AddWithValue("@Login", obj.Login);
                        query.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
                        query.Parameters.AddWithValue("@AccessLevel", obj.AccessLevel);
                        query.Parameters.AddWithValue("@id", obj.EmployeeID);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Remove(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "DELETE FROM Auth WHERE EmployeeID = @id;";
                        query.Parameters.AddWithValue("@id", id);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

    }
}
