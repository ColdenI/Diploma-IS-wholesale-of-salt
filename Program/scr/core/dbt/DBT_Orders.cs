using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Orders
    {
        public int ID;
        public int ClientID;
        public int EmployeeID;
        public DateTime OrderDateTime;
        public decimal TotalAmount;
        public string Status;


        public static List<DBT_Orders> GetAll()
        {
            var objs = new List<DBT_Orders>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Orders";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Orders();

                                obj.ID = reader.GetInt32(0);
                                obj.ClientID = reader.GetInt32(1);
                                obj.EmployeeID = reader.GetInt32(2);
                                obj.OrderDateTime = DateTime.Parse(reader.GetValue(3).ToString());
                                obj.TotalAmount = reader.GetDecimal(4);
                                obj.Status = reader.GetString(5);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Orders GetById(int id)
        {
            var obj = new DBT_Orders();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Orders WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.ClientID = reader.GetInt32(1);
                                obj.EmployeeID = reader.GetInt32(2);
                                obj.OrderDateTime = DateTime.Parse(reader.GetValue(3).ToString());
                                obj.TotalAmount = reader.GetDecimal(4);
                                obj.Status = reader.GetString(5);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Orders obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Orders VALUES (@ClientID, @EmployeeID, @OrderDateTime, @TotalAmount, @Status);";
                        query.Parameters.AddWithValue("@ClientID", obj.ClientID);
                        query.Parameters.AddWithValue("@EmployeeID", obj.EmployeeID);
                        query.Parameters.AddWithValue("@OrderDateTime", obj.OrderDateTime);
                        query.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                        query.Parameters.AddWithValue("@Status", obj.Status);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Orders obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Orders SET ClientID = @ClientID, EmployeeID = @EmployeeID, OrderDateTime = @OrderDateTime, TotalAmount = @TotalAmount, Status = @Status WHERE ID = @id;";
                        query.Parameters.AddWithValue("@ClientID", obj.ClientID);
                        query.Parameters.AddWithValue("@EmployeeID", obj.EmployeeID);
                        query.Parameters.AddWithValue("@OrderDateTime", obj.OrderDateTime);
                        query.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                        query.Parameters.AddWithValue("@Status", obj.Status);
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
                        query.CommandText = "DELETE FROM Orders WHERE ID = @id;";
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
