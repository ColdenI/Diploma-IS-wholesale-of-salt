using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core.dbt
{
    public class DBT_Suppliers
    {
        public int ID;
        public string CompanyName;
        public string? ContactPerson;
        public string? Phone;
        public string? Email;
        public string? Address;


        public static List<DBT_Suppliers> GetAll()
        {
            var objs = new List<DBT_Suppliers>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Suppliers";
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new DBT_Suppliers();

                                obj.ID = reader.GetInt32(0);
                                obj.CompanyName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.ContactPerson = string.Empty;
                                else obj.ContactPerson = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);
                                if (reader.IsDBNull(5)) obj.Address = string.Empty;
                                else obj.Address = reader.GetString(5);

                                objs.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { objs = null; }
            return objs;
        }
        public static DBT_Suppliers GetById(int id)
        {
            var obj = new DBT_Suppliers();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT * FROM Suppliers WHERE ID = @id";
                        query.Parameters.AddWithValue("@id", id);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj.ID = reader.GetInt32(0);
                                obj.CompanyName = reader.GetString(1);
                                if (reader.IsDBNull(2)) obj.ContactPerson = string.Empty;
                                else obj.ContactPerson = reader.GetString(2);
                                if (reader.IsDBNull(3)) obj.Phone = string.Empty;
                                else obj.Phone = reader.GetString(3);
                                if (reader.IsDBNull(4)) obj.Email = string.Empty;
                                else obj.Email = reader.GetString(4);
                                if (reader.IsDBNull(5)) obj.Address = string.Empty;
                                else obj.Address = reader.GetString(5);
                            }
                        }
                    }
                }
            }
            catch { obj = null; }
            return obj;
        }

        public static int Create(DBT_Suppliers obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "INSERT INTO Suppliers VALUES (@CompanyName, @ContactPerson, @Phone, @Email, @Address);";
                        query.Parameters.AddWithValue("@CompanyName", obj.CompanyName);
                        query.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                        query.Parameters.AddWithValue("@Phone", obj.Phone);
                        query.Parameters.AddWithValue("@Email", obj.Email);
                        query.Parameters.AddWithValue("@Address", obj.Address);
                        query.ExecuteNonQuery();
                    }
                }
            }
            catch { return -1; }
            return 0;
        }

        public static int Edit(DBT_Suppliers obj)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "UPDATE Suppliers SET CompanyName = @CompanyName, ContactPerson = @ContactPerson, Phone = @Phone, Email = @Email, Address = @Address WHERE ID = @id;";
                        query.Parameters.AddWithValue("@CompanyName", obj.CompanyName);
                        query.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                        query.Parameters.AddWithValue("@Phone", obj.Phone);
                        query.Parameters.AddWithValue("@Email", obj.Email);
                        query.Parameters.AddWithValue("@Address", obj.Address);
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
                        query.CommandText = "DELETE FROM Suppliers WHERE ID = @id;";
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
