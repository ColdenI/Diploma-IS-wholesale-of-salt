using Microsoft.Data.SqlClient;
using Program.scr.core;
using Program.scr.core.dbt;

namespace Program.scr.windows
{
    public partial class Orders_ViewForm : Form
    {
        DataGridView dataGridView;
        TextBox textBox_search;
        Button button_edit;
        Button button_update;
        Button button_create;
        Button button_remove;

        public Orders_ViewForm()
        {
            InitializeComponent();

            this.Load += Orders_ViewForm_Load;
            this.Disposed += Orders_ViewForm_Disposed;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Size = new Size(1200, 650);
            this.Text = "Заказы";

            button_create = new Button()
            {
                Text = "Добавить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_edit = new Button()
            {
                Text = "Изменить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_remove = new Button()
            {
                Text = "Удалить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_update = new Button()
            {
                Text = "Обновить",
                Height = 30,
                Dock = DockStyle.Bottom
            };
            textBox_search = new TextBox()
            {
                Dock = DockStyle.Top
            };
            dataGridView = new DataGridView()
            {
                Dock = DockStyle.Fill
            };

            button_update.Click += Button_update_Click;
            button_remove.Click += Button_remove_Click;
            button_create.Click += Button_create_Click;
            button_edit.Click += Button_edit_Click;
            textBox_search.TextChanged += TextBox_search_TextChanged;

            this.Controls.Add(button_create);
            this.Controls.Add(button_edit);
            this.Controls.Add(button_remove);
            this.Controls.Add(button_update);
            this.Controls.Add(textBox_search);
            this.Controls.Add(dataGridView);
        }

        private void Orders_ViewForm_Disposed(object? sender, EventArgs e)
        {
            this.Load -= Orders_ViewForm_Load;
            this.Disposed -= Orders_ViewForm_Disposed;
            button_update.Click -= Button_update_Click;
            button_remove.Click -= Button_remove_Click;
            button_create.Click -= Button_create_Click;
            button_edit.Click -= Button_edit_Click;
            textBox_search.TextChanged -= TextBox_search_TextChanged;
        }

        private void Button_edit_Click(object? sender, EventArgs e)
        {
            new Orders_AddEditForm(DBT_Orders.GetById((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value)).ShowDialog();
            UpdateTable();
        }

        private void Button_create_Click(object? sender, EventArgs e)
        {
            new Orders_AddEditForm().ShowDialog();
            UpdateTable();
        }

        private void Button_remove_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить запись?\r\nОтменить будет невозможно!\r\n", "Удалить", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            int id = (int)dataGridView.CurrentCell.OwningRow.Cells[0].Value;
            // вернуть всё на склад по данным из БД
            foreach (var i in DBT_OrderItems.GetByOrderID(id))
            {
                DBT_Stock.DebitFromWarehouse(i.ProductID, -i.Quantity);
            }
            // удалить связи
            DBT_OrderItems.RemoveByOrderId(id);
            int res = DBT_Orders.Remove(id);
            if (res == -1) MessageBox.Show("Ошибка удаления!");
            else if (res == 0) MessageBox.Show("Успешно удалено!");
            UpdateTable();
        }

        private void TextBox_search_TextChanged(object? sender, EventArgs e) => UpdateTable();
        private void Button_update_Click(object? sender, EventArgs e) => UpdateTable();
        private void Orders_ViewForm_Load(object sender, EventArgs e) => UpdateTable();

        private void UpdateTable()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.BringToFront();
            dataGridView.ReadOnly = true;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.Columns.Add("ID", "ID"); 
            dataGridView.Columns.Add("ClientID", "Клиент");
            dataGridView.Columns.Add("EmployeeID", "Сотрудник");
            dataGridView.Columns.Add("OrderDateTime", "Дата заказа");
            dataGridView.Columns.Add("TotalAmount", "Сумма заказа"); 
            dataGridView.Columns.Add("Status", "Статус");
            dataGridView.Columns.Add("Products", "Товары"); dataGridView.Columns[6].Width = 600;

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
                            string search = textBox_search.Text.ToLower();
                            if (!string.IsNullOrWhiteSpace(search))
                                if (
                                    !reader.GetValue(0).ToString().Contains(search) &&
                                    !reader.GetValue(1).ToString().Contains(search) &&
                                    !reader.GetValue(2).ToString().Contains(search) &&
                                    !reader.GetValue(3).ToString().Contains(search) &&
                                    !reader.GetValue(4).ToString().Contains(search) &&
                                    !reader.GetValue(5).ToString().Contains(search)
                                ) continue;

                            var index = dataGridView.Rows.Add();
                            dataGridView.Rows[index].Cells[0].Value = reader.GetInt32(0);
                            dataGridView.Rows[index].Cells[1].Value = DBT_Clients.GetById(reader.GetInt32(1)).FullName;
                            dataGridView.Rows[index].Cells[2].Value = DBT_Employees.GetById(reader.GetInt32(2)).FullName;
                            dataGridView.Rows[index].Cells[3].Value = DateTime.Parse(reader.GetValue(3).ToString());
                            //dataGridView.Rows[index].Cells[4].Value = reader.GetDecimal(4);
                            dataGridView.Rows[index].Cells[5].Value = reader.GetString(5);
                            string products = string.Empty;
                            decimal total = 0;
                            uint itter = 1;
                            foreach(var i in DBT_OrderItems.GetByOrderID(reader.GetInt32(0)))
                            {
                                var product = DBT_Products.GetById(i.ProductID);
                                products += $"{itter} -> {product.Name} | {i.PriceAtOrderTime} руб. * {i.Quantity} {product.UnitOfMeasure} = {i.Subtotal} руб.\n";
                                total += (decimal)i.Subtotal;
                                itter++;
                            }
                            dataGridView.Rows[index].Cells[6].Value = products;
                            dataGridView.Rows[index].Cells[4].Value = total;

                        }
                    }
                }
            }
        }
    }


}
