using DB_Project.attendee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class tickets : Form
    {
        private List<object> selectedEvents;
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";


        public tickets(List<object> bookings)
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (var booking in bookings)
            {
                var bookingDetails = booking as dynamic;
                Console.WriteLine($"BookingID: {bookingDetails.BookingID}, EventID: {bookingDetails.EventID}");
                // Display booking details in your UI or process as needed
            }
        }


        public tickets()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }



        private void button2_Click(object sender, EventArgs e)
        {

            // Check if at least one row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a ticket to download.");
                return;
            }

            // Get the first selected row
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Check if the TicketID exists in the selected row
            if (selectedRow.Cells["TicketID"].Value == null)
            {
                MessageBox.Show("No TicketID found in the selected row.");
                return;
            }

            // Fetch the TicketID value from the selected row
            int ticketID = Convert.ToInt32(selectedRow.Cells["TicketID"].Value);

            // Debugging: Verify selected TicketID
            Console.WriteLine($"Selected TicketID: {ticketID}");

            // Fetch ticket data from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM EventTickets WHERE TicketID = @TicketID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@TicketID", ticketID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string ticketDetails = $"Ticket ID: {reader["TicketID"]}\n" +
                                               $"Booking ID: {reader["BookingID"]}\n" +
                                               $"QR Code: {reader["QRCode"]}\n" +
                                               $"Check-In Status: {reader["CheckInStatus"]}";

                        // Generate file path for the ticket
                        // Specify the desired folder path
                        string directoryPath = @"C:\Users\DELL\OneDrive\Desktop\i222426_i222458_i222493_DB_B\attendee";

                        // Check if the directory exists, if not, create it
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);  // Creates the directory if it doesn't exist
                        }

                        // Combine the directory path with the file name
                        string filePath = Path.Combine(directoryPath, $"Ticket_{ticketID}.txt");

                        // Write the ticket details to the file
                        File.WriteAllText(filePath, ticketDetails);

                        // Show a message confirming the download
                        MessageBox.Show($"E-ticket downloaded successfully as {filePath}");

                    }
                    else
                    {
                        MessageBox.Show("No ticket found for the selected TicketID.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error downloading ticket: {ex.Message}");
                }
            }
        }



        private void InsertEventTicket(int bookingID, string qrCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO EventTickets (BookingID, QRCode) VALUES (@BookingID, @QRCode)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@BookingID", bookingID);
                    cmd.Parameters.AddWithValue("@QRCode", qrCode);  // Ensure QRCode is passed as a string

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Ticket created and added successfully.");
                        DisplaySelectedEvents();  // Refresh the data grid
                    }
                    else
                    {
                        MessageBox.Show("Error adding ticket.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting ticket: {ex.Message}");
                }
            }
        }

        private void DisplaySelectedEvents()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM EventTickets";  // Ensure this retrieves all necessary columns, including TicketID
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // Verify column names after data is loaded
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        Console.WriteLine(column.Name);  // This will help you check the column names
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
            }
        }


        // Assuming you want to insert tickets and show them when the form loads
        private void tickets_Load(object sender, EventArgs e)
        {
            // Optional: Insert a sample ticket if needed
            InsertEventTicket(1, "SampleQRCode123");

            // Display the selected events/tickets
            DisplaySelectedEvents();
        }


        private void label7_Click(object sender, EventArgs e)
        {
            profile profileForm = new profile("abdul moiz");
            profileForm.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            search searchForm = new search();
            searchForm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Dashboard dashboardForm = new Dashboard();
            dashboardForm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Feedback feedbackForm = new Feedback(1,4);
            feedbackForm.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Empty event handler
        }



        private void label4_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            // Empty event handler
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Empty event handler
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
