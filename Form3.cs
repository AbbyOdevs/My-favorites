using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace MyFavorites
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) // BACK TO THE LOG IN FORM
        {
            Form1 form2 = new Form1();
            form2.Show();
            this.Hide();
        }
        // BACK TO THE LOG IN FORM
        private void Form3_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e) // CREATE BUTTON START CODE
        {
            // Check if username or password is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("The username should be filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("The password should be filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Connection string
            string conString = "server=localhost;uid=Favorite;pwd=Abigail123;database=users";

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();
                    string query = "INSERT INTO users (User_name, password) VALUES (@User_name, @password)";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@User_name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@password", textBox2.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Account Created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} // CREATE BUTTON END CODE
