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
    public partial class Dashboard : Form
    {
        private OleDbConnection con;
        public Dashboard()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\Money\\Money\\money.accdb");
            loadDatagrid();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double principal = Convert.ToDouble(txtprincipalamount.Text);
                double rate = (Convert.ToDouble(txtinterestrate.Text) / 100);
                double interest = principal * rate;
                double term = Convert.ToDouble(txtterms.Text);
                double total = principal + interest;
                double topay = total / term;

                string inter = Convert.ToString(interest);
                txtinterest.Text = inter;
                string totalam = Convert.ToString(total);
                txttotalamount.Text = totalam;
                string atp = topay.ToString("0.00");
                txtamountopay.Text = atp;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid or blank fields.");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\Money\\Money\\money.accdb";

            OleDbConnection connection = new OleDbConnection(connectionString);

            string first = txtfirstname.Text;
            string last = txtlastname.Text;
            string prinam = txtprincipalamount.Text;
            string rat = txtinterestrate.Text;
            string inter = txtinterest.Text;
            string tots = txttotalamount.Text;
            string period = cmbperiod.Text;
            string termm = txtterms.Text;
            string top = txtamountopay.Text;

            if (string.IsNullOrWhiteSpace(last) || string.IsNullOrWhiteSpace(prinam) || string.IsNullOrWhiteSpace(rat)
                || string.IsNullOrWhiteSpace(inter) || string.IsNullOrWhiteSpace(tots) || string.IsNullOrWhiteSpace(period)
                || string.IsNullOrWhiteSpace(termm) || string.IsNullOrWhiteSpace(top))
            {
                MessageBox.Show("Please enter a value for all fields.");
                return;
            }

            string insert = "INSERT INTO [moneytable] (First_Name, Last_Name, Amount, Interest, Period, Terms, Amount_to_Pay) VALUES (?, ?, ?, ?, ?, ?, ?)";
            OleDbCommand command = new OleDbCommand(insert, connection);
            command.Parameters.AddWithValue("@First_Name", first);
            command.Parameters.AddWithValue("@Last_Name", last);
            command.Parameters.AddWithValue("@Amount", prinam);
            command.Parameters.AddWithValue("@Interest", inter);
            command.Parameters.AddWithValue("@Period", period);
            command.Parameters.AddWithValue("@Terms", termm);
            command.Parameters.AddWithValue("@Amount_to_Pay", top);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            txtfirstname.Clear();
            txtlastname.Clear();
            txtprincipalamount.Clear();
            txtinterestrate.Clear();
            txtinterest.Clear();
            txttotalamount.Clear();
            txtterms.Clear();
            txtamountopay.Clear();
            con.Close();
            loadDatagrid();
            MessageBox.Show("Added Successfully!");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                dataGridView1.Rows.Remove(selectedRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the row: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtfirstname.Text = dataGridView1.Rows[e.RowIndex].Cells["First_Name"].Value.ToString();
            txtlastname.Text = dataGridView1.Rows[e.RowIndex].Cells["Last_Name"].Value.ToString();
            txtprincipalamount.Text = dataGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString();
            txtinterest.Text = dataGridView1.Rows[e.RowIndex].Cells["Interest"].Value.ToString();
            cmbperiod.Text = dataGridView1.Rows[e.RowIndex].Cells["Period"].Value.ToString();
            txtterms.Text = dataGridView1.Rows[e.RowIndex].Cells["Terms"].Value.ToString();
            txtamountopay.Text = dataGridView1.Rows[e.RowIndex].Cells["Amount_to_Pay"].Value.ToString();
        }

        private void loadDatagrid()
        {
            con.Open();
            OleDbCommand com = new OleDbCommand("SELECT * FROM moneytable ORDER BY [First_Name] ASC", con);
            com.ExecuteNonQuery();
            OleDbDataAdapter adap = new OleDbDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            dataGridView1.DataSource = tab;

            con.Close();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Check if a row has been selected
            {
                con.Open();

                OleDbCommand com = new OleDbCommand("UPDATE moneytable SET First_Name= @First_Name, Last_Name= @Last_Name, Amount= @Amount, Interest= @Interest, Period= @Period, Terms= @Terms, Amount_to_Pay= @Amount_to_Pay  WHERE First_Name= @First_Name", con);
                com.Parameters.AddWithValue("@First_Name", txtfirstname.Text);
                com.Parameters.AddWithValue("@Last_Name", txtlastname.Text);
                com.Parameters.AddWithValue("@Amount", txtprincipalamount.Text);
                com.Parameters.AddWithValue("@Interest", txtinterest.Text);
                com.Parameters.AddWithValue("@Period", cmbperiod.Text);
                com.Parameters.AddWithValue("@Terms", txtterms.Text);
                com.Parameters.AddWithValue("@Amount_to_Pay", txtamountopay.Text);
                com.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Successfully Edited!", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadDatagrid();
            }
            else
            {
                MessageBox.Show("Please select a row to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {

        }

        private void btnPay_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                con.Open();
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                object value = selectedRow.Cells["Terms"].Value;
                int currentValue = Convert.ToInt32(value);
                int newValue = currentValue - 1;
                string query = "SELECT First_Name, Last_Name, Amount, Interest, Period, Terms, Amount_to_Pay FROM moneytable WHERE First_Name";
                OleDbCommand command = new OleDbCommand(query, con);
                OleDbDataReader reader = command.ExecuteReader();
                MessageBox.Show("Successfully PAID!");
                OleDbCommand updateCommand = new OleDbCommand("UPDATE moneytable SET Terms = @Terms WHERE [First_Name] = @First_Name", con);
                updateCommand.Parameters.AddWithValue("@Terms", newValue);
                updateCommand.Parameters.AddWithValue("@First_Name", selectedRow.Cells["First_Name"].Value);
                updateCommand.ExecuteNonQuery();
                if (newValue == 0)
                {
                    // Delete the row
                    
                    if (reader.Read())
                    {
                        string value1 = reader["First_Name"].ToString();
                        string value2 = reader["Last_Name"].ToString();
                        string value3 = reader["Amount"].ToString();
                        string value4 = reader["Interest"].ToString();
                        string value5 = reader["Period"].ToString();
                        string value6 = reader["Terms"].ToString();
                        string value7 = reader["Amount_to_Pay"].ToString();
                        MessageBox.Show("\nJOHN'S MONEY BORROWING SYSTEM" +
                                        "\n++++++++++++++++++++++++++++" +
                                         "\n" +
                                         "\nFirst Name: " + value1 +
                                         "\nLast Nane: " + value2 +
                                         "\nAmount: " + value3 +
                                         "\nInterest: " + value4 +
                                         "\nPeriod: " + value5 +
                                         "\nTerms: " + value6 +
                                         "\nAmount to Pay: " + value7 +
                                         "\nStatus: PAID");
                        OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM moneytable WHERE [First_Name] = @First_Name", con);
                        deleteCommand.Parameters.AddWithValue("@First_Name", selectedRow.Cells["First_Name"].Value);
                        deleteCommand.ExecuteNonQuery();
                        con.Close();
                        loadDatagrid();
                    }
                   
                }
                con.Close();
                loadDatagrid();
            }


        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtsearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchValue))
            {
                con.Open();
                OleDbCommand com = new OleDbCommand("SELECT * FROM moneytable WHERE [First_Name] LIKE @searchValue OR Last_Name LIKE @searchValue OR Amount LIKE @searchValue " +
                    "OR Interest LIKE @searchValue OR Period LIKE @searchValue OR Terms LIKE @searchValue OR Amount_to_Pay LIKE @searchValue", con);
                com.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                OleDbDataAdapter adap = new OleDbDataAdapter(com);
                DataTable tab = new DataTable();

                adap.Fill(tab);
                dataGridView1.DataSource = tab;

                con.Close();
            }
            else
            {
                loadDatagrid();
            }
        }
    }
}

