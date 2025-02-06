using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class organizerRegistration : Form
    {
        public organizerRegistration()
        {
            InitializeComponent();
        }

        // LinkLabel1 Click Event - Navigate to Attendee Login Form
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var attendeeLoginForm = new attendeeLogin();  // Assuming this form exists
            attendeeLoginForm.Show();
            this.Hide(); // Hide the current form
        }

        // Button1 Click Event - Register Organizer
        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox5.Text; // Organizer username
            string organizationName = textBox1.Text; // Organization name
            string email = textBox2.Text; // Email
            string password = textBox3.Text; // Password
            string contactNo = textBox4.Text; // Contact number

            // Basic validation for empty fields
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(organizationName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(contactNo))
            {
                MessageBox.Show("Please fill in all the fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Email Format (simple validation)
            if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Database connection string
            string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if email already exists in Users table
                    string checkUserEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(checkUserEmailQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        int userEmailCount = (int)command.ExecuteScalar();

                        if (userEmailCount > 0)
                        {
                            MessageBox.Show("The email is already registered in the system. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Insert the organizer data into Users table first
                    string insertUserQuery = @"
                        INSERT INTO Users (UserName, Email, Password, ContactNo, UserType)
                        VALUES (@UserName, @Email, @Password, @ContactNo, 'Organizer')";

                    using (SqlCommand userCommand = new SqlCommand(insertUserQuery, connection))
                    {
                        userCommand.Parameters.AddWithValue("@UserName", userName);
                        userCommand.Parameters.AddWithValue("@Email", email);
                        userCommand.Parameters.AddWithValue("@Password", password); // You should hash the password before storing it
                        userCommand.Parameters.AddWithValue("@ContactNo", contactNo);

                        int userRowsAffected = userCommand.ExecuteNonQuery();

                        if (userRowsAffected > 0)
                        {
                            // Insert new organizer into the Organizers table after successful user registration
                            string insertOrganizerQuery = @"
                                INSERT INTO Organizers (UserName, Email, Password, ContactNo, OrganizationName)
                                VALUES (@UserName, @Email, @Password, @ContactNo, @OrganizationName)";

                            using (SqlCommand organizerCommand = new SqlCommand(insertOrganizerQuery, connection))
                            {
                                organizerCommand.Parameters.AddWithValue("@UserName", userName);
                                organizerCommand.Parameters.AddWithValue("@Email", email);
                                organizerCommand.Parameters.AddWithValue("@Password", password); // You should hash the password before storing it
                                organizerCommand.Parameters.AddWithValue("@ContactNo", contactNo);
                                organizerCommand.Parameters.AddWithValue("@OrganizationName", organizationName);

                                int organizerRowsAffected = organizerCommand.ExecuteNonQuery();

                                if (organizerRowsAffected > 0)
                                {
                                    MessageBox.Show("Organizer registered successfully in both systems!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.ClearFields();  // Clear the fields after registration
                                }
                                else
                                {
                                    MessageBox.Show("An error occurred while registering the organizer in Organizers table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while registering the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Function to clear all fields after successful registration
        private void ClearFields()
        {
            textBox5.Clear(); // Username
            textBox1.Clear(); // Organization Name
            textBox2.Clear(); // Email
            textBox3.Clear(); // Password
            textBox4.Clear(); // Contact No
        }

        // Optional: You can implement other picture box or label click events as needed
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void UserName_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void pictureBox6_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}
