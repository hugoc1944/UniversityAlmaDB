using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversityAlmaApp
{
    public class CategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryType { get; set; }

        public override string ToString()
        {
            return CategoryType;
        }
    }
    public partial class UploadCourse : Form
    {
        private int mentorId;
        private int sessionNumber = 1;
        private Dictionary<int, FlowLayoutPanel> sessionPanels = new Dictionary<int, FlowLayoutPanel>();

        public UploadCourse(int mentorId)
        {
            InitializeComponent();
            this.mentorId = mentorId;
            LoadCategories();
            btnAddSession.Click += BtnAddSession_Click;
            btnSubmit.Click += BtnSubmit_Click;
            btnCancel.Click += BtnCancel_Click;

            flpSessions.AutoScroll = true;
            flpSessions.FlowDirection = FlowDirection.TopDown;
        }
        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT CategoryId, Type FROM UniversityAlma.Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryId = reader.GetInt32(0);
                            string categoryType = reader.GetString(1);
                            cbCategory.Items.Add(new CategoryItem { CategoryId = categoryId, CategoryType = categoryType });
                        }
                    }
                }
            }
        }
        private void BtnAddSession_Click(Object sender, EventArgs e)
        {
            AddSessionControls();
        }
        private void AddSessionControls()
        {
            Label lblSessionNumber = new Label { Text = "Session " + sessionNumber, AutoSize = true };
            TextBox txtSessionTitle = new TextBox { Name = "txtSessionTitle" + sessionNumber, Width = 200 };
            PictureBox pictureBoxMedia = new PictureBox { Name = "pictureBoxMedia" + sessionNumber, Width = 200, Height = 30, BorderStyle = BorderStyle.FixedSingle };
            Button btnSelectMedia = new Button { Text = "Select Media", Tag = sessionNumber, Size = new Size(111, 30) };
            Button btnRemoveSession = new Button { Text = "Remove", Tag=sessionNumber, Size = new Size(80, 30) };

            btnSelectMedia.Click += BtnSelectMedia_Click;
            btnRemoveSession.Click += BtnRemoveSession_Click;
            FlowLayoutPanel sessionPanel = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            sessionPanel.Controls.Add(lblSessionNumber);
            sessionPanel.Controls.Add(txtSessionTitle);
            sessionPanel.Controls.Add(pictureBoxMedia);
            sessionPanel.Controls.Add(btnSelectMedia);
            sessionPanel.Controls.Add(btnRemoveSession);

            flpSessions.Controls.Add(sessionPanel);
            sessionPanels.Add(sessionNumber, sessionPanel);
            sessionNumber++;
        }
        private void BtnSelectMedia_Click(object sender, EventArgs e)
        {
            Button btnSelectMedia = sender as Button;
            int sessionNumber = (int)btnSelectMedia.Tag;
            PictureBox pictureBoxMedia = flpSessions.Controls.Find("pictureBoxMedia" + sessionNumber, true).FirstOrDefault() as PictureBox;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxMedia.Tag = openFileDialog.FileName;
                }
            }
        }
        private void BtnRemoveSession_Click(object sender, EventArgs e)
        {
            Button btnRemoveSession = sender as Button;
            int sessionNumberToRemove = (int)btnRemoveSession.Tag;

            if (sessionPanels.ContainsKey(sessionNumberToRemove))
            {
                FlowLayoutPanel sessionPanel = sessionPanels[sessionNumberToRemove];
                flpSessions.Controls.Remove(sessionPanel);
                sessionPanels.Remove(sessionNumberToRemove);

                UpdateSessionNumbers();
            }
        }
        private void UpdateSessionNumbers()
        {
            int newSessionNumber = 1;
            foreach (var panel in sessionPanels.Values)
            {
                Label lblSessionNumber = panel.Controls.OfType<Label>().FirstOrDefault();
                if (lblSessionNumber != null)
                {
                    lblSessionNumber.Text = "Session " + newSessionNumber;
                }
                newSessionNumber++;
            }

            sessionNumber = newSessionNumber;
        }
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string title = txtCourseTitle.Text.Trim();
            string description = txtCourseDescription.Text.Trim();
            var selectedCategory = cbCategory.SelectedItem as CategoryItem;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || selectedCategory == null)
            {
                MessageBox.Show("Please fill in all course details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int categoryId = selectedCategory.CategoryId;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertCourseQuery = "INSERT INTO UniversityAlma.Course (Title, Description, CategoryId, MentorId) VALUES (@Title, @Description, @CategoryId, @MentorId); SELECT SCOPE_IDENTITY();";
                    int courseId;
                    using (SqlCommand cmd = new SqlCommand(insertCourseQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmd.Parameters.AddWithValue("@MentorId", mentorId);
                        courseId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Insert sessions
                    for (int i = 1; i < sessionNumber; i++)
                    {
                        TextBox txtSessionTitle = flpSessions.Controls.Find("txtSessionTitle" + i, true).FirstOrDefault() as TextBox;
                        PictureBox pictureBoxMedia = flpSessions.Controls.Find("pictureBoxMedia" + i, true).FirstOrDefault() as PictureBox;

                        if (txtSessionTitle != null && pictureBoxMedia != null && pictureBoxMedia.Tag != null)
                        {
                            string sessionTitle = txtSessionTitle.Text.Trim();
                            string mediaPath = pictureBoxMedia.Tag.ToString();
                            int duration = 90;

                            string insertSessionQuery = "INSERT INTO UniversityAlma.Session (CourseId, Number, Title, Media, Duration) VALUES (@CourseId, @Number, @Title, @Media, @Duration)";
                            using (SqlCommand cmd = new SqlCommand(insertSessionQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CourseId", courseId);
                                cmd.Parameters.AddWithValue("@Number", i);
                                cmd.Parameters.AddWithValue("@Title", sessionTitle);
                                cmd.Parameters.AddWithValue("@Media", mediaPath);
                                cmd.Parameters.AddWithValue("@Duration", duration);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Course uploaded successfullly.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"An error occurred while uploading the course. Please try again.\n\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }   
}
