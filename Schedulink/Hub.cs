using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.AxHost;

namespace Schedulink
{
    public partial class Hub : Form
    {
        //Width and Height of the form
        int x;// = Screen.PrimaryScreen.Bounds.Width;
        int y;// = Screen.PrimaryScreen.Bounds.Height;

        //Static variables
        public static int _year, _month;
        public static Hub instance;
        static List<string> pname = new List<string>();
        static string id, specialty;

        //List for weekdays
        string[] days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

        //Dictionary for prioritazation
        Dictionary<string, int> priority = new Dictionary<string, int>();

        //Variables used
        string selectedDate, selectedReason,time;
        int selectedReasonPriority;

        //Generation of BaseHandler
        BaseHandler baseHandler = new BaseHandler();

        //Generation of UI
        Button makeAppointmentButton = new Button();
        Button backButton = new Button();
        Button previousMonthButton = new Button();
        Button nextMonthButton = new Button();
        FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel();
        List<ucDays> monthDays = new List<ucDays>();

        //Temps



        public Hub()
        {
            InitializeComponent();
        }

        public Hub(string userSpecialty, string user_id)
        {
            id = user_id;
            specialty = userSpecialty;
            priority.Add("Book Recomendation", 1);
            priority.Add("General Talk", 1);
            priority.Add("Thesis Writing", 2);
            priority.Add("Regrading", 2);
            priority.Add("reason", 0);
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            Image backGround = new Bitmap("Background.png");
            this.BackgroundImage = backGround;
            
            this.Icon = new Icon("Icon.ico");
            instance = this;

            //Controls Itialization
            makeAppointmentButton.Name = "makeAppointmentButton";
            makeAppointmentButton.Text = "Make Appointment";
            makeAppointmentButton.Font = new Font("Arial", 11f);
            makeAppointmentButton.ForeColor = Color.White;
            makeAppointmentButton.BackColor = Color.LightBlue;
            makeAppointmentButton.Padding = new Padding(0, 0, 0, 0);
            makeAppointmentButton.Size = new Size(250, 100);
            makeAppointmentButton.Enabled = false;
            makeAppointmentButton.Visible = false;
            makeAppointmentButton.Click += new EventHandler(makeAppointmentButtonClick);


            backButton.Name = "backButton";
            backButton.Text = "Back";
            backButton.Font = new Font("Arial", 7f);
            backButton.Padding = new Padding(0, 0, 0, 0);
            backButton.ForeColor = Color.White;
            backButton.BackColor = Color.LightBlue;
            backButton.Size = new Size(75, 50);
            backButton.Location = new Point(0, 0);
            backButton.Visible = false;
            backButton.Click += new EventHandler(backButtonClick);


            previousMonthButton.Name = "previousMonthButton";
            previousMonthButton.Text = "p";
            previousMonthButton.Font = new Font("Arial", 10f);
            previousMonthButton.ForeColor = Color.White;
            previousMonthButton.BackColor = Color.LightBlue;
            previousMonthButton.Padding = new Padding(0 , 0, 0, 0);
            previousMonthButton.Size = new Size(50, 50);
            previousMonthButton.Enabled = false;
            previousMonthButton.Visible = false;
            previousMonthButton.Click += new EventHandler(previousMonthButtonClick);

            nextMonthButton.Name = "nextMonthButton";
            nextMonthButton.Text = "n";
            nextMonthButton.Font = new Font("Arial", 10f);
            nextMonthButton.ForeColor = Color.White;
            nextMonthButton.BackColor = Color.LightBlue;
            nextMonthButton.Padding = new Padding(0, 0, 0, 0);
            nextMonthButton.Size = new Size(50, 50);
            nextMonthButton.Enabled = false;
            nextMonthButton.Visible = false;
            nextMonthButton.Click += new EventHandler(nextMonthButtonClick);


            

            //Form customizing
            this.WindowState = FormWindowState.Maximized;
        }

        

        private void Hub_Load(object sender, EventArgs e)
        {
            //Logic initialization
            if (specialty.Equals("s"))
            {
                generationStudent();
            }
            else
            {
                generationProfessor();
            }


        }

        //Method to close application if form is closed
        private void Hub_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //Different methods for the two cases of UI generation 
        private void generationStudent()
        {
            flowLayoutPanel1.Controls.Clear();
            this.Controls.Clear();

            //Menu Generation
            MenuStrip studentMenu = new MenuStrip();
            studentMenu.Name = "studentMenu";
            studentMenu.Visible = true;
            studentMenu.Items.Add(new ToolStripMenuItem("Make Apointment"));
            studentMenu.Items.Add(new ToolStripMenuItem("See Apointments", null, seeAppointmentsClick));
            studentMenu.Items.Add(new ToolStripMenuItem("Close", null, exitButtonClick));
            studentMenu.Dock = DockStyle.Left;
            studentMenu.Visible = true;


            //BaseHandler
            //Calling BringProfs() from BaseHandler for the initialization of studentmenu
            List<string> tmp= new List<string>(baseHandler.BringProfessors());

            foreach (string i in tmp)
            {
                (studentMenu.Items[0] as ToolStripMenuItem).DropDownItems.Add(i, null, ProfessorSelected);
            }
            this.Controls.Add(studentMenu);
            
        }

        //Methods used in generation Student
        private void ProfessorSelected(object sender, EventArgs e)
        {
            //Pass the professor name for the hours
            pname= sender.ToString().Split(" ").Select(s => s.ToString()).ToList();
            ucDays.FetchNames(pname[0], pname[1]);
            this.Controls.Clear();
            //Panel dimensions
            flowLayoutPanel1.Name = "datePanel";
            flowLayoutPanel1.Size = new Size(this.Width - 400, this.Height - 300);
            flowLayoutPanel1.Location = new Point(400, 100);
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Visible = false;
            Controls.Add(flowLayoutPanel1);

            //Back Button Generation
            Controls.Add(backButton);
            backButton.Visible = true;

            //Buttons
            Controls.Add(previousMonthButton);
            previousMonthButton.Location = new Point(flowLayoutPanel1.Location.X + flowLayoutPanel1.Width - 300, this.Height - 350);
            previousMonthButton.Enabled = true;
            previousMonthButton.Visible = true;
            Controls.Add(nextMonthButton);
            nextMonthButton.Location = new Point(flowLayoutPanel1.Location.X + flowLayoutPanel1.Width - 230, this.Height - 350);
            nextMonthButton.Enabled = true;
            nextMonthButton.Visible = true; 

            //Button Generation
            Controls.Add(makeAppointmentButton);
            makeAppointmentButton.Location = new Point(flowLayoutPanel1.Location.X + flowLayoutPanel1.Width - 150, this.Height - 250);
            makeAppointmentButton.Visible = true;
            

            //Days generation
            x = flowLayoutPanel1.Size.Width;
            y = flowLayoutPanel1.Size.Height;
            showDays(DateTime.Now.Month, DateTime.Now.Year);

            //Labels generation
            labelManagement(flowLayoutPanel1.Width, flowLayoutPanel1.Height);
        }
        private void seeAppointmentsClick(object sender, EventArgs e)
        {
            this.Controls.Clear();
            

            //Making the DataGridView
            DataGridView appointmentsTable = new DataGridView();
            appointmentsTable.Name = "appointmentsTable";
            appointmentsTable.Width = this.Width - 500;
            appointmentsTable.Location = new Point(350, 100);
            appointmentsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            appointmentsTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            appointmentsTable.Visible = true;
            appointmentsTable.BackgroundColor = Color.Beige;
            appointmentsTable.AutoGenerateColumns = true;
            appointmentsTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            appointmentsTable.DataSource = baseHandler.ShowAppointments(id, specialty);
            appointmentsTable.DataBindingComplete += (s, e) =>
            {
                calculateTableHeight(appointmentsTable);
            };
            appointmentsTable.SelectionChanged += new EventHandler(appointmentSelected);
            Controls.Add(appointmentsTable);


            //Back Button Generation
            Controls.Add(backButton);
            backButton.Visible = true;
        }

        private void appointmentSelected(object sender, EventArgs e)
        {
            if (sender is DataGridView)
            {


                DataGridView dgv = (DataGridView)sender;
                if (dgv.SelectedRows.Count > 0)
                {
                    int selectedAppointmentIndex;
                    selectedAppointmentIndex = dgv.SelectedRows[0].Index;
                    //selectedDate = dgv.Rows[selectedAppointmentIndex].Cells["appDate"].Value.ToString();

                    DateTime selectedDate2 = Convert.ToDateTime(dgv.Rows[selectedAppointmentIndex].Cells["appDate"].Value);
                    string fdate = selectedDate2.ToString("yyyy-MM-dd");

                    selectedReason = dgv.Rows[selectedAppointmentIndex].Cells["reason"].Value.ToString();
                    time = dgv.Rows[selectedAppointmentIndex].Cells["time"].Value.ToString();
                    selectedReasonPriority = priority[selectedReason];
                    //MessageBox.Show(fdate + " " + selectedReason+" "+time + " " + selectedReasonPriority.ToString());

                    Button deleteButton = new Button();
                    deleteButton.Name = "deleteButton";
                    deleteButton.Text = "Delete Appointment";
                    deleteButton.Size = new Size(250, 150);
                    deleteButton.Location = new Point(350, this.Height - 300);
                    deleteButton.ForeColor = Color.White;
                    deleteButton.BackColor = Color.LightBlue;
                    deleteButton.Click += (sender, e) => deleteButtonClick(sender, e, fdate, time);
                    Controls.Add(deleteButton);
                }
            }
        }

        private void deleteButtonClick(object sender, EventArgs e, string date, string time)
        {
            baseHandler.DeleteAppointment(date, time);
        }

        private void exitButtonClick(object sender, EventArgs e) 
        {
            Application.Exit();
        }


        private void backButtonClick(object sender, EventArgs e)
        {
            if (specialty.Equals("s"))
            {
                generationStudent();
            }
            else
            {
                generationProfessor();
            }
        }

        


        //Click method for the button
        private void makeAppointmentButtonClick(object sender, EventArgs e)
        {
            
            //Appointment app = new Appointment(ucDays.selectedDay, ucDays.hour, ucDays.reason);
            //MessageBox.Show(app.ToString());
            baseHandler.MakeAppointment(pname, id, ucDays.selectedDay, ucDays.hour, ucDays.reason);
        }

        private void nextMonthButtonClick(object sender, EventArgs e)
        {
            if(_month < 12)
            {
                _month++;
            }
            else
            {
                _month = 1;
                _year++;
            }
            showDays(_month, _year);
        }

        private void previousMonthButtonClick(object sender, EventArgs e)
        {
            if(!(DateTime.Now > DateTime.Now.AddMonths(-1)))
            {
                if (_month > 1)
                {
                    _month--;
                }
                else
                {
                    _month = 12;
                    _year--;
                }
                showDays(_month, _year);
            }
            else
            {
                MessageBox.Show("Cannot go to the past, sorry!");
            }
        }


        //Method to generate the professor UI
        private void generationProfessor()
        {
            flowLayoutPanel1.Controls.Clear();
            this.Controls.Clear();
            MenuStrip professorMenu = new MenuStrip();
            professorMenu.Name = "professorMenu";
            professorMenu.Dock = DockStyle.Left;
            //professorMenu.BackColor = Color.Transparent;
            professorMenu.Visible = true;
            professorMenu.Items.Add(new ToolStripMenuItem("See Appointments", null, seeAppointmentsClick));
            professorMenu.Items.Add(new ToolStripMenuItem("Exit", null, exitButtonClick));
            Controls.Add(professorMenu); 
        }

        

        private void Hub_Resize(object sender, EventArgs e)
        {
            
            //generationStudent();
            //flowLayoutPanel1.Update(); 
        }

        //Method to display the calendar
        private void showDays(int month, int year)
        {
            flowLayoutPanel1.Controls.Clear();
            _month = month;
            _year = year;
            
            string monthName = new DateTimeFormatInfo().GetMonthName(month);
            //Na valw to label gia to onoma tou mina
            DateTime startOfTheMonth = new DateTime(year, month, 1);
            int day = DateTime.DaysInMonth(year, month);
            int week = Convert.ToInt32(startOfTheMonth.DayOfWeek.ToString("d")) - 1;
            //Generation for Day-Boxes
            int fullX = (x / 7 - 10) * 7 + 45;
            int fullY = y / (DateTime.DaysInMonth(year, month) / 4) * 5 + 30;
            for (int i = 0; i < week; i++)
            {
                ucDays uc = new ucDays("", (x / 7) - 10, y / (DateTime.DaysInMonth(year, month) / 4));
                flowLayoutPanel1.Controls.Add(uc);
            }
            for (int i = 0; i < day; i++)
            {
                ucDays uc = new ucDays(i+1 + "", (x / 7) - 10, y / (DateTime.DaysInMonth(year, month) / 4));
                monthDays.Add(uc);
                flowLayoutPanel1.Controls.Add(uc);
                
            }

            flowLayoutPanel1.Size = new Size(fullX, fullY);
            flowLayoutPanel1.Visible = true;
            
        }

        private void labelManagement(int x, int y)
        {
            
            int startX = flowLayoutPanel1.Location.X;
            int startY = flowLayoutPanel1.Location.Y;
            for (int i = 0; i < 7; i++)
            {
                System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
                lbl.Name = "Label" + (i + 1).ToString();
                lbl.Font = new Font("Arial", flowLayoutPanel1.Height /40);
                lbl.BackColor = Color.Transparent;
                lbl.Text = days[i];
                lbl.Size = new Size(x / 7, y / 15);
                lbl.Location = new Point(startX + lbl.Width * i, startY - 50);
                lbl.Visible = true;
                Controls.Add(lbl);
                
            }
            
            
        }

        private void calculateTableHeight(DataGridView table)
        {
            int totalHeight = 0;
            // Resize height to fit all rows + column headers
            int totalRowHeight = 0;

            // Sum height of each visible row
            foreach (DataGridViewRow row in table.Rows)
            {
                if (row.Visible)
                {
                    totalRowHeight += row.Height;
                }          
            }

            // Add height of column headers
            int headerHeight = table.ColumnHeadersVisible ? table.ColumnHeadersHeight : 0;

            // Optional: Add a small buffer to avoid clipping
            int padding = 2;

            totalHeight = totalRowHeight + headerHeight + padding;

            table.Height = totalHeight;
        }
        
    }
}
