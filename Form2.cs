using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MyFavorites
{
    public partial class Form2 : Form
    {
        // Database connection string
        private const string ConnectionString = "server=localhost;uid=Favorite;pwd=Abigail123;database=users";

        public Form2()
        {
            InitializeComponent();
        }

        // Form Load → Executes after successful login
        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Load data from database and display in DataGridView
        private void LoadData()
        {
            using (var con = new MySqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM favorite", con))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Allow editing in DataGridView
                    dataGridView1.DataSource = dt;
                    dataGridView1.ReadOnly = false;
                    dataGridView1.AllowUserToAddRows = false;
                }
            }
        }

        // Get selected row data and display in textboxes
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];
            textBox4.Text = row.Cells["id"].Value.ToString();
            textBox1.Text = row.Cells["colors"].Value.ToString();
            textBox2.Text = row.Cells["food"].Value.ToString();
            textBox3.Text = row.Cells["numbers"].Value.ToString();
        }

        
        private void button2_Click(object sender, EventArgs e)// ENTER button → Add new record
        {// Basic validation → ensure all fields are filled
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            // Validate numeric input for numbers
            if (!int.TryParse(textBox3.Text, out int num))
            {
                MessageBox.Show("Numbers must be numeric.");
                return;
            }
            using (var con = new MySqlConnection(ConnectionString))
            { con.Open();
                // Check for duplicate entry → prevent adding same color + food + number
            string checkQuery = "SELECT COUNT(*) FROM favorite WHERE colors=@colors AND food=@food AND numbers=@numbers";
                using (var checkCmd = new MySqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@colors", textBox1.Text);
                    checkCmd.Parameters.AddWithValue("@food", textBox2.Text);
                    checkCmd.Parameters.AddWithValue("@numbers", num);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Duplicate entry detected.");
                        return;
                    }
                }
                // Insert new record
                string insertQuery = "INSERT INTO favorite (colors, food, numbers) VALUES (@colors, @food, @numbers)";
                using (var insertCmd = new MySqlCommand(insertQuery, con))
                {
                    insertCmd.Parameters.AddWithValue("@colors", textBox1.Text);
                    insertCmd.Parameters.AddWithValue("@food", textBox2.Text);
                    insertCmd.Parameters.AddWithValue("@numbers", num);
                    insertCmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Record saved successfully!");
            LoadData(); // Refresh DataGridView
        }

        // DELETE button → Delete selected record
        private void button4_Click(object sender, EventArgs e)
        {
            // Validate ID
            if (!int.TryParse(textBox4.Text, out int id))
            {
                MessageBox.Show("Please enter a valid ID.");
                return;
            }

            using (var con = new MySqlConnection(ConnectionString))
            {
                con.Open();
                string deleteQuery = "DELETE FROM favorite WHERE id=@id";
                using (var cmd = new MySqlCommand(deleteQuery, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show(rows > 0 ? "Record deleted successfully." : "ID not found.");
                }
            }

            LoadData(); // Refresh DataGridView
        }

        // Allow inline editing in DataGridView and auto-update database
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                string colors = dataGridView1.Rows[e.RowIndex].Cells["colors"].Value.ToString();
                string food = dataGridView1.Rows[e.RowIndex].Cells["food"].Value.ToString();
                string numbers = dataGridView1.Rows[e.RowIndex].Cells["numbers"].Value.ToString();

                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    string updateQuery = "UPDATE favorite SET colors=@colors, food=@food, numbers=@numbers WHERE id=@id";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@colors", colors);
                        cmd.Parameters.AddWithValue("@food", food);
                        cmd.Parameters.AddWithValue("@numbers", numbers);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // LOG OUT button → Go back to Form1
        private void button5_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }
    }
}
