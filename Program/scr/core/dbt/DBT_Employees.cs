using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Employees
    {
        public int ID;
        public string FullName;
        public string? Position;
        public string? Phone;
        public string? Email;
        public DateTime HireDate;


        public static List<DBT_Employees> GetAll()
        {
            var objs = new List<DBT_Employees>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Employees";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Employees();

                                obj.ID = reader.GetInt32(0);
                                obj.FullName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.Position = string.Empty;
                                else obj.Position = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);
                                obj.HireDate = DateTime.Parse(reader.GetValue(5).ToString());

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Employees GetById(int id)
        {
            var obj = new DBT_Employees();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Employees WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.FullName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.Position = string.Empty;
                                else obj.Position = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);
                                obj.HireDate = DateTime.Parse(reader.GetValue(5).ToString());
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Employees obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Employees VALUES (@FullName, @Position, @Phone, @Email, @HireDate);";
                        query.Parameters.AddWithValue("@FullName", obj.FullName);
                        query.Parameters.AddWithValue("@Position", obj.Position);
                        query.Parameters.AddWithValue("@Phone", obj.Phone);
                        query.Parameters.AddWithValue("@Email", obj.Email);
                        query.Parameters.AddWithValue("@HireDate", obj.HireDate);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Employees obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Employees SET FullName = @FullName, Position = @Position, Phone = @Phone, Email = @Email, HireDate = @HireDate WHERE ID = @id;";
                        query.Parameters.AddWithValue("@FullName", obj.FullName);
                        query.Parameters.AddWithValue("@Position", obj.Position);
                        query.Parameters.AddWithValue("@Phone", obj.Phone);
                        query.Parameters.AddWithValue("@Email", obj.Email);
                        query.Parameters.AddWithValue("@HireDate", obj.HireDate);
                        query.Parameters.AddWithValue("@id", obj.ID);
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
                        query.CommandText = "DELETE FROM Employees WHERE ID = @id;";
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
