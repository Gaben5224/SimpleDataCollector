using System;
using System.Windows.Forms;


namespace DataCollector
{
    public partial class DataInputForm : Form
    {
        private Employee _employee = new Employee();
        public DataInputForm()
        {
            InitializeComponent();
            SetValues();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Collect = new Collect
            {
                EmployeeName = this.comboBox1.SelectedItem.ToString(),
                WorkType = this.comboBox2.SelectedItem.ToString(),
                StartNumber = this.textBox1.Text,
                EndNumber = this.textBox2.Text,
                TotalValue = this.textBox3.Text,
                StartTime = this.maskedTextBox1.Text,
                EndTime = this.maskedTextBox2.Text,
                Comment = this.textBox4.Text
            };

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

        private void LoginFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var LoginFormform = new LoginFormForm();
            LoginFormform.ShowDialog();
        }

        private void DataInputForm_Activated(object sender, EventArgs e)
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

            foreach (var employeename in employeeNames)
            {
                this.comboBox1.Items.Add(employeename);
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
            var user = new User();

            if (user.IsUserLogged)
            {
                this.LoginFormToolStripMenuItem.Enabled = false;
                this.EmployeeToolStripMenuItem.Enabled = true;
                this.JobToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.LoginFormToolStripMenuItem.Enabled = true;
                this.EmployeeToolStripMenuItem.Enabled = false;
                this.JobToolStripMenuItem.Enabled = false;
            }
        }

        private void EmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var employeeForm = new EmployeeListForm();
            employeeForm.ShowDialog();
        }
    }
}
