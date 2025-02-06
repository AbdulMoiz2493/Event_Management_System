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
    public partial class userAndOrganizer : Form
    {
        string connectionString = @"Data Source=DELL-LATITUDE-7\SQLEXPRESS;Initial Catalog=EV;Integrated Security=True;";

        public userAndOrganizer()
        {
            InitializeComponent();
            // Ensure event subscription
            this.dataGridView3.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentClick);
        }

        private void userAndOrganizer_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadComplaints(); // Load complaints when the form loads
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void LoadComplaints()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Complaints";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click event if needed
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click event if needed
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("None");
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Link to User and Organizer (current form)
            userAndOrganizer userAndOrganizerForm = new userAndOrganizer();
            userAndOrganizerForm.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Link to Event Management
            eventManagement eventManagementForm = new eventManagement();
            eventManagementForm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Link to Reports and Platform
            analytics analyticsForm = new analytics();
            analyticsForm.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Link to Feedback Moderation
            Feedback feedbackForm = new Feedback();
            feedbackForm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Link to Complaint
            complain complaintForm = new complain();
            complaintForm.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
