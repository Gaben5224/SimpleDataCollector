using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DataCollector
{
    public partial class EmployeeListForm : Form
    {
        private Employee _employee = new Employee();
        private bool _isNewEmployee;
        public EmployeeListForm()
        {
            var names = _employee.GetAllEmployeesName();
            InitializeComponent();
            this.panel3.Enabled = false;
            this.panel3.Visible = false;
            this.comboBox1.Items.Add("");

            
            foreach(var name in names)
            {
                this.comboBox1.Items.Add(name);
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.checkBox2.Checked = false;
                this.panel3.Enabled = false;
                this.panel3.Visible = false;
                this.panel4.Enabled = true;
                this.panel4.Visible = true;
                this.textBox1.ResetText();
                this._isNewEmployee = false;
            }
            else
            {
                this.checkBox2.Checked = true;
                this.panel3.Enabled = true;
                this.panel3.Visible = true;
                this.panel4.Enabled = false;
                this.panel4.Visible = false;
                this.textBox2.ResetText();
                this.textBox3.ResetText();
                this.comboBox1.ResetText();
                this._isNewEmployee = true;
            }  
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
                this.checkBox1.Checked = false;
            else
                this.checkBox1.Checked = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                _employee.GetEmployeeDataByIndex(comboBox1.SelectedIndex - 1);

                textBox2.Text = _employee.Id.ToString();
                textBox3.Text = _employee.JobTitle;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this._isNewEmployee)
            {
                try
                {
                    _employee.EditEmployee(_employee.NodeID, comboBox1.Text, Convert.ToInt32(textBox2.Text), textBox3.Text);

                    MessageBox.Show("Sikeres mentés!", "OK");
                }
                catch (Exception ex)
                {
                    var exceptions = _employee.Exceptions;

                    foreach(var exception in exceptions)
                    {
                        MessageBox.Show(this, exception, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
