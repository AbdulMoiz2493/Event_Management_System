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
    public partial class Feedback : Form
    {
        string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True;";

        public Feedback()
        {
            InitializeComponent();
            LoadFeedbacks();
            LoadRatings();
        }

        private void Feedback_Load(object sender, EventArgs e)
        {
            LoadFeedbacks();
            LoadRatings();
        }

        private void LoadFeedbacks()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FeedbackID, FeedbackText FROM Feedbacks";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void LoadRatings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FeedbackID, Rating FROM Feedbacks";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadFeedbacks();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadRatings();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Navigate to Feedback Moderation (current form)
            Feedback feedbackForm = new Feedback();
            feedbackForm.Show();
            this.Hide();
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
           analytics analyticsForm = new analytics();
            analyticsForm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Navigate to Complaint
            complain complaintForm = new complain();
            complaintForm.Show();
            this.Hide();
        }
    }
}
