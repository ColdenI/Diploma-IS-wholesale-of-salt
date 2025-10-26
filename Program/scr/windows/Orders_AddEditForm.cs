using Program.scr.core.dbt;
using Program.scr.windows.customListBox;

namespace Program.scr.windows
{
    public partial class Orders_AddEditForm : Form
    {
        DBT_Orders Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        ComboBox comboBox_ClientID;
        ComboBox comboBox_EmployeeID;
        DateTimePicker dateTimePicker_OrderDateTime;
        TextBox textBox_TotalAmount;
        TextBox textBox_Status;
        CustomNumericListBox customListBox;

        private List<DBT_Clients> Clients = new List<DBT_Clients>();
        private List<DBT_Employees> Employees = new List<DBT_Employees>();

        public Orders_AddEditForm()
        {
            InitializeComponent();
            Init();

            foreach(var i in DBT_Products.GetAll()) customListBox.Add(i.ID, i.Name, 0, DBT_Stock.GetByProductID(i.ID).QuantityOnStock);   
        }
        public Orders_AddEditForm(DBT_Orders obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            comboBox_ClientID.SelectedIndex = indexOf_Clients(obj.ClientID);
            comboBox_EmployeeID.SelectedIndex = indexOf_Employees(obj.EmployeeID);
            dateTimePicker_OrderDateTime.Value = (DateTime)((obj.OrderDateTime == null) ? DateTime.Now : obj.OrderDateTime);
            decimal total = 0;
            foreach (var i in DBT_OrderItems.GetByOrderID(obj.ID))
            {
                var product = DBT_Products.GetById(i.ProductID);
                total += (decimal)i.Subtotal;
            }
            textBox_TotalAmount.Text = total.ToString();
            textBox_Status.Text = obj.Status.ToString();

            foreach (var i in DBT_Products.GetAll())
            {
                var result = DBT_OrderItems.GetByOrderIDAndProductID(obj.ID, i.ID);
                decimal value = 0;
                if (result != null) if (result.Count > 0) if (result[0] != null) value = result[0].Quantity;
                customListBox.Add(i.ID, i.Name, value, DBT_Stock.GetByProductID(i.ID).QuantityOnStock);
            }

            foreach(var i in customListBox.Items) i.numericUpDown.Enabled = false;
            comboBox_ClientID.Enabled = false;
            comboBox_EmployeeID.Enabled = false;
        }

        private void Init()
        {
            this.Size = new Size(1200, 650);
            this.Text = "Заказы - " + (Object == null ? "Добавить" : "Изменить").ToString();
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
                Dock = DockStyle.Top,
                Height = 190,
                ColumnCount = 2,
                RowCount = 6
            };

            Label label_ClientID = new Label();
            SetLabel(ref label_ClientID, "Клиент");
            tableLayout.Controls.Add(label_ClientID, 0, 0);
            comboBox_ClientID = new ComboBox();
            comboBox_ClientID.Dock = DockStyle.Fill;
            comboBox_ClientID.MaxLength = 254;
            tableLayout.Controls.Add(comboBox_ClientID, 1, 0);

            Label label_EmployeeID = new Label();
            SetLabel(ref label_EmployeeID, "Сотрудник");
            tableLayout.Controls.Add(label_EmployeeID, 0, 1);
            comboBox_EmployeeID = new ComboBox();
            comboBox_EmployeeID.Dock = DockStyle.Fill;
            comboBox_EmployeeID.MaxLength = 254;
            tableLayout.Controls.Add(comboBox_EmployeeID, 1, 1);

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
            textBox_TotalAmount.ReadOnly = true;
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

            Label label_Products = new Label();
            SetLabel(ref label_Products, "Товары");
            tableLayout.Controls.Add(label_Products, 0, 5);

            customListBox = new CustomNumericListBox();
            customListBox.Dock = DockStyle.Fill;
            customListBox.Width = 400;
            customListBox.ValueChanged += (sender, e) =>
            {
                decimal total = 0;
                foreach (var i in customListBox.Items)
                {
                    total += i.Value * DBT_Products.GetById(i.Id).PricePerUnit;
                }
                total = Math.Round(total, 2);
                textBox_TotalAmount.Text = total.ToString();
            };
            this.Controls.Add(customListBox);


            this.Controls.Add(tableLayout);
            this.Controls.Add(button_apply);

            LoadComboBox_Clients();
            LoadComboBox_Employees();
        }

        private void LoadComboBox_Clients()
        {
            Clients = DBT_Clients.GetAll();

            comboBox_ClientID.Items.Clear();
            foreach (var i in Clients)
                comboBox_ClientID.Items.Add(i.FullName);
        }
        private int indexOf_Clients(int id)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].ID == id) return i;
            }
            return -1;
        }
        private void LoadComboBox_Employees()
        {
            Employees = DBT_Employees.GetAll();

            comboBox_EmployeeID.Items.Clear();
            foreach (var i in Employees)
                comboBox_EmployeeID.Items.Add(i.FullName);
        }
        private int indexOf_Employees(int id)
        {
            for (int i = 0; i < Employees.Count; i++)
            {
                if (Employees[i].ID == id) return i;
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
            if (comboBox_ClientID.SelectedIndex == -1) { MessageBox.Show("Поле 'Клиент' имеет некорректное значение!"); return; }
            if (comboBox_EmployeeID.SelectedIndex == -1) { MessageBox.Show("Поле 'Сотрудник' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_TotalAmount.Text, out decimal tp_TotalAmount)) { MessageBox.Show("Поле 'Сумма заказа' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_Status.Text)) { MessageBox.Show("Поле 'Статус' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Orders.Create(
                    new DBT_Orders()
                    {
                        ClientID = Clients[comboBox_ClientID.SelectedIndex].ID,
                        EmployeeID = Employees[comboBox_EmployeeID.SelectedIndex].ID,
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
                        ClientID = Clients[comboBox_ClientID.SelectedIndex].ID,
                        EmployeeID = Employees[comboBox_EmployeeID.SelectedIndex].ID,
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
