namespace UniversityAlmaApp
{
    partial class UploadCourse
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
            lblTitle = new Label();
            lblCourseTitle = new Label();
            txtCourseTitle = new TextBox();
            txtCourseDescription = new TextBox();
            lblCourseDescription = new Label();
            lblCategory = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            cbCategory = new ComboBox();
            flpSessions = new FlowLayoutPanel();
            btnSubmit = new Button();
            btnCancel = new Button();
            btnAddSession = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Open Sans", 18F);
            lblTitle.Location = new Point(12, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(226, 44);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Upload Course";
            // 
            // lblCourseTitle
            // 
            lblCourseTitle.AutoSize = true;
            lblCourseTitle.Font = new Font("Segoe UI", 12F);
            lblCourseTitle.Location = new Point(31, 79);
            lblCourseTitle.Name = "lblCourseTitle";
            lblCourseTitle.Size = new Size(49, 28);
            lblCourseTitle.TabIndex = 1;
            lblCourseTitle.Text = "Title";
            // 
            // txtCourseTitle
            // 
            txtCourseTitle.Location = new Point(31, 110);
            txtCourseTitle.Name = "txtCourseTitle";
            txtCourseTitle.Size = new Size(207, 27);
            txtCourseTitle.TabIndex = 2;
            // 
            // txtCourseDescription
            // 
            txtCourseDescription.Location = new Point(31, 178);
            txtCourseDescription.Multiline = true;
            txtCourseDescription.Name = "txtCourseDescription";
            txtCourseDescription.Size = new Size(289, 83);
            txtCourseDescription.TabIndex = 4;
            // 
            // lblCourseDescription
            // 
            lblCourseDescription.AutoSize = true;
            lblCourseDescription.Font = new Font("Segoe UI", 12F);
            lblCourseDescription.Location = new Point(31, 147);
            lblCourseDescription.Name = "lblCourseDescription";
            lblCourseDescription.Size = new Size(112, 28);
            lblCourseDescription.TabIndex = 3;
            lblCourseDescription.Text = "Description";
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Segoe UI", 12F);
            lblCategory.Location = new Point(31, 277);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(92, 28);
            lblCategory.TabIndex = 5;
            lblCategory.Text = "Category";
            // 
            // cbCategory
            // 
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(31, 308);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(188, 28);
            cbCategory.TabIndex = 6;
            // 
            // flpSessions
            // 
            flpSessions.Location = new Point(379, 110);
            flpSessions.Name = "flpSessions";
            flpSessions.Size = new Size(369, 236);
            flpSessions.TabIndex = 7;
            // 
            // btnSubmit
            // 
            btnSubmit.Font = new Font("Segoe UI", 12F);
            btnSubmit.Location = new Point(30, 369);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(113, 45);
            btnSubmit.TabIndex = 8;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 12F);
            btnCancel.Location = new Point(163, 369);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(113, 45);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAddSession
            // 
            btnAddSession.Font = new Font("Segoe UI", 10F);
            btnAddSession.Location = new Point(379, 70);
            btnAddSession.Name = "btnAddSession";
            btnAddSession.Size = new Size(111, 34);
            btnAddSession.TabIndex = 10;
            btnAddSession.Text = "Add Session";
            btnAddSession.UseVisualStyleBackColor = true;
            // 
            // UploadCourse
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnAddSession);
            Controls.Add(btnCancel);
            Controls.Add(btnSubmit);
            Controls.Add(flpSessions);
            Controls.Add(cbCategory);
            Controls.Add(lblCategory);
            Controls.Add(txtCourseDescription);
            Controls.Add(lblCourseDescription);
            Controls.Add(txtCourseTitle);
            Controls.Add(lblCourseTitle);
            Controls.Add(lblTitle);
            Name = "UploadCourse";
            Text = "UploadCourse";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblCourseTitle;
        private TextBox txtCourseTitle;
        private TextBox txtCourseDescription;
        private Label lblCourseDescription;
        private Label lblCategory;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ComboBox cbCategory;
        private FlowLayoutPanel flpSessions;
        private Button btnSubmit;
        private Button btnCancel;
        private Button btnAddSession;
    }
}