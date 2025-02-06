using DB_Project.attendee;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class Dashboard : Form
    {
        private string connectionString = @"Data Source = DELL - LATITUDE - 7\SQLEXPRESS;Initial Catalog = EV; Integrated Security = True";

        public Dashboard()
        {
            InitializeComponent();
            ConfigureDataGrids();
        }

        private void ConfigureDataGrids()
        {
            // Configuring DataGridView1 for Registered Events
            dataGridView1.Columns.Add("EventID", "Event ID");
            dataGridView1.Columns.Add("EventTitle", "Event Title");
            dataGridView1.Columns.Add("BookingDate", "Booking Date");

            // Configuring DataGridView2 for Event Details
            dataGridView2.Columns.Add("EventID", "Event ID");
            dataGridView2.Columns.Add("EventTitle", "Event Title");
            dataGridView2.Columns.Add("EventDescription", "Description");
            dataGridView2.Columns.Add("EventLocation", "Location");
            dataGridView2.Columns.Add("StartDate", "Start Date");
            dataGridView2.Columns.Add("EndDate", "End Date");

            // Configuring DataGridView3 for Upcoming Schedules
            dataGridView3.Columns.Add("EventID", "Event ID");
            dataGridView3.Columns.Add("EventTitle", "Event Title");
            dataGridView3.Columns.Add("StartDate", "Start Date");
            dataGridView3.Columns.Add("EndDate", "End Date");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void none_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Redirecting to Profile.cs
            profile profileForm = new profile("username_here"); // Replace with the actual username
            profileForm.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Redirecting to Search.cs
            search searchForm = new search();
            searchForm.Show();
            this.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Placeholder for any required functionality
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Redirecting to Feedback.cs
            Feedback feedbackForm = new Feedback(1, 4); // Replace with actual AttendeeID and EventID
            feedbackForm.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Placeholder for handling cell clicks in DataGridView1
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Placeholder for handling cell clicks in DataGridView2
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Placeholder for handling cell clicks in DataGridView3
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Load data into the DataGridViews when Show Data button is clicked
            LoadRegisteredEvents();
            LoadEventDetails();
            LoadUpcomingSchedules();
        }

        private void LoadRegisteredEvents()
        {
            // Clear previous data
            dataGridView1.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT EB.EventID, E.EventTitle, EB.BookingDate
                    FROM EventBooking EB
                    INNER JOIN Events E ON EB.EventID = E.EventID
                    WHERE EB.AttendeeID = @AttendeeID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AttendeeID", 1); // Replace with actual attendee ID
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(
                                reader["EventID"],
                                reader["EventTitle"],
                                reader["BookingDate"]);
                        }
                    }
                }
            }
        }

        private void LoadEventDetails()
        {
            // Clear previous data
            dataGridView2.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Events";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView2.Rows.Add(
                                reader["EventID"],
                                reader["EventTitle"],
                                reader["EventDescription"],
                                reader["EventLocation"],
                                reader["EventStartDate"],
                                reader["EventEndDate"]);
                        }
                    }
                }
            }
        }

        private void LoadUpcomingSchedules()
        {
            // Clear previous data
            dataGridView3.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT EventID, EventTitle, EventStartDate, EventEndDate
                    FROM Events
                    WHERE EventStartDate > GETDATE()";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView3.Rows.Add(
                                reader["EventID"],
                                reader["EventTitle"],
                                reader["EventStartDate"],
                                reader["EventEndDate"]);
                        }
                    }
                }
            }
        }
    }
}
