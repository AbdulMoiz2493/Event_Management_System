using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_Project.vendorAndSponsor
{
    public partial class contractAndPaymentTracking : Form

    {

        private string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True";
        public contractAndPaymentTracking()
        {
            InitializeComponent();
        }

        private void contractAndPaymentTracking_Load(object sender, EventArgs e)
        {
            // Populate data grids
            LoadEventAndVendorSponsorDetails();
            LoadSponsorBudget();
            LoadVendorAndSponsorFullTable();
        }

        private void LoadEventAndVendorSponsorDetails()
        {
            // Display Event ID and Vendor/Sponsor Details in DataGridView1
            string query = @"
                SELECT 
                    Sponsors.SponsorID AS [ID], 
                    Sponsors.UserName AS [Sponsor Name], 
                    Sponsors.CompanyName AS [Company Name], 
                    Vendors.VendorID AS [Vendor ID], 
                    Vendors.UserName AS [Vendor Name], 
                    Vendors.BusinessType AS [Business Type]
                FROM Sponsors
                FULL OUTER JOIN Vendors ON Sponsors.SponsorID = Vendors.VendorID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data for DataGridView1: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadSponsorBudget()
        {
            // Display Budget from Sponsors Table in DataGridView2
            string query = @"
                SELECT 
                    SponsorID AS [Sponsor ID], 
                    UserName AS [Sponsor Name], 
                    Budget AS [Budget]
                FROM Sponsors";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data for DataGridView2: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadVendorAndSponsorFullTable()
        {
            // Display Full Vendor and Sponsor Table in DataGridView3
            string vendorQuery = "SELECT * FROM Vendors";
            string sponsorQuery = "SELECT * FROM Sponsors";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Load Vendors
                    SqlDataAdapter vendorAdapter = new SqlDataAdapter(vendorQuery, connection);
                    DataTable vendorTable = new DataTable();
                    vendorAdapter.Fill(vendorTable);

                    // Load Sponsors
                    SqlDataAdapter sponsorAdapter = new SqlDataAdapter(sponsorQuery, connection);
                    DataTable sponsorTable = new DataTable();
                    sponsorAdapter.Fill(sponsorTable);

                    // Combine both tables
                    DataTable combinedTable = vendorTable.Clone();
                    foreach (DataColumn col in sponsorTable.Columns)
                    {
                        if (!combinedTable.Columns.Contains(col.ColumnName))
                            combinedTable.Columns.Add(col.ColumnName, col.DataType);
                    }

                    foreach (DataRow row in vendorTable.Rows)
                        combinedTable.ImportRow(row);

                    foreach (DataRow row in sponsorTable.Rows)
                        combinedTable.ImportRow(row);

                    dataGridView3.DataSource = combinedTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data for DataGridView3: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Open serviceAndSponsorshipBidding.cs Form
            serviceAndSponsorshipBidding biddingForm = new serviceAndSponsorshipBidding();
            biddingForm.Show();
            this.Hide(); // Optionally hide the current form
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void label11_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Implement if necessary
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Refresh all DataGridViews when the button is clicked
            LoadEventAndVendorSponsorDetails();
            LoadSponsorBudget();
            LoadVendorAndSponsorFullTable();

            MessageBox.Show("Data has been refreshed successfully!", "Refresh Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
