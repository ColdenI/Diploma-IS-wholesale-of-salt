using Program.scr.core.dbt;

namespace Program.scr.windows
{
    public partial class Employees_AddEditForm : Form
    {
        DBT_Employees Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_FullName;
        TextBox textBox_Position;
        TextBox textBox_Phone;
        TextBox textBox_Email;
        DateTimePicker dateTimePicker_HireDate;

        public Employees_AddEditForm()
        {
            InitializeComponent();
            Init();
        }
        public Employees_AddEditForm(DBT_Employees obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_FullName.Text = obj.FullName.ToString();
            textBox_Position.Text = obj.Position.ToString();
            textBox_Phone.Text = obj.Phone.ToString();
            textBox_Email.Text = obj.Email.ToString();
            dateTimePicker_HireDate.Value = (DateTime)((obj.HireDate == null) ? DateTime.Now : obj.HireDate);
        }

        private void Init()
        {
            this.Size = new Size(1200, 650);
            this.Text = "Сотрудники - " + (Object == null ? "Добавить" : "Изменить").ToString();
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
            SetLabel(ref label_FullName, "ФИО");
            tableLayout.Controls.Add(label_FullName, 0, 0);
            textBox_FullName = new TextBox();
            textBox_FullName.Dock = DockStyle.Fill;
            textBox_FullName.MaxLength = 254;
            tableLayout.Controls.Add(textBox_FullName, 1, 0);

            Label label_Position = new Label();
            SetLabel(ref label_Position, "Должность");
            tableLayout.Controls.Add(label_Position, 0, 1);
            textBox_Position = new TextBox();
            textBox_Position.Dock = DockStyle.Fill;
            textBox_Position.MaxLength = 254;
            tableLayout.Controls.Add(textBox_Position, 1, 1);

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

            Label label_HireDate = new Label();
            SetLabel(ref label_HireDate, "Дата найма");
            tableLayout.Controls.Add(label_HireDate, 0, 4);
            dateTimePicker_HireDate = new DateTimePicker();
            dateTimePicker_HireDate.Dock = DockStyle.Fill;
            tableLayout.Controls.Add(dateTimePicker_HireDate, 1, 4);
            dateTimePicker_HireDate.Format = DateTimePickerFormat.Custom;
            dateTimePicker_HireDate.CustomFormat = "yyyy.MM.dd HH:mm:ss";

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
            if (string.IsNullOrWhiteSpace(textBox_FullName.Text)) { MessageBox.Show("Поле 'ФИО' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Employees.Create(
                    new DBT_Employees()
                    {
                        FullName = textBox_FullName.Text,
                        Position = textBox_Position.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text,
                        HireDate = dateTimePicker_HireDate.Value
                    }
                );
            }
            else
            {
                res = DBT_Employees.Edit(
                    new DBT_Employees()
                    {
                        ID = Object.ID,
                        FullName = textBox_FullName.Text,
                        Position = textBox_Position.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text,
                        HireDate = dateTimePicker_HireDate.Value
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            else this.Close();
        }
    }
}
