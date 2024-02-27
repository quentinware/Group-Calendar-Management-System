using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Calendar_Management_System
{
    class Event
    {
        string eventName;
        string startTime;
        string endTime;
        string description;
        string location;
        DateTime eventDay;
        int eventID;
        string manager;
        string meetingBool;


        public Event()
        {

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public String[] employeeBoxFill()
        {
            string[] employeesForBox = new string[100];


            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonemployee";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                MySqlDataReader myReader = cmd.ExecuteReader();
                int i = 0;
                while (myReader.Read())
                {

                    employeesForBox[i] = myReader["username"].ToString();

                    i++;
                }

                myReader.Close();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            return employeesForBox;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public bool checkManager(string username)
        {
            bool manager = false;

            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonemployee where username=@username";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    String isManagerBool = myReader.GetString("managerBool");

                    Console.Write("is Manager? ");
                    Console.WriteLine(isManagerBool);



                    if (isManagerBool.Equals("True"))
                    {
                        manager = true;
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

            return manager;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void retrieveEventData(string username)
        {
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonevents where username=@username";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    string t1 = myReader["eventName"].ToString();
                    setEventName(myReader["eventName"].ToString());
                    
                    

                    if (t1.Equals(eventName))
                    {
                        setStartTime(myReader["starttime"].ToString());
                        setEndTime(myReader["endtime"].ToString());
                        setLocation(myReader["location"].ToString());
                        setDescription(myReader["description"].ToString());
                        setEventID(int.Parse(myReader["eventid"].ToString()));



                        Console.WriteLine(eventID);
                        Console.WriteLine("This works here");


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



        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void insertMeeting(int eventID, string manager, string[] participant, string username)
        {
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {

                for (int i = 0; i < participant.Length; i++)
                {

                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = "INSERT INTO pattersonmeeting(meetingID, manager, participant) VALUES(@eventID, @manager, @participant)";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                    //cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                    Console.Write("Event ID: ");
                    Console.WriteLine(eventID);

                    //eventID = Int32.Parse(textBox7.Text.ToString());
                    Console.Write("Event ID: ");
                    Console.WriteLine(eventID);
                    Console.Write("Manager: ");
                    Console.WriteLine(manager);
                    Console.Write("Participant: ");
                    Console.WriteLine(participant[i]);

                    cmd.Parameters.AddWithValue("@eventID", eventID);
                    //cmd.Parameters.Add("@eventID", eventID);
                    cmd.Parameters.AddWithValue("@manager", username);
                    cmd.Parameters.AddWithValue("@participant", participant[i]);


                    MySqlDataReader myReader2 = cmd.ExecuteReader();

                    while (myReader2.Read())
                    {

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
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public string[] eventBoxFill(string username, string today)
        {
            string[] events = new string[100];


            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM pattersonevents where username=@username AND meetingBool=@noMeeting";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@noMeeting", "false");
                MySqlDataReader myReader = cmd.ExecuteReader();

                int i = 0;
                while (myReader.Read())
                {

                    string day = myReader["eventDay"].ToString();

                    if (today.Equals(day))
                    {

                        events[i] = myReader["eventName"].ToString();
                        i++;
                    }

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
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM pattersonevents JOIN pattersonmeeting ON pattersonevents.eventID = pattersonmeeting.meetingID";
                //string sql = "SELECT * FROM pattersonevents JOIN pattersonmeeting ON pattersonevents.eventID = pattersonmeeting.meetingID where pattersonevents.username=pattersonmeeting.participant";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                //cmd.Parameters.AddWithValue("@username", textBox6.Text.ToString());

                MySqlDataReader myReader = cmd.ExecuteReader();
                Console.WriteLine("THE PARTICIPANT FUNCTION GOT THIS FAR");

                int i = 0;
                while (myReader.Read())
                {

                    string day = myReader["eventDay"].ToString();
                    Console.WriteLine(today);
                    Console.WriteLine(day);
                    if (today.Equals(day))
                    {
                        events[i] = myReader["eventName"].ToString();
                        i++;


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

            return events;

        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void saveEvent(string eventName, string startTime, string endTime, string location, string description, DateTime eventDay, string username)
        {

            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO pattersonevents (eventName, startTime, endTime, location, description, eventDay, username) VALUES(@eventName, @startTime, @endTime, @location, @description, @eventDay, @username)";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                cmd.Parameters.AddWithValue("@eventName", eventName);
                cmd.Parameters.AddWithValue("@startTime", startTime);
                cmd.Parameters.AddWithValue("@endTime", endTime);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@description",description);
                cmd.Parameters.AddWithValue("@eventDay", eventDay);
                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader myReader = cmd.ExecuteReader();


                myReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void saveMeetingEvent(string eventName, string startTime, string endTime, string location, string description, DateTime eventDay, string username)
        {
            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            //string eventName = "";
            //int eventID = 0;
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO pattersonevents (eventName, startTime, endTime, location, description, eventDay, username, meetingBool) VALUES(@eventName, @startTime, @endTime, @location, @description, @eventDay, @username, @isMeeting)";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                cmd.Parameters.AddWithValue("@eventName", eventName);
                cmd.Parameters.AddWithValue("@startTime", startTime);
                cmd.Parameters.AddWithValue("@endTime", endTime);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@eventDay", eventDay);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@isMeeting", 1);

                MySqlDataReader myReader = cmd.ExecuteReader();

                //eventName = textBox1.Text.ToString();


                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONEVENTS CODE");
            }
            conn.Close();
            Console.WriteLine("Done.");
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public int getMeetingID(string eventName)
        {
            int eventID = 0;


            string connStr = "server=csitmariadb.eku.edu;user=student;database=csc340_db;port=3306;password=Maroon@21?;";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "select * from pattersonevents where eventName=@eventName";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@eventName", eventName);

                MySqlDataReader myReader = cmd.ExecuteReader();


                while (myReader.Read())
                {
                    eventID = Int32.Parse(myReader["eventID"].ToString());
                    
                }

                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("THE PROBLEM IS WITH THE INSERT TO PATTERSONMEETING CODE PART 1");
            }
            conn.Close();
            Console.WriteLine("Done.");

            return eventID;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //GETTERS AND SETTERS
        public string getEventName()
        {
            return eventName;
        }
        public void setEventName(string s)
        {
            eventName = s;
        }
        public string getStartTime()
        {
            return startTime;
        }
        public void setStartTime(string s)
        {
            startTime = s;
        }
        public string getEndTime()
        {
            return endTime;
        }
        public void setEndTime(string s)
        {
            endTime = s;
        }
        public string getManager()
        {
            return manager;
        }
        public void setManager(string s)
        {
            manager = s;
        }
        public string getMeetingBool()
        {
            return meetingBool;
        }
        public void setMeetingBool(string s)
        {
            meetingBool = s;
        }
        public string getDescription()
        {
            return description;
        }
        public void setDescription(string s)
        {
            description = s;
        }
        public string getLocation()
        {
            return location;
        }
        public void setLocation(string s)
        {
            location = s;
        }
        public int getEventID()
        {
            return eventID;
        }
        public void setEventID(int i)
        {
            eventID = i;
        }
        public DateTime getEventDay()
        {
            return eventDay;
        }
        public void setEventDay(DateTime D)
        {
            eventDay = D;
        }

    }
}
