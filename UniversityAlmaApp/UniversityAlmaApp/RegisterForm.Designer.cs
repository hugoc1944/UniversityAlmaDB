namespace UniversityAlmaApp
{
    partial class RegisterForm
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
            label1 = new Label();
            label2 = new Label();
            txtName = new TextBox();
            txtUsername = new TextBox();
            label3 = new Label();
            txtPassword = new TextBox();
            label4 = new Label();
            txtEmail = new TextBox();
            label5 = new Label();
            txtPhoneNumber = new TextBox();
            label6 = new Label();
            label7 = new Label();
            cmbGender = new ComboBox();
            dtpBirthday = new DateTimePicker();
            label8 = new Label();
            chkMailList = new CheckBox();
            label9 = new Label();
            btnRegister = new Button();
            pictureBoxProfilePic = new PictureBox();
            label10 = new Label();
            label11 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProfilePic).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Open Sans", 21F);
            label1.Location = new Point(12, 63);
            label1.Name = "label1";
            label1.Size = new Size(155, 50);
            label1.TabIndex = 0;
            label1.Text = "Register";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(27, 130);
            label2.Name = "label2";
            label2.Size = new Size(64, 28);
            label2.TabIndex = 1;
            label2.Text = "Name";
            // 
            // txtName
            // 
            txtName.Location = new Point(26, 160);
            txtName.Name = "txtName";
            txtName.Size = new Size(198, 27);
            txtName.TabIndex = 2;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(311, 160);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(198, 27);
            txtUsername.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(311, 129);
            label3.Name = "label3";
            label3.Size = new Size(99, 28);
            label3.TabIndex = 3;
            label3.Text = "Username";
            label3.Click += label3_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(311, 231);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(198, 27);
            txtPassword.TabIndex = 6;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(311, 200);
            label4.Name = "label4";
            label4.Size = new Size(93, 28);
            label4.TabIndex = 5;
            label4.Text = "Password";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(26, 304);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(239, 27);
            txtEmail.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(26, 273);
            label5.Name = "label5";
            label5.Size = new Size(134, 28);
            label5.TabIndex = 7;
            label5.Text = "Email Address";
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Location = new Point(26, 231);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new Size(198, 27);
            txtPhoneNumber.TabIndex = 10;
            txtPhoneNumber.KeyPress += txtPhoneNumber_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(26, 200);
            label6.Name = "label6";
            label6.Size = new Size(144, 28);
            label6.TabIndex = 9;
            label6.Text = "Phone Number";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(311, 273);
            label7.Name = "label7";
            label7.Size = new Size(76, 28);
            label7.TabIndex = 11;
            label7.Text = "Gender";
            // 
            // cmbGender
            // 
            cmbGender.FormattingEnabled = true;
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.Location = new Point(311, 304);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(198, 28);
            cmbGender.TabIndex = 13;
            // 
            // dtpBirthday
            // 
            dtpBirthday.Location = new Point(26, 375);
            dtpBirthday.Name = "dtpBirthday";
            dtpBirthday.Size = new Size(239, 27);
            dtpBirthday.TabIndex = 14;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(26, 344);
            label8.Name = "label8";
            label8.Size = new Size(85, 28);
            label8.TabIndex = 15;
            label8.Text = "Birthday";
            label8.Click += label8_Click;
            // 
            // chkMailList
            // 
            chkMailList.AutoSize = true;
            chkMailList.Location = new Point(311, 379);
            chkMailList.Name = "chkMailList";
            chkMailList.Size = new Size(214, 24);
            chkMailList.TabIndex = 16;
            chkMailList.Text = "Subscribe to our Newsletter";
            chkMailList.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F);
            label9.Location = new Point(311, 344);
            label9.Name = "label9";
            label9.Size = new Size(105, 28);
            label9.TabIndex = 17;
            label9.Text = "Newsletter";
            // 
            // btnRegister
            // 
            btnRegister.Font = new Font("Segoe UI", 12F);
            btnRegister.Location = new Point(26, 427);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(93, 37);
            btnRegister.TabIndex = 18;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // pictureBoxProfilePic
            // 
            pictureBoxProfilePic.BackColor = Color.FromArgb(224, 224, 224);
            pictureBoxProfilePic.Cursor = Cursors.Hand;
            pictureBoxProfilePic.Location = new Point(592, 211);
            pictureBoxProfilePic.Name = "pictureBoxProfilePic";
            pictureBoxProfilePic.Size = new Size(154, 90);
            pictureBoxProfilePic.TabIndex = 19;
            pictureBoxProfilePic.TabStop = false;
            pictureBoxProfilePic.Tag = "";
            pictureBoxProfilePic.Click += pictureBoxProfilePic_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 12F);
            label10.Location = new Point(592, 180);
            label10.Name = "label10";
            label10.Size = new Size(133, 28);
            label10.TabIndex = 20;
            label10.Text = "Profile Picture";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.FromArgb(224, 224, 224);
            label11.Font = new Font("Segoe UI", 12F);
            label11.Location = new Point(613, 240);
            label11.Name = "label11";
            label11.Size = new Size(112, 28);
            label11.TabIndex = 21;
            label11.Text = "Upload File";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 524);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(pictureBoxProfilePic);
            Controls.Add(btnRegister);
            Controls.Add(label9);
            Controls.Add(chkMailList);
            Controls.Add(label8);
            Controls.Add(dtpBirthday);
            Controls.Add(cmbGender);
            Controls.Add(label7);
            Controls.Add(txtPhoneNumber);
            Controls.Add(label6);
            Controls.Add(txtEmail);
            Controls.Add(label5);
            Controls.Add(txtPassword);
            Controls.Add(label4);
            Controls.Add(txtUsername);
            Controls.Add(label3);
            Controls.Add(txtName);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "RegisterForm";
            Text = "RegisterForm";
            ((System.ComponentModel.ISupportInitialize)pictureBoxProfilePic).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtName;
        private TextBox txtUsername;
        private Label label3;
        private TextBox txtPassword;
        private Label label4;
        private TextBox txtEmail;
        private Label label5;
        private TextBox txtPhoneNumber;
        private Label label6;
        private Label label7;
        private ComboBox cmbGender;
        private DateTimePicker dtpBirthday;
        private Label label8;
        private CheckBox chkMailList;
        private Label label9;
        private Button btnRegister;
        private PictureBox pictureBoxProfilePic;
        private Label label10;
        private Label label11;
    }
}