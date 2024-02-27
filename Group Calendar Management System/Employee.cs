using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Calendar_Management_System
{
    class Employee
    {
        string name;
        string username;
        string password;

        public static bool validateIdentification(string uName, string uPassword)
        {
            // Retrieve data from the database
            //string s = "Tom";
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonemployee WHERE username=@name and password=@passd";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", uName);
                cmd.Parameters.AddWithValue("@passd", uPassword);
                MySqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.Read())
                {
                    /*
                    textBox1.Text = myReader["FirstName"].ToString();
                    textBox2.Text = myReader["LastName"].ToString();
                    textBox3.Text = myReader["ID"].ToString();
                    */
                    myReader.Close();
                    //conn.Close();
                }
                else
                {
                    myReader.Close();
                    conn.Close();
                    return false;
                }
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            return true;
            // If the data exists, return true, otherwise return false
        }
    }
}
