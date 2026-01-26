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
    public partial class AnalyticViewForm : Form
    {
        WebBrowser webBrowser;
        /*
        public AnalyticViewForm(string path)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            if (string.IsNullOrWhiteSpace(path)) Close();
            if (Path.Exists(path)) Close();

            webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Fill;
            this.Controls.Add(webBrowser);

            webBrowser.DocumentText = File.ReadAllText(path);
        }
        */
        public AnalyticViewForm(string html)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Аналитический отчёт за период";

            webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Fill;
            this.Controls.Add(webBrowser);

            webBrowser.DocumentText = html;
        }
    }
}
