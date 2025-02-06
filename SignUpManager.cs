using DB_Project.admin;
using DB_Project.vendorAndSponsor;
using System;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class SignUpManager : Form
    {
        public SignUpManager()
        {
            InitializeComponent();

            // Populate comboBox1 with user type options
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("Attendee");
            comboBox1.Items.Add("Organizer");
            comboBox1.Items.Add("Vendor/Sponsor");

            // Set default selection to the first item (optional)
            comboBox1.SelectedIndex = 0;
        }

        private void SignUpManager_Load(object sender, EventArgs e)
        {
            // Optional: Add any logic to run when the form loads
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Optional: Handle picture box click event
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Optional: Handle label click event
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Handle comboBox1 selection change if needed
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected user type from the comboBox
            string selectedUserType = comboBox1.SelectedItem.ToString();

            // Navigate to the appropriate registration form based on the selection
            switch (selectedUserType)
            {
                case "Admin":
                    registerAdmin registerAdminForm = new registerAdmin();
                    registerAdminForm.Show();
                    this.Hide();
                    break;

                case "Attendee":
                    attendeeRegister attendeeRegisterForm = new attendeeRegister();
                    attendeeRegisterForm.Show();
                    this.Hide();
                    break;

                case "Organizer":
                    organizerRegistration organizerRegistrationForm = new organizerRegistration();
                    organizerRegistrationForm.Show();
                    this.Hide();
                    break;

                case "Vendor/Sponsor":
                    register vendorSponsorRegisterForm = new register();
                    vendorSponsorRegisterForm.Show();
                    this.Hide();
                    break;

                default:
                    MessageBox.Show("Please select a valid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
