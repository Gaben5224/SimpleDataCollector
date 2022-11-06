using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataCollector
{
    public partial class LoginFormForm : Form
    {
        public LoginFormForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = new User();

            try
            {
                user.LoginForm(this.textBox1.Text);
                this.Close();
            }
            catch (Exception exception)
            {

                var exceptionlist = user.Exceptions;

                foreach(var ex in exceptionlist)
                {
                    MessageBox.Show(this, ex, exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
