using Microsoft.Data.SqlClient;
using Program.scr.core;
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
    public partial class Products_ViewForm : Form
    {
        DataGridView dataGridView;
        TextBox textBox_search;
        Button button_edit;
        Button button_update;
        Button button_create;
        Button button_remove;

        public Products_ViewForm()
        {
            InitializeComponent();

            this.Load += Products_ViewForm_Load;
            this.Disposed += Products_ViewForm_Disposed;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Text = "Товары";

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

        private void Products_ViewForm_Disposed(object? sender, EventArgs e)
        {
            this.Load -= Products_ViewForm_Load;
            this.Disposed -= Products_ViewForm_Disposed;
            button_update.Click -= Button_update_Click;
            button_remove.Click -= Button_remove_Click;
            button_create.Click -= Button_create_Click;
            button_edit.Click -= Button_edit_Click;
            textBox_search.TextChanged -= TextBox_search_TextChanged;
        }

        private void Button_edit_Click(object? sender, EventArgs e)
        {
            new Products_AddEditForm(DBT_Products.GetById((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value)).ShowDialog();
            UpdateTable();
        }

        private void Button_create_Click(object? sender, EventArgs e)
        {
            new Products_AddEditForm().ShowDialog();
            UpdateTable();
        }

        private void Button_remove_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить запись?\r\nОтменить будет невозможно!\r\n", "Удалить", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            int res = DBT_Products.Remove((int)dataGridView.CurrentCell.OwningRow.Cells[0].Value);
            if (res == -1) MessageBox.Show("Ошибка удаления!");
            else if (res == 0) MessageBox.Show("Успешно удалено!");
            UpdateTable();
        }

        private void TextBox_search_TextChanged(object? sender, EventArgs e) => UpdateTable();
        private void Button_update_Click(object? sender, EventArgs e) => UpdateTable();
        private void Products_ViewForm_Load(object sender, EventArgs e) => UpdateTable();

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
            dataGridView.Columns.Add("SupplierID", "Поставщик");
            dataGridView.Columns.Add("Name", "Наименование");
            dataGridView.Columns.Add("Category", "Категория");
            dataGridView.Columns.Add("UnitOfMeasure", "Единица измерения");
            dataGridView.Columns.Add("PricePerUnit", "Цена за ед. товара");
            dataGridView.Columns.Add("Description", "Описание");

            using (SqlConnection connection = new SqlConnection(SQL._sqlConnectStr))
            {
                connection.Open();
                using (var query = connection.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM Products";
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
                                    !reader.GetValue(5).ToString().Contains(search) &&
                                    !reader.GetValue(6).ToString().Contains(search)
                                ) continue;

                            var index = dataGridView.Rows.Add();
                            dataGridView.Rows[index].Cells[0].Value = reader.GetInt32(0);
                            dataGridView.Rows[index].Cells[1].Value = reader.GetInt32(1);
                            dataGridView.Rows[index].Cells[2].Value = reader.GetString(2);
                            if (reader.IsDBNull(3)) dataGridView.Rows[index].Cells[3].Value = "-";
                            else dataGridView.Rows[index].Cells[3].Value = reader.GetString(3);
                            if (reader.IsDBNull(4)) dataGridView.Rows[index].Cells[4].Value = "-";
                            else dataGridView.Rows[index].Cells[4].Value = reader.GetString(4);
                            dataGridView.Rows[index].Cells[5].Value = reader.GetDecimal(5);
                            if (reader.IsDBNull(6)) dataGridView.Rows[index].Cells[6].Value = "-";
                            else dataGridView.Rows[index].Cells[6].Value = reader.GetString(6);

                        }
                    }
                }
            }
        }
    }


}
