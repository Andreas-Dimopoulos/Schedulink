using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Schedulink
{
    

    internal class BaseHandler
    {
        String connectionString = "Data source=appointments.db;Version=3";
        SQLiteConnection connection;
        
        public BaseHandler()
        {
            connection = new SQLiteConnection(connectionString);
        }

        private String openConnection()
        {
            //Initialize tables the moment the constructor is called
            connection.Open();

            try
            {
                String createAppointmentTableSQL = "Create table if not exists appointment(" +
                    "id integer primary key autoincrement," +
                    "date Text," +
                    "ProfID Text," +
                    "StudID Text," +
                    "reason Text)";
                SQLiteCommand command1 = new SQLiteCommand(createAppointmentTableSQL, connection);
                command1.ExecuteNonQuery();

                String createProfessorTableSQL = "Create table if not exists professors(" +
                    "name Text," +
                    "surname Text," +
                    "username Text primary key ," +
                    "password Text," +
                    "email Text," +
                    "phone Numeric)";
                SQLiteCommand command2 = new SQLiteCommand(createProfessorTableSQL, connection);
                command2.ExecuteNonQuery();

                String createStudentTableSQL = "Create table if not exists students(" +
                    "name Text," +
                    "surname Text," +
                    "username Text primary key not null," +
                    "password Text not null," +
                    "email Text)";

                SQLiteCommand command3 = new SQLiteCommand(createStudentTableSQL, connection);
                command3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            connection.Close();
            return "all good";
        }

        public void MakeAppointment( List<String> Pnames, String ID, DateTime date, String hour, String re)
        {

            string pID = FindProfessorID(Pnames);
            //Insert Appointment into the base
            connection.Open();
            string insertSQL = "Insert into appointment (ProfID," +
                "StudID,date,hour, reason) values(" +
                "@profId,@studId,@Date,@Hour,@Reason)";
            SQLiteCommand command = new SQLiteCommand(insertSQL, connection);
            command.Parameters.AddWithValue("profId", pID);
            command.Parameters.AddWithValue("studId", ID);
            command.Parameters.AddWithValue("Date", date);
            command.Parameters.AddWithValue("Hour", hour);
            command.Parameters.AddWithValue("Reason", re);
            command.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Made appointment");
        }

        public void DeleteAppointment(string date, string time)
        {
           
            connection.Open();
            String deleteSQL = "Delete from appointment where date like @Date and hour=@time";
            SQLiteCommand command = new SQLiteCommand(deleteSQL, connection);
            command.Parameters.AddWithValue("Date", date+ "%");
            command.Parameters.AddWithValue("time", time);
            command.ExecuteNonQuery();
            MessageBox.Show("Appointment Deleted");
            MessageBox.Show("Go Back");
            connection.Close();


        }

        public string CheckLogIn(String name,String password)
        {
            String result="none"; //In case the isn't any record in the base

            connection.Open ();
            String selectSQL = "Select specialty from logs where username=@uname and password=@pass";
            SQLiteCommand command = new SQLiteCommand(selectSQL, connection);
            command.Parameters.AddWithValue ("uname", name);
            command.Parameters.AddWithValue("pass",password);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) { 
                result=reader.GetString(0);
            }
            connection.Close();
            return result;
        }

        public List<String> BringHours(String name, String surname)
        {
            List<String> hours = new List<String>();
            String h;
            connection.Open ();
            String selectHours = "Select * from professors where name=@n and surname=@surn";
            SQLiteCommand command = new SQLiteCommand( selectHours, connection);
            command.Parameters.AddWithValue("n", name);
            command.Parameters.AddWithValue("surn", surname);
            
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                h = reader.GetString(6);
                try
                {
                    hours = h.Split(",").Select(s => s.ToString()).ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }


            }
            connection.Close();

            return hours;
        }

        public List<String> BringProfessors()
        {
            List<String> profs = new List<String>();
            connection.Open();
            String selectProfs = "Select * from professors";
            SQLiteCommand command = new SQLiteCommand(selectProfs, connection);
            
            SQLiteDataReader sQLiteDataReader = command.ExecuteReader();
            while (sQLiteDataReader.Read()) { 
                profs.Add(sQLiteDataReader.GetString(0)+" "+ sQLiteDataReader.GetString(1));
                
            }
            
            connection.Close();
            return profs;
        }

        private string FindProfessorID(List<string> pnames)
        {
            string id="";
            connection.Open();
            String selectHours = "Select * from professors where name=@n and surname=@surn";
            SQLiteCommand command = new SQLiteCommand(selectHours, connection);
            command.Parameters.AddWithValue("n", pnames[0]);
            command.Parameters.AddWithValue("surn", pnames[1]);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id=reader.GetString(2);
            }
            connection.Close();
            return id;
        }

   
        
        public List<Appointment> ShowAppointments(string Id, string specialty)
        {

            List<Appointment> appointment = new List<Appointment>();
            connection.Open();

            if (specialty == "p") {
                String selectProfs = "Select * from appointment where ProfID=@Pid";
                SQLiteCommand command = new SQLiteCommand(selectProfs, connection);
                command.Parameters.AddWithValue("Pid", Id);
                SQLiteDataReader sQLiteDataReader = command.ExecuteReader();
                while (sQLiteDataReader.Read())
                {
                    Appointment appointment1 = new Appointment(sQLiteDataReader.GetString(2), sQLiteDataReader.GetDateTime(3), sQLiteDataReader.GetString(4), sQLiteDataReader.GetString(5));
                    appointment.Add(appointment1);

                }
            }
            else
            {
                String selectProfs = "Select * from appointment where StudID=@Sid";
                SQLiteCommand command = new SQLiteCommand(selectProfs, connection);
                command.Parameters.AddWithValue("Sid", Id);
                SQLiteDataReader sQLiteDataReader = command.ExecuteReader();
                while (sQLiteDataReader.Read())
                {
                    Appointment appointment1 = new Appointment(sQLiteDataReader.GetString(1), sQLiteDataReader.GetDateTime(3), sQLiteDataReader.GetString(4), sQLiteDataReader.GetString(5));
                    appointment.Add(appointment1);

                }
            }
            connection.Close();
            return appointment;
        }
    }
        
    
}
