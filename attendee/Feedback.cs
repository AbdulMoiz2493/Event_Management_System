using DB_Project.attendee;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class Feedback : Form
    {
        // Connection string for the database
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";
        private int attendeeID; // Store the attendee's ID
        private int eventID;    // Store the event's ID (to be set when the form loads)

        public Feedback(int attendeeID, int eventID)
        {
            InitializeComponent();
            this.attendeeID = attendeeID;
            this.eventID = eventID;
        }

        private void Feedback_Load(object sender, EventArgs e)
        {
            // Populate the rating ComboBox with values 1 to 5
            comboBox1.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                comboBox1.Items.Add(i.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Handle selection logic for the feedback list if needed.
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Handle any logic when a rating is selected.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Retrieve feedback and rating from the controls
            string feedbackText = textBox1.Text.Trim(); // Fixed the issue here
            string selectedRating = comboBox1.SelectedItem?.ToString();

            // Validate input
            if (string.IsNullOrEmpty(feedbackText))
            {
                MessageBox.Show("Please provide your feedback.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(selectedRating) || !int.TryParse(selectedRating, out int rating))
            {
                MessageBox.Show("Please select a valid rating.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rating < 1 || rating > 5)
            {
                MessageBox.Show("Rating must be between 1 and 5.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Store feedback in the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "INSERT INTO AttendeeFeedbacks (EventID, AttendeeID, FeedbackText, Rating, FeedbackDate) " +
                                   "VALUES (@EventID, @AttendeeID, @FeedbackText, @Rating, GETDATE());";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventID", eventID);
                        cmd.Parameters.AddWithValue("@AttendeeID", attendeeID);
                        cmd.Parameters.AddWithValue("@FeedbackText", feedbackText);
                        cmd.Parameters.AddWithValue("@Rating", rating);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thank you for your feedback!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Optionally clear the controls after successful submission
                            textBox1.Clear(); // Clear TextBox
                            comboBox1.SelectedIndex = -1; // Reset ComboBox selection
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit feedback. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Open Profile.cs
            profile profileForm = new profile("abdul moiz"); // Pass attendeeID
            profileForm.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Open Search.cs
            search searchForm = new search(); // Pass attendeeID
            searchForm.Show();
            this.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Open Dashboard.cs
            Dashboard dashboardForm = new Dashboard(); // Pass attendeeID
            dashboardForm.Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic if needed for this label
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic if needed for this label
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic if needed for this label
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic if needed for this label
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic if needed for this label
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic for this PictureBox
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic for this PictureBox
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Optional: Add custom logic for this PictureBox
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Handle text changes in the TextBox if needed
        }
    }
}
