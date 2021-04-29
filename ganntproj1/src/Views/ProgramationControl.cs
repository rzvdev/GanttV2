using ganntproj1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

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

        public ProgramationControl (string order,string line,string depart,DateTime date, string article, int totalQty = 0, int carico = 0, double qtyH = 0.0)
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
            Order = order;
            Line = line;
            Department = depart;
            DateTimes = date;
            Article = article;
            TotalQty = totalQty;
            Carico = carico;

            if (Store.Default.sectorId == 8)
            {
                QtyHSartoria = qtyH;
            }

            lblQtyHSart.Text = QtyHSartoria.ToString();
        }

        private void ProgramationControl_Load(object sender, EventArgs e)
        {
            dateTimePicker1.ShowCheckBox = Store.Default.manualDate;
            dateTimePicker1.Enabled = Store.Default.manualDate;
            numericUpDown1.Enabled = Store.Default.manualMembers;

            if (Store.Default.sectorId != 2)
            {
                groupBox4.Enabled = false;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

            cbLaunched.Visible = Store.Default.sectorId == 7;
           
            SetDefaultValues();

            if (Store.Default.manualDate)
            {
                lblDateInfo.Visible = true;
            }

            //check suggested date and enable datetime picker even if manualDate is disabled

            if (Store.Default.manualDate == false && DateTimes == DateTime.MinValue || DateTimes == Config.MinimalDate)
            {
                dateTimePicker1.Enabled = true;
                lblDateInfo.Text = "First order in line " + Line;
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

                Text = Order + " - " + Line;

                if (Text == string.Empty)
                {
                    Text = "Programmation options";
                }

                lblCommessaBox.Text = Order;
                lblArtBox.Text = Article;
                lblQtyBox.Text = TotalQty.ToString();
                lblCaricoSugg.Text = "Carico: " + Carico.ToString();

                if (Carico == 0)
                {
                    checkBox1.Enabled = false;
                    checkBox1.Checked = true;
                    ByTotalQty = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendValues()
        {
            int.TryParse(numericUpDown1.Value.ToString(), out var memb);
            Members = memb;

            if (memb == 0)
            {
                MessageBox.Show("Members/machines equals to 0 is not a good value.\nPlease check Settings>Line", "Programming options", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SettingsHour() == 0)
            {
                MessageBox.Show("Working hours equals to 0 are not a good value.\nPlease check Settings>Sectors (Weekdays hour input)", "Programming options", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Launched = Store.Default.sectorId != 7 ? false : cbLaunched.Checked;

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
            ByTotalQty = checkBox1.Checked;
            if (Store.Default.sectorId == 2) {
                double.TryParse(lblQtyH.Text, out var qth);
                QtyH = qth;
            }
        }

        public string Order { get; set; }
        public string Line { get; set; }
        public string Department { get; set; }
        public DateTime DateTimes { get; set; }
        public int Members { get; set; }
        public bool ByTotalQty { get; set; }
        public bool UseManualDate { get; set; }
        public string Operation { get; set; }
        public int OperationId { get; set; }
        public double QtyH { get; set; }
        public double QtyHSartoria { get; set; }
        public string Article { get; set; }

        public int TotalQty { get; set; }
        public int Carico { get; set; }

        public bool Launched { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Enabled==true && dateTimePicker1.Checked==true && dateTimePicker1.Value < DateTimes)
            {
                var diag = MessageBox.Show("Are you sure you want to program over existing order?","Workflow", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (diag == DialogResult.No)
                {
                    return;
                }
            }

            SendValues();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var artId = 0;
            Operation = comboBox1.Text;

            if (comboBox1.SelectedIndex == 0)
            {
                OperationId = 313;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                OperationId = 314;
            }
            var artQuery = "select id from Articole where articol='" + Article + "' and idsector=2";
            using (var c = new SqlConnection(Central.ConnStr))
            {
                var cmd = new SqlCommand(artQuery, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int.TryParse(dr[0].ToString(), out artId);
                    }
                }
                c.Close();
            }

            var operatQuery = "select BucatiOra from OperatiiArticol where idOperatie='" + OperationId.ToString() + "' and IdSector=2" +
                " and idarticol='" + artId + "'";

            using (var c = new SqlConnection(Central.ConnStr))
            {
                var cmd = new SqlCommand(operatQuery, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        double.TryParse(dr[0].ToString(), out var qtyh);
                        QtyH = qtyh;
                    }
                }
                c.Close();
            }

            if (QtyH == 0.0)
            {
                MessageBox.Show("QtyH is not valid for " + Operation);
                lblQtyH.Text = "QtyH: 0.0";
            }
            else
            {
                lblQtyH.Text = QtyH.ToString();
            }
        }

        private double SettingsHour()
        {
            var hour = 0;
            switch (Store.Default.sectorId)
            {
                case 1:
                    hour = Convert.ToInt32(Store.Default.confHour);
                    break;
                case 2:
                    hour = Convert.ToInt32(Store.Default.stiroHour);
                    break;
                case 7:
                    hour = Convert.ToInt32(Store.Default.tessHour);
                    break;
                case 8:
                    hour = Convert.ToInt32(Store.Default.sartHour);
                    break;
            }

            return hour;
        }
    }
}
