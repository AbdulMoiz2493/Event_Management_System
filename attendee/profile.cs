using DB_Project.attendee;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class profile : Form
    {
        // Database connection string - MODIFY AS PER YOUR ENVIRONMENT
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";
        private string username;

        // Constructor receiving username
        public profile(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        // Form Load Event
        private void profile_Load(object sender, EventArgs e)
        {
            try
            {
                // Setup DataGridView columns first
                SetupUserDetailsGrid();
                SetupEventsGrid();

                // Load data after the columns are set
                LoadUserDetails();
                LoadApprovedEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Loading Profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Setup User Details DataGridView with columns
        private void SetupUserDetailsGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("UserName", "Username");
            dataGridView1.Columns.Add("Email", "Email");
            dataGridView1.Columns.Add("ContactNo", "Contact Number");
            dataGridView1.Columns.Add("Preferences", "Preferences");

            // Set column widths and formatting
            dataGridView1.Columns["UserName"].Width = 150;
            dataGridView1.Columns["Email"].Width = 200;
            dataGridView1.Columns["ContactNo"].Width = 120;
            dataGridView1.Columns["Preferences"].Width = 180;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Setup Events DataGridView with columns
        private void SetupEventsGrid()
        {
           
        }

        // Load User Details into DataGridView
        private void LoadUserDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    dataGridView1.Columns.Clear();
                    dataGridView1.Columns.Add("UserName", "Username");
                    dataGridView1.Columns.Add("Email", "Email");
                    dataGridView1.Columns.Add("ContactNo", "Contact Number");
                    dataGridView1.Columns.Add("Preferences", "Preferences");

                    // Set column widths and formatting
                    dataGridView1.Columns["UserName"].Width = 150;
                    dataGridView1.Columns["Email"].Width = 200;
                    dataGridView1.Columns["ContactNo"].Width = 120;
                    dataGridView1.Columns["Preferences"].Width = 180;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    string query = "SELECT UserName, Email, ContactNo, Preferences FROM Attendees WHERE UserName = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear any existing rows
                            dataGridView1.Rows.Clear();

                            if (reader.Read())
                            {
                                // Add the row manually
                                dataGridView1.Rows.Add(
                                    reader["UserName"].ToString(),
                                    reader["Email"].ToString(),
                                    reader["ContactNo"].ToString(),
                                    reader["Preferences"].ToString()
                                );
                            }
                            else
                            {
                                MessageBox.Show("No user details found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Loading User Details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Load Approved Events into DataGridView
        private void LoadApprovedEvents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    dataGridView2.Columns.Clear();
                    dataGridView2.Columns.Add("EventTitle", "Event Title");
                    dataGridView2.Columns.Add("EventLocation", "Location");
                    dataGridView2.Columns.Add("EventStartDate", "Start Date");
                    dataGridView2.Columns.Add("EventEndDate", "End Date");

                    // Set column widths and formatting
                    dataGridView2.Columns["EventTitle"].Width = 200;
                    dataGridView2.Columns["EventLocation"].Width = 150;
                    dataGridView2.Columns["EventStartDate"].Width = 120;
                    dataGridView2.Columns["EventEndDate"].Width = 120;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    string query = "SELECT EventTitle, EventLocation, EventStartDate, EventEndDate FROM Events WHERE Status = 'Approved'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear any existing rows
                            dataGridView2.Rows.Clear();

                            while (reader.Read())
                            {
                                // Add each row manually
                                dataGridView2.Rows.Add(
                                    reader["EventTitle"].ToString(),
                                    reader["EventLocation"].ToString(),
                                    ((DateTime)reader["EventStartDate"]).ToShortDateString(),
                                    ((DateTime)reader["EventEndDate"]).ToShortDateString()
                                );
                            }

                            // Check if no rows were added
                            if (dataGridView2.Rows.Count == 0)
                            {
                                MessageBox.Show("No approved events found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Loading Events: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Navigation Methods
        private void button2_Click(object sender, EventArgs e) // Update Profile
        {
            Update_Profile updateForm = new Update_Profile(username);
            updateForm.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e) // Search
        {
            search searchForm = new search();
            searchForm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e) // Dashboard
        {
            Dashboard dashboardForm = new Dashboard();
            dashboardForm.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e) // Feedback
        {
            Feedback feedbackForm = new Feedback(1,4);
            feedbackForm.Show();
            this.Hide();
        }

        // Optional: Reload Data Button
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        // Placeholder methods for additional event handlers
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LoadUserDetails();
            LoadApprovedEvents();
        }
    }
}