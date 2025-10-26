namespace Program.scr.windows
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;

            if (core.Core.AccessLevel == 0)
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
            }
            if (core.Core.AccessLevel == 1)
            {
                button1.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
            }
            if (core.Core.AccessLevel == 2)
            {
                button2.Visible = true;
                button4.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
            }

            label1.Text = $"Добро пожаловать, {core.Core.ThisEmployee.FullName}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Suppliers_ViewForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Employees_ViewForm().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Auth_ViewForm().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Clients_ViewForm().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Products_ViewForm().ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Orders_ViewForm().ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Services_ViewForm().ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new ProvidedServices_ViewForm().ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new Stock_ViewForm().ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
