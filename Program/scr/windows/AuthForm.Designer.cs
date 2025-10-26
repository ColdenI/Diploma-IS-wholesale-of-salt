namespace Program
{
    partial class AuthForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            textBox_login = new TextBox();
            textBox_password = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Variable Display", 11.7818184F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(60, 24);
            label1.TabIndex = 0;
            label1.Text = "Логин";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Variable Display", 11.7818184F);
            label2.Location = new Point(12, 88);
            label2.Name = "label2";
            label2.Size = new Size(73, 24);
            label2.TabIndex = 1;
            label2.Text = "Пароль";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Variable Display", 11.7818184F);
            button1.Location = new Point(184, 170);
            button1.Name = "button1";
            button1.Size = new Size(91, 37);
            button1.TabIndex = 3;
            button1.Text = "Войти";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox_login
            // 
            textBox_login.Font = new Font("Segoe UI Variable Display", 11.7818184F);
            textBox_login.Location = new Point(12, 36);
            textBox_login.Name = "textBox_login";
            textBox_login.Size = new Size(440, 31);
            textBox_login.TabIndex = 1;
            // 
            // textBox_password
            // 
            textBox_password.Font = new Font("Segoe UI Variable Display", 11.7818184F);
            textBox_password.Location = new Point(12, 115);
            textBox_password.Name = "textBox_password";
            textBox_password.PasswordChar = '*';
            textBox_password.Size = new Size(440, 31);
            textBox_password.TabIndex = 2;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 223);
            Controls.Add(textBox_password);
            Controls.Add(textBox_login);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "AuthForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private TextBox textBox_login;
        private TextBox textBox_password;
    }
}
