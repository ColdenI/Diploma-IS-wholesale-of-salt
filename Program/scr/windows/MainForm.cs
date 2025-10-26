namespace Program.scr.windows
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
