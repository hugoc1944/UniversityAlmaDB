namespace UniversityAlmaApp
{
    partial class SessionForm
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
            lblSessionTitle = new Label();
            lblSessionNumber = new Label();
            btnPrev = new Button();
            btnNext = new Button();
            SuspendLayout();
            // 
            // lblSessionTitle
            // 
            lblSessionTitle.AutoSize = true;
            lblSessionTitle.Font = new Font("Open Sans", 21F);
            lblSessionTitle.ImageAlign = ContentAlignment.TopLeft;
            lblSessionTitle.Location = new Point(31, 36);
            lblSessionTitle.Name = "lblSessionTitle";
            lblSessionTitle.Size = new Size(0, 50);
            lblSessionTitle.TabIndex = 0;
            lblSessionTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblSessionNumber
            // 
            lblSessionNumber.AutoSize = true;
            lblSessionNumber.Font = new Font("Segoe UI", 12F);
            lblSessionNumber.ImageAlign = ContentAlignment.MiddleLeft;
            lblSessionNumber.Location = new Point(31, 146);
            lblSessionNumber.Name = "lblSessionNumber";
            lblSessionNumber.Size = new Size(0, 28);
            lblSessionNumber.TabIndex = 1;
            lblSessionNumber.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(12, 313);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(94, 29);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(517, 313);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(94, 29);
            btnNext.TabIndex = 3;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // SessionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(623, 364);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(lblSessionNumber);
            Controls.Add(lblSessionTitle);
            Name = "SessionForm";
            Text = "SessionForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSessionTitle;
        private Label lblSessionNumber;
        private Button btnPrev;
        private Button btnNext;
    }
}