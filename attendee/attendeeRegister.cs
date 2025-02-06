using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class attendeeRegister : Form
    {
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";

        public attendeeRegister()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void textBox4_TextChanged(object sender, EventArgs e) { }

        private void textBox5_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve values from textboxes
            string userName = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string contactNo = textBox4.Text.Trim();
            string preferences = textBox5.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(contactNo))
            {
                MessageBox.Show("All fields except preferences are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the email already exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    SqlCommand checkEmailCommand = new SqlCommand(checkEmailQuery, connection);
                    checkEmailCommand.Parameters.AddWithValue("@Email", email);

                    int emailCount = (int)checkEmailCommand.ExecuteScalar();
                    if (emailCount > 0)
                    {
                        MessageBox.Show("An account with this email already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Insert into Users table
                    string insertUserQuery = "INSERT INTO Users (UserName, Email, Password, ContactNo, UserType, Status) " +
                                             "VALUES (@UserName, @Email, @Password, @ContactNo, 'Attendee', 'Active')";
                    SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, connection);
                    insertUserCommand.Parameters.AddWithValue("@UserName", userName);
                    insertUserCommand.Parameters.AddWithValue("@Email", email);
                    insertUserCommand.Parameters.AddWithValue("@Password", password); // Hash passwords in production
                    insertUserCommand.Parameters.AddWithValue("@ContactNo", contactNo);

                    insertUserCommand.ExecuteNonQuery();

                    // Insert into Attendees table
                    string insertAttendeeQuery = "INSERT INTO Attendees (UserName, Email, Password, ContactNo, Preferences) " +
                                                 "VALUES (@UserName, @Email, @Password, @ContactNo, @Preferences)";
                    SqlCommand insertAttendeeCommand = new SqlCommand(insertAttendeeQuery, connection);
                    insertAttendeeCommand.Parameters.AddWithValue("@UserName", userName);
                    insertAttendeeCommand.Parameters.AddWithValue("@Email", email);
                    insertAttendeeCommand.Parameters.AddWithValue("@Password", password); // Hash passwords in production
                    insertAttendeeCommand.Parameters.AddWithValue("@ContactNo", contactNo);
                    insertAttendeeCommand.Parameters.AddWithValue("@Preferences", string.IsNullOrEmpty(preferences) ? DBNull.Value : (object)preferences);

                    insertAttendeeCommand.ExecuteNonQuery();

                    MessageBox.Show("Registration successful! Please log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Redirect to login form
                    attendeeLogin loginForm = new attendeeLogin();
                    loginForm.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Redirect to login form
            attendeeLogin loginForm = new attendeeLogin();
            loginForm.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            attendeeLogin loginForm = new attendeeLogin();
            loginForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void UserName_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }

        private void label6_Click(object sender, EventArgs e) { }

        private void pictureBox6_Click(object sender, EventArgs e) { }

        private void pictureBox5_Click(object sender, EventArgs e) { }

        private void pictureBox4_Click(object sender, EventArgs e) { }

        private void pictureBox3_Click(object sender, EventArgs e) { }

        private void pictureBox2_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void attendeeRegister_Load(object sender, EventArgs e)
        {

        }
    }
}
