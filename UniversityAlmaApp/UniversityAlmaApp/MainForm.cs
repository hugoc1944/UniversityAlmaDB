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
    public partial class MainForm : Form
    {
        private int userInt;
        private string userName;
        public MainForm(int userId)
        {
            InitializeComponent();
            this.userInt = userId;
            this.userName = GetUserName(userId);
            lblUserName.Text = "Welcome, " + userName + "!";
            if (IsUserMentor(userId))
            {
                btnBecomeMentor.Visible = false;
            }

            LoadCategories();
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnBecomeMentor_Click(object sender, EventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm(); ;
            loginForm.ShowDialog();
            this.Close();
        }



        private bool IsUserMentor(int userId)
        {
            bool isMentor = false;
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT COUNT(1) FROM UniversityAlma.Mentor WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isMentor = true;
                    }
                }
            }
            return isMentor;
        }

        private string GetUserName(int userId)
        {
            string name = string.Empty;
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT Name FROM UniversityAlma.Profile p JOIN UniversityAlma.[User] u ON p.ProfileId = u.ProfileId WHERE u.UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    name = (string)cmd.ExecuteScalar();
                }
            }
            return name;
        }

        private void LoadCategories()
        {
            panelCategories.Controls.Clear();
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT CategoryId, Type FROM UniversityAlma.Category ORDER BY CategoryId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryId = reader.GetInt32(0);
                            string categoryType = reader.GetString(1);

                            Button categoryButton = new Button()
                            {
                                Text = categoryType,
                                Tag = categoryId,
                                AutoSize = true,
                                Padding = new Padding(10)
                            };
                            categoryButton.Click += CategoryButtonClick;
                            panelCategories.Controls.Add(categoryButton);
                        }
                    }
                }
            }
            // Load courses for the first category
            if (panelCategories.Controls.Count > 0)
            {
                Button firstCategoryButton = (Button)panelCategories.Controls[0];
                firstCategoryButton.PerformClick();
            }
        }

        private void CategoryButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int categoryId = (int)clickedButton.Tag;
            LoadCourses(categoryId);
        }

        private void LoadCourses(int categoryId)
        {
            panelCourses.Controls.Clear();
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = @"
                    SELECT c.CourseId, c.Title, c.Description, COUNT(s.SessionId) AS SessionCount
                    FROM UniversityAlma.Course c
                    LEFT JOIN UniversityAlma.Session s ON c.CourseId = s.CourseId
                    WHERE c.CategoryId = @CategoryId
                    GROUP BY c.CourseId, c.Title, c.Description";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int courseId = reader.GetInt32(0);
                            string courseTitle = reader.GetString(1);
                            string courseDescription = reader.GetString(2);
                            int sessionCount = reader.GetInt32(3);

                            Panel coursePanel = new Panel
                            {
                                BorderStyle = BorderStyle.FixedSingle,
                                Padding = new Padding(10),
                                Margin = new Padding(10),
                                Size = new Size(200, 200),
                            };

                            TableLayoutPanel courseLayout = new TableLayoutPanel
                            {
                                RowCount = 4,
                                ColumnCount = 1,
                                Dock = DockStyle.Fill
                            };

                            Label lblTitle = new Label
                            {
                                Text = courseTitle,
                                Font = new Font("Open Sans", 12F, FontStyle.Bold),
                                AutoSize = true
                            };

                            Label lblDescription = new Label
                            {
                                Text = courseDescription,
                                Font = new Font("Segoe UI", 10F),
                                AutoSize = false,
                                Height = 60,
                                Dock = DockStyle.Fill,
                                TextAlign = ContentAlignment.TopLeft
                            };

                            Label lblSessionCount = new Label
                            {
                                Text = sessionCount + " Sessions",
                                Font = new Font("Segoe UI", 10F),
                                AutoSize = true
                            };

                            Button btnPlay = new Button
                            {
                                Text = "Play",
                                Tag = courseId,
                                AutoSize = true
                            };

                            btnPlay.Click += BtnPlayClick;

                            courseLayout.Controls.Add(lblTitle, 0, 0);
                            courseLayout.Controls.Add(lblDescription, 0, 1);
                            courseLayout.Controls.Add(lblSessionCount, 0, 2);
                            courseLayout.Controls.Add(btnPlay, 0, 3);

                            coursePanel.Controls.Add(courseLayout);
                            panelCourses.Controls.Add(coursePanel);
                        }
                    }
                }
            }
        } 
    
        private void BtnPlayClick(object sender, EventArgs e)
        {
            Button playButton = (Button)sender;
            int courseId = (int)playButton.Tag;

            // Course play logic
            MessageBox.Show("Playing course with ID: " + courseId);
        }
    }
}
