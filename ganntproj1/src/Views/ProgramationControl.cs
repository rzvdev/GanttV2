using ganntproj1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1.src.Views
{
    public partial class ProgramationControl : Form
    {
        public ProgramationControl()
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
        }

        public ProgramationControl (string order,string line,string depart,DateTime date)
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
            Order = order;
            Line = line;
            Department = depart;
            DateTimes = date;
        }

        private void ProgramationControl_Load(object sender, EventArgs e)
        {
            dateTimePicker1.ShowCheckBox = Store.Default.manualDate;
            dateTimePicker1.Enabled = Store.Default.manualDate;
            numericUpDown1.Enabled = Store.Default.manualMembers;
            
            SetDefaultValues();

            if (Store.Default.manualDate)
            {
                label1.Visible = true;
            }

            //check suggested date and enable datetime picker even if manualDate is disabled

            if (Store.Default.manualDate == false && DateTimes == DateTime.MinValue || DateTimes == Config.MinimalDate)
            {
                dateTimePicker1.Enabled = true;
            }
        }

        private void SetDefaultValues()
        {
            try
            {
                var defaultMembers = from lines in Tables.Lines
                                     where lines.Description == Line && lines.Department == Department
                                     select lines;
                var dm = defaultMembers.FirstOrDefault().Members;
                decimal.TryParse(dm.ToString(), out var members);
                numericUpDown1.Value = members;
                dateTimePicker1.Value = DateTimes;

                lblLine.Text = Line;
                lblOrder.Text = Order;
                checkBox1.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendValues()
        {
            DateTimes = dateTimePicker1.Value;

            var b = Store.Default.manualDate;
            if (b)
            {
                b = dateTimePicker1.Checked;
            }
            else
            {
                b = false;
            }
            
            UseManualDate = b;
            int.TryParse(numericUpDown1.Value.ToString(), out var memb);
            Members = memb;
            ByTotalQty = checkBox1.Checked;
        }

        public string Order { get; set; }
        public string Line { get; set; }
        public string Department { get; set; }
        public DateTime DateTimes { get; set; }
        public int Members { get; set; }
        public bool ByTotalQty { get; set; }
        public bool UseManualDate { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            SendValues();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
