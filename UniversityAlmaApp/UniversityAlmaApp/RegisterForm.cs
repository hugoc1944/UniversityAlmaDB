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
    public partial class RegisterForm : Form
    {
        private byte[] profilePicData;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
               string.IsNullOrWhiteSpace(txtPassword.Text) ||
               string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                cmbGender.SelectedItem == null ||
                dtpBirthday.Value == null)
            {
                MessageBox.Show("Please fill in all fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculate age
            DateTime birthday = dtpBirthday.Value;
            int age = DateTime.Now.Year - birthday.Year;
            if (birthday > DateTime.Now.AddYears(-age)) age--;
            if (age < 16)
            {
                MessageBox.Show("You must be at least 16 years old to register.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = txtName.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string gender = cmbGender.SelectedItem.ToString();
            bool mailList = chkMailList.Checked;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", name),
                new SqlParameter("@Password", password),
                new SqlParameter("@Email", email),
                new SqlParameter("@Username", username),
                new SqlParameter("@PhoneNumber", phoneNumber),
                new SqlParameter("@Gender", gender),
                new SqlParameter("@ProfilePic", SqlDbType.VarBinary) {Value = ( profilePicData ?? (object)DBNull.Value )},
                new SqlParameter("@Birthday", birthday),
                new SqlParameter("@MailList", mailList)
            };

            // Call stored procedure
            string message;
            int userId = DatabaseHelper.RegisterUser("UniversityAlma.RegisterUser", parameters, out message);
            // Display the result message
            MessageBox.Show(message);
            if (userId > 0)
            {
                // Registration successfull
                MainForm mainForm = new MainForm(userId);
                this.Hide();
                mainForm.ShowDialog();
                this.Show();
            }


        }

        private void pictureBoxProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    pictureBoxProfilePic.Image = Image.FromFile(filePath);
                    profilePicData = File.ReadAllBytes(filePath);
                }
            }
        }
        private void txtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numeric input
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            // Hide the register form
            this.Hide();

            // Show login form
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}
