using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DB_Project.attendee
{
    public partial class search : Form
    {
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";

        public search()
        {
            InitializeComponent();
        }

        private void search_Load(object sender, EventArgs e)
        {
            // Initialize ComboBox with proper ticket types
            comboBox1.Items.Clear();
            comboBox1.Items.Add("VIP");
            comboBox1.Items.Add("General Admission");

            // Load all events into the DataGridView
            LoadAllEvents();
        }

        private void LoadAllEvents()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT EventID, EventTitle, EventDescription, EventLocation, EventStartDate, EventEndDate, TicketCategories " +
                                   "FROM Events WHERE Status = 'Approved'";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Add a checkbox column for selection
                    if (!dataGridView1.Columns.Contains("SelectColumn"))
                    {
                        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                        {
                            HeaderText = "Select",
                            Name = "SelectColumn", // Ensure the column name is exactly the same
                            TrueValue = true,
                            FalseValue = false
                        };
                        dataGridView1.Columns.Add(checkBoxColumn);
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        // Display "No events found" in DataGridView
                        dataTable.Rows.Add("No events found");
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading events: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void SetupDataGridView()
        {
            // Clear existing columns if any
            dataGridView1.Columns.Clear();

            // Add columns to the DataGridView
            dataGridView1.Columns.Add("EventID", "Event ID");
            dataGridView1.Columns.Add("EventTitle", "Event Title");
            dataGridView1.Columns.Add("EventDescription", "Event Description");
            dataGridView1.Columns.Add("EventLocation", "Event Location");
            dataGridView1.Columns.Add("EventStartDate", "Start Date");
            dataGridView1.Columns.Add("EventEndDate", "End Date");
            dataGridView1.Columns.Add("TicketCategory", "Ticket Category");

            // Optionally, add a checkbox column
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Select";
            checkColumn.HeaderText = "Select";
            dataGridView1.Columns.Add(checkColumn);
        }






        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Placeholder for DataGridView cell click logic
        }

        private void label7_Click(object sender, EventArgs e) // Open Profile
        {
            
            string username = "abdul moiz"; // Replace this with the actual logged-in username
            profile p = new profile(username); // Pass the username to the profile constructor
            p.Show();
            this.Hide();

        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Placeholder for additional label logic
        }

        private void label9_Click(object sender, EventArgs e) // Open Dashboard
        {
            Dashboard dashboardForm = new Dashboard();
            dashboardForm.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e) // Open Feedback
        {
            Feedback feedbackForm = new Feedback(1,4);
            feedbackForm.Show();
            this.Hide();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Placeholder for DateTimePicker logic
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Placeholder for TextBox logic
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Placeholder for TextBox logic
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Placeholder for TextBox logic
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Placeholder for ComboBox logic
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SetupDataGridView();
                // Retrieve filter values
                string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string EventCategory = textBox2.Text.Trim();
                string location = textBox4.Text.Trim();
                string ticketType = comboBox1.SelectedItem?.ToString();

                // Filter query logic (modify based on your database schema)
                string query = "SELECT * FROM Events WHERE 1=1";

                if (!string.IsNullOrEmpty(EventCategory))
                {
                    query += $" AND EventCategory LIKE '%{EventCategory}%'";
                }
                if (!string.IsNullOrEmpty(location))
                {
                    query += $" AND EventLocation LIKE '%{location}%'";
                }
                if (!string.IsNullOrEmpty(ticketType))
                {
                    query += $" AND TicketCategories LIKE '%{ticketType}%'";
                }


                // Fetch filtered events
                SqlConnection conn = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable filteredEvents = new DataTable();
                adapter.Fill(filteredEvents);

                // Clear DataGridView
                dataGridView1.Rows.Clear();

                // Check if events were found
                if (filteredEvents.Rows.Count > 0)
                {
                    foreach (DataRow row in filteredEvents.Rows)
                    {
                        dataGridView1.Rows.Add(
                            row["EventID"],
                            row["EventTitle"],
                            row["EventDescription"],
                            row["EventLocation"],
                            Convert.ToDateTime(row["EventStartDate"]).ToString("yyyy-MM-dd"),
                            Convert.ToDateTime(row["EventEndDate"]).ToString("yyyy-MM-dd"),
                            row["TicketCategories"]);
                    }
                }
                else
                {
                    // Display a single row indicating no events were found
                    dataGridView1.Rows.Add(null, "No events found", "", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering events: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // List to hold BookingID, EventID, and TicketType for selected events
            List<object> bookingDetails = new List<object>();

            // Assuming you have a way to get the logged-in AttendeeID (example here as "1")
            int attendeeID = 1;  // Replace with actual logged-in AttendeeID
            string ticketType = comboBox1.SelectedItem?.ToString();  // Assuming a ComboBox for ticket type (VIP, General Admission)

            // Validate ticket type selection
            if (string.IsNullOrEmpty(ticketType))
            {
                MessageBox.Show("Please select a ticket type.", "No Ticket Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Collect selected rows based on existing data (no SelectColumn used)
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var eventId = row.Cells["EventID"].Value?.ToString();

                if (!string.IsNullOrEmpty(eventId))
                {
                    // Create a booking for each event
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string insertBookingQuery = "INSERT INTO EventBooking (AttendeeID, EventID, TicketType, BookingDate) " +
                                                        "OUTPUT INSERTED.BookingID, INSERTED.EventID " +
                                                        "VALUES (@AttendeeID, @EventID, @TicketType, @BookingDate)";

                            SqlCommand command = new SqlCommand(insertBookingQuery, connection);
                            command.Parameters.AddWithValue("@AttendeeID", attendeeID);
                            command.Parameters.AddWithValue("@EventID", eventId);
                            command.Parameters.AddWithValue("@TicketType", ticketType);
                            command.Parameters.AddWithValue("@BookingDate", DateTime.Now);

                            // Execute the query and get the BookingID and EventID
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                int bookingID = reader.GetInt32(0); // BookingID
                                int eventID = reader.GetInt32(1);   // EventID

                                // Add BookingID and EventID to the list
                                bookingDetails.Add(new { BookingID = bookingID, EventID = eventID });
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error creating bookings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            if (bookingDetails.Count == 0)
            {
                MessageBox.Show("Please select at least one event to book tickets.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pass the booking details to the tickets form
            tickets ticketsForm = new tickets(bookingDetails);  // Pass to tickets form
            ticketsForm.Show();
            this.Hide();
        }








        private void button3_Click(object sender, EventArgs e)
        {
            // Placeholder for additional button logic
        }

        private void search_Load_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
