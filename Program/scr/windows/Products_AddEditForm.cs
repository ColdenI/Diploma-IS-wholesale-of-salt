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
    public partial class Products_AddEditForm : Form
    {
        DBT_Products Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_SupplierID;
        TextBox textBox_Name;
        TextBox textBox_Category;
        TextBox textBox_UnitOfMeasure;
        TextBox textBox_PricePerUnit;
        TextBox textBox_Description;

        public Products_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Products_AddEditForm(DBT_Products obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_SupplierID.Text = obj.SupplierID.ToString();
            textBox_Name.Text = obj.Name.ToString();
            textBox_Category.Text = obj.Category.ToString();
            textBox_UnitOfMeasure.Text = obj.UnitOfMeasure.ToString();
            textBox_PricePerUnit.Text = obj.PricePerUnit.ToString();
            textBox_Description.Text = obj.Description.ToString();
        }

        private void Init()
        {
            this.Text = "Товары - " + Object == null ? "Добавить" : "Изменить";
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
                RowCount = 6
            };

            Label label_SupplierID = new Label();
            SetLabel(ref label_SupplierID, "Поставщик");
            tableLayout.Controls.Add(label_SupplierID, 0, 0);
            textBox_SupplierID = new TextBox();
            textBox_SupplierID.Dock = DockStyle.Fill;
            textBox_SupplierID.MaxLength = 254;
            tableLayout.Controls.Add(textBox_SupplierID, 1, 0);

            Label label_Name = new Label();
            SetLabel(ref label_Name, "Наименование");
            tableLayout.Controls.Add(label_Name, 0, 1);
            textBox_Name = new TextBox();
            textBox_Name.Dock = DockStyle.Fill;
            textBox_Name.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Name, 1, 1);

            Label label_Category = new Label();
            SetLabel(ref label_Category, "Категория");
            tableLayout.Controls.Add(label_Category, 0, 2);
            textBox_Category = new TextBox();
            textBox_Category.Dock = DockStyle.Fill;
            textBox_Category.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Category, 1, 2);

            Label label_UnitOfMeasure = new Label();
            SetLabel(ref label_UnitOfMeasure, "Единица измерения");
            tableLayout.Controls.Add(label_UnitOfMeasure, 0, 3);
            textBox_UnitOfMeasure = new TextBox();
            textBox_UnitOfMeasure.Dock = DockStyle.Fill;
            textBox_UnitOfMeasure.MaxLength = 254;
            tableLayout.Controls.Add(textBox_UnitOfMeasure, 1, 3);

            Label label_PricePerUnit = new Label();
            SetLabel(ref label_PricePerUnit, "Цена за ед. товара");
            tableLayout.Controls.Add(label_PricePerUnit, 0, 4);
            textBox_PricePerUnit = new TextBox();
            textBox_PricePerUnit.Dock = DockStyle.Fill;
            textBox_PricePerUnit.MaxLength = 254;
            tableLayout.Controls.Add(textBox_PricePerUnit, 1, 4);

            Label label_Description = new Label();
            SetLabel(ref label_Description, "Описание");
            tableLayout.Controls.Add(label_Description, 0, 5);
            textBox_Description = new TextBox();
            textBox_Description.Dock = DockStyle.Fill;
            textBox_Description.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Description, 1, 5);

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
            if (!int.TryParse(textBox_SupplierID.Text, out int tp_SupplierID)) { MessageBox.Show("Поле 'Поставщик' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_Name.Text)) { MessageBox.Show("Поле 'Наименование' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_PricePerUnit.Text, out decimal tp_PricePerUnit)) { MessageBox.Show("Поле 'Цена за ед. товара' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Products.Create(
                    new DBT_Products()
                    {
                        SupplierID = int.Parse(textBox_SupplierID.Text),
                        Name = textBox_Name.Text,
                        Category = textBox_Category.Text,
                        UnitOfMeasure = textBox_UnitOfMeasure.Text,
                        PricePerUnit = decimal.Parse(textBox_PricePerUnit.Text),
                        Description = textBox_Description.Text
                    }
                );
            }
            else
            {
                res = DBT_Products.Edit(
                    new DBT_Products()
                    {
                        ID = Object.ID,
                        SupplierID = int.Parse(textBox_SupplierID.Text),
                        Name = textBox_Name.Text,
                        Category = textBox_Category.Text,
                        UnitOfMeasure = textBox_UnitOfMeasure.Text,
                        PricePerUnit = decimal.Parse(textBox_PricePerUnit.Text),
                        Description = textBox_Description.Text
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
