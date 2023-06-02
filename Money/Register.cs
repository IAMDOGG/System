using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Money
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\Money\\Money\\Password.accdb";

            OleDbConnection connection = new OleDbConnection(connectionString);
            string User = txtUser.Text;
            string Password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Password)) {
                MessageBox.Show("Please enter a value for all fields.");
                return;
            }

            string insert = "INSERT INTO [Password] (Username, [Password]) VALUES (?, ?)";
            OleDbCommand command = new OleDbCommand(insert, connection);
            command.Parameters.AddWithValue("@Username", User);
            command.Parameters.AddWithValue("@Password", Password);
            

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            txtUser.Clear();
            txtPass.Clear();
            

            MessageBox.Show("Registration successful!");
            Login login = new Login();
            this.Hide();
            login.Show();
        }
    }
}
