using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Schedulink
{
    public partial class ucDays : UserControl
    {
        string day, date, weekday;


        //Static variables
        public static DateTime selectedDay;
        public static string reason, hour;
        public static string Pname, Psurname;

        //Generation of BaseHandler
        BaseHandler baseHandler = new BaseHandler();

        public ucDays(string day, int x, int y)
        {
            InitializeComponent();
            this.day = day;
            this.Size = new Size(x, y);
            label1.Text = day;
            checkBox1.Hide();
            label1.Font = new Font("Arial", y / 7, FontStyle.Bold);
            


            //separating the blanks from the usable controls
            if (day.Equals("") || int.Parse(day) <= DateTime.Now.Day)
            {
                panel1.BackColor = Color.LightGray;
                this.Enabled = false;
            }
            else
            {
                panel1.BackColor = Color.LightGreen;
                correctDate(day, Hub._month);
            }
        }

        //Method to make the correct format for the date
        private void correctDate(string day, int month)
        {
            string correctDay, correctMonth;
            try
            {
                int tempDay = int.Parse(day);
                if (tempDay < 10)
                {
                    correctDay = "0" + tempDay;
                }
                else
                {
                    correctDay = day;
                }
                if(month < 10)
                {
                    correctMonth = "0" + month;
                }
                else
                {
                    correctMonth = month.ToString();
                }

                this.date = (correctDay + "/" + correctMonth + "/" + Hub._year).ToString();

            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        public static void FetchNames(String n,String s)
        {
            Pname = n; Psurname = s;
            
        }
        private void ucDays_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {


            if (checkBox1.Checked == false)
            {
                string format = "dd/MM/yyyy";
                try
                {
                    selectedDay = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                }
                catch(Exception ex)
                {
                    selectedDay = DateTime.Now;
                    MessageBox.Show(ex.Message);
                    
                }
                
                checkBox1.Checked = true;
                this.BackColor = Color.Red;
            }
            else
            {
                checkBox1.Checked = false;
                this.BackColor = Color.White;
            }

            //The making of the comboBoxes for choosing a date
            var panelContainer = this.Parent as Panel;
            try
            {
                var hubForm = panelContainer.TopLevelControl as Form;
                var panel = hubForm.Controls.Find("datePanel", true);
                Panel myPanel = (Panel)panel[0];
                var button = hubForm.Controls.Find("makeAppointmentButton", true);
                Button b = (Button)button[0];


                //Making the reasoning comboBox
                ComboBox reasonBox = new ComboBox();
                reasonBox.Name = "professorBox";
                reasonBox.Location = new Point(myPanel.Location.X, myPanel.Height + 150);
                reasonBox.Size = new Size(150, 75);
                reasonBox.Font = new Font("Arial", 13f);
                reasonBox.Items.Add("Book Recomendation");
                reasonBox.Items.Add("General Talk");
                reasonBox.Items.Add("Thesis Writing");
                reasonBox.Items.Add("Regrading");
                hubForm.Controls.Add(reasonBox);
                //Making the hours comboBox
                ComboBox hoursBox = new ComboBox();
                hoursBox.Name = "hoursBox";
                hoursBox.Visible = true;
                hoursBox.Location = new Point(myPanel.Location.X + 300, myPanel.Height + 150);
                hoursBox.Size = new Size(150, 75);
                hoursBox.Font = new Font("Arial", 13f);


                //BaseHandler
                //Fetch hours from Database
                List<String> hours = new List<String>();
                hours= baseHandler.BringHours(Pname,Psurname); //returns a list
                foreach (string i in hours)
                {
                    hoursBox.Items.Add(i);
                }
                hubForm.Controls.Add(hoursBox);


                //Event methods
                reasonBox.SelectedIndexChanged += new EventHandler(professorSelected);
                hoursBox.SelectedIndexChanged += new EventHandler(timeSelected);


                //Making the methods of the boxes
                void professorSelected(object sender, EventArgs e)
                {
                    reason = reasonBox.Text;
                    hoursBox.Enabled = true;
                }


                void timeSelected(object sender, EventArgs e)
                {
                    hour = hoursBox.Text;
                    b.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("here");
            }

        }
    }
}
