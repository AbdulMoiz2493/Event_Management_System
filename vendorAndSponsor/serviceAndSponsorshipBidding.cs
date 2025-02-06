using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project.vendorAndSponsor
{
    public partial class serviceAndSponsorshipBidding : Form
    {
        // Replace with your database connection string
        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";

        public serviceAndSponsorshipBidding()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string EventTitle = textBox1.Text; // Event name
            string services = textBox3.Text; // Services offered
            string biddingAmountText = textBox4.Text; // Bidding amount

            if (string.IsNullOrEmpty(EventTitle) || string.IsNullOrEmpty(services) || string.IsNullOrEmpty(biddingAmountText))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(biddingAmountText, out decimal biddingAmount) || biddingAmount <= 0)
            {
                MessageBox.Show("Please enter a valid bidding amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the event exists
                    string eventValidationQuery = "SELECT COUNT(*) FROM Events WHERE EventTitle = @EventTitle";

                    using (SqlCommand eventValidationCommand = new SqlCommand(eventValidationQuery, connection))
                    {
                        eventValidationCommand.Parameters.AddWithValue("@EventTitle", EventTitle);

                        int eventCount = (int)eventValidationCommand.ExecuteScalar();

                        if (eventCount > 0)
                        {
                            // Update the sponsor's budget with the bidding amount
                            string updateBudgetQuery = @"
                                UPDATE Sponsors
                                SET Budget = @BiddingAmount
                                WHERE SponsorID = (SELECT TOP 1 SponsorID FROM Sponsors ORDER BY SponsorID)"; // Update for the first sponsor as an example

                            using (SqlCommand updateBudgetCommand = new SqlCommand(updateBudgetQuery, connection))
                            {
                                updateBudgetCommand.Parameters.AddWithValue("@BiddingAmount", biddingAmount);

                                int rowsAffected = updateBudgetCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Event validated successfully! Your bid has been submitted and budget updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update the sponsor's budget.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("The specified event does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the bid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Open the contractAndPaymentTracking form
            var contractForm = new contractAndPaymentTracking();
            contractForm.Show();
            this.Hide(); // Optionally hide the current form
        }

        private void label8_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}
