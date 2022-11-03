using System;
using System.Windows.Forms;


namespace DataCollector
{
    public partial class Form1 : Form
    {
        private Employee _employee = new Employee();
        private bool _isLoggedIn = Properties.Settings.Default.userIsLogged;
        public Form1()
        {
            InitializeComponent();
            SetValues();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Collect = new Collect();
            Collect.EmployeeName = this.comboBox1.SelectedItem.ToString();
            Collect.WorkType = this.comboBox2.SelectedItem.ToString();
            Collect.StartNumber = this.textBox1.Text;
            Collect.EndNumber = this.textBox2.Text;
            Collect.TotalValue = this.textBox3.Text;
            Collect.StartTime = this.maskedTextBox1.Text;
            Collect.EndTime = this.maskedTextBox2.Text;
            Collect.Comment = this.textBox4.Text;

            try
            {
                Collect.SaveData();
                MessageBox.Show("Sikeres mentés!", "Kész", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetValues();
            }
            catch (Exception ex)
            {
                for (int i = 0; i < Collect.Exceptions.Count; i++)
                {
                    MessageBox.Show(this, Collect.Exceptions[i], ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Loginform = new Login();
            Loginform.ShowDialog();

            this.label10.Text = Properties.Settings.Default.userIsLogged.ToString();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.SetMenuStripItems();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetValues()
        {
            var employeeNames = this._employee.GetAllEmployeesName();

            this.comboBox1.Items.Add("");

            foreach (var name in employeeNames)
            {
                this.comboBox1.Items.Add(name);
            }

            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.textBox1.Text = "0";
            this.textBox2.Text = "0";
            this.textBox3.Text = "0";
            this.textBox4.ResetText();
            this.maskedTextBox1.ResetText();
            this.maskedTextBox2.ResetText();
        }

        private void SetMenuStripItems()
        {
            if (Properties.Settings.Default.userIsLogged)
            {
                this.LoginToolStripMenuItem.Enabled = false;
                this.EmployeeToolStripMenuItem.Enabled = true;
                this.JobToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.LoginToolStripMenuItem.Enabled = true;
                this.EmployeeToolStripMenuItem.Enabled = false;
                this.JobToolStripMenuItem.Enabled = false;
            }
        }
    }
}
