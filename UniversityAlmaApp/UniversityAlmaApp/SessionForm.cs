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
    public partial class SessionForm : Form
    {
        private int courseId;
        private int currentSessionNumber;
        private int totalSessions;
        public SessionForm(int courseId)
        {
            InitializeComponent();
            this.courseId = courseId;
            LoadTotalSessions();
            LoadSession(1);
        }

        private void LoadTotalSessions()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT COUNT(*) FROM UniversityAlma.Session WHERE CourseId = @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    conn.Open();
                    totalSessions = (int)cmd.ExecuteScalar();
                }
            }
        }

        private void LoadSession(int sessionNumber)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.connectionString))
            {
                string query = "SELECT Number, Title FROM UniversityAlma.Session WHERE CourseId = @CourseId AND Number = @Number";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@Number", sessionNumber);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentSessionNumber = reader.GetInt32(0);
                            lblSessionNumber.Text = "Session" + currentSessionNumber;
                            lblSessionTitle.Text = reader.GetString(1);
                        }
                    }
                }
            }
            btnPrev.Visible = currentSessionNumber > 1;
            btnNext.Visible = currentSessionNumber < totalSessions;

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentSessionNumber > 1)
            {
                LoadSession(currentSessionNumber - 1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentSessionNumber < totalSessions) { 
                LoadSession(currentSessionNumber + 1);
            }

        }
    }
}
