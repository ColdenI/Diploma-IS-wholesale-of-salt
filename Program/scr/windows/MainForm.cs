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
    }
}
