using System;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class FrmSchedule : Form
        {
        public FrmSchedule()
            {
            InitializeComponent();
            }

        private void tbZoom_Scroll(object sender, EventArgs e)
            {
            var tbVal = Convert.ToDouble(tbZoom.Value);
            var normal = 31.0;
            var result = 0.0;

            if (tbVal == normal)
                {
                lblZoomPerc.Text = "1%";
                }
            else
                {

                if (tbVal > normal)
                    {
                    result = Math.Round((tbVal / normal), 2);
                    lblZoomPerc.Text = "+ " + result.ToString() + "%";
                    }
                else
                    {
                    result = Math.Round((normal / tbVal), 2);
                    lblZoomPerc.Text = "- " + result.ToString() + "%";
                    }
                }
            }

        public DateTime DateFocus { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public int ZoomVal { get; set; }

        public bool HasError { get; set; }
        private void btnOk_Click(object sender, EventArgs e)
            {
            DateFocus = dtpDate.Value;
            FromTime = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day,
                dtpTimeFrom.Value.Hour, dtpTimeFrom.Value.Minute, 0);
            ToTime = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day,
                dtpTimeTo.Value.Hour, dtpTimeTo.Value.Minute, 0);
            ZoomVal = tbZoom.Value;

            if (ToTime.Subtract(FromTime).TotalHours < 22)
                {
                MessageBox.Show("(Demo procedure required 22 hours (min)) - Invalid range selection.", "Timestamp bonding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                HasError = true;
                Close();
                }
            else
                {
                HasError = false;
                Close();
                }
            }

        private void btnCancel_Click(object sender, EventArgs e)
            {
            HasError = true;
            Close();
            }
        }
    }
