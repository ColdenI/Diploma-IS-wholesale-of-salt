namespace Program.scr.windows
{
    partial class LagerverwalterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            textBox_search = new TextBox();
            button_update = new Button();
            button_editStatus = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(0, 26);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 47;
            dataGridView.Size = new Size(800, 376);
            dataGridView.TabIndex = 0;
            // 
            // textBox_search
            // 
            textBox_search.Dock = DockStyle.Top;
            textBox_search.Location = new Point(0, 0);
            textBox_search.Name = "textBox_search";
            textBox_search.Size = new Size(800, 26);
            textBox_search.TabIndex = 1;
            // 
            // button_update
            // 
            button_update.Location = new Point(702, 10);
            button_update.Name = "button_update";
            button_update.Size = new Size(86, 26);
            button_update.TabIndex = 2;
            button_update.Text = "Обновить";
            button_update.UseVisualStyleBackColor = true;
            // 
            // button_editStatus
            // 
            button_editStatus.Location = new Point(610, 10);
            button_editStatus.Name = "button_editStatus";
            button_editStatus.Size = new Size(86, 26);
            button_editStatus.TabIndex = 3;
            button_editStatus.Text = "Изменить статус";
            button_editStatus.UseVisualStyleBackColor = true;
            button_editStatus.Click += button_editStatus_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(115, 11);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(489, 26);
            textBox1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(97, 19);
            label1.TabIndex = 5;
            label1.Text = "Новый статус:";
            // 
            // panel1
            // 
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button_update);
            panel1.Controls.Add(button_editStatus);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 402);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 48);
            panel1.TabIndex = 6;
            // 
            // LagerverwalterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView);
            Controls.Add(panel1);
            Controls.Add(textBox_search);
            Name = "LagerverwalterForm";
            Text = "Окно кладовщика";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private TextBox textBox_search;
        private Button button_update;
        private Button button_editStatus;
        private TextBox textBox1;
        private Label label1;
        private Panel panel1;
    }
}