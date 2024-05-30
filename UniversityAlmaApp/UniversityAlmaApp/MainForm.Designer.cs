namespace UniversityAlmaApp
{
    partial class MainForm
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
            btnCourses = new Button();
            btnNotifications = new Button();
            btnProfile = new Button();
            btnLogOut = new Button();
            btnBecomeMentor = new Button();
            lblUserName = new Label();
            panelCategories = new FlowLayoutPanel();
            panelCourses = new FlowLayoutPanel();
            cbSorting = new ComboBox();
            txtSearch = new TextBox();
            lblSearch = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Open Sans", 16F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(215, 39);
            label1.TabIndex = 0;
            label1.Text = "University Alma";
            // 
            // btnCourses
            // 
            btnCourses.Font = new Font("Segoe UI", 11F);
            btnCourses.Location = new Point(12, 86);
            btnCourses.Name = "btnCourses";
            btnCourses.Size = new Size(103, 41);
            btnCourses.TabIndex = 1;
            btnCourses.Text = "Courses";
            btnCourses.UseVisualStyleBackColor = true;
            btnCourses.Click += btnCourses_Click;
            // 
            // btnNotifications
            // 
            btnNotifications.Font = new Font("Segoe UI", 11F);
            btnNotifications.Location = new Point(121, 86);
            btnNotifications.Name = "btnNotifications";
            btnNotifications.Size = new Size(128, 41);
            btnNotifications.TabIndex = 2;
            btnNotifications.Text = "Notifications";
            btnNotifications.UseVisualStyleBackColor = true;
            btnNotifications.Click += btnNotifications_Click;
            // 
            // btnProfile
            // 
            btnProfile.Font = new Font("Segoe UI", 11F);
            btnProfile.Location = new Point(255, 86);
            btnProfile.Name = "btnProfile";
            btnProfile.Size = new Size(103, 41);
            btnProfile.TabIndex = 3;
            btnProfile.Text = "Profile";
            btnProfile.UseVisualStyleBackColor = true;
            btnProfile.Click += btnProfile_Click;
            // 
            // btnLogOut
            // 
            btnLogOut.Font = new Font("Segoe UI", 11F);
            btnLogOut.Location = new Point(685, 86);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.Size = new Size(103, 41);
            btnLogOut.TabIndex = 4;
            btnLogOut.Text = "Log Out";
            btnLogOut.UseVisualStyleBackColor = true;
            btnLogOut.Click += btnLogOut_Click;
            // 
            // btnBecomeMentor
            // 
            btnBecomeMentor.Font = new Font("Segoe UI", 11F);
            btnBecomeMentor.Location = new Point(510, 86);
            btnBecomeMentor.Name = "btnBecomeMentor";
            btnBecomeMentor.Size = new Size(169, 41);
            btnBecomeMentor.TabIndex = 5;
            btnBecomeMentor.Text = "Become a Mentor";
            btnBecomeMentor.UseVisualStyleBackColor = true;
            btnBecomeMentor.Click += btnBecomeMentor_Click;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 11F);
            lblUserName.Location = new Point(12, 48);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(0, 25);
            lblUserName.TabIndex = 6;
            // 
            // panelCategories
            // 
            panelCategories.Location = new Point(12, 147);
            panelCategories.Name = "panelCategories";
            panelCategories.Size = new Size(444, 53);
            panelCategories.TabIndex = 7;
            // 
            // panelCourses
            // 
            panelCourses.Location = new Point(48, 266);
            panelCourses.Name = "panelCourses";
            panelCourses.Size = new Size(704, 348);
            panelCourses.TabIndex = 8;
            // 
            // cbSorting
            // 
            cbSorting.FormattingEnabled = true;
            cbSorting.Location = new Point(48, 232);
            cbSorting.Name = "cbSorting";
            cbSorting.Size = new Size(151, 28);
            cbSorting.TabIndex = 9;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(544, 232);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(208, 27);
            txtSearch.TabIndex = 10;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 12F);
            lblSearch.Location = new Point(544, 201);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(70, 28);
            lblSearch.TabIndex = 11;
            lblSearch.Text = "Search";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 648);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(cbSorting);
            Controls.Add(panelCourses);
            Controls.Add(panelCategories);
            Controls.Add(lblUserName);
            Controls.Add(btnBecomeMentor);
            Controls.Add(btnLogOut);
            Controls.Add(btnProfile);
            Controls.Add(btnNotifications);
            Controls.Add(btnCourses);
            Controls.Add(label1);
            Name = "MainForm";
            Text = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnCourses;
        private Button btnNotifications;
        private Button btnProfile;
        private Button btnLogOut;
        private Button btnBecomeMentor;
        private Label lblUserName;
        private FlowLayoutPanel panelCategories;
        private FlowLayoutPanel panelCourses;
        private ComboBox cbSorting;
        private TextBox txtSearch;
        private Label lblSearch;
    }
}