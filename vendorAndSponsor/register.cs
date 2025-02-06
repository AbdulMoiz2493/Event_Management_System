using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project.vendorAndSponsor
{
    public partial class register : Form
    {
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";

        public register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox6.Text;
            string email = textBox1.Text;
            string password = textBox3.Text;
            string contactNo = textBox4.Text;
            string businessType = textBox2.Text;
            string services = textBox5.Text;
            string UserType = "VendorSponsor";

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username, Email, and Password are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Ensure Email exists in Users table
                    string insertUserQuery = @"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
            BEGIN
                INSERT INTO Users (Email, Password, UserName, ContactNo, UserType)
                VALUES (@Email, @Password, @UserName, @ContactNo, @UserType)
            END";

                    using (SqlCommand command = new SqlCommand(insertUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@ContactNo", contactNo);
                        command.Parameters.AddWithValue("@UserType", UserType);
                        command.ExecuteNonQuery();
                    }

                    // Insert into Vendors table
                    string insertVendorQuery = @"
            INSERT INTO Vendors (UserName, Email, Password, ContactNo, BusinessType, ServicesOffered)
            VALUES (@UserName, @Email, @Password, @ContactNo, @BusinessType, @ServicesOffered)";

                    using (SqlCommand command = new SqlCommand(insertVendorQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@ContactNo", contactNo);
                        command.Parameters.AddWithValue("@BusinessType", businessType);
                        command.Parameters.AddWithValue("@ServicesOffered", services);
                        command.ExecuteNonQuery();
                    }

                    // Insert dummy data into Sponsors table
                    string insertSponsorQuery = @"
                        INSERT INTO Sponsors (UserName, Email, Password, ContactNo, CompanyName, SponsoredEvents, Budget)
                        VALUES (@UserName, @Email, @Password, @ContactNo, 'Company ZORO', '{""EventIDs"": [1, 2]}',10000 )";

                    using (SqlCommand command = new SqlCommand(insertSponsorQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@ContactNo", contactNo);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Redirect to Attendee Login form
            attendeeLogin attendeeLoginForm = new attendeeLogin();
            attendeeLoginForm.Show();
            this.Hide();
        }

        // Empty functions to retain the existing interface
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void UserName_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void pictureBox7_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void pictureBox6_Click(object sender, EventArgs e) { }
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}
