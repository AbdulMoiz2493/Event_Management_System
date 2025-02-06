using DB_Project.admin;
using DB_Project.attendee;
using DB_Project.organizer;
using DB_Project.vendorAndSponsor;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class attendeeLogin : Form
    {
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True;";

        public attendeeLogin()
        {
            InitializeComponent();
        }

        private void attendeeLogin_Load(object sender, EventArgs e)
        {
            // Initialize comboBox1 with user types
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("Attendee");
            comboBox1.Items.Add("Organizer");
            comboBox1.Items.Add("VendorSponsor");
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Navigate to attendee register form
            SignUpManager signupmanagerform = new SignUpManager();
            signupmanagerform.Show();
            this.Hide(); // Hide the login form
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string userType = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userType))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate user credentials
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT UserName, Password FROM Users WHERE UserName = @UserName AND UserType = @UserType AND Status = 'Active'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@UserType", userType);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Simulate password validation (Hashing would be better)
                        if (reader["Password"].ToString() == password)
                        {
                            reader.Close();
                            LaunchInterface(userType);
                        }
                        else
                        {
                            MessageBox.Show("Invalid password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("User not found or inactive.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void LaunchInterface(string userType)
        {
            

            switch (userType)
            {
                case "Admin":
                    userAndOrganizer adminForm = new userAndOrganizer();
                    adminForm.Show();
                    this.Hide(); // Hide the login form
                    break;

                case "Attendee":
                    search attendeeForm = new search();
                    attendeeForm.Show();
                    this.Hide(); // Hide the login form
                    break;

                case "Organizer":
                    createEvent organizerForm = new createEvent();
                    organizerForm.Show();
                    this.Hide(); // Hide the login form
                    break;

                case "VendorSponsor":
                    serviceAndSponsorshipBidding vendorSponsorForm = new serviceAndSponsorshipBidding();
                    vendorSponsorForm.Show();
                    this.Hide(); // Hide the login form
                    break;

                default:
                    MessageBox.Show("Invalid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
