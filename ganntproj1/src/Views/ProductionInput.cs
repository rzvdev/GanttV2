using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing.Drawing2D;
using ganntproj1.Models;
using System.Runtime.InteropServices;
using System.Data;

namespace ganntproj1
{
    public partial class ProductionInput : Form
    {
        private readonly Config _config = new Config();
        private bool _deleting;
        private bool _includeHours;
        private static AutoCompleteStringCollection OrdersAcsc()
        {
            var asc = new AutoCompleteStringCollection();

            foreach (var view in Central.ListOfModels)
            {
                asc.Add(view.Name);
            }

            return asc;
        }
        private PictureBox _pbExc;
        private readonly Geometry _geometry = new Geometry();
        private Label _controlButton;
        private delegate void SelectionIndexDelegate();
        private int _dailyQty;
        private double _price;
        private int _abatim;
        private bool _enterActive = true;
        private double _qtyH;
        private DataTable _dtComm = new DataTable();
        #region FormMovementService
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        ReSize resize = new ReSize();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
        int nLeftRect,
        int nTopRect,
        int nRightRect,
        int nBottomRect,
        int nWidthEllipse,
        int nHeightEllipse

            );
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;

        public struct Margins
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            int x = (int)(m.LParam.ToInt64() & 0xFFFF);
            int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
            Point pt = PointToClient(new Point(x, y));

            if (m.Msg == 0x84)
            {
                switch (resize.getMosuePosition(pt, this))
                {
                    case "l": m.Result = (IntPtr)10; return;
                    case "r": m.Result = (IntPtr)11; return;
                    case "a": m.Result = (IntPtr)12; return;
                    case "la": m.Result = (IntPtr)13; return;
                    case "ra": m.Result = (IntPtr)14; return;
                    case "u": m.Result = (IntPtr)15; return;
                    case "lu": m.Result = (IntPtr)16; return;
                    case "ru": m.Result = (IntPtr)17; return;
                    case "": m.Result = pt.Y < 32 ? (IntPtr)2 : (IntPtr)1; return;

                }
            }

            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        Margins margins = new Margins()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;

        }

        #endregion FormMovementService

        public ProductionInput()
        {
            InitializeComponent();
        }

        private void LoadShift()
        {
            var q = "select shift from shifts where sectorid='" + Store.Default.sectorId + "'";
            using  (var c = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read()) cboShift.Items.Add(dr[0].ToString());
                }
                c.Close();
                dr.Close();
            }

            if (cboShift.Items.Count > 0 )
            {
                if (DateTime.Now.Hour > 14 && cboShift.Items.Count > 1)
                {
                    cboShift.SelectedIndex = 1;
                }
                else
                {
                    cboShift.SelectedIndex = 0;
                }
            }
        }

        private void CommInput_Load_1(object sender, EventArgs e)
        {
            LoadShift();
            KeyPreview = true;
            _includeHours = false;
            LoadInputComponents();
            txtComm.Text = Workflow.TargetOrder;
            cbCommLinea.SelectedIndex = 1;
            cbCommLinea.SelectedIndex = cbCommLinea.FindString(Workflow.TargetLine);
            txtCommCapi.Focus();
            LoadTable();
            ShowProductionDateRange();
            pnHeader.MouseMove += (s, mv) =>
            {
                if (mv.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
            var models = Central.ListOfModels.Where(x => x.Name == Workflow.TargetOrder && x.Aim == Workflow.TargetLine 
            && x.Department == Workflow.TargetDepartment).SingleOrDefault();

            if (models == null)
            {
                MessageBox.Show("Model fatal error. Check support for the following problem.");
                return;
            }

            var linex = (from ln in Models.Tables.Lines
                         where ln.Department == Workflow.TargetDepartment && ln.Line == Workflow.TargetLine
                         select ln).SingleOrDefault();

            if (linex == null)
            {
                MessageBox.Show("Line constructor is not valid. Check support for the following problem.");
                return;
            }
            lblAbatim.Text = linex.Abatimento.ToString() + "%";

            lblMaxCapi.Text = "Capi (" + models.LoadedQty.ToString() + ")"
                + Environment.NewLine + "Opt. Min (" + models.DailyProd.ToString() + ")";
            txtCommCapi.Select();
            _dailyQty = models.DailyProd;
            _price = models.ArtPrice;


            lblTotalQty.Text = models.LoadedQty.ToString();

            var lineQuery = (from lin in Models.Tables.Lines
                             where lin.Line == Workflow.TargetLine && 
                             lin.Department == Workflow.TargetDepartment
                             select new { lin.Abatimento }).SingleOrDefault();
            int.TryParse(lineQuery.Abatimento.ToString(), out _abatim);

            var qhQuery = (from m in Central.ListOfModels
                           where m.Name == Workflow.TargetOrder
                           && m.Aim == Workflow.TargetLine
                           && m.Department == Workflow.TargetDepartment
                           select new { m.QtyH }).SingleOrDefault();
            double.TryParse(qhQuery.QtyH.ToString(), out _qtyH);

            txtCommCapi.TextChanged += delegate
            {
                int.TryParse(txtCommCapi.Text, out var numberStyles);
                var maxValue = models.LoadedQty; //number that will not be shown to the user 
                if (numberStyles > maxValue)
                {
                    txtCommCapi.Clear();
                }
            };

            if (Workflow.TargetModelColor == Color.Gray || Workflow.TargetLocked && Workflow.TargetLockedProd)
            {
                pnHeader.BackColor = Color.Gray;
                lblSave.Enabled = false;
                txtCommCapi.ForeColor = Color.Gray;
                txtComm.ForeColor = Color.DimGray;
                lblProdStartInfo.Text = "Commessa has been closed by user";
                lblProdStartInfo.ForeColor = Color.Gray;
                pnQtyInfo.Enabled = false;
                tableView1.ReadOnly = true;
                _enterActive = false;

                if (Workflow.TargetClosedByUser)
                {
                    lblCloseOrder.Image = Properties.Resources.edit_big;
                    lblCloseOrder.Text = "Recover commessa";
                }
            }
            lblStartDate.Text = Workflow
                .TargetModelStartDate.ToString("dd/MM/yyyy HH:mm");
        }
        private void ShowProductionDateRange()
        {
            var model = Central.ListOfModels.Where(x => x.Name == Workflow.TargetOrder && x.Aim == Workflow.TargetLine 
            && x.Department == Workflow.TargetDepartment).SingleOrDefault();

            if (model == null) return;

            if (!(model.ProductionStartDate.Date.Year >= 2017))
            {
                lblProdStartInfo.Text = "No available production";
                lblProdStartInfo.ForeColor = Color.Red;
            }
            else
            {
                lblProdStartInfo.Text = "Da: " + model.ProductionStartDate.ToString("dd/MM/yyyy")
                + "   |   "
                + "A: " + model.ProductionEndDate.ToString("dd/MM/yyyy HH:mm");
                lblProdStartInfo.ForeColor = Color.Black;
            }
        }
        private void LoadInputComponents()
        {
            _config.Set_sql_conn(new SqlConnection(_config.ReadSqlConnectionString(1)));

            var lineDictionary = new System.Collections.Generic.Dictionary<string, string>();

            var linesQuery = from line in Tables.Lines
                             where line.Department == Workflow.TargetDepartment
                             orderby line.Line.Remove(0, 5)
                             select line;

            foreach (var line in linesQuery)
            {
                if (!lineDictionary.ContainsKey(line.Line))
                    lineDictionary.Add(line.Line, line.Members.ToString());
            }

            cbCommLinea.DataSource = new BindingSource(lineDictionary, null);
            cbCommLinea.DisplayMember = "key";
            cbCommLinea.ValueMember = "value";

            //cbCommLinea.SelectedIndexChanged += (send, evargs) =>
            //{
            //    txtPersone.Text = cbCommLinea.SelectedValue.ToString();
            //};

            var membersQuery = (from models in Central.ListOfModels
                               where models.Name == Workflow.TargetOrder
                               && models.Aim == Workflow.TargetLine
                               && models.Department == Workflow.TargetDepartment
                               select models).FirstOrDefault();

            txtPersone.Text = membersQuery.Members.ToString();

            if (txtComm != null)
            {
                txtComm.AutoCompleteCustomSource.Clear();
                txtComm.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtComm.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtComm.AutoCompleteCustomSource = OrdersAcsc();
            }

            if (cbCommLinea.Items.Count > 1)
            {
                cbCommLinea.Invoke(new SelectionIndexDelegate(SelectFirstIndex));
            }
        }
        private void SelectFirstIndex()
        {
            cbCommLinea.SelectedIndex = 1;
            cbCommLinea.SelectedIndex = 0;
        }
        private void txtCommCapi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        } 
        private void pbCheck_MouseDown(object sender, MouseEventArgs e)
        {
            _pbExc = sender as PictureBox;
            if (_pbExc != null) _pbExc.Location = new Point(_pbExc.Location.X + 2, _pbExc.Location.Y + 2);
        }
        private void pbCheck_MouseUp(object sender, MouseEventArgs e)
        {
            _pbExc.Location = new Point(_pbExc.Location.X - 2, _pbExc.Location.Y - 2);
        }
        private void lblDetails_Click(object sender, EventArgs e)
        {

        }
        private void label8_MouseEnter(object sender, EventArgs e)
        {
            _controlButton = (Label)sender;
            _controlButton.BackColor = Color.Gainsboro;
        }
        private void label8_MouseLeave(object sender, EventArgs e)
        {
            if (_controlButton != null)
            {
                _controlButton.BackColor = Color.FromArgb(242, 242, 242);
            }
        }
        private void SaveData()
        {
            var selectedDate = dtpCommData.Value;
            tableView1.Invalidate();
            tableView1.EndEdit();
            //
            // Delete data by order
            //
            var groupForDelete = from prod in Tables.Productions
                                 where prod.Commessa == Workflow.TargetOrder
                                 && prod.Line == Workflow.TargetLine
                                && prod.Department == Workflow.TargetDepartment
                                 select prod;
            var items = groupForDelete.ToList();
            foreach (var item in items)
            {
                Tables.Productions.DeleteOnSubmit(item);
            }
            Config.GetGanttConn().SubmitChanges();
            // Check if it's a new input
            if (!string.IsNullOrEmpty(txtCommCapi.Text) && !string.IsNullOrEmpty(txtPersone.Text))
            {
                bool wantToSave = true;
                if (!_deleting && selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    var diag = MessageBox.Show("Are you sure you want to add production for " + 
                        selectedDate.DayOfWeek.ToString() + "?", "Workflow controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    wantToSave = diag == DialogResult.Yes ? true : false;
                }
                if (!wantToSave) return;
                InsertNewRecord();
                txtCommCapi.Text = "";
                txtCommCapi.Focus();
            }

            lblSavedInfo.Visible = true;
            lblSavedInfo.Location = new Point(Width / 2 - lblSavedInfo.Width / 2, lblSavedInfo.Location.Y);
            lblSavedInfo.BringToFront();
            lblSavedInfo.Refresh();

            foreach (DataRow dr in _dtComm.Rows)
            {
                int.TryParse(dr[9].ToString(), out var dQ);
                double.TryParse(dr[10].ToString(), out var price);
                bool.TryParse(dr[11].ToString(), out var incHours);
                int.TryParse(dr[12].ToString(), out var abatim);
                double.TryParse(dr[13].ToString(), out var qtyH);
                var shift = dr[14].ToString();

                var pd = new Production
                {
                    Data = Convert.ToDateTime(dr[1].ToString()),
                    Commessa = dr[2].ToString(),
                    Capi = Convert.ToInt32(dr[3]),
                    Line = dr[4].ToString(),
                    Members = Convert.ToInt32(dr[5]),
                    Department = dr[6].ToString(),
                    Abovenormal = Convert.ToBoolean(dr[7]),
                    Times = Convert.ToDateTime(dr[8].ToString()),
                    Dailyqty = dQ,
                    Price = price,
                    IncludeHours = incHours,
                    Abatim = abatim,
                    QtyH = qtyH,
                    Shift = shift,
                };
                Tables.Productions.InsertOnSubmit(pd);
            }
            try
            {                
                Config.GetGanttConn().SubmitChanges();
                if (tableView1.Rows.Count <= 0)
                {
                    using (var context = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                    {
                        // delete existing records
                        context.ExecuteCommand("update objects set " +
                            "startprod={0},endprod={1},prodqty={2},delayts={3}, delaystart={4},delayend={5} " +
                            "where ordername={6} and aim={7} and department={8}",
                            0L,
                            0L,
                            0L,0L, 0L, 0L,
                            Workflow.TargetOrder,Workflow.TargetLine, Workflow.TargetDepartment);
                    }
                }
                else
                {
                    var jMod = new JobModel();
                    jMod.GetJobContinum(Workflow.TargetOrder, Workflow.TargetLine, Workflow.TargetDepartment);
                }
            
                var menu = new Central();
                menu.GetBase();
            }
            catch (Exception ex)
            {
                lblSavedInfo.Visible = false;
                lblSavedInfo.Refresh();
                MessageBox.Show(ex.Message);
            }
            if (Width < 700)
            {
                Close();
            }
            else
            {
                ShowProductionDateRange();
                System.Threading.Thread.Sleep(200);
                lblSavedInfo.Visible = false;
                lblSavedInfo.Refresh();
            }
        }
        private void lblSave_Click(object sender, EventArgs e)
        {
            _deleting = false;
            SaveData();
            GetTotalQty();
            Config.InsertOperationLog("manual_sp", "on " + txtComm.Text + " - " + cbCommLinea.Text, "produzione");
        }
        private void txtCommCapi_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_enterActive) return;
            if (e.KeyCode == Keys.Enter)
            {
                SaveData();
                GetTotalQty();
            }
        }

        #region WorkWithTable    
        private void LoadTable()
        {
            _dtComm = new DataTable();

            _dtComm.Columns.Add("Id");
            _dtComm.Columns.Add("Data");
            _dtComm.Columns.Add("Commessa");
            _dtComm.Columns.Add("Capi");
            _dtComm.Columns.Add("Line");
            _dtComm.Columns.Add("Members");
            _dtComm.Columns.Add("Department");
            _dtComm.Columns.Add("Abovenorm");
            _dtComm.Columns.Add("Time");
            _dtComm.Columns.Add("DailyQty");
            _dtComm.Columns.Add("Price");   //10
            _dtComm.Columns.Add("IncludeHours");
            _dtComm.Columns.Add("Abatim");
            _dtComm.Columns.Add("QtyH");
            _dtComm.Columns.Add("Shift");

            var data = from prod in Tables.Productions
                       where prod.Commessa == Workflow.TargetOrder
                       && prod.Line == Workflow.TargetLine
                       && prod.Department == Workflow.TargetDepartment
                       select prod;

            var dataList = data.ToList();

            foreach (var p in dataList)
            {
                var newRow = _dtComm.NewRow();

                newRow[0] = p.Id;
                newRow[1] = p.Data;//.ToString("yyyy-MM-dd");
                newRow[2] = p.Commessa;
                newRow[3] = p.Capi;
                newRow[4] = p.Line;
                newRow[5] = p.Members;
                newRow[6] = p.Department;
                newRow[7] = p.Abovenormal;
                newRow[8] = p.Times;
                newRow[9] = p.Dailyqty;
                newRow[10] = p.Price;
                newRow[11] = p.IncludeHours;
                newRow[12] = p.Abatim;
                newRow[13] = p.QtyH;
                newRow[14] = p.Shift;

                _dtComm.Rows.Add(newRow);
            }

            tableView1.DataSource = _dtComm;

            tableView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 52, 68);
            tableView1.Columns[0].Visible = false;
            tableView1.Columns[2].Visible = false;
            tableView1.Columns[6].Visible = false;
            tableView1.Columns[7].Visible = false;

            tableView1.ReadOnly = false;
            tableView1.Columns[4].ReadOnly = true;
            tableView1.Columns[4].Visible = false;
            tableView1.Columns[1].DefaultCellStyle.BackColor = Color.LightYellow;
            tableView1.Columns[3].DefaultCellStyle.BackColor = Color.LightYellow;
            tableView1.Columns[8].DefaultCellStyle.BackColor = Color.LightYellow;

            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            GetTotalQty();
        }
        private void DeleteFromTable()
        {
            if (tableView1.CurrentCell == null) return;
            var curIdx = tableView1.CurrentCell.RowIndex;

            _dtComm.AcceptChanges();
            _dtComm.Rows.RemoveAt(curIdx);
            _dtComm.AcceptChanges();

            if (tableView1.DataSource != null) tableView1.DataSource = null;

            tableView1.DataSource = _dtComm;

            tableView1.Columns[0].Visible = false;
            tableView1.Columns[2].Visible = false;
            tableView1.Columns[tableView1.Columns.Count - 1].Visible = false;

            tableView1.ReadOnly = false;
            tableView1.Columns[4].ReadOnly = true;
            tableView1.Columns[5].ReadOnly = true;

            var w = tableView1.Width / 4;
            for (var i = 0; i <= tableView1.Columns.Count - 1; i++)
            {
                tableView1.Columns[i].Width = w;
            }

            Config.InsertOperationLog("manual_dp", txtComm.Text + "-" + cbCommLinea.Text, "produzione");

            GetTotalQty();
        }
        private void lblSingle_Click(object sender, EventArgs e)
        {
            if (Width >= 1100) return;

            Width = 1180;
            CenterToScreen();
        }
        private void InsertNewRecord()
        {
            if (dtpCommData.Value.Date < Workflow.TargetModelStartDate.Date)
            {
                MessageBox.Show("Not possible to add production before planned date.",
                    "Workflow controller", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            var selDate = new DateTime(dtpCommData.Value.Year, dtpCommData.Value.Month, dtpCommData.Value.Day);
            var checkHoliday = Central.ListOfHolidays
                .Where(x => x.Holiday == selDate && x.Line == Workflow.TargetLine).SingleOrDefault();
            if (checkHoliday != null)
            {
                MessageBox.Show("Not possible to add production during holiday.",
                   "Workflow controller", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return;
            }
            var newRow = _dtComm.NewRow();
            /*check for qty greater loadedQty*/
            var model = Central.ListOfModels
                .Where(x => x.Name == Workflow.TargetOrder &&
                x.Aim == Workflow.TargetLine && x.Department == Workflow.TargetDepartment).SingleOrDefault();

            var bound = model.LoadedQty;
            var prodQty = model.ProdQty;
            int.TryParse(txtCommCapi.Text, out var insertedQty);
            var testOverQty = prodQty + insertedQty;
            var overQty = 0;
            if (testOverQty > bound) overQty = testOverQty - bound;
            if (overQty > 0)
            {
                var msg = MessageBox.Show("Are you sure you want to add more qty than total commesa?", 
                    "Produzione input", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                if (msg == DialogResult.No) insertedQty -= overQty;
            }
            var prodStartDt = new DateTime();
            var trgtStart = Workflow.TargetModelStartDate;

            if (dtpCommData.Value.Date == trgtStart.Date)
                prodStartDt = new DateTime(trgtStart.Year, trgtStart.Month, trgtStart.Day, 
                    trgtStart.Hour, trgtStart.Minute, trgtStart.Second);
            else
                prodStartDt = new DateTime(dtpCommData.Value.Year, dtpCommData.Value.Month, dtpCommData.Value.Day,
                    7, 0, 0, 0);

            if (insertedQty > 0)
            {
                newRow[1] = prodStartDt;
                newRow[2] = Workflow.TargetOrder;
                newRow[3] = insertedQty;
                newRow[4] = Workflow.TargetLine;
                newRow[5] = txtPersone.Text;
                newRow[6] = Workflow.TargetDepartment;
                newRow[7] = false;
                /*calculate production in hours */
                var hh = 0; // default starthift
                int min = 0;
                int sec = 0;
                if (_includeHours)
                {
                    if (insertedQty > 0) 
                    //convert difference into hours
                    {
                        double.TryParse(insertedQty.ToString(), out var q);
                        double.TryParse(txtPersone.Text, out var members);
                        insertedQty = Convert.ToInt32(q / _qtyH / members);
                        var startH = prodStartDt.Hour;
                        if (startH > 7 && startH < 15) hh = startH + Convert.ToInt32(insertedQty);
                        else hh = startH + (7 - startH) + Convert.ToInt32(insertedQty);
                        min = 0;
                        sec = 0;
                        if (hh > 15)
                        {
                            MessageBox.Show("Production per hour is in out of bounds. " +                                            
                                "The system will switch to daily production.",                                 
                                "Production input",                                 
                                MessageBoxButtons.OK,   
                                MessageBoxIcon.Information);
                            rbDay.Checked = true;
                            _includeHours = false;
                            hh = 23;
                            min = 59;
                            sec = 59;
                        }
                    }
                }
                else
                    hh = 23; min = 59; sec = 59;

                newRow[8] = new DateTime(dtpCommData.Value.Year, dtpCommData.Value.Month, dtpCommData.Value.Day,
                    hh, min, sec); 
                //add additional column values
                newRow[9] = _dailyQty;
                newRow[10] = _price;
                newRow[11] = _includeHours;
                newRow[12] = _abatim;
                newRow[13] = _qtyH;
                newRow[14] = cboShift.Text;

                _dtComm.Rows.Add(newRow);

                Config.InsertOperationLog("manual_ip", txtComm.Text + "-" + cbCommLinea.Text, "produzione");
            }
        }
        #endregion

        private void dtpCommData_CloseUp(object sender, EventArgs e)
        {
            if (dtpCommData.Value.Date > DateTime.Now.Date)
            {
                dtpCommData.Value = DateTime.Now;
            }
        } 
        private void btnDelete_Click(object sender, EventArgs e)
        {
            tableView1.Focus();
            if (tableView1.Rows.Count <= 0)             
            {
                MessageBox.Show("Nothing to delete.", "Production", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var diag = MessageBox.Show("Are you sure you want to delete this record?", "Workflow controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (diag == DialogResult.No) return;

            _deleting = true;
            DeleteFromTable();
            SaveData();
            GetTotalQty();
        }
        private void GetTotalQty()
        {
            var query = (from models in Central.ListOfModels
                         where models.Name == Workflow.TargetOrder
                         && models.Aim == Workflow.TargetLine
                         && models.Department == Workflow.TargetDepartment
                         select models).ToList();
            var qty = 0;
            var loadedQty = 0;
            var delay = new TimeSpan();
            var startDelay = new DateTime();
            var endDelay = new DateTime();
            var aim = "";
            foreach (var q in query)
            {
                qty = q.ProdQty;
                loadedQty = q.LoadedQty;
                delay = TimeSpan.FromTicks(q.DelayTime);
                startDelay = q.DelayStartDate;
                endDelay = q.DelayEndDate;
                aim = q.Aim;
            }
            lblProdQty.Text = qty.ToString();
            lblDifQty.Text = (loadedQty - qty).ToString();
            var overQty = 0;
            overQty = qty - loadedQty;
            if (overQty < 0 || qty <= 0) overQty = 0;
            lblOverQty.Text = overQty.ToString();

            var delDiff = JobModel.SkipDateRange(startDelay, endDelay, aim);
            var delDateDiff = endDelay.Subtract(startDelay);
         
            string strTime;
            var dd = delDateDiff.Days - delDiff;
            var hh = delDateDiff.Hours;
            if (hh <= 8)
                strTime = dd.ToString() + " dd " + hh.ToString() + "hh ";
            else
            {
                strTime = (dd + 1).ToString() +
                    " dd " + hh.ToString() + " hh ";
            }

            lblDelay.Text = strTime;
        }
        private void lblProdStartInfo_Click(object sender, EventArgs e)
        {
            Refresh();
            return;
        }
        private void PbCloseOrder_Click(object sender, EventArgs e)
        {
            if (Workflow.TargetClosedByUser)
            {
                var dr = MessageBox.Show("Are you sure you want to recover this order?", "Workflow controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                    {
                        ctx.ExecuteCommand("update objects set closedord='false' where ordername={0} and aim={1}",
                            Workflow.TargetOrder, Workflow.TargetLine);

                    }
                    var c = new Central();
                    c.GetBase();

                    Close();
                }
            }
            else
            {
                var dr = MessageBox.Show("Are you sure you want to close this order?", "Workflow controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                    {
                        ctx.ExecuteCommand("update objects set closedord='true' where ordername={0} and aim={1}",
                            Workflow.TargetOrder, Workflow.TargetLine);
                    }
                    var c = new Central();
                    c.GetBase();

                    Close();
                }
            }
        }
        private void RbDay_CheckedChanged(object sender, EventArgs e)
        {
            _includeHours = false;
        }
        private void RbHour_CheckedChanged(object sender, EventArgs e)
        {
            _includeHours = true;
        }
        private void CommInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        #region Paintings

        private void lblClose_Paint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 5))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                lbl.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }
        private void lblCommTit_Paint(object sender, PaintEventArgs e)
        {

        }
        private void lblSave_Paint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 7))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // e.Graphics.DrawPath(pen, path);

                lbl.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            var pn = (Panel)sender;

            var pen = new Pen(new SolidBrush(Color.SeaGreen), 3);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, pn.Width, pn.Height), 9))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                e.Graphics.DrawPath(pen, path);

                pn.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }
        #endregion

        private void Label1_Click(object sender, EventArgs e)
        {
            tableView1.ExportToExcel("Comview");
        }

        private void PnHeader_Paint(object sender, PaintEventArgs e)
        {
            var pn = (Panel)sender;
            var fnt = new Font("Segoe UI", 12);
            var logoRect = new Rectangle(10, 5, 102, 40);
            e.Graphics.DrawImage(Properties.Resources.Logo, logoRect);

            var refreshTitle = "Produzione";
            var refreshTitleSize = e.Graphics.MeasureString(refreshTitle, fnt);

            var posX = pn.Width / 2 - refreshTitleSize.Width / 2;
            var posY = pn.Height / 2 - refreshTitleSize.Height / 2;
            e.Graphics.DrawString(refreshTitle, fnt, Brushes.WhiteSmoke, posX, posY);
        }

        private void ProductionInput_SizeChanged(object sender, EventArgs e)
        {
            pnHeader.Invalidate();
        }

        private void LblAbatim_DoubleClick(object sender, EventArgs e)
        {
            var lbl = (Label)sender;
            var txt = "Abatimento: " + lbl.Text;
            lbl.ForeColor = Color.Crimson;
            lbl.Refresh();
            MessageBox.Show(txt, "Produzione", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            lbl.ForeColor = Color.ForestGreen;
            lbl.Refresh();
        }
    }
}