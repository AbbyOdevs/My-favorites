using System;
using System.Data;
using System.Windows.Forms;

namespace MyFavorites
{
    public partial class Form1 : Form
    {
        private readonly DB _db = new DB();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Optional initialization code
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Exit application
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Login
            if (!ValidateInputs())
                return;

            string query = $"SELECT COUNT(*) FROM users WHERE User_name = '{textBox1.Text}' AND password = '{textBox2.Text}'";
            if (Convert.ToInt32(_db.QueryScalar(query)) <= 0)
            {
                MessageBox.Show("Username or password is incorrect.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Open Form2 on successful login
            new Form2().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open Sign-up Form
            new Form3().Show();
            this.Hide();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a username.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter a password.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                return false;
            }

            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Optional: handle text changed
        }
    }
}
