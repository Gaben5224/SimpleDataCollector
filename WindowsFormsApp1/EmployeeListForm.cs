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
        private int _numberOfEmployees;
        public EmployeeListForm()
        {
            InitializeComponent();
            SetStyleDataGridView1();
            this.FillEmployeeList();


        }

        private void FillEmployeeList()
        {
            try
            {
                var array = _employee.GetAllEmployeesData();
                var rowsCount = array.GetLength(0);
                var columnCount = array.GetLength(1);
                this._numberOfEmployees = rowsCount;

                for (int i = 0; i < rowsCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (j == 0)
                            this.dataGridView1.Rows.Add();

                        this.dataGridView1.Rows[i].Cells[j].Value = array.GetValue(i, j);
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptions = this._employee.Exceptions;
                foreach(string exception in exceptions)
                {
                    MessageBox.Show(this, exception, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void SetStyleDataGridView1()
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Calibri", 11.0f);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.RowTemplate.Height = 30;
            //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 12.0f);
            dataGridView1.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 5, 0, 5);
            dataGridView1.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dataGridView1.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;

            //DataGridViewButtonColumn style = (DataGridViewButtonColumn)dataGridView1.Columns["EditButton"];
            //style.FlatStyle = FlatStyle.Flat;
            //style.DefaultCellStyle.BackColor = Color.Green;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = e.RowIndex;

            if (_numberOfEmployees > index)
            {
                var nodeID = dataGridView1.Rows[index].Cells[0].Value;
                var id = dataGridView1.Rows[index].Cells[1].Value;
                var name = dataGridView1.Rows[index].Cells[2].Value;
                var jobTitle = dataGridView1.Rows[index].Cells[3].Value;

                this.textBox1.Text = name.ToString();
                this.textBox2.Text = id.ToString();
                this.textBox3.Text = jobTitle.ToString();

                this.button1.Text = "Szerkesztés";

                _employee.EditEmployee(Convert.ToInt32(nodeID), name.ToString(), Convert.ToInt32(id), jobTitle.ToString());

                this.dataGridView1.Refresh();
            }
            else
            {
                this.textBox1.ResetText();
                this.textBox2.ResetText();
                this.textBox3.ResetText();

                this.button1.Text = "Hozzáadás";
            }
            
        }
    }
}
