using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Services
    {
        public int ID;
        public string Name;
        public string? Description;
        public decimal Cost;


        public static List<DBT_Services> GetAll()
        {
            var objs = new List<DBT_Services>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Services";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Services();

                                obj.ID = reader.GetInt32(0);
                                obj.Name = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.Description = string.Empty;
                                else obj.Description = reader.GetString(2);
                                obj.Cost = reader.GetDecimal(3);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Services GetById(int id)
        {
            var obj = new DBT_Services();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Services WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.Name = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.Description = string.Empty;
                                else obj.Description = reader.GetString(2);
                                obj.Cost = reader.GetDecimal(3);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Services obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Services VALUES (@Name, @Description, @Cost);";
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@Description", obj.Description);
                        query.Parameters.AddWithValue("@Cost", obj.Cost);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Services obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Services SET Name = @Name, Description = @Description, Cost = @Cost WHERE ID = @id;";
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@Description", obj.Description);
                        query.Parameters.AddWithValue("@Cost", obj.Cost);
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
                        query.CommandText = "DELETE FROM Services WHERE ID = @id;";
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
