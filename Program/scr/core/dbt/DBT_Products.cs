using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Products
    {
        public int ID;
        public int SupplierID;
        public string Name;
        public string? Category;
        public string? UnitOfMeasure;
        public decimal PricePerUnit;
        public string? Description;


        public static List<DBT_Products> GetAll()
        {
            var objs = new List<DBT_Products>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Products";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Products();

                                obj.ID = reader.GetInt32(0);
                                obj.SupplierID = reader.GetInt32(1);
                                obj.Name = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Category = string.Empty;
                                else obj.Category = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.UnitOfMeasure = string.Empty;
                                else obj.UnitOfMeasure = reader.GetString(4);
                                obj.PricePerUnit = reader.GetDecimal(5);
                                if (reader.IsDBNull(6)) obj.Description = string.Empty;
                                else obj.Description = reader.GetString(6);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Products GetById(int id)
        {
            var obj = new DBT_Products();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Products WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.SupplierID = reader.GetInt32(1);
                                obj.Name = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Category = string.Empty;
                                else obj.Category = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.UnitOfMeasure = string.Empty;
                                else obj.UnitOfMeasure = reader.GetString(4);
                                obj.PricePerUnit = reader.GetDecimal(5);
                                if (reader.IsDBNull(6)) obj.Description = string.Empty;
                                else obj.Description = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Products obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Products VALUES (@SupplierID, @Name, @Category, @UnitOfMeasure, @PricePerUnit, @Description);";
                        query.Parameters.AddWithValue("@SupplierID", obj.SupplierID);
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@Category", obj.Category);
                        query.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        query.Parameters.AddWithValue("@PricePerUnit", obj.PricePerUnit);
                        query.Parameters.AddWithValue("@Description", obj.Description);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Products obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Products SET SupplierID = @SupplierID, Name = @Name, Category = @Category, UnitOfMeasure = @UnitOfMeasure, PricePerUnit = @PricePerUnit, Description = @Description WHERE ID = @id;";
                        query.Parameters.AddWithValue("@SupplierID", obj.SupplierID);
                        query.Parameters.AddWithValue("@Name", obj.Name);
                        query.Parameters.AddWithValue("@Category", obj.Category);
                        query.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        query.Parameters.AddWithValue("@PricePerUnit", obj.PricePerUnit);
                        query.Parameters.AddWithValue("@Description", obj.Description);
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
                        query.CommandText = "DELETE FROM Products WHERE ID = @id;";
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
