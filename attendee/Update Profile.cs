using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class Update_Profile : Form
    {
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";
        private string username;  // Store the logged-in username

        // Constructor to initialize the form and accept the logged-in username
        public Update_Profile(string loggedInUsername)
        {
            InitializeComponent();
            username = loggedInUsername;  // Store the logged-in username
        }

        // Event handler when form loads to fetch current user data
        private void Update_Profile_Load(object sender, EventArgs e)
        {
            LoadUserData();  // Load user data into textboxes when form is loaded
        }

        // Method to fetch current user data and display it in the textboxes
        private void LoadUserData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch basic user data from Users table (username, email, password, contact)
                    string query = "SELECT Username, Email, Password, ContactNo FROM Users WHERE Username = @Username";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Username", username);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Bind data to TextBoxes
                        textBox1.Text = reader["Username"].ToString();
                        textBox2.Text = reader["Email"].ToString();
                        textBox3.Text = reader["Password"].ToString();
                        textBox4.Text = reader["ContactNo"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("User not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    reader.Close();

                    // Now fetch Preferences from the Attendees table
                    string preferencesQuery = "SELECT Preferences FROM Attendees WHERE UserName = @Username";
                    SqlCommand preferencesCmd = new SqlCommand(preferencesQuery, connection);
                    preferencesCmd.Parameters.AddWithValue("@Username", username);

                    object preferencesResult = preferencesCmd.ExecuteScalar();
                    textBox5.Text = preferencesResult != DBNull.Value ? preferencesResult.ToString() : string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Event handler for the 'Update' button click event
        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs to ensure none are empty
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceed to update user data in the database
            UpdateUserProfile();
        }

        // Method to update user profile in the database
        private void UpdateUserProfile()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update basic user data in Users table (username, email, password, contact)
                    string updateUserQuery = "UPDATE Users SET Email = @Email, Password = @Password, ContactNo = @ContactNo WHERE Username = @Username";
                    SqlCommand updateUserCmd = new SqlCommand(updateUserQuery, connection);
                    updateUserCmd.Parameters.AddWithValue("@Username", username); // This should be the logged-in user's username
                    updateUserCmd.Parameters.AddWithValue("@Email", textBox2.Text);
                    updateUserCmd.Parameters.AddWithValue("@Password", textBox3.Text);
                    updateUserCmd.Parameters.AddWithValue("@ContactNo", textBox4.Text);

                    int rowsAffected = updateUserCmd.ExecuteNonQuery();

                    // Update Preferences in Attendees table
                    string updatePreferencesQuery = "UPDATE Attendees SET Preferences = @Preferences WHERE UserName = @Username";
                    SqlCommand updatePreferencesCmd = new SqlCommand(updatePreferencesQuery, connection);
                    updatePreferencesCmd.Parameters.AddWithValue("@Username", username);
                    updatePreferencesCmd.Parameters.AddWithValue("@Preferences", string.IsNullOrEmpty(textBox5.Text) ? DBNull.Value : (object)textBox5.Text);

                    updatePreferencesCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // After successful update, navigate back to profile form
                        profile profileForm = new profile("abdul moiz");  // Assuming profile form doesn't require parameters
                        profileForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Profile update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating user profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Placeholder for text changed events, can be removed if not needed
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }

        // Placeholder click event for pictureBox1, can be removed if not needed
        private void pictureBox1_Click(object sender, EventArgs e) { }

        // Placeholder click event for label4, can be removed if not needed
        private void label4_Click(object sender, EventArgs e) { }

        // Placeholder click event for pictureBox2, can be removed if not needed
        private void pictureBox2_Click(object sender, EventArgs e) { }

        // Placeholder click event for UserName label, can be removed if not needed
        private void UserName_Click(object sender, EventArgs e) { }

        // Placeholder click event for label2, can be removed if not needed
        private void label2_Click(object sender, EventArgs e) { }

        // Placeholder click event for pictureBox3, can be removed if not needed
        private void pictureBox3_Click(object sender, EventArgs e) { }

        // Placeholder click event for label1, can be removed if not needed
        private void label1_Click(object sender, EventArgs e) { }

        // Placeholder click event for pictureBox4, can be removed if not needed
        private void pictureBox4_Click(object sender, EventArgs e) { }

        // Placeholder click event for label5, can be removed if not needed
        private void label5_Click(object sender, EventArgs e) { }

        // Placeholder click event for pictureBox5, can be removed if not needed
        private void pictureBox5_Click(object sender, EventArgs e) { }

        // Placeholder click event for label6, can be removed if not needed
        private void label6_Click(object sender, EventArgs e) { }

        // Placeholder click event for pictureBox6, can be removed if not needed
        private void pictureBox6_Click(object sender, EventArgs e) { }

        private void Update_Profile_Load_1(object sender, EventArgs e)
        {

        }
    }
}
