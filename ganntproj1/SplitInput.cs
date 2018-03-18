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
    public partial class SplitInput : Form
        {
        #region FormMovementService

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        public SplitInput()
            {
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
                txtComm.Text = WorkflowController.TargetOrder;

                var splitQuery = from split in ObjectModels.Tables.ProductionSplits
                                 where split.Commessa == WorkflowController.TargetOrder
                                 select split;

                var splittedCount = splitQuery.ToList().Count();

                if (splittedCount > 0) return;
                
                lblError.Visible = false;
                pnSplitCommands.Enabled = true;

                var orginal = Central.ListOfModels.SingleOrDefault(x => x.Name == WorkflowController.TargetOrder && x.Aim == WorkflowController.TargetLine);

                _originalLine = orginal.Aim;
                _originalCapi = orginal.LoadedQty;
                _originalDuration = orginal.Duration;
                _orginalStart = orginal.StartDate;
                _orginalEnd = orginal.EndDate;

                lblOrigLine.Text = _originalLine;
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
            var linesQuery = from lines in ObjectModels.Tables.Lines
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

            SaveData();
            Close();
            }

        private void SaveData()
            {
            int.TryParse(txtCommCapi.Text, out var newCapi);
            var splitQty = _originalCapi - newCapi;

            if (splitQty < 0)
                {
                MessageBox.Show("Value must be between 0 and maximum qty by 'commessa'");
                return;
                }
            
            try
                {
                var split = new ObjectModels.ProductionSplit
                    {
                    Commessa = WorkflowController.TargetOrder,
                    Line = _originalLine,
                    Qty = splitQty,
                    Startdate = _orginalStart,
                    Enddate = _orginalEnd,
                    Base = true,
                    };

                ObjectModels.Tables.ProductionSplits.InsertOnSubmit(split);

                var splitNew = new ObjectModels.ProductionSplit
                    {
                    Commessa = WorkflowController.TargetOrder + " ",
                    Line = cbCommLinea.Text,
                    Qty = newCapi,
                    Startdate = dtpCommData.Value,
                    Enddate = dtpCommData.Value.AddDays(+_originalDuration),
                    Base = false
                    };

                ObjectModels.Tables.ProductionSplits.InsertOnSubmit(splitNew);

                Config.GetGanttConn().SubmitChanges();

                var m = new Central();
                m.GetBase(null);

                var modelsQuery = from models in Central.ListOfModels
                                  where models.Name == WorkflowController.TargetOrder && models.Aim == _originalLine
                                  select models;

                if (modelsQuery == null) return;
                
                var programEndDate = modelsQuery.Select(x => x.EndDate).SingleOrDefault();

                using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
                // update job aim
                    {
                    ctx.ExecuteCommand("update comenzi set DataFine={0} where NrComanda={1}", programEndDate.AddDays(-1), WorkflowController.TargetOrder);
                    }

                MessageBox.Show("Commessa splitted successfully.");
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            }

        private void pbHistory_Click(object sender, EventArgs e)
            {
            var frmSplitHistory = new SplitHistory();
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
        }
    }
