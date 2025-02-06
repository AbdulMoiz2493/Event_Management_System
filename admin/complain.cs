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
    public partial class complain : Form
    {
        string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True;";

        public complain()
        {
            InitializeComponent();
            LoadAllComplaints();
            LoadApprovedComplaints();
            LoadPendingComplaints();
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
            // Navigate to Event Management
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
            // Navigate to Complaint (current form)
            complain complainForm = new complain();
            complainForm.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadAllComplaints();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadApprovedComplaints();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadPendingComplaints();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResolveSelectedComplaints();
        }

        private void LoadAllComplaints()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Complaints";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void LoadApprovedComplaints()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Complaints WHERE ComplaintStatus = 'Approved'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
            }
        }

        private void LoadPendingComplaints()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ComplaintID, ComplaintText, ComplaintDate, UserID, ComplaintStatus FROM Complaints WHERE ComplaintStatus = 'Pending'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView3.DataSource = dt;

                // Add a checkbox column to DataGridView
                if (!dataGridView3.Columns.Contains("Select"))
                {
                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.HeaderText = "Select";
                    checkBoxColumn.Name = "Select";
                    dataGridView3.Columns.Add(checkBoxColumn);
                }
            }
        }

        private void ResolveSelectedComplaints()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value == true)
                    {
                        int complaintID = Convert.ToInt32(row.Cells["ComplaintID"].Value);

                        string query = "UPDATE Complaints SET ComplaintStatus = 'Approved' WHERE ComplaintID = @ComplaintID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ComplaintID", complaintID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Selected complaints have been resolved.");
                LoadPendingComplaints(); // Refresh pending complaints
                LoadApprovedComplaints(); // Refresh approved complaints
            }
        }

        private void complain_Load(object sender, EventArgs e)
        {

        }
    }
}
