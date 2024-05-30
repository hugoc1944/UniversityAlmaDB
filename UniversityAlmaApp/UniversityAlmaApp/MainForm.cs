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
        private int categoryId;
        private Panel panelNotifications;
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

            cbSorting.Items.Add("Oldest");
            cbSorting.Items.Add("Newest");
            cbSorting.Items.Add("Most Favorites");
            cbSorting.Items.Add("Least Favorites");
            cbSorting.SelectedIndex = 0; //Default sorting option
            cbSorting.SelectedIndexChanged += cbSortingSelectedIndexChanged;
            cbSorting.Visible = false; // Hide sorting ComboBox initially

            lblSearch.Visible = false;
            txtSearch.Visible = false;
            txtSearch.TextChanged += txtSearchTextChanged;

            LoadCategories();

            // Initialize notifications panel
            panelNotifications = new FlowLayoutPanel
            {
                AutoScroll = true,
                Dock = DockStyle.None,
                Size = new Size(500, 400),
                Location = new Point(48, 200),
                Visible = false // Initially hidden
            };
            Controls.Add(panelNotifications);
        }

        private void cbSortingSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses(categoryId);
        }
        private void txtSearchTextChanged(object sender, EventArgs e)
        {
            LoadCourses(categoryId);
        }
        private void btnCourses_Click(object sender, EventArgs e)
        {
            panelNotifications.Visible = false;
            panelCategories.Visible = true;
            panelCourses.Visible = true;
            cbSorting.Visible = true;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            LoadCategories();
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            panelNotifications.Visible = true;
            panelCategories.Visible = false;
            panelCourses.Visible = false;
            cbSorting.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            LoadNotifications();
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
            categoryId = (int)clickedButton.Tag;
            cbSorting.Visible = true; // Show sorting ComboBox
            lblSearch.Visible = true;
            txtSearch.Visible = true;
            LoadCourses(categoryId);
        }

        private void LoadCourses(int categoryId)
        {
            panelCourses.Controls.Clear();
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string sortingCriteria = "";
                switch (cbSorting.SelectedItem.ToString())
                {
                    case "Oldest":
                        sortingCriteria = "CourseId ASC";
                        break;
                    case "Newest":
                        sortingCriteria = "CourseId DESC";
                        break;
                    case "Most Favorites":
                        sortingCriteria = "FavCount DESC";
                        break;
                    case "Least Favorites":
                        sortingCriteria = "FavCount ASC";
                        break;
                }
                string searchTerm = txtSearch.Text.Trim();
                string query;
                SqlCommand cmd;
                if (string.IsNullOrEmpty(searchTerm))
                {
                    query = $"SELECT CourseId, Title, Description, SessionCount, FavCount FROM UniversityAlma.vwCourseDetails WHERE CategoryId = @CategoryId ORDER BY {sortingCriteria}";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                }
                else
                {
                    query = $"SELECT CourseId, Title, Description, SessionCount, FavCount FROM UniversityAlma.fnSearchCourses(@CategoryId, @SearchTerm) ORDER BY {sortingCriteria}";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                }
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int courseId = reader.GetInt32(0);
                            string courseTitle = reader.GetString(1);
                            string courseDescription = reader.GetString(2);
                            long sessionCount = reader.GetInt64(3);
                            int favCount = reader.GetInt32(4);

                            Panel coursePanel = new Panel
                            {
                                BorderStyle = BorderStyle.FixedSingle,
                                Padding = new Padding(10),
                                Margin = new Padding(10),
                                Size = new Size(215, 225),
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

                            Label lblFav = new Label
                            {
                                Text = $"{favCount} Favorites",
                                Font = new Font("Segoe UI", 10F),
                                AutoSize = true
                            };

                            Button btnPlay = new Button
                            {
                                Text = "Play",
                                Tag = courseId,
                                AutoSize = true
                            };

                            Button btnFav = new Button
                            {
                                Text = IsFavorite(courseId) ? "Unfavourite" : "Favourite",
                                Tag = courseId,
                                AutoSize = true
                            };

                            btnPlay.Click += BtnPlayClick;
                            btnFav.Click += BtnFavClick;

                            courseLayout.Controls.Add(lblTitle, 0, 0);
                            courseLayout.Controls.Add(lblDescription, 0, 1);
                            courseLayout.Controls.Add(lblSessionCount, 0, 2);
                            courseLayout.Controls.Add(lblFav, 0, 3);
                            courseLayout.Controls.Add(btnPlay, 0, 4);
                            courseLayout.Controls.Add(btnFav, 0, 5);

                            coursePanel.Controls.Add(courseLayout);
                            panelCourses.Controls.Add(coursePanel);
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

        private void BtnFavClick(object sender, EventArgs e)
        {
            Button favButton = (Button)sender;

            if (favButton.Tag == null)
            {
                MessageBox.Show("Course ID not found");
                return;
            }

            int courseId = (int)favButton.Tag;
            if (IsFavorite(courseId))
            {
                RemoveFavorite(courseId);
                favButton.Text = "Favourite";
            }
            else
            {
                AddFavorite(courseId);
                favButton.Text = "Unfavourite";
            }
            LoadCourses(categoryId);
        }

        private bool IsFavorite(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT 1 FROM UniversityAlma.Favorites WHERE ProfileId = @ProfileId AND CourseId = @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProfileId", userInt);
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    conn.Open();
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        private void AddFavorite(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "EXEC UniversityAlma.AddFavorite @ProfileId, @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProfileId", userInt);
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    conn.Open();
                    cmd.ExecuteScalar();
                }
            }
        }
        private void RemoveFavorite(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "EXEC UniversityAlma.RemoveFavorite @ProfileId, @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProfileId", userInt);
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    conn.Open();
                    cmd.ExecuteScalar();
                }
            }
        }

        // Load Notifications
        private void LoadNotifications()
        {
            panelNotifications.Controls.Clear();
            panelNotifications.AutoScroll = true;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT NotificationId, Title, Info FROM UniversityAlma.Notification WHERE UserId = @UserId AND Checked = 0";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userInt);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int notificationId = reader.GetInt32(0);
                            string title = reader.GetString(1);
                            string info = reader.GetString(2);

                            Panel notificationPanel = new Panel
                            {
                                BorderStyle = BorderStyle.FixedSingle,
                                Padding = new Padding(10),
                                Margin = new Padding(10),
                                Size = new Size(400, 110),
                            };
                            TableLayoutPanel notificationLayout = new TableLayoutPanel
                            {
                                RowCount = 3,
                                ColumnCount = 1,
                                Dock = DockStyle.Fill
                            };
                            Label lblTitle = new Label
                            {
                                Text = title,
                                Font = new Font("Open Sans", 12F, FontStyle.Bold),
                                AutoSize = true
                            };
                            Label lblInfo = new Label
                            {
                                Text = info,
                                Font = new Font("Segoe UI", 10F),
                                AutoSize = true
                            };
                            Button btnCheck = new Button
                            {
                                Text = "Check",
                                Tag = notificationId,
                                AutoSize = true
                            };

                            btnCheck.Click += BtnCheckClick;

                            notificationLayout.Controls.Add(lblTitle, 0, 0);
                            notificationLayout.Controls.Add(lblInfo, 0, 1);
                            notificationLayout.Controls.Add(btnCheck, 0, 2);

                            notificationPanel.Controls.Add(notificationLayout);
                            panelNotifications.Controls.Add(notificationPanel);
                        }
                    }
                }
            }
        }
        private void BtnCheckClick(object sender, EventArgs e)
        {
            Button checkButton = (Button)sender;
            int notificationId = (int)checkButton.Tag;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "UPDATE UniversityAlma.Notification SET Checked = 1 WHERE NotificationId = @NotificationId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NotificationId", notificationId);
                    conn.Open();
                    cmd.ExecuteScalar();
                }
            }

            // Reload notifications
            LoadNotifications();
        }
    }
}
