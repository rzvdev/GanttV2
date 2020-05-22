using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class Split : Form
        {
        #region FormMovementService

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        public string Order { get; set; }
        public string Aim { get; set; }
        public string Department { get; set; }

        public Split()
            {
            InitializeComponent();
            }

        public Split(string order,string aim,string depart)
        {
            Order = order;
            Aim = aim;
            Department = depart;
            InitializeComponent();
        }


        private string _originalLine;
        private int _originalCapi;
        private int _originalDuration;
        private DateTime _orginalStart;
        private DateTime _orginalEnd;

        private void SplitInput_Load(object sender, EventArgs e)
            {
            lblError.Visible = true;
            pnSplitCommands.Enabled = false;

            lblClose.MouseEnter += delegate {
                lblClose.BackColor = Color.White;
                lblClose.ForeColor = Color.CadetBlue;
                };
            lblClose.MouseLeave += delegate {
                lblClose.BackColor = Color.CadetBlue;
                lblClose.ForeColor = Color.White;
                };

            pnHeader.MouseMove += (s, mv) =>
            {
                if (mv.Button == MouseButtons.Left)
                    {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    }
            };

            try
                {
                txtComm.Text = Order;

                var splitQuery = from split in Central.ListOfModels
                                 where split.Name == Order && 
                                 split.Department == Department && split.IsBase == false
                                 select split;

                var splittedCount = splitQuery.ToList().Count();

                if (splittedCount > 0) return;

                lblError.Visible = false;
                pnSplitCommands.Enabled = true;

                var orginal = Central.ListOfModels.SingleOrDefault(x => x.Name == Workflow.TargetOrder && x.Aim == Workflow.TargetLine);

                _originalLine = orginal.Aim;
                _originalCapi = orginal.LoadedQty;
                _originalDuration = orginal.Duration;
                _orginalStart = orginal.StartDate;
                _orginalEnd = orginal.EndDate;

                lblOrigLine.Text = Aim;
                lblOrigCapi.Text = _originalCapi.ToString();
                lblStart.Text = _orginalStart.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lblEnd.Text = _orginalEnd.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    + "\n (" + _originalDuration.ToString() + ")";

                LoadLines();

                var bc = lblSave.BackColor;

                lblSave.MouseEnter += delegate
                    {
                        lblSave.BackColor = Color.Gainsboro;
                        };
                lblSave.MouseLeave += delegate
                    {
                        lblSave.BackColor = bc;
                        };

                lblCapiTit.Text = "Capi " + "(" + _originalCapi.ToString() + ")";

                txtCommCapi.TextChanged += delegate
                    {
                        int.TryParse(txtCommCapi.Text, out var numberStyles);

                        var maxValue = _originalCapi; //number that will not be shown to the user 

                        if (numberStyles > maxValue)
                            {
                            txtCommCapi.Clear();
                            }
                        };
                }
            catch
                {
                lblError.Visible = true;
                lblError.Text = "Unexpected error has occurred.";
                pnSplitCommands.Enabled = false;
                }
            }

        private void LoadLines()
            {
            var linesQuery = from lines in Models.Tables.Lines
                             where lines.Department == Department
                             select lines;
            var lnLst = linesQuery.ToList();
            foreach (var line in lnLst)
                {
                cbCommLinea.Items.Add(line.Line);
                }
            }

        private readonly Geometry _geometry = new Geometry();

        protected override void OnPaint(PaintEventArgs e)
            {
            var pen = new Pen(new SolidBrush(Color.LightGray), 1);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, Width, Height), 8))
                {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Region = new Region(path);
                e.Graphics.SmoothingMode = old;
                }

            base.OnPaint(e);
            }

        private void lblCommTit_Paint(object sender, PaintEventArgs e)
            {
            var lbl = (Label)sender;

            var pen = new Pen(new SolidBrush(Color.LightGray), 1);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 5))
                {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                lbl.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
                }
            }

        private void lblClose_Click(object sender, EventArgs e)
            {
            Close();
            }

        private void lblClose_Paint(object sender, PaintEventArgs e)
            {
            var lbl = (Label)sender;

            var pen = new Pen(new SolidBrush(Color.LightGray), 1);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 5))
                {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                lbl.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
                }
            }

        private void lblSave_Click(object sender, EventArgs e)
            {
            if (string.IsNullOrEmpty(_originalCapi.ToString())
                || string.IsNullOrEmpty(_originalLine.ToString())
                || cbCommLinea.Text == string.Empty 
                || txtCommCapi.Text == string.Empty)
                {
                MessageBox.Show("Data are not valid.");
                return;
                }

            if (_originalLine == cbCommLinea.Text)
                {
                MessageBox.Show("Line cannot be the same as original line.");
                return;
                }

            var check = CheckProductionExist();
            if (check)
            {
                MessageBox.Show("Cannot split or move objects inside production fields.", 
                    "Workflow controller", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                return;
            }

            SaveData();
            Close();
            }

        private void SaveData()
            {
            int.TryParse(txtCommCapi.Text, out var newCapi);
            var splitQty = _originalCapi - newCapi;
            var j = new JobModel();

            var jobModel = (from jobs in Central.ListOfModels
                            where jobs.Name ==
                            Order &&
                            jobs.Aim == Aim &&
                            jobs.Department == Department
                            select jobs).SingleOrDefault();

            if (splitQty < 0)
                {
                MessageBox.Show("Value must be between 0 and maximum qty by 'commessa'");
                return;
                }
            
            try
                {

                using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    //startdate,duration,enddate
                    var newDur = j.CalculateJobDuration(Aim, splitQty, jobModel.QtyH, jobModel.Department);
                    var startdate = jobModel.StartDate;
                    var endDate = startdate.AddDays(+newDur);
                    var updateQuery = "update objects set loadedQty=@param1, StartDate=@paramStart, duration=@paramDur, endDate=@paramEnd where ordername=@param2 and aim=@param3 and department=@param4";
                    var cmd = new System.Data.SqlClient.SqlCommand(updateQuery, con);
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = splitQty;
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = Order;
                    cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = Aim;
                    cmd.Parameters.Add("@param4", SqlDbType.NVarChar).Value = Department;
                    cmd.Parameters.Add("@paramDur", SqlDbType.Int).Value = newDur;
                    cmd.Parameters.Add("@paramStart", SqlDbType.BigInt).Value = JobModel.GetLSpan(startdate);
                    cmd.Parameters.Add("@paramEnd", SqlDbType.BigInt).Value = JobModel.GetLSpan(endDate);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                var q = "insert into objects (ordername,aim,article,stateid,loadedqty,qtyh,startdate,duration,enddate,dvc,rdd,startprod,endprod,dailyprod,prodqty, " +
              "overqty,prodoverdays,delayts,prodoverts,locked,holiday,closedord,artprice,hasprod,lockedprod,delaystart,delayend,doneprod,base,department) values " +
              "(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10," +
              "@param11,@param12,@param13,@param14,@param15,@param16,@param17,@param18,@param19," +
              "@param20,@param21,@param22,@param23,@param24,@param25,@param26,@param27,@param28,@param29,@param30)";

                var jobDuration = j.CalculateJobDuration(cbCommLinea.Text, newCapi, jobModel.QtyH, Department);    //production duration

                var end = dtpCommData.Value;
                var eDate = end.AddDays(+jobDuration);

                using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                    cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = Order + ".1";
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = cbCommLinea.Text;
                    cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = jobModel.Article;
                    cmd.Parameters.Add("@param4", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@param5", SqlDbType.Int).Value = newCapi;
                    cmd.Parameters.Add("@param6", SqlDbType.Float).Value = jobModel.QtyH; ;
                    cmd.Parameters.Add("@param7", System.Data.SqlDbType.BigInt).Value = JobModel.GetLSpan(dtpCommData.Value);
                    cmd.Parameters.Add("@param8", System.Data.SqlDbType.Int).Value = jobDuration;
                    cmd.Parameters.Add("@param9", System.Data.SqlDbType.BigInt).Value = JobModel.GetLSpan(eDate);
                    cmd.Parameters.Add("@param10", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param11", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param12", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param13", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param14", System.Data.SqlDbType.Int).Value = jobModel.DailyProd;
                    cmd.Parameters.Add("@param15", System.Data.SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param16", System.Data.SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param17", System.Data.SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param18", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param19", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param20", System.Data.SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param21", System.Data.SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param22", System.Data.SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param23", System.Data.SqlDbType.Float).Value = jobModel.ArtPrice;
                    cmd.Parameters.Add("@param24", System.Data.SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param25", System.Data.SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param26", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param27", System.Data.SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param28", System.Data.SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param29", System.Data.SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param30", System.Data.SqlDbType.NVarChar).Value = Workflow.TargetDepartment;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                Config.InsertOperationLog("manual_splitting", Workflow.TargetOrder + "-" + cbCommLinea.Text + "-" + Workflow.TargetDepartment, "splitorder");

                var c = new Central();
                c.GetBase();
            }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            }

        private void pbHistory_Click(object sender, EventArgs e)
            {
            var frmSplitHistory = new SplitHistory(Order, Department);
            frmSplitHistory.IsFromSplit = true;
            frmSplitHistory.ShowDialog();
            frmSplitHistory.IsFromSplit = false;
            frmSplitHistory.Dispose();
            }

        private void txtCommCapi_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                if (string.IsNullOrEmpty(_originalCapi.ToString())
               || string.IsNullOrEmpty(_originalLine.ToString())
               || cbCommLinea.Text == string.Empty
               || txtCommCapi.Text == string.Empty)
                    {
                    MessageBox.Show("Data are not valid.");
                    return;
                    }

                if (_originalLine == cbCommLinea.Text)
                    {
                    MessageBox.Show("Line cannot be the same as original line.");
                    return;
                    }

                SaveData();
                Close();
                }
            }

        private bool CheckProductionExist()
        {
            var selDate = dtpCommData.Value.Date;

            var q = from prod in Models.Tables.Productions
                    where prod.Data >= selDate && prod.Line == cbCommLinea.Text && prod.Department == Department
                    select prod;

            if (q.ToList().Count > 0) return true;
            else return false;        
        }       
    }    
}
