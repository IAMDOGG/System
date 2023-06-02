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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Money
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string query = "SELECT password FROM [Password] WHERE username = @Username";
            string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\Money\\Money\\Password.accdb";

            using (OleDbConnection connect = new OleDbConnection(con))
            {
                connect.Open();

                using (OleDbCommand cmd = new OleDbCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@Username", txtUser.Text);

                    string passwordFromDatabase = (string)cmd.ExecuteScalar();

                    if (txtPass.Text == passwordFromDatabase)
                    {
                        MessageBox.Show("Login Successful");
                        this.Hide();
                        Dashboard dash = new Dashboard();
                        dash.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ïncorrect Username or Password!");
                    }
                }
            }
                    txtPass.PasswordChar = '*';
        }
    }
}
