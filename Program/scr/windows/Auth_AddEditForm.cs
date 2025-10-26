using Program.scr.core.dbt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.scr.windows
{
    public partial class Auth_AddEditForm : Form
    {
        DBT_Auth Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_Login;
        TextBox textBox_PasswordHash;
        TextBox textBox_AccessLevel;

        public Auth_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Auth_AddEditForm(DBT_Auth obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_Login.Text = obj.Login.ToString();
            textBox_PasswordHash.Text = obj.PasswordHash.ToString();
            textBox_AccessLevel.Text = obj.AccessLevel.ToString();
        }

        private void Init()
        {
            this.Text = "Авторизация - " + Object == null ? "Добавить" : "Изменить";
            this.MinimumSize = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            button_apply = new Button()
            {
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_apply.Click += Button_apply_Click;
            button_apply.Text = Object == null ? "Добавить" : "Изменить";
            this.Controls.Add(button_apply);

            tableLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3
            };

            Label label_Login = new Label();
            SetLabel(ref label_Login, "Логин");
            tableLayout.Controls.Add(label_Login, 0, 0);
            textBox_Login = new TextBox();
            textBox_Login.Dock = DockStyle.Fill;
            textBox_Login.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Login, 1, 0);

            Label label_PasswordHash = new Label();
            SetLabel(ref label_PasswordHash, "Пароль");
            tableLayout.Controls.Add(label_PasswordHash, 0, 1);
            textBox_PasswordHash = new TextBox();
            textBox_PasswordHash.Dock = DockStyle.Fill;
            textBox_PasswordHash.MaxLength = 254;
            tableLayout.Controls.Add(textBox_PasswordHash, 1, 1);

            Label label_AccessLevel = new Label();
            SetLabel(ref label_AccessLevel, "Уровень доступа");
            tableLayout.Controls.Add(label_AccessLevel, 0, 2);
            textBox_AccessLevel = new TextBox();
            textBox_AccessLevel.Dock = DockStyle.Fill;
            textBox_AccessLevel.MaxLength = 254;
            tableLayout.Controls.Add(textBox_AccessLevel, 1, 2);

            this.Controls.Add(tableLayout);
        }

        private void SetLabel(ref Label label, string text = "")
        {
            label.Font = new Font(Font.FontFamily, 12);
            label.TextAlign = ContentAlignment.TopLeft;
            label.Dock = DockStyle.Fill;
            label.AutoSize = false;
            label.Width = 200;
            label.Text = text;
        }

        private void Button_apply_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_Login.Text)) { MessageBox.Show("Поле 'Логин' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_PasswordHash.Text)) { MessageBox.Show("Поле 'Пароль' имеет некорректное значение!"); return; }
            if (!int.TryParse(textBox_AccessLevel.Text, out int tp_AccessLevel)) { MessageBox.Show("Поле 'Уровень доступа' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Auth.Create(
                    new DBT_Auth()
                    {
                        Login = textBox_Login.Text,
                        PasswordHash = textBox_PasswordHash.Text,
                        AccessLevel = int.Parse(textBox_AccessLevel.Text)
                    }
                );
            }
            else
            {
                res = DBT_Auth.Edit(
                    new DBT_Auth()
                    {
                        EmployeeID = Object.EmployeeID,
                        Login = textBox_Login.Text,
                        PasswordHash = textBox_PasswordHash.Text,
                        AccessLevel = int.Parse(textBox_AccessLevel.Text)
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
