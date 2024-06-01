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
        private Panel panelUsers;
        private Panel panelProfile;
        public MainForm(int userId)
        {
            InitializeComponent();
            this.userInt = userId;
            this.userName = GetUserName(userId);
            lblUserName.Text = "Welcome, " + userName + "!";
            btnUpload.Visible = false;
            if (IsUserAdmin(userId))
            {
                btnBecomeMentor.Visible = false;
                btnUser.Visible = true;
            }
            else
            {
                btnUser.Visible = false;
                if (IsUserMentor(userId))
                {
                    btnBecomeMentor.Visible = false;
                    btnUpload.Visible = true;
                }
            }
            btnUpload.Click += BtnUpload_Click;

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
            panelUsers = new Panel
            {
                Dock = DockStyle.None,
                Size = new Size(700, 300),
                Location = new Point(48, 185),
                Visible = false // Initially hidden
            };
            Controls.Add(panelUsers);
            panelProfile = new Panel
            {
                Dock = DockStyle.None,
                Size = new Size(700, 300),
                Location = new Point(48, 185),
                Visible = true // Show profile panel
            };
            Controls.Add(panelProfile);
            btnUser.Click += btnUser_Click;

            panelCourseContainer.Controls.Add(panelCourses);
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
            panelUsers.Visible = false;
            panelCategories.Visible = true;
            panelCourses.Visible = true;
            cbSorting.Visible = true;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            panelProfile.Visible = false;
            panelCourseContainer.Visible = true;
            LoadCategories();
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            panelNotifications.Visible = true;
            panelUsers.Visible = false;
            panelCategories.Visible = false;
            panelCourses.Visible = false;
            panelCourseContainer.Visible = false;
            cbSorting.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            panelProfile.Visible = false;
            LoadNotifications();
        }
        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using var uploadForm = new UploadCourse(GetMentorId(userInt));
            {
                uploadForm.ShowDialog();
            }
        }
        private void btnProfile_Click(object sender, EventArgs e)
        {
            panelNotifications.Visible = false;
            panelUsers.Visible = false;
            panelCategories.Visible = false;
            panelCourses.Visible = false;
            panelCourseContainer.Visible = false;
            cbSorting.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            panelProfile.Visible = true;
            LoadUserProfile();
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
        private bool IsUserAdmin(int userId)
        {
            bool isAdmin = false;
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT COUNT(1) FROM UniversityAlma.Admin WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isAdmin = true;
                    }
                }
            }
            return isAdmin;
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
        private int GetMentorId(int userId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT MentorId FROM UniversityAlma.Mentor WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
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
                    query = $"SELECT CourseId, Title, Description, SessionCount, FavCount, MentorId, IsDeleted FROM UniversityAlma.vwCourseDetails WHERE CategoryId = @CategoryId AND IsDeleted = 0 ORDER BY {sortingCriteria}";
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
                        int courseMentorId = reader.GetInt32(5);
                        int height = IsUserAdmin(userInt) || IsUserMentor(userInt) ? 265 : 225;

                        Panel coursePanel = new Panel
                        {
                            BorderStyle = BorderStyle.FixedSingle,
                            Padding = new Padding(10),
                            Margin = new Padding(10),
                            Size = new Size(215, height),


                        };

                        TableLayoutPanel courseLayout = new TableLayoutPanel
                        {
                            RowCount = 4,
                            ColumnCount = 1,
                            Dock = DockStyle.Fill,
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

                        // Add delte btn if user is admin
                        if (IsUserAdmin(userInt) || (IsUserMentor(userInt) && courseMentorId == GetMentorId(userInt)))
                        {
                            Button btnDelete = new Button
                            {
                                Text = "Delete",
                                Tag = courseId,
                                AutoSize = true
                            };
                            btnDelete.Click += BtnDeleteClick;
                            courseLayout.Controls.Add(btnDelete, 0, 6);
                        }

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
            using var sessionForm = new SessionForm(courseId);
            sessionForm.ShowDialog();
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
        private void BtnDeleteClick(object sender, EventArgs e)
        {
            Button deleteButton = (Button)sender;
            int courseId = (int)deleteButton.Tag;
            DialogResult result = MessageBox.Show("Are you sure you want to delete this course?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Retrieve the course name
                        string courseName = "";
                        int courseCreatorUserId = 0;
                        string getCourseDetailsQuery = @"
                            SELECT c.Title, m.UserId
                            FROM UniversityAlma.Course c
                            JOIN UniversityAlma.Mentor m ON c.MentorId = m.MentorId
                            WHERE c.CourseId = @CourseId";
                        using (SqlCommand cmd = new SqlCommand(getCourseDetailsQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CourseId", courseId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    courseName = reader.GetString(0);
                                    courseCreatorUserId = reader.GetInt32(1);
                                }
                            }
                        }
                        string deleteQuery = "UPDATE UniversityAlma.Course SET IsDeleted = 1 WHERE CourseId = @CourseId";
                        using(SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CourseId", courseId);
                            cmd.ExecuteScalar();
                        }
                        // Create Audit Entry
                        if (IsUserAdmin(userInt))
                        {
                            string insertAuditQuery = @"
                            INSERT INTO UniversityAlma.Audits (AdminId, UserId, CourseId, AuditTypeId, Date)
                            VALUES (@AdminId, @UserId, @CourseId, @AuditTypeId, GETDATE())";
                            using (SqlCommand cmd = new SqlCommand(insertAuditQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@AdminId", GetAdminId(userInt));
                                cmd.Parameters.AddWithValue("@UserId", courseCreatorUserId);
                                cmd.Parameters.AddWithValue("@CourseId", courseId);
                                cmd.Parameters.AddWithValue("@AuditTypeId", 4);
                                cmd.ExecuteNonQuery();
                            }

                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"An error occurred while deleting the course. Please try again.\n\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Reload courses
                LoadCourses(categoryId);
            }
        }
        private int GetAdminId(int userId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT AdminId FROM UniversityAlma.Admin WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
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

        private void LoadUsers()
        {
            DataGridView dgvUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panelUsers.Controls.Clear();
            panelUsers.Controls.Add(dgvUsers);
            DataTable usersTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT Name, Username, Age, Email, Mentor FROM UniversityAlma.vwUserDetails";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(usersTable);
                }
            }
            dgvUsers.DataSource = usersTable;
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            panelNotifications.Visible = false;
            panelCategories.Visible = false;
            panelCourses.Visible = false;
            panelCourseContainer.Visible = false;
            panelUsers.Visible = true;
            cbSorting.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            LoadUsers();
        }

        private void LoadUserProfile()
        {

            PictureBox pictureBoxProfile = new PictureBox
            {
                Location = new Point(20, 20),
                Size = new Size(150, 150),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            panelProfile.Controls.Add(pictureBoxProfile);

            Label lblName = new Label { Location = new Point(200, 20), AutoSize = true };
            panelProfile.Controls.Add(lblName);
            Label lblUsername = new Label { Location = new Point(200, 50), AutoSize = true };
            panelProfile.Controls.Add(lblUsername);
            Label lblEmail = new Label { Location = new Point(200, 80), AutoSize = true };
            panelProfile.Controls.Add(lblEmail);
            Label lblPhoneNumber = new Label { Location = new Point(200, 110), AutoSize = true };
            panelProfile.Controls.Add(lblPhoneNumber);
            Label lblBirthday = new Label { Location = new Point(200, 140), AutoSize = true };
            panelProfile.Controls.Add(lblBirthday);

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT Name, Username, Email, PhoneNumber, Birthday, ProfilePic FROM UniversityAlma.Profile p JOIN UniversityAlma.[User] u ON p.ProfileId = u.ProfileId WHERE u.UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userInt);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblName.Text = "Name: " + reader.GetString(0);
                            lblUsername.Text = "Username: " + reader.GetString(1);
                            lblEmail.Text = "Email: " + reader.GetString(2);
                            lblPhoneNumber.Text = "Phone Number: " + reader.GetString(3);
                            lblBirthday.Text = "Birthday: " + reader.GetDateTime(4).ToShortDateString();
                            if (!reader.IsDBNull(5))
                            {
                                byte[] profilePic = (byte[])reader[5];
                                using (var ms = new System.IO.MemoryStream(profilePic))
                                {
                                    pictureBoxProfile.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                pictureBoxProfile.Image = Properties.Resources.defaultPfp;
                            }
                        }
                    }
                }
            }
        }

    }
}
