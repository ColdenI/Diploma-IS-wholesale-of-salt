using Program.scr.core.dbt;

namespace Program.scr.windows
{
    public partial class Stock_AddEditForm : Form
    {
        DBT_Stock Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        ComboBox comboBox_ProductID;
        TextBox textBox_QuantityOnStock;
        //DateTimePicker dateTimePicker_LastUpdated;

        private List<DBT_Products> Products = new List<DBT_Products>();

        public Stock_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Stock_AddEditForm(DBT_Stock obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            comboBox_ProductID.SelectedIndex = indexOf_Products(obj.ProductID);
            textBox_QuantityOnStock.Text = obj.QuantityOnStock.ToString();
            //dateTimePicker_LastUpdated.Value = (DateTime)((obj.LastUpdated == null) ? DateTime.Now : obj.LastUpdated);
        }

        private void Init()
        {
            this.Size = new Size(1200, 650);
            this.Text = "Склад - " + (Object == null ? "Добавить" : "Изменить").ToString();
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

            Label label_ProductID = new Label();
            SetLabel(ref label_ProductID, "Товар");
            tableLayout.Controls.Add(label_ProductID, 0, 0);
            comboBox_ProductID = new ComboBox();
            comboBox_ProductID.Dock = DockStyle.Fill;
            comboBox_ProductID.MaxLength = 254;
            tableLayout.Controls.Add(comboBox_ProductID, 1, 0);
            LoadComboBox_Products();

            Label label_QuantityOnStock = new Label();
            SetLabel(ref label_QuantityOnStock, "Количество на складе");
            tableLayout.Controls.Add(label_QuantityOnStock, 0, 1);
            textBox_QuantityOnStock = new TextBox();
            textBox_QuantityOnStock.Dock = DockStyle.Fill;
            textBox_QuantityOnStock.MaxLength = 254;
            tableLayout.Controls.Add(textBox_QuantityOnStock, 1, 1);

            //Label label_LastUpdated = new Label();
            //SetLabel(ref label_LastUpdated, "Дата изменения");
            //tableLayout.Controls.Add(label_LastUpdated, 0, 2);
            //dateTimePicker_LastUpdated = new DateTimePicker();
            //dateTimePicker_LastUpdated.Dock = DockStyle.Fill;
            //tableLayout.Controls.Add(dateTimePicker_LastUpdated, 1, 2);
            //dateTimePicker_LastUpdated.Format = DateTimePickerFormat.Custom;
            //dateTimePicker_LastUpdated.CustomFormat = "yyyy.MM.dd HH:mm:ss";

            this.Controls.Add(tableLayout);
        }

        private void LoadComboBox_Products()
        {
            Products = DBT_Products.GetAll();

            comboBox_ProductID.Items.Clear();
            foreach (var i in Products)
                comboBox_ProductID.Items.Add(i.Name);
        }
        private int indexOf_Products(int id)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].ID == id) return i;
            }
            return -1;
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
            if (comboBox_ProductID.SelectedIndex == -1) { MessageBox.Show("Поле 'Товар' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_QuantityOnStock.Text, out decimal tp_QuantityOnStock)) { MessageBox.Show("Поле 'Количество на складе' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Stock.Create(
                    new DBT_Stock()
                    {
                        ProductID = Products[comboBox_ProductID.SelectedIndex].ID,
                        QuantityOnStock = decimal.Parse(textBox_QuantityOnStock.Text),
                        LastUpdated = DateTime.Now
                    }
                );
            }
            else
            {
                res = DBT_Stock.Edit(
                    new DBT_Stock()
                    {
                        ID = Object.ID,
                        ProductID = Products[comboBox_ProductID.SelectedIndex].ID,
                        QuantityOnStock = decimal.Parse(textBox_QuantityOnStock.Text),
                        LastUpdated = DateTime.Now
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
