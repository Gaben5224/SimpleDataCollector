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
    public partial class teszt : Form
    {
        public teszt()
        {
            InitializeComponent();

            var employee = new Employee();
            employee.AddEmployee(203, "FPV nagymester", "Czerné Gecse Anita");
        }
    }
}
