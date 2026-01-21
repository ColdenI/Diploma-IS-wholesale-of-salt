using Microsoft.Data.SqlClient;
using Program.scr.core;
using Program.scr.core.dbt;

namespace Program.scr.windows
{
    public partial class LagerverwalterForm : Form
    {
        public LagerverwalterForm()
        {
            InitializeComponent();

            textBox_search.TextChanged += TextBox_search_TextChanged;
            button_update.Click += Button_update_Click;

            UpdateTable();
        }

        private void Button_update_Click(object? sender, EventArgs e) => UpdateTable();
        private void TextBox_search_TextChanged(object? sender, EventArgs e) => UpdateTable();
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
                            if (reader.GetString(5) == "Отменён" || reader.GetString(5) == "Завершён") continue;

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
                            foreach (var i in DBT_OrderItems.GetByOrderID(reader.GetInt32(0)))
                            {
                                var product = DBT_Products.GetById(i.ProductID);
                                products += $"{itter} -> {product.Name} | {i.PriceAtOrderTime} руб. * {i.Quantity} {product.UnitOfMeasure} = {i.Subtotal} руб.\n";
                                total += (decimal)i.Subtotal;
                                itter++;
                            }
                            dataGridView.Rows[index].Cells[6].Value = products;
                            dataGridView.Rows[index].Cells[4].Value = total;

                            string search = textBox_search.Text.ToLower();
                            if (!string.IsNullOrWhiteSpace(search))
                                if (
                                    !dataGridView.Rows[index].Cells[0].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[1].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[2].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[3].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[4].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[5].Value.ToString().ToLower().Contains(search) &&
                                    !dataGridView.Rows[index].Cells[6].Value.ToString().ToLower().Contains(search)

                                ) dataGridView.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        private void button_editStatus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите статус");
                return;
            }

            var select = DBT_Orders.GetById((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value);
            select.Status = textBox1.Text;
            DBT_Orders.Edit(
                select
                );

            textBox1.Text = string.Empty;
            UpdateTable();
        }
    }
}
