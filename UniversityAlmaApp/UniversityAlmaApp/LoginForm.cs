using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversityAlmaApp
{
    public partial class LoginForm : Form
    {
        public int LoggedInUserId { get; private set; }
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
            int userId;
            bool loginSuccess = DatabaseHelper.ExecuteLogin("UniversityAlma.LoginUser", parameters, out userId);
            if (loginSuccess)
            {
                LoggedInUserId = userId;
                MessageBox.Show("Login successful! UserId: " + userId);

                // Open MainForm
                MainForm mainForm = new MainForm(userId);
                this.Hide();
                mainForm.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or passowrd.");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Hide login form
            this.Hide();

            // Show the register form
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();

            
        }
    }
}
