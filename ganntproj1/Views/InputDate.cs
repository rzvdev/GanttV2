using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
{
    public partial class InputDate : Form
    {
        public InputDate()
        {
            InitializeComponent();
        }
        private void InputDate_Load(object sender, EventArgs e)
        {

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            DateTime.TryParse(dateTimePicker1.Value.ToString(), out var dt);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
            LoadingJob.StartDateValue = dt;
            Close();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            LoadingJob.StartDateValue = DateTime.MinValue;
            Close();
        }
    }
}
