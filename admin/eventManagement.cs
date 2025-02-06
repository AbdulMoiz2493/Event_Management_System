using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Project.admin
{
    public partial class eventManagement : Form
    {
        string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True;";

        public eventManagement()
        {
            InitializeComponent();
            LoadEventCategories();
            LoadPendingEvents();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEventDetails();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // No need to call LoadEventDetails() here as it's handled by comboBox1_SelectedIndexChanged
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Show all events with pending status and a checkbox for approval
            LoadPendingEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApproveSelectedEvents();
        }

        private void LoadEventCategories()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT EventCategory FROM Events";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "EventCategory";
            }
        }

        private void LoadEventDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string selectedCategory = comboBox1.Text;

                string query = "SELECT EventTitle, EventDescription, EventLocation, EventStartDate, EventEndDate FROM Events WHERE EventCategory = @EventCategory";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@EventCategory", selectedCategory);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void LoadPendingEvents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT EventID, EventTitle, EventDescription, EventLocation, EventStartDate, EventEndDate FROM Events WHERE Status = 'Pending'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;

                // Add a checkbox column to DataGridView
                if (!dataGridView2.Columns.Contains("Approve"))
                {
                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.HeaderText = "Approve";
                    checkBoxColumn.Name = "Approve";
                    dataGridView2.Columns.Add(checkBoxColumn);
                }
            }
        }

        private void ApproveSelectedEvents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["Approve"].Value != null && (bool)row.Cells["Approve"].Value == true)
                    {
                        int eventId = Convert.ToInt32(row.Cells["EventID"].Value);

                        string query = "UPDATE Events SET Status = 'Approved' WHERE EventID = @EventID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@EventID", eventId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Selected events have been approved.");
                LoadPendingEvents(); // Refresh pending events
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Navigate to User and Organizer
            userAndOrganizer userAndOrganizerForm = new userAndOrganizer();
            userAndOrganizerForm.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Navigate to Event Management (current form)
            eventManagement eventManagementForm = new eventManagement();
            eventManagementForm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Navigate to Reports and Platform
           analytics reportsAndPlatformForm = new analytics();
            reportsAndPlatformForm.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Navigate to Feedback Moderation
            Feedback feedbackForm = new Feedback();
            feedbackForm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Navigate to Complaint
            complain complainForm = new complain();
            complainForm.Show();
            this.Hide();
        }

        private void eventManagement_Load(object sender, EventArgs e)
        {

        }
    }
}
