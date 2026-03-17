namespace Schedulink
{
    public partial class Form1 : Form
    {
        //Form Dimensions
        int x;
        int y;

        //Temporary objects for testing 
        Student s = new Student("Andreas", "Dimopoulos", "p", "a", "p22038@unipi.gr");
         
        

        //Generation of BaseHandler
        BaseHandler baseHandler = new BaseHandler();

        
        String specialty;

        //UI initialization
        TextBox txtUserName = new TextBox();
        TextBox txtPassword = new TextBox();
        Button b = new Button();

        public Form1()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            this.Icon = new Icon("Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Generating UI
            generation();
            
        }

        //Login button click method will be replaced and put in BaseHandler class. Start of replacement code{
        private void bClick(object sender, EventArgs e)
        {
            String specialty;
            specialty = baseHandler.CheckLogIn(txtUserName.Text,txtPassword.Text);

            if (specialty.Equals("s") || specialty.Equals("p"))
            {
                this.Hide();
                Hub hub = new Hub(specialty, txtUserName.Text);
                hub.Show();
            }
            else
            {
                MessageBox.Show("Incorrect login info. Try again.");
            }
            
        }
        //End of replacement code}


        //Method for the form's UI generation
        private void generation()
        {
            //Making the log-in textboxes

            txtUserName.Size = new Size(x / 4, y / 7);
            txtUserName.Font = new Font("Arial", 11f);
            txtUserName.Location = new Point(3 * x / 8, y / 2 - 40);
            txtUserName.Visible = true;
            Controls.Add(txtUserName);

            txtPassword.Size = new Size(x / 4, y / 7);
            txtPassword.Font = new Font("Arial", 11f);
            txtPassword.Location = new Point(3 * x / 8, y / 2);
            txtPassword.Visible = true;
            Controls.Add(txtPassword);

            //Making advance button

            b.Text = "LOG IN";
            b.Font = new Font("Arial", 11f);
            b.BackColor = Color.White;
            b.Size = new Size(x / 6, y / 6);
            b.Location = new Point(5 * x / 12, y / 2 + 50);
            b.Click += new EventHandler(bClick);
            b.Visible = true;
            Controls.Add(b);
        }
    }
}
