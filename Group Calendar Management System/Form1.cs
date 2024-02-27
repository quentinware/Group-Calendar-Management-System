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


/*
 * AUTHORS: Quentin Patterson, Adam Clark, Christopher Witt
 * 
 */

namespace Calendar_Management_System
{

    /*
     *  THIS IS THE FORM THAT HANDLES THE MAIN PAGE
     * 
     */
    public partial class Form1 : Form
    {
        

        public Form1(string username)
        {
            InitializeComponent();

            label7.Hide();
            label8.Hide();
            label9.Hide();
            textBox8.Hide();
            listBox2.Hide();
            listBox3.Hide();
            button6.Hide();
            button7.Hide();
            button8.Hide();
            textBox9.Hide();

            textBox6.Text = username;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;

            Event initialEvent = new Event();

            string[] empBox = new string[100];
            empBox = initialEvent.employeeBoxFill();

            for(int i = 0; i < empBox.Length; i++)
            {
                if(empBox[i] != null)
                {
                    listBox2.Items.Add(empBox[i]);
                }
                
            }

            bool isManager = initialEvent.checkManager(textBox6.Text.ToString()); ;

            if(isManager == true)
            {
                button6.Show();
                label7.Show();
                label8.Show();
                label9.Show();
                textBox8.Show();
                listBox2.Show();
                listBox3.Show();

                textBox8.Text = textBox6.Text.ToString();
            }

            

            string today = monthCalendar1.SelectionRange.Start.ToString();
            string[] events = new string[100];

            events = initialEvent.eventBoxFill(username, today);

            for(int i = 0; i < events.Length; i++)
            {
                if(events[i] != null)
                {
                    listBox1.Items.Add(events[i]);
                }
                
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Event button1Event = new Event();

            // FOR THE SAVE BUTTON
            if (button1.Text.Equals("Save"))
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                button1Event.saveEvent(textBox1.Text.ToString(), textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), textBox5.Text.ToString(), monthCalendar1.SelectionRange.Start, textBox6.Text.ToString());

                button1.Text = "Edit";
                button2.Text = "Delete";

            }
            // FOR THE SAVE MEETING BUTTON
            if (button1.Text.Equals("Save Meeting"))
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                string eventName = textBox1.Text.ToString();
                int eventID = 0;

                button1Event.saveMeetingEvent(textBox1.Text.ToString(), textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), textBox5.Text.ToString(), monthCalendar1.SelectionRange.Start, textBox6.Text.ToString());
                

                eventID = button1Event.getMeetingID(eventName);


                button1Event.retrieveEventData(textBox6.Text.ToString());
                textBox1.Text = button1Event.getEventName();
                textBox2.Text = button1Event.getStartTime();
                textBox3.Text = button1Event.getEndTime();
                textBox4.Text = button1Event.getLocation();
                textBox5.Text = button1Event.getDescription();

                textBox7.Text = button1Event.getEventID().ToString();


                string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                bool available = true;

                try
                {
                    
                    Console.WriteLine(listBox3.Items.Count);
                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        available = true;
                        try
                        {
                            Console.WriteLine("Connecting to MySQL...");
                            conn.Open();
                            string sql = "SELECT * FROM pattersonevents where username=@username";
                            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                            cmd.Parameters.AddWithValue("@username", listBox3.Items[i].ToString());

                            MySqlDataReader myReader = cmd.ExecuteReader();


                            while (myReader.Read())
                            {

                                string day = myReader["eventDay"].ToString();
                                string startTime = myReader["startTime"].ToString();
                                string endTime = myReader["endTime"].ToString();
                                Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                                Console.WriteLine(day);
                                if (DateTime.Parse(textBox2.Text.ToString()) >= DateTime.Parse(startTime) && DateTime.Parse(textBox3.Text.ToString()) <= DateTime.Parse(endTime))
                                {
                                    if (day.Equals(monthCalendar1.SelectionRange.Start.ToString()))
                                    {
                                        available = false;
                                    }
                                    
                                }

                            }

                            myReader.Close();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        conn.Close();
                        Console.WriteLine("Done.");

                        if (available == true)
                        {
                            Console.WriteLine("Connecting to MySQL...");
                            conn.Open();
                            string sql = "INSERT INTO pattersonmeeting(meetingID, manager, participant) VALUES(@eventID, @manager, @participant)";
                            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                            

                            Console.Write("Event ID: ");
                            Console.WriteLine(textBox7.Text.ToString());

                            Console.Write("Event ID: ");
                            Console.WriteLine(textBox7.Text.ToString());
                            Console.Write("Manager: ");
                            Console.WriteLine(textBox8.Text.ToString());
                            Console.Write("Participant: ");
                            Console.WriteLine(listBox3.Items[i].ToString());

                            cmd.Parameters.AddWithValue("@eventID", textBox7.Text);
                            
                            cmd.Parameters.AddWithValue("@manager", textBox6.Text.ToString());
                            cmd.Parameters.AddWithValue("@participant", listBox3.Items[i].ToString());

                            MySqlDataReader myReader2 = cmd.ExecuteReader();

                            myReader2.Close();

                            conn.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONMEETING CODE PART 2");
                }
                conn.Close();
                Console.WriteLine("Done.");



                label7.Show();
                label8.Show();
                label9.Show();
                textBox8.Show();
                listBox2.Show();
                listBox3.Show();
                button7.Hide();
                button8.Hide();

                button1.Text = "Edit";
                button2.Text = "Delete";

            }

            // FOR THE EDIT BUTTON
            if (button1.Text.Equals("Edit"))
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                

                button1.Text = "Confirm";
                button2.Text = "Cancel";

            } 
            // FOR THE CONFIRM BUTTON
            else if (button1.Text.Equals("Confirm"))
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                string eventID = textBox7.Text.ToString();
                string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

                // UPDATE ANY CHANGES
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "UPDATE pattersonevents SET eventName=@eventName, startTime=@startTime, endTime=@endTime, location=@location, description=@description, eventDay=@eventDay, username=@username WHERE eventID=@ID";

                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@ID", eventID);
                    cmd.Parameters.AddWithValue("@eventName", textBox1.Text.ToString());
                    cmd.Parameters.AddWithValue("@startTime", textBox2.Text.ToString());
                    cmd.Parameters.AddWithValue("@endTime", textBox3.Text.ToString());
                    cmd.Parameters.AddWithValue("@location", textBox4.Text.ToString());
                    cmd.Parameters.AddWithValue("@description", textBox5.Text.ToString());
                    
                    cmd.Parameters.AddWithValue("@eventDay", monthCalendar1.SelectionRange.Start);
                    cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());

                    MySqlDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        string t1 = myReader["eventName"].ToString();

                        if (t1.Equals(listBox1.SelectedItem.ToString()))
                        {
                            string t2 = myReader["starttime"].ToString();
                            string t3 = myReader["endtime"].ToString();
                            string t4 = myReader["location"].ToString();
                            string t5 = myReader["description"].ToString();

                            textBox1.Text = listBox1.SelectedItem.ToString();
                            textBox2.Text = t2;
                            textBox3.Text = t3;
                            textBox4.Text = t4;
                            textBox5.Text = t5;
                        }
                    }




                    myReader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                Console.WriteLine("Done.");

                // UPDATE EVENTS LISTBOX
                listBox1.Items.Clear();

                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "SELECT * FROM pattersonevents where username=@username";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());

                    MySqlDataReader myReader = cmd.ExecuteReader();


                    while (myReader.Read())
                    {
                        
                        string day = myReader["eventDay"].ToString();
                        Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                        Console.WriteLine(day);
                        if (monthCalendar1.SelectionRange.Start.ToString().Equals(day))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }

                    }

                    myReader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                Console.WriteLine("Done.");

                // SHOW MEETINGS THAT THE EMPLOYEE IS A PARTICIPANT OF
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = "SELECT * FROM pattersonevents JOIN pattersonmeeting ON pattersonevents.eventID = pattersonmeeting.meetingID";
                    
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                    MySqlDataReader myReader = cmd.ExecuteReader();
                    Console.WriteLine("THE PARTICIPANT FUNCTION GOT THIS FAR");

                    while (myReader.Read())
                    {

                        string day = myReader["eventDay"].ToString();
                        Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                        Console.WriteLine(day);
                        if (monthCalendar1.SelectionRange.Start.ToString().Equals(day))
                        {
                            Console.WriteLine("This is the inside of the join");
                            Console.WriteLine(myReader["eventName"]);
                            listBox1.Items.Add(myReader["eventName"]);
                        }

                    }

                    myReader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("THE PROGRAM FAILED AT THE PARTICIPANTS");
                }
                conn.Close();
                Console.WriteLine("Done.");

                button1.Text = "Edit";
                button2.Text = "Delete";
            }
            // BUTTON FOR EDIT MEETING
            if (button1.Text.Equals("Edit Meeting"))
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                
                button7.Show();
                button8.Show();

                button1.Text = "Confirm Meeting";
                button2.Text = "Cancel Meeting";

            }
            // BUTTON FOR CONFIRM MEETING
            else if (button1.Text.Equals("Confirm Meeting"))
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                string eventID = textBox7.Text.ToString();
                string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

                // UPDATE MEETING
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "UPDATE pattersonevents SET eventName=@eventName, startTime=@startTime, endTime=@endTime, location=@location, description=@description, eventDay=@eventDay, username=@username WHERE eventID=@ID";

                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@ID", eventID);
                    cmd.Parameters.AddWithValue("@eventName", textBox1.Text.ToString());
                    cmd.Parameters.AddWithValue("@startTime", textBox2.Text.ToString());
                    cmd.Parameters.AddWithValue("@endTime", textBox3.Text.ToString());
                    cmd.Parameters.AddWithValue("@location", textBox4.Text.ToString());
                    cmd.Parameters.AddWithValue("@description", textBox5.Text.ToString());
                    
                    cmd.Parameters.AddWithValue("@eventDay", monthCalendar1.SelectionRange.Start);
                    cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());

                    MySqlDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        string t1 = myReader["eventName"].ToString();

                        if (t1.Equals(listBox1.SelectedItem.ToString()))
                        {
                            string t2 = myReader["starttime"].ToString();
                            string t3 = myReader["endtime"].ToString();
                            string t4 = myReader["location"].ToString();
                            string t5 = myReader["description"].ToString();

                            textBox1.Text = listBox1.SelectedItem.ToString();
                            textBox2.Text = t2;
                            textBox3.Text = t3;
                            textBox4.Text = t4;
                            textBox5.Text = t5;
                        }
                    }




                    myReader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                Console.WriteLine("Done.");

                
                // DELETE ALL MEETINGS ENTRIES IN THE DATABASE BEFORE RE-MAKING THEM
                try
                {

                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = "DELETE FROM pattersonmeeting WHERE meetingID=@eventID";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                    

                    Console.Write("Event ID: ");
                    Console.WriteLine(textBox7.Text.ToString());

                    Console.Write("Event ID: ");
                    Console.WriteLine(textBox7.Text.ToString());

                    cmd.Parameters.AddWithValue("@eventID", textBox7.Text);


                    MySqlDataReader myReader2 = cmd.ExecuteReader();

                    while (myReader2.Read())
                    {

                    }

                    myReader2.Close();
                    conn.Close();
                    Console.WriteLine("Done.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("THE PROBLEM IS WITH THE UPDATE TO PATTERSONMEETING CODE DELETE");
                }

                bool available = true;

                try
                {


                    Console.WriteLine(listBox3.Items.Count);
                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        available = true;
                        try
                        {
                            
                            Console.WriteLine("Connecting to MySQL...");
                            conn.Open();
                            string sql = "SELECT * FROM pattersonevents where username=@username";
                            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                            cmd.Parameters.AddWithValue("@username", listBox3.Items[i].ToString());

                            MySqlDataReader myReader = cmd.ExecuteReader();


                            while (myReader.Read())
                            {

                                string day = myReader["eventDay"].ToString();
                                string startTime = myReader["startTime"].ToString();
                                string endTime = myReader["endTime"].ToString();
                                Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                                Console.WriteLine(day);
                                if (DateTime.Parse(textBox2.Text.ToString()) >= DateTime.Parse(startTime) && DateTime.Parse(textBox3.Text.ToString()) <= DateTime.Parse(endTime))
                                {
                                    if (day.Equals(monthCalendar1.SelectionRange.Start.ToString()))
                                    {
                                        available = false;
                                    }
                                }

                            }

                            myReader.Close();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        conn.Close();
                        Console.WriteLine("Done.");

                        if (available == true)
                        {
                            Console.WriteLine("Connecting to MySQL...");
                            conn.Open();
                            string sql = "INSERT INTO pattersonmeeting(meetingID, manager, participant) VALUES(@eventID, @manager, @participant)";
                            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                            

                            Console.Write("Event ID: ");
                            Console.WriteLine(textBox7.Text.ToString());

                            Console.Write("Event ID: ");
                            Console.WriteLine(textBox7.Text.ToString());
                            Console.Write("Manager: ");
                            Console.WriteLine(textBox8.Text.ToString());
                            Console.Write("Participant: ");
                            Console.WriteLine(listBox3.Items[i].ToString());

                            cmd.Parameters.AddWithValue("@eventID", textBox7.Text);
                            
                            cmd.Parameters.AddWithValue("@manager", textBox6.Text.ToString());
                            cmd.Parameters.AddWithValue("@participant", listBox3.Items[i].ToString());

                            MySqlDataReader myReader2 = cmd.ExecuteReader();

                            myReader2.Close();

                            conn.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONMEETING CODE PART 2");
                }
                conn.Close();
                Console.WriteLine("Done.");



                label7.Show();
                label8.Show();
                label9.Show();
                textBox8.Show();
                listBox2.Show();
                listBox3.Show();
                button7.Hide();
                button8.Hide();

                button1.Text = "Edit";
                button2.Text = "Delete";

                

                listBox1.Items.Clear();

                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "SELECT * FROM pattersonevents where username=@username";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());

                    MySqlDataReader myReader = cmd.ExecuteReader();


                    while (myReader.Read())
                    {

                        string day = myReader["eventDay"].ToString();
                        Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                        Console.WriteLine(day);
                        if (monthCalendar1.SelectionRange.Start.ToString().Equals(day))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }

                    }

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

                    string sql = "SELECT * FROM pattersonevents JOIN pattersonmeeting ON pattersonevents.eventID = pattersonmeeting.meetingID";
                    
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                    MySqlDataReader myReader = cmd.ExecuteReader();
                    Console.WriteLine("THE PARTICIPANT FUNCTION GOT THIS FAR");

                    while (myReader.Read())
                    {

                        string day = myReader["eventDay"].ToString();
                        Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                        Console.WriteLine(day);
                        if (monthCalendar1.SelectionRange.Start.ToString().Equals(day))
                        {
                            Console.WriteLine("This is the inside of the join");
                            Console.WriteLine(myReader["eventName"]);
                            listBox1.Items.Add(myReader["eventName"]);
                        }

                    }

                    myReader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("THE PROGRAM FAILED AT THE PARTICIPANTS");
                }
                conn.Close();
                Console.WriteLine("Done.");

                listBox3.Items.Clear();
                try
                {

                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        Console.WriteLine("Connecting to MySQL...");
                        conn.Open();

                        string sql = "SELECT * FROM pattersonmeeting where meetingID=@eventID";
                        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@eventID", textBox7.Text.ToString());

                        MySqlDataReader myReader2 = cmd.ExecuteReader();

                        while (myReader2.Read())
                        {
                            listBox3.Items.Add(myReader2["participant"]);
                        }

                        myReader2.Close();

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONMEETING CODE PART 2");
                }
                conn.Close();
                Console.WriteLine("Done.");

                button7.Hide();
                button8.Hide();
                button1.Text = "Edit";
                button2.Text = "Delete";
            }

            

        }

        // THIS IS FOR WHEN THE LIST BOX  FOR THE EVENTS CHANGES
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox7.ReadOnly = true;
            button1.Show();
            button2.Show();
            label7.Hide();
            textBox8.Hide();
            textBox8.Clear();
            
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonevents where username=@username and meetingBool=@notMeeting";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());
                cmd.Parameters.AddWithValue("@notMeeting", 0);

                MySqlDataReader myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    string t1 = myReader["eventName"].ToString();

                    if (t1.Equals(listBox1.SelectedItem.ToString()))
                    {
                        string t2 = myReader["starttime"].ToString();
                        string t3 = myReader["endtime"].ToString();
                        string t4 = myReader["location"].ToString();
                        string t5 = myReader["description"].ToString();
                        string t7 = myReader["eventid"].ToString();
                        string t9 = myReader["meetingBool"].ToString();

                        textBox1.Text = listBox1.SelectedItem.ToString();
                        textBox2.Text = t2;
                        textBox3.Text = t3;
                        textBox4.Text = t4;
                        textBox5.Text = t5;
                        textBox7.Text = t7;
                        textBox9.Text = "False";


                    }
                }
                
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
                string sql = "SELECT * FROM pattersonevents where username=@username and meetingBool=@isMeeting";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());
                cmd.Parameters.AddWithValue("@isMeeting", 1);

                MySqlDataReader myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    bool alreadyOnList = false;

                    string t1 = myReader["eventName"].ToString();

                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if (t1.Equals(listBox1.Items[i].ToString()))
                        {
                            alreadyOnList = true;
                        }
                    }


                    if (t1.Equals(listBox1.SelectedItem.ToString()) && alreadyOnList == false)
                    {
                        string t2 = myReader["starttime"].ToString();
                        string t3 = myReader["endtime"].ToString();
                        string t4 = myReader["location"].ToString();
                        string t5 = myReader["description"].ToString();
                        string t7 = myReader["eventid"].ToString();
                        string t9 = myReader["meetingBool"].ToString();

                        textBox1.Text = listBox1.SelectedItem.ToString();
                        textBox2.Text = t2;
                        textBox3.Text = t3;
                        textBox4.Text = t4;
                        textBox5.Text = t5;
                        textBox7.Text = t7;
                        textBox8.Text = textBox6.Text.ToString();
                        textBox9.Text = t9;


                    }
                }

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

                string sql = "SELECT * FROM pattersonevents JOIN pattersonmeeting ON pattersonevents.eventID = pattersonmeeting.meetingID";
                
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                MySqlDataReader myReader = cmd.ExecuteReader();
                Console.WriteLine("THE PARTICIPANT FUNCTION GOT THIS FAR");

                while (myReader.Read())
                {

                    string t1 = myReader["eventName"].ToString();
                    

                    if (t1.Equals(listBox1.SelectedItem.ToString()))
                    {
                        string t2 = myReader["starttime"].ToString();
                        string t3 = myReader["endtime"].ToString();
                        string t4 = myReader["location"].ToString();
                        string t5 = myReader["description"].ToString();
                        string t7 = myReader["eventid"].ToString();
                        string t8 = myReader["manager"].ToString();
                        string t9 = myReader["meetingBool"].ToString();

                        string t6 = textBox6.Text.ToString();
                        
                        if (!t6.Equals(t8))
                        {

                        }

                        textBox1.Text = listBox1.SelectedItem.ToString();
                        textBox2.Text = t2;
                        textBox3.Text = t3;
                        textBox4.Text = t4;
                        textBox5.Text = t5;
                        textBox7.Text = t7;
                        textBox8.Text = t8;
                        textBox9.Text = t9;

                        button1.Hide();
                        button2.Hide();

                        if (textBox8.Text.ToString().Equals(textBox6.Text.ToString()))
                        {
                            button1.Show();
                            button2.Show();
                        }
                        label7.Show();
                        textBox8.Show();

                    }

                }

                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("THE PROGRAM FAILED AT THE PARTICIPANTS");
            }
            conn.Close();
            Console.WriteLine("Done.");
            

            button1.Hide();
            button2.Hide();

            if (textBox8.Text.ToString().Equals(textBox6.Text.ToString()) || textBox8.Text.ToString().Equals(""))
            {
                button1.Show();
                button2.Show();
            }
            label7.Show();
            textBox8.Show();

            if (textBox9.Text.Equals("True"))
            {
                button1.Text = "Edit Meeting";
                button2.Text = "Delete Meeting";
            }
            else
            {
                button1.Text = "Edit";
                button2.Text = "Delete";
            }



            listBox3.Items.Clear();


            try
            {

            
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM pattersonmeeting where meetingID=@eventID";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@eventID", textBox7.Text.ToString());

                MySqlDataReader myReader2 = cmd.ExecuteReader();

                while (myReader2.Read())
                {
                    listBox3.Items.Add(myReader2["participant"]);
                }

                myReader2.Close();
                
             

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONMEETING SELECT CODE IN LIST BOX");
            }
            conn.Close();
            Console.WriteLine("Done.");


        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";

            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;

            button1.Text = "Save";
            button2.Text = "Cancel";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (button2.Text.Equals("Cancel"))
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox7.Text = "";

                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                button1.Text = "Edit";
                button2.Text = "Delete";

            }
            else if (button2.Text.Equals("Delete"))
            {


                string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

                

                this.Hide();
                Form2 form2 = new Form2(textBox7.Text, textBox6.Text);
                form2.Closed += (s, args) => this.Close();
                form2.Show();

                

                listBox1.Items.Clear();
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "SELECT * FROM pattersonevents";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    MySqlDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        listBox1.Items.Add(myReader["eventName"]);
                    }

                    myReader.Close();

                    string sql2 = "SELECT COUNT(*) FROM pattersonevents";
                    MySql.Data.MySqlClient.MySqlCommand cmd2 = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    Int32 counter = (Int32)cmd2.ExecuteScalar();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                Console.WriteLine("Done.");



            }

            if (button2.Text.Equals("Cancel Meeting"))
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox7.Text = "";

                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                button1.Text = "Edit";
                button2.Text = "Delete";

                button7.Hide();
                button8.Hide();
                

            }
            else if (button2.Text.Equals("Delete Meeting"))
            {


                string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);



                this.Hide();
                Form2 form2 = new Form2(textBox7.Text, textBox6.Text);
                form2.Closed += (s, args) => this.Close();
                form2.Show();



                listBox1.Items.Clear();
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    string sql = "SELECT * FROM pattersonevents";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    MySqlDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        listBox1.Items.Add(myReader["eventName"]);
                    }

                    myReader.Close();

                    string sql2 = "SELECT COUNT(*) FROM pattersonevents";
                    MySql.Data.MySqlClient.MySqlCommand cmd2 = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    Int32 counter = (Int32)cmd2.ExecuteScalar();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                Console.WriteLine("Done.");

            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            this.Hide();
            var form3 = new Form3();
            form3.Closed += (s, args) => this.Close();
            form3.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            listBox1.Items.Clear();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonevents where username=@username";


                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());
                Console.WriteLine(monthCalendar1.SelectionRange.Start.Month.ToString());
                MySqlDataReader myReader = cmd.ExecuteReader();

                

                while (myReader.Read())
                {

                    if (myReader["eventDay"].ToString()[0] == '1' && myReader["eventDay"].ToString()[1] == '0')
                    {
                        char monthChar1 = myReader["eventDay"].ToString()[0];
                        char monthChar2 = myReader["eventDay"].ToString()[1];
                        char[] monthChar = { monthChar1, monthChar2 };
                        string month = new string(monthChar);
                        if (month.Equals(monthCalendar1.SelectionRange.Start.Month.ToString()))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }
                    }
                    else if (myReader["eventDay"].ToString()[0] == '1' && myReader["eventDay"].ToString()[1] == '1')
                    {
                        char monthChar1 = myReader["eventDay"].ToString()[0];
                        char monthChar2 = myReader["eventDay"].ToString()[1];
                        char[] monthChar = { monthChar1, monthChar2 };

                        string month = new string(monthChar);
                        if (month.Equals(monthCalendar1.SelectionRange.Start.Month.ToString()))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }
                    }
                    else if (myReader["eventDay"].ToString()[0] == '1' && myReader["eventDay"].ToString()[1] == '2')
                    {
                        char monthChar1 = myReader["eventDay"].ToString()[0];
                        char monthChar2 = myReader["eventDay"].ToString()[1];
                        char[] monthChar = { monthChar1, monthChar2 };
                        string month = new string(monthChar);
                        if (month.Equals(monthCalendar1.SelectionRange.Start.Month.ToString()))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }
                    }
                    else
                    {
                        char monthChar1 = myReader["eventDay"].ToString()[0];
                        char[] monthChar = { monthChar1 };
                        string month = new string(monthChar);
                        if (month.Equals(monthCalendar1.SelectionRange.Start.Month.ToString()))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }
                    }

                    Console.WriteLine(myReader["eventDay"]);
                    Console.WriteLine(myReader["eventDay"].ToString()[0]);
                    
                    
                }

                myReader.Close();

                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Enter code for date selected here:
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            listBox1.Items.Clear();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonevents where username=@username";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());
                //cmd.Parameters.AddWithValue("@noMeeting", 0);

                MySqlDataReader myReader = cmd.ExecuteReader();

                
                while (myReader.Read())
                {
                    
                    string day = myReader["eventDay"].ToString();
                    Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                    Console.WriteLine(day);
                    if (monthCalendar1.SelectionRange.Start.ToString().Equals(day))
                    {
                        listBox1.Items.Add(myReader["eventName"]);
                    }

                }

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

                string sql = "SELECT * FROM pattersonevents JOIN pattersonmeeting ON pattersonevents.eventID = pattersonmeeting.meetingID";
                
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                MySqlDataReader myReader = cmd.ExecuteReader();
                Console.WriteLine("THE PARTICIPANT FUNCTION GOT THIS FAR");

                while (myReader.Read())
                {

                    string day = myReader["eventDay"].ToString();
                    Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString());
                    Console.WriteLine(day);
                    if (monthCalendar1.SelectionRange.Start.ToString().Equals(day))
                    {
                        Console.WriteLine("This is the inside of the join");
                        Console.WriteLine(myReader["eventName"]);
                        string t6 = textBox6.Text.ToString();
                        string t8 = myReader["username"].ToString();
                        if (!t6.Equals(t8))
                        {
                            listBox1.Items.Add(myReader["eventName"]);
                        }
                        
                        string t1 = myReader["eventName"].ToString();


                    }

                }

                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("THE PROGRAM FAILED AT THE PARTICIPANTS");
            }
            conn.Close();
            Console.WriteLine("Done.");

            

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //this.Refresh();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Manager label
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Participants label
        }


        // THIS IS FOR WHEN THE LIST BOX  FOR THE EMPLOYEES CHANGES
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Employee listbox
        }

        // THIS IS FOR WHEN THE LIST BOX  FOR THE PARTICIPANTS CHANGES
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Participants listbox
        }

        // ADD EMPLOYEES TO PARTICIPANTS LISTBOX
        private void button7_Click(object sender, EventArgs e)
        {
            bool addThisItem = true;
            for(int i = 0; i < listBox3.Items.Count; i++)
            {
                String username1 = listBox2.SelectedItem.ToString();
                String username2 = listBox3.Items[i].ToString();
                if (username1.Equals(username2)){
                    addThisItem = false;
                }
                
            }
            if(addThisItem == true)
            {
                listBox3.Items.Add(listBox2.SelectedItem.ToString());
            }
            
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // Manager textbox
        }

        // REMOVE PARTICIPANTS FROM LISTBOX
        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0)
            {
                listBox3.Items.Remove(listBox3.SelectedItem.ToString());
            }
            
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            // This is where we add meeting events

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";

            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;
            label7.Show();
            label8.Show();
            label9.Show();
            textBox8.Show();
            listBox2.Show();
            listBox3.Show();
            button7.Show();
            button8.Show();

            button1.Text = "Save Meeting";
            button2.Text = "Cancel Meeting";


        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Employee label
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private string insertData()
        {
            string eventName3 = "";
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO pattersonevents (eventName, startTime, endTime, location, description, eventDay, username, meetingBool) VALUES(@eventName, @startTime, @endTime, @location, @description, @eventDay, @username, @isMeeting)";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                cmd.Parameters.AddWithValue("@eventName", textBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@startTime", textBox2.Text.ToString());
                cmd.Parameters.AddWithValue("@endTime", textBox3.Text.ToString());
                cmd.Parameters.AddWithValue("@location", textBox4.Text.ToString());
                cmd.Parameters.AddWithValue("@description", textBox5.Text.ToString());
                cmd.Parameters.AddWithValue("@eventDay", monthCalendar1.SelectionRange.Start);
                cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());
                cmd.Parameters.AddWithValue("@isMeeting", 1);

                MySqlDataReader myReader = cmd.ExecuteReader();
                string eventName2 = "";
                while (myReader.Read())
                {
                    string t1 = myReader["eventName"].ToString();
                    string eventName = myReader["eventName"].ToString();

                    if (t1.Equals(listBox1.SelectedItem.ToString()))
                    {
                        string t2 = myReader["starttime"].ToString();
                        string t3 = myReader["endtime"].ToString();
                        string t4 = myReader["location"].ToString();
                        string t5 = myReader["description"].ToString();


                        textBox1.Text = listBox1.SelectedItem.ToString();
                        textBox2.Text = t2;
                        textBox3.Text = t3;
                        textBox4.Text = t4;
                        textBox5.Text = t5;



                    }
                    eventName2 = eventName;
                }

                myReader.Close();
                conn.Close();
                Console.WriteLine("Done.");
                return eventName2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONEVENTS CODE");
            }

            return eventName3;



        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
