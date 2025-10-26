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
    public partial class Services_AddEditForm : Form
    {
        DBT_Services Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_Name;
        TextBox textBox_Description;
        TextBox textBox_Cost;

        public Services_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Services_AddEditForm(DBT_Services obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_Name.Text = obj.Name.ToString();
            textBox_Description.Text = obj.Description.ToString();
            textBox_Cost.Text = obj.Cost.ToString();
        }

        private void Init()
        {
            this.Text = "Услуги - " + Object == null ? "Добавить" : "Изменить";
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

            Label label_Name = new Label();
            SetLabel(ref label_Name, "Наименование");
            tableLayout.Controls.Add(label_Name, 0, 0);
            textBox_Name = new TextBox();
            textBox_Name.Dock = DockStyle.Fill;
            textBox_Name.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Name, 1, 0);

            Label label_Description = new Label();
            SetLabel(ref label_Description, "Описание");
            tableLayout.Controls.Add(label_Description, 0, 1);
            textBox_Description = new TextBox();
            textBox_Description.Dock = DockStyle.Fill;
            textBox_Description.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Description, 1, 1);

            Label label_Cost = new Label();
            SetLabel(ref label_Cost, "Цена");
            tableLayout.Controls.Add(label_Cost, 0, 2);
            textBox_Cost = new TextBox();
            textBox_Cost.Dock = DockStyle.Fill;
            textBox_Cost.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Cost, 1, 2);

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
            if (string.IsNullOrWhiteSpace(textBox_Name.Text)) { MessageBox.Show("Поле 'Наименование' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_Cost.Text, out decimal tp_Cost)) { MessageBox.Show("Поле 'Цена' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Services.Create(
                    new DBT_Services()
                    {
                        Name = textBox_Name.Text,
                        Description = textBox_Description.Text,
                        Cost = decimal.Parse(textBox_Cost.Text)
                    }
                );
            }
            else
            {
                res = DBT_Services.Edit(
                    new DBT_Services()
                    {
                        ID = Object.ID,
                        Name = textBox_Name.Text,
                        Description = textBox_Description.Text,
                        Cost = decimal.Parse(textBox_Cost.Text)
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
