using Program.scr.core.dbt;

namespace Program.scr.windows
{
    public partial class Clients_AddEditForm : Form
    {
        DBT_Clients Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_FullName;
        TextBox textBox_ContactPerson;
        TextBox textBox_Phone;
        TextBox textBox_Email;
        TextBox textBox_Address;

        public Clients_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Clients_AddEditForm(DBT_Clients obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_FullName.Text = obj.FullName.ToString();
            textBox_ContactPerson.Text = obj.ContactPerson.ToString();
            textBox_Phone.Text = obj.Phone.ToString();
            textBox_Email.Text = obj.Email.ToString();
            textBox_Address.Text = obj.Address.ToString();
        }

        private void Init()
        {
            this.Size = new Size(1200, 650);
            this.Text = "Клиенты - " + (Object == null ? "Добавить" : "Изменить").ToString();
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

            Label label_FullName = new Label();
            SetLabel(ref label_FullName, "Организация");
            tableLayout.Controls.Add(label_FullName, 0, 0);
            textBox_FullName = new TextBox();
            textBox_FullName.Dock = DockStyle.Fill;
            textBox_FullName.MaxLength = 254;
            tableLayout.Controls.Add(textBox_FullName, 1, 0);

            Label label_ContactPerson = new Label();
            SetLabel(ref label_ContactPerson, "Контактное лицо");
            tableLayout.Controls.Add(label_ContactPerson, 0, 1);
            textBox_ContactPerson = new TextBox();
            textBox_ContactPerson.Dock = DockStyle.Fill;
            textBox_ContactPerson.MaxLength = 254;
            tableLayout.Controls.Add(textBox_ContactPerson, 1, 1);

            Label label_Phone = new Label();
            SetLabel(ref label_Phone, "Номер телефона");
            tableLayout.Controls.Add(label_Phone, 0, 2);
            textBox_Phone = new TextBox();
            textBox_Phone.Dock = DockStyle.Fill;
            textBox_Phone.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Phone, 1, 2);

            Label label_Email = new Label();
            SetLabel(ref label_Email, "Электронная почта");
            tableLayout.Controls.Add(label_Email, 0, 3);
            textBox_Email = new TextBox();
            textBox_Email.Dock = DockStyle.Fill;
            textBox_Email.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Email, 1, 3);

            Label label_Address = new Label();
            SetLabel(ref label_Address, "Адрес");
            tableLayout.Controls.Add(label_Address, 0, 4);
            textBox_Address = new TextBox();
            textBox_Address.Dock = DockStyle.Fill;
            textBox_Address.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Address, 1, 4);

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
            if (string.IsNullOrWhiteSpace(textBox_FullName.Text)) { MessageBox.Show("Поле 'Организация' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Clients.Create(
                    new DBT_Clients()
                    {
                        FullName = textBox_FullName.Text,
                        ContactPerson = textBox_ContactPerson.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text,
                        Address = textBox_Address.Text
                    }
                );
            }
            else
            {
                res = DBT_Clients.Edit(
                    new DBT_Clients()
                    {
                        ID = Object.ID,
                        FullName = textBox_FullName.Text,
                        ContactPerson = textBox_ContactPerson.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text,
                        Address = textBox_Address.Text
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
