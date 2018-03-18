using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing.Drawing2D;
using ganntproj1.ObjectModels;
using System.Runtime.InteropServices;
using System.Data;

namespace ganntproj1
{
    public partial class CommInput : Form
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
        #endregion
        public CommInput()
        {
            InitializeComponent();
        }
        private void CommInput_Load_1(object sender, EventArgs e)
        {
            KeyPreview = true;
            _includeHours = false;
            LoadInputComponents();
            txtComm.Text = WorkflowController.TargetOrder;
            cbCommLinea.SelectedIndex = 1;
            cbCommLinea.SelectedIndex = cbCommLinea.FindString(WorkflowController.TargetLine);
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
            var models = Central.ListOfModels.Where(x => x.Name == WorkflowController.TargetOrder && x.Aim == WorkflowController.TargetLine 
            && x.Department == Store.Default.selDept).SingleOrDefault();
            lblMaxCapi.Text = "Capi (" + models.LoadedQty.ToString() + ")"
                + Environment.NewLine + "Opt. Min (" + models.DailyProd.ToString() + ")";
            txtCommCapi.Select();
            _dailyQty = models.DailyProd;
            _price = models.ArtPrice;

            lblTotalQty.Text = models.LoadedQty.ToString();

            var lineQuery = (from lin in ObjectModels.Tables.Lines
                             where lin.Line == WorkflowController.TargetLine && 
                             lin.Department == Store.Default.selDept
                             select new { lin.Abatimento }).SingleOrDefault();
            int.TryParse(lineQuery.Abatimento.ToString(), out _abatim);

            var qhQuery = (from m in Central.ListOfModels
                           where m.Name == WorkflowController.TargetOrder
                           && m.Aim == WorkflowController.TargetLine
                           && m.Department == Store.Default.selDept
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
            pbCloseOrder.MouseEnter += delegate
            {
                pbCloseOrder.BackColor = Color.Silver;
                lblCloseOrder.BackColor = Color.Silver;
            };
            pbCloseOrder.MouseLeave += delegate
            {
                pbCloseOrder.BackColor = Color.WhiteSmoke;
                lblCloseOrder.BackColor = Color.WhiteSmoke;
            };
            lblCloseOrder.MouseEnter += delegate
            {
                pbCloseOrder.BackColor = Color.Silver;
                lblCloseOrder.BackColor = Color.Silver;
            };
            lblCloseOrder.MouseLeave += delegate
            {
                pbCloseOrder.BackColor = Color.WhiteSmoke;
                lblCloseOrder.BackColor = Color.WhiteSmoke;
            };

            if (WorkflowController.TargetModelColor == Color.Gray || WorkflowController.TargetLocked && WorkflowController.TargetLockedProd)
            {
                pnHeader.BackColor = Color.Gray;
                lblCommTit.ForeColor = Color.Gray;
                lblSave.Enabled = false;
                txtCommCapi.ForeColor = Color.Gray;
                txtComm.ForeColor = Color.DimGray;
                lblProdStartInfo.Text = "Commessa has been closed by user";
                lblProdStartInfo.ForeColor = Color.Gray;
                pnQtyInfo.Enabled = false;
                tableView1.ReadOnly = true;
                _enterActive = false;

                if (WorkflowController.TargetClosedByUser)
                {
                    pbCloseOrder.Image = Properties.Resources.recovery_48;
                    lblCloseOrder.Text = "Recover commessa";
                }
            }
            lblStartDate.Text = WorkflowController
                .TargetModelStartDate.ToString("dd/MM/yyyy HH:mm");
        }
        private void ShowProductionDateRange()
        {
            var model = Central.ListOfModels.Where(x => x.Name == WorkflowController.TargetOrder && x.Aim == WorkflowController.TargetLine).SingleOrDefault();

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

            cbCommLinea.SelectedIndexChanged += (send, evargs) =>
            {
                txtPersone.Text = cbCommLinea.SelectedValue.ToString();
            };

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
                _controlButton.BackColor = Color.WhiteSmoke;
            }
        }
        private void label9_Click(object sender, EventArgs e)
        {
            Close();
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
                                 where prod.Commessa == WorkflowController.TargetOrder
                                 && prod.Line == WorkflowController.TargetLine
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
                            WorkflowController.TargetOrder,WorkflowController.TargetLine, Store.Default.selDept);
                    }
                }
                else
                {
                    var jMod = new JobModel();
                    jMod.GetJobContinum(WorkflowController.TargetOrder, WorkflowController.TargetLine);
                }
            
                var menu = new Central();
                menu.GetBase(null);
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

            _dtComm.Columns.Add("id");
            _dtComm.Columns.Add("data");
            _dtComm.Columns.Add("commessa");
            _dtComm.Columns.Add("capi");
            _dtComm.Columns.Add("line");
            _dtComm.Columns.Add("members");
            _dtComm.Columns.Add("department");
            _dtComm.Columns.Add("abovenorm");
            _dtComm.Columns.Add("time");
            _dtComm.Columns.Add("dQty");
            _dtComm.Columns.Add("price");   //10
            _dtComm.Columns.Add("includeHours");
            _dtComm.Columns.Add("abatim");
            _dtComm.Columns.Add("qtyH");

            var data = from prod in Tables.Productions
                       where prod.Commessa == WorkflowController.TargetOrder
                       && prod.Line == WorkflowController.TargetLine
                       && prod.Department == Store.Default.selDept
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

                _dtComm.Rows.Add(newRow);
            }
            tableView1.DataSource = _dtComm;

            tableView1.Columns[0].Visible = false;
            tableView1.Columns[2].Visible = false;
            tableView1.Columns[6].Visible = false;
            tableView1.Columns[7].Visible = false;

            tableView1.ReadOnly = false;
            tableView1.Columns[4].ReadOnly = true;
            tableView1.Columns[4].Visible = false;
            tableView1.Columns[5].ReadOnly = true;
            tableView1.Columns[1].DefaultCellStyle.BackColor = Color.LightYellow;
            tableView1.Columns[3].DefaultCellStyle.BackColor = Color.LightYellow;
            tableView1.Columns[8].DefaultCellStyle.BackColor = Color.LightYellow;
            //TODO: Hide those two columns at the end
            //tableView1.Columns[12].Visible = false;
            //tableView1.Columns[13].Visible = false;
            //var w = tableView1.Width / 4;
            //for (var i = 0; i <= tableView1.Columns.Count - 1; i++)
            //{
            //    tableView1.Columns[i].Width = w;
            //}
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

            GetTotalQty();
        }
        private void lblSingle_Click(object sender, EventArgs e)
        {
            if (Width >= 1100) return;

            SuspendLayout();
            Width = 1180;
            Refresh();
            CenterToScreen();
            ResumeLayout(true);
        }
        private void InsertNewRecord()
        {
            if (dtpCommData.Value.Date < WorkflowController.TargetModelStartDate.Date)
            {
                MessageBox.Show("Not possible to add production before planned date.",
                    "Workflow controller", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            var selDate = new DateTime(dtpCommData.Value.Year, dtpCommData.Value.Month, dtpCommData.Value.Day);
            var checkHoliday = Central.ListOfHolidays
                .Where(x => x.Holiday == selDate && x.Line == WorkflowController.TargetLine).SingleOrDefault();
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
                .Where(x => x.Name == WorkflowController.TargetOrder &&
                x.Aim == WorkflowController.TargetLine).SingleOrDefault();

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
            var trgtStart = WorkflowController.TargetModelStartDate;

            if (dtpCommData.Value.Date == trgtStart.Date)
                prodStartDt = new DateTime(trgtStart.Year, trgtStart.Month, trgtStart.Day, 
                    trgtStart.Hour, trgtStart.Minute, trgtStart.Second);
            else
                prodStartDt = new DateTime(dtpCommData.Value.Year, dtpCommData.Value.Month, dtpCommData.Value.Day,
                    7, 0, 0, 0);

            if (insertedQty > 0)
            {
                newRow[1] = prodStartDt;
                newRow[2] = WorkflowController.TargetOrder;
                newRow[3] = insertedQty;
                newRow[4] = WorkflowController.TargetLine;
                newRow[5] = txtPersone.Text;
                newRow[6] = Store.Default.selDept;
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
                        insertedQty /= 60;
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

                _dtComm.Rows.Add(newRow); 
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
            if (tableView1.Rows.Count <= 0) return;
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
                         where models.Name == WorkflowController.TargetOrder
                         && models.Aim == WorkflowController.TargetLine
                         select models).ToList();
            var qty = 0;
            var loadedQty = 0;
            var delay = new TimeSpan();
            foreach (var q in query)
            {
                qty = q.ProdQty;
                loadedQty = q.LoadedQty;
                delay = TimeSpan.FromTicks(q.DelayTime);
            }
            lblProdQty.Text = qty.ToString();
            lblDifQty.Text = (loadedQty - qty).ToString();
            var overQty = 0;
            overQty = qty - loadedQty;
            if (overQty < 0 || qty <= 0) overQty = 0;
            lblOverQty.Text = overQty.ToString();
           //delay label
            var dd = delay.Days;
            var hh = delay.Hours;
            string strTime;
            if (hh <= 7)            
                strTime = dd.ToString() + " dd " + hh.ToString() + "hh";
            else
            {
                var h = hh - 7;
                strTime = dd.ToString() + 
                    " dd " + h.ToString() + " hh";
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
            if (WorkflowController.TargetClosedByUser)
            {
                var dr = MessageBox.Show("Are you sure you want to recover this order?", "Workflow controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                    {
                        ctx.ExecuteCommand("update objects set closedord='false' where ordername={0} and aim={1}",
                            WorkflowController.TargetOrder, WorkflowController.TargetLine);

                        //ctx.ExecuteCommand("insert into orderclose (commessa,line) values ({0},{1})",
                        //    WorkflowController.TargetOrder, WorkflowController.TargetLine);
                    }
                    var c = new Central();
                    c.GetBase(null);

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
                            WorkflowController.TargetOrder, WorkflowController.TargetLine);

                        //ctx.ExecuteCommand("insert into orderclose (commessa,line) values ({0},{1})",
                        //    WorkflowController.TargetOrder, WorkflowController.TargetLine);
                    }
                    var c = new Central();
                    c.GetBase(null);

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
        protected override void OnPaint(PaintEventArgs e)
        {
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, Width, Height), 8))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            }

            base.OnPaint(e);
        }
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
    }
}