using Microsoft.Data.SqlClient;

namespace Program.scr.core.dbt
{
    public class DBT_OrderItems
    {
        public int ID;
        public int OrderID;
        public int ProductID;
        public decimal Quantity;
        public decimal PriceAtOrderTime;
        public decimal? Subtotal;


        public static List<DBT_OrderItems> GetByOrderID(int OrderID)
        {
            var objs = new List<DBT_OrderItems>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM OrderItems WHERE OrderID = @id";
                        query.Parameters.AddWithValue("@id", OrderID);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_OrderItems();

                                obj.ID = reader.GetInt32(0);
                                obj.OrderID = reader.GetInt32(1);
                                obj.ProductID = reader.GetInt32(2);
                                obj.Quantity = reader.GetDecimal(3);
                                obj.PriceAtOrderTime = reader.GetDecimal(4);
                                if (reader.IsDBNull(5)) obj.Subtotal = null;
                                else obj.Subtotal = reader.GetDecimal(5);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static List<DBT_OrderItems> GetByOrderIDAndProductID(int OrderID, int ProductID)
        {
            var objs = new List<DBT_OrderItems>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM OrderItems WHERE OrderID = @id and ProductID = @pr_id";
                        query.Parameters.AddWithValue("@id", OrderID);
                        query.Parameters.AddWithValue("@pr_id", ProductID);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_OrderItems();

                                obj.ID = reader.GetInt32(0);
                                obj.OrderID = reader.GetInt32(1);
                                obj.ProductID = reader.GetInt32(2);
                                obj.Quantity = reader.GetDecimal(3);
                                obj.PriceAtOrderTime = reader.GetDecimal(4);
                                if (reader.IsDBNull(5)) obj.Subtotal = null;
                                else obj.Subtotal = reader.GetDecimal(5);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_OrderItems GetById(int id)
        {
            var obj = new DBT_OrderItems();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM OrderItems WHERE ID = @id";
                        
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.OrderID = reader.GetInt32(1);
                                obj.ProductID = reader.GetInt32(2);
                                obj.Quantity = reader.GetDecimal(3);
                                obj.PriceAtOrderTime = reader.GetDecimal(4);
                                if (reader.IsDBNull(5)) obj.Subtotal = null;
                                else obj.Subtotal = reader.GetDecimal(5);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_OrderItems obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO OrderItems VALUES (@OrderID, @ProductID, @Quantity, @PriceAtOrderTime);";
                        query.Parameters.AddWithValue("@OrderID", obj.OrderID);
                        query.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        query.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        query.Parameters.AddWithValue("@PriceAtOrderTime", obj.PriceAtOrderTime);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_OrderItems obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE OrderItems SET OrderID = @OrderID, ProductID = @ProductID, Quantity = @Quantity, PriceAtOrderTime = @PriceAtOrderTime WHERE ID = @id;";
                        query.Parameters.AddWithValue("@OrderID", obj.OrderID);
                        query.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        query.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        query.Parameters.AddWithValue("@PriceAtOrderTime", obj.PriceAtOrderTime);
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
                        query.CommandText = "DELETE FROM OrderItems WHERE ID = @id;";
                        query.Parameters.AddWithValue("@id", id);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int RemoveByOrderId(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "DELETE FROM OrderItems WHERE OrderID = @id;";
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
