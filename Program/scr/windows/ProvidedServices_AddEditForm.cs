using Program.scr.core.dbt;

namespace Program.scr.windows
{
    public partial class ProvidedServices_AddEditForm : Form
    {
        DBT_ProvidedServices Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        ComboBox comboBox_ClientID;
        ComboBox comboBox_EmployeeID;
        ComboBox comboBox_ServiceID;
        DateTimePicker dateTimePicker_ServiceDateTime;
        TextBox textBox_Status;

        private List<DBT_Clients> Clients = new List<DBT_Clients>();
        private List<DBT_Employees> Employees = new List<DBT_Employees>();
        private List<DBT_Services> Services = new List<DBT_Services>();

        public ProvidedServices_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public ProvidedServices_AddEditForm(DBT_ProvidedServices obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            comboBox_ClientID.SelectedIndex = indexOf_Clients(obj.ClientID);
            comboBox_EmployeeID.SelectedIndex = indexOf_Employees(obj.EmployeeID);
            comboBox_ServiceID.SelectedIndex = indexOf_Services(obj.ServiceID);
            dateTimePicker_ServiceDateTime.Value = (DateTime)((obj.ServiceDateTime == null) ? DateTime.Now : obj.ServiceDateTime);
            textBox_Status.Text = obj.Status.ToString();
        }

        private void Init()
        {
            this.Size = new Size(1200, 650);
            this.Text = "Оказанные услуги - " + (Object == null ? "Добавить" : "Изменить").ToString();
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

            Label label_ServiceID = new Label();
            SetLabel(ref label_ServiceID, "Услуга");
            tableLayout.Controls.Add(label_ServiceID, 0, 2);
            comboBox_ServiceID = new ComboBox();
            comboBox_ServiceID.Dock = DockStyle.Fill;
            comboBox_ServiceID.MaxLength = 254;
            tableLayout.Controls.Add(comboBox_ServiceID, 1, 2);

            Label label_ServiceDateTime = new Label();
            SetLabel(ref label_ServiceDateTime, "Дата оказания");
            tableLayout.Controls.Add(label_ServiceDateTime, 0, 3);
            dateTimePicker_ServiceDateTime = new DateTimePicker();
            dateTimePicker_ServiceDateTime.Dock = DockStyle.Fill;
            tableLayout.Controls.Add(dateTimePicker_ServiceDateTime, 1, 3);
            dateTimePicker_ServiceDateTime.Format = DateTimePickerFormat.Custom;
            dateTimePicker_ServiceDateTime.CustomFormat = "yyyy.MM.dd HH:mm:ss";

            Label label_Status = new Label();
            SetLabel(ref label_Status, "Статус");
            tableLayout.Controls.Add(label_Status, 0, 4);
            textBox_Status = new TextBox();
            textBox_Status.Dock = DockStyle.Fill;
            textBox_Status.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Status, 1, 4);

            LoadComboBox_Clients();
            LoadComboBox_Employees();
            LoadComboBox_Services();

            this.Controls.Add(tableLayout);
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
        private void LoadComboBox_Services()
        {
            Services = DBT_Services.GetAll();

            comboBox_ServiceID.Items.Clear();
            foreach (var i in Services)
                comboBox_ServiceID.Items.Add(i.Name);
        }
        private int indexOf_Services(int id)
        {
            for (int i = 0; i < Services.Count; i++)
            {
                if (Services[i].ID == id) return i;
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
            if (comboBox_ServiceID.SelectedIndex == -1) { MessageBox.Show("Поле 'Услуга' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_Status.Text)) { MessageBox.Show("Поле 'Статус' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_ProvidedServices.Create(
                    new DBT_ProvidedServices()
                    {
                        ClientID = Clients[comboBox_ClientID.SelectedIndex].ID,
                        EmployeeID = Employees[comboBox_EmployeeID.SelectedIndex].ID,
                        ServiceID = Services[comboBox_ServiceID.SelectedIndex].ID,
                        ServiceDateTime = dateTimePicker_ServiceDateTime.Value,
                        Status = textBox_Status.Text
                    }
                );
            }
            else
            {
                res = DBT_ProvidedServices.Edit(
                    new DBT_ProvidedServices()
                    {
                        ID = Object.ID,
                        ClientID = Clients[comboBox_ClientID.SelectedIndex].ID,
                        EmployeeID = Employees[comboBox_EmployeeID.SelectedIndex].ID,
                        ServiceID = Services[comboBox_ServiceID.SelectedIndex].ID,
                        ServiceDateTime = dateTimePicker_ServiceDateTime.Value,
                        Status = textBox_Status.Text
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
