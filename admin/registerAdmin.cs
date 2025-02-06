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

namespace DB_Project.admin
{
    public partial class registerAdmin : Form
    {
        private ErrorProvider errorProvider1;
        string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True;";

        public registerAdmin()
        {
            InitializeComponent();

            // Initialize the ErrorProvider
            errorProvider1 = new ErrorProvider();
        }

        // Event handler for the UserName (textBox1) field
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string userName = textBox1.Text;

            // Check if the username field is empty
            if (string.IsNullOrWhiteSpace(userName))
            {
                errorProvider1.SetError(textBox1, "UserName cannot be empty.");
            }
            // Check if the username is too short
            else if (userName.Length < 3)
            {
                errorProvider1.SetError(textBox1, "UserName must be at least 3 characters long.");
            }
            else
            {
                // Clear the error if input is valid
                errorProvider1.SetError(textBox1, string.Empty);
            }
        }

        // Event handler for the email (textBox2) field
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string email = textBox2.Text;

            // Validate email
            if (string.IsNullOrWhiteSpace(email))
            {
                errorProvider1.SetError(textBox2, "Email cannot be empty.");
            }
            else if (!email.Contains("@") || !email.Contains("."))
            {
                errorProvider1.SetError(textBox2, "Enter a valid email address.");
            }
            else
            {
                // Clear the error if input is valid
                errorProvider1.SetError(textBox2, string.Empty);
            }
        }

        // Event handler for the password (textBox3) field
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string password = textBox3.Text;

            // Validate password
            if (string.IsNullOrWhiteSpace(password))
            {
                errorProvider1.SetError(textBox3, "Password cannot be empty.");
            }
            else if (password.Length < 6)
            {
                errorProvider1.SetError(textBox3, "Password must be at least 6 characters long.");
            }
            else
            {
                // Clear the error if input is valid
                errorProvider1.SetError(textBox3, string.Empty);
            }
        }

        // Event handler for the contact number (textBox4) field
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string contactNumber = textBox4.Text;

            // Validate contact number
            if (string.IsNullOrWhiteSpace(contactNumber))
            {
                errorProvider1.SetError(textBox4, "Contact Number cannot be empty.");
            }
            else if (contactNumber.Length < 10 || contactNumber.Length > 15)
            {
                // Validate length: Assuming a valid contact number has between 10 to 15 digits
                errorProvider1.SetError(textBox4, "Contact Number must be between 10 and 15 digits.");
            }
            else if (!contactNumber.All(char.IsDigit))
            {
                // Validate that all characters are digits
                errorProvider1.SetError(textBox4, "Contact Number must only contain digits.");
            }
            else
            {
                // Clear the error if input is valid
                errorProvider1.SetError(textBox4, string.Empty);
            }
        }

        // Event handler for the register button (button1) click event
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if there are validation errors
            if (!string.IsNullOrWhiteSpace(errorProvider1.GetError(textBox1)) ||
                !string.IsNullOrWhiteSpace(errorProvider1.GetError(textBox2)) ||
                !string.IsNullOrWhiteSpace(errorProvider1.GetError(textBox3)) ||
                !string.IsNullOrWhiteSpace(errorProvider1.GetError(textBox4)))
            {
                MessageBox.Show("Please fix the errors before submitting.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If no errors, process registration
            string userName = textBox1.Text;
            string password = textBox3.Text; // Corrected to textBox3 for password
            string email = textBox2.Text;
            string contactNumber = textBox4.Text; // Added contact number

            // You may want to hash the password before storing it (optional)
            // Example:
            // password = BCrypt.Net.BCrypt.HashPassword(password); // If you are using BCrypt

            // Establish the database connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL Query to insert the admin details into the Users table
                    string query = "INSERT INTO Users (UserName, Password, Email, ContactNo, UserType, Status, RegistrationDate) VALUES (@UserName, @Password, @Email, @ContactNo, 'Admin', 'Active', GETDATE())";

                    // Prepare the SQL command
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use parameterized queries to prevent SQL Injection
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password); // You may hash the password here
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@ContactNo", contactNumber); // Add contact number

                        // Execute the command
                        command.ExecuteNonQuery();
                    }

                    // Show success message
                    MessageBox.Show($"Admin registered successfully!\n\nUsername: {userName}\nEmail: {email}\nContact: {contactNumber}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear all fields after successful registration
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    textBox3.Text = string.Empty;
                    textBox4.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    // Display error message in case of failure
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Check if the email exists in the Users table
        private bool EmailExistsInUsers(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND UserType = 'Admin'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            attendeeLogin loginForm = new attendeeLogin();
            loginForm.Show();
            this.Hide();
        }

        private void registerAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
