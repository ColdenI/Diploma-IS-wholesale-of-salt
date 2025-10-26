using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_ProvidedServices
    {
        public int ID;
        public int ClientID;
        public int EmployeeID;
        public int ServiceID;
        public DateTime ServiceDateTime;
        public string Status;


        public static List<DBT_ProvidedServices> GetAll()
        {
            var objs = new List<DBT_ProvidedServices>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM ProvidedServices";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_ProvidedServices();

                                obj.ID = reader.GetInt32(0);
                                obj.ClientID = reader.GetInt32(1);
                                obj.EmployeeID = reader.GetInt32(2);
                                obj.ServiceID = reader.GetInt32(3);
                                obj.ServiceDateTime = DateTime.Parse(reader.GetValue(4).ToString());
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
        public static DBT_ProvidedServices GetById(int id)
        {
            var obj = new DBT_ProvidedServices();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM ProvidedServices WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.ClientID = reader.GetInt32(1);
                                obj.EmployeeID = reader.GetInt32(2);
                                obj.ServiceID = reader.GetInt32(3);
                                obj.ServiceDateTime = DateTime.Parse(reader.GetValue(4).ToString());
                                obj.Status = reader.GetString(5);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_ProvidedServices obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO ProvidedServices VALUES (@ClientID, @EmployeeID, @ServiceID, @ServiceDateTime, @Status);";
                        query.Parameters.AddWithValue("@ClientID", obj.ClientID);
                        query.Parameters.AddWithValue("@EmployeeID", obj.EmployeeID);
                        query.Parameters.AddWithValue("@ServiceID", obj.ServiceID);
                        query.Parameters.AddWithValue("@ServiceDateTime", obj.ServiceDateTime);
                        query.Parameters.AddWithValue("@Status", obj.Status);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_ProvidedServices obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE ProvidedServices SET ClientID = @ClientID, EmployeeID = @EmployeeID, ServiceID = @ServiceID, ServiceDateTime = @ServiceDateTime, Status = @Status WHERE ID = @id;";
                        query.Parameters.AddWithValue("@ClientID", obj.ClientID);
                        query.Parameters.AddWithValue("@EmployeeID", obj.EmployeeID);
                        query.Parameters.AddWithValue("@ServiceID", obj.ServiceID);
                        query.Parameters.AddWithValue("@ServiceDateTime", obj.ServiceDateTime);
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
                        query.CommandText = "DELETE FROM ProvidedServices WHERE ID = @id;";
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
