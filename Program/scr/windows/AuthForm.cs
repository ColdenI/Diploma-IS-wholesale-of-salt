using Microsoft.Data.SqlClient;
using Program.scr.core;
using Program.scr.core.dbt;
using Program.scr.windows;

namespace Program
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
                {
                    connection.Open();
                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT COUNT(*) FROM Auth WHERE Login = @login AND PasswordHash = @password;";
                        query.Parameters.AddWithValue("@login", textBox_login.Text);
                        query.Parameters.AddWithValue("@password", textBox_password.Text);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt32(0) == 0)
                                {
                                    MessageBox.Show("Не верный логин или пароль!");
                                    return;
                                }
                            }
                        }
                    }

                    using (var query = connection.CreateCommand())
                    {
                        query.CommandText = "SELECT Employees.ID FROM Employees WHERE Employees.ID = (SELECT Auth.EmployeeID FROM Auth WHERE Login = @login AND PasswordHash = @password);";
                        query.Parameters.AddWithValue("@login", textBox_login.Text);
                        query.Parameters.AddWithValue("@password", textBox_password.Text);
                        using (var reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0))
                                {
                                    MessageBox.Show("Ошибка авторизации");
                                    return;
                                }
                                else
                                {
                                    Core.ThisEmployee = DBT_Employees.GetById(reader.GetInt32(0));
                                    Core.AccessLevel = DBT_Auth.GetById(reader.GetInt32(0)).AccessLevel;
                                    if (Core.AccessLevel == -1)
                                    {
                                        MessageBox.Show("Пользователь не имеет доступа");
                                        return;
                                    }
                                    if (Core.ThisEmployee == null)
                                    {
                                        MessageBox.Show("Ошибка авторизации");
                                        return;
                                    }
                                    else
                                    {
                                        this.Hide();
                                        new MainForm().ShowDialog();
                                        this.Show();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка авторизации");
                return;
            }
        }
    }
}
