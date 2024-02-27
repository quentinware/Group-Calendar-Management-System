using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Calendar_Management_System
{

    /*
     *  THIS IS THE FORM THAT HANDLES THE DELETE CONFIRMATION
     * 
     */
    public partial class Form2 : Form
    {
        public Form2(string itemToDelete, string username)
        {
            InitializeComponent();
            textBox2.Text = itemToDelete;
            textBox2.Hide();
            textBox3.Text = username;
            textBox3.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string eventID = textBox2.Text.ToString();
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "DELETE FROM pattersonevents where eventID=@ID";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", eventID);

                MySqlDataReader myReader = cmd.ExecuteReader();

                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "DELETE FROM pattersonmeeting where meetingID=@ID";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", eventID);

                MySqlDataReader myReader = cmd.ExecuteReader();

                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            this.Hide();
            var form1 = new Form1(textBox3.Text);
            form1.Closed += (s, args) => this.Close();
            form1.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form1 = new Form1(textBox3.Text);
            form1.Closed += (s, args) => this.Close();
            form1.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
