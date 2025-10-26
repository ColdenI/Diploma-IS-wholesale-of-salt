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
    public partial class Orders_AddEditForm : Form
    {
        DBT_Orders Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_ClientID;
        TextBox textBox_EmployeeID;
        DateTimePicker dateTimePicker_OrderDateTime;
        TextBox textBox_TotalAmount;
        TextBox textBox_Status;

        public Orders_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Orders_AddEditForm(DBT_Orders obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_ClientID.Text = obj.ClientID.ToString();
            textBox_EmployeeID.Text = obj.EmployeeID.ToString();
            dateTimePicker_OrderDateTime.Value = (DateTime)((obj.OrderDateTime == null) ? DateTime.Now : obj.OrderDateTime);
            textBox_TotalAmount.Text = obj.TotalAmount.ToString();
            textBox_Status.Text = obj.Status.ToString();
        }

        private void Init()
        {
            this.Text = "Заказы - " + Object == null ? "Добавить" : "Изменить";
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
                RowCount = 5
            };

            Label label_ClientID = new Label();
            SetLabel(ref label_ClientID, "Клиент");
            tableLayout.Controls.Add(label_ClientID, 0, 0);
            textBox_ClientID = new TextBox();
            textBox_ClientID.Dock = DockStyle.Fill;
            textBox_ClientID.MaxLength = 254;
            tableLayout.Controls.Add(textBox_ClientID, 1, 0);

            Label label_EmployeeID = new Label();
            SetLabel(ref label_EmployeeID, "Сотрудник");
            tableLayout.Controls.Add(label_EmployeeID, 0, 1);
            textBox_EmployeeID = new TextBox();
            textBox_EmployeeID.Dock = DockStyle.Fill;
            textBox_EmployeeID.MaxLength = 254;
            tableLayout.Controls.Add(textBox_EmployeeID, 1, 1);

            Label label_OrderDateTime = new Label();
            SetLabel(ref label_OrderDateTime, "Дата заказа");
            tableLayout.Controls.Add(label_OrderDateTime, 0, 2);
            dateTimePicker_OrderDateTime = new DateTimePicker();
            dateTimePicker_OrderDateTime.Dock = DockStyle.Fill;
            tableLayout.Controls.Add(dateTimePicker_OrderDateTime, 1, 2);
            dateTimePicker_OrderDateTime.Format = DateTimePickerFormat.Custom;
            dateTimePicker_OrderDateTime.CustomFormat = "yyyy.MM.dd HH:mm:ss";

            Label label_TotalAmount = new Label();
            SetLabel(ref label_TotalAmount, "Сумма заказа");
            tableLayout.Controls.Add(label_TotalAmount, 0, 3);
            textBox_TotalAmount = new TextBox();
            textBox_TotalAmount.Dock = DockStyle.Fill;
            textBox_TotalAmount.MaxLength = 254;
            tableLayout.Controls.Add(textBox_TotalAmount, 1, 3);

            Label label_Status = new Label();
            SetLabel(ref label_Status, "Статус");
            tableLayout.Controls.Add(label_Status, 0, 4);
            textBox_Status = new TextBox();
            textBox_Status.Dock = DockStyle.Fill;
            textBox_Status.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Status, 1, 4);

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
            if (!int.TryParse(textBox_ClientID.Text, out int tp_ClientID)) { MessageBox.Show("Поле 'Клиент' имеет некорректное значение!"); return; }
            if (!int.TryParse(textBox_EmployeeID.Text, out int tp_EmployeeID)) { MessageBox.Show("Поле 'Сотрудник' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_TotalAmount.Text, out decimal tp_TotalAmount)) { MessageBox.Show("Поле 'Сумма заказа' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_Status.Text)) { MessageBox.Show("Поле 'Статус' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Orders.Create(
                    new DBT_Orders()
                    {
                        ClientID = int.Parse(textBox_ClientID.Text),
                        EmployeeID = int.Parse(textBox_EmployeeID.Text),
                        OrderDateTime = dateTimePicker_OrderDateTime.Value,
                        TotalAmount = decimal.Parse(textBox_TotalAmount.Text),
                        Status = textBox_Status.Text
                    }
                );
            }
            else
            {
                res = DBT_Orders.Edit(
                    new DBT_Orders()
                    {
                        ID = Object.ID,
                        ClientID = int.Parse(textBox_ClientID.Text),
                        EmployeeID = int.Parse(textBox_EmployeeID.Text),
                        OrderDateTime = dateTimePicker_OrderDateTime.Value,
                        TotalAmount = decimal.Parse(textBox_TotalAmount.Text),
                        Status = textBox_Status.Text
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
