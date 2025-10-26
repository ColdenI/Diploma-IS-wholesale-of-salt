using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Stock
    {
        public int ID;
        public int ProductID;
        public decimal QuantityOnStock;
        public DateTime? LastUpdated;


        public static List<DBT_Stock> GetAll()
        {
            var objs = new List<DBT_Stock>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Stock";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Stock();

                                obj.ID = reader.GetInt32(0);
                                obj.ProductID = reader.GetInt32(1);
                                obj.QuantityOnStock = reader.GetDecimal(2);
                                if (reader.IsDBNull(3)) obj.LastUpdated = null;
                                else obj.LastUpdated = DateTime.Parse(reader.GetValue(3).ToString());

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Stock GetById(int id)
        {
            var obj = new DBT_Stock();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Stock WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.ProductID = reader.GetInt32(1);
                                obj.QuantityOnStock = reader.GetDecimal(2);
                                if (reader.IsDBNull(3)) obj.LastUpdated = null;
                                else obj.LastUpdated = DateTime.Parse(reader.GetValue(3).ToString());
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }
        public static DBT_Stock GetByProductID(int ProductID)
        {
            var obj = new DBT_Stock();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Stock WHERE ProductID = @ProductID";
                        query.Parameters.AddWithValue("@ProductID", ProductID);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.ProductID = reader.GetInt32(1);
                                obj.QuantityOnStock = reader.GetDecimal(2);
                                if (reader.IsDBNull(3)) obj.LastUpdated = null;
                                else obj.LastUpdated = DateTime.Parse(reader.GetValue(3).ToString());
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Stock obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Stock VALUES (@ProductID, @QuantityOnStock, @LastUpdated);";
                        query.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        query.Parameters.AddWithValue("@QuantityOnStock", obj.QuantityOnStock);
                        query.Parameters.AddWithValue("@LastUpdated", obj.LastUpdated);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Stock obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Stock SET ProductID = @ProductID, QuantityOnStock = @QuantityOnStock, LastUpdated = @LastUpdated WHERE ID = @id;";
                        query.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        query.Parameters.AddWithValue("@QuantityOnStock", obj.QuantityOnStock);
                        query.Parameters.AddWithValue("@LastUpdated", obj.LastUpdated);
                        query.Parameters.AddWithValue("@id", obj.ID);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int DebitFromWarehouse(int ID, decimal value)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Stock SET QuantityOnStock = QuantityOnStock - @QuantityOnStock, LastUpdated = @LastUpdated WHERE ProductID = @id;";
                        query.Parameters.AddWithValue("@QuantityOnStock", value);
                        query.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                        query.Parameters.AddWithValue("@id", ID);
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
                        query.CommandText = "DELETE FROM Stock WHERE ID = @id;";
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
