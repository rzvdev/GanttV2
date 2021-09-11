using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class Holidays : Form
        {
        public Holidays()
            {
            InitializeComponent();
            dgvCheck.DoubleBuffered(true);
            dgvCheck.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCheck.DataBindingComplete += dgvCheck_Dbc;
            }

        private void dgvCheck_Dbc(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvCheck.RowTemplate.Height = 30;
        }

        private int Month { get; set; }
        private int Year { get; set; }
        private DataTable _dataTable = new DataTable();
        private List<LineHolidays> _lstLineHld = new List<LineHolidays>();
        
        protected override void OnLoad(EventArgs e)
            {
            for (var i = DateTime.Now.Year - 2; i <= DateTime.Now.Year + 1; i++)
                {
                cboYears.Items.Add(i);
                }

            cboMonth.SelectedIndexChanged += (s, ev) =>
                {
                    if (_firstRead) return;

                    Month = cboMonth.SelectedIndex + 1;
                    LoadData();
                };

            cboYears.SelectedIndexChanged += (s, ev) =>
                {
                    if (_firstRead) return;

                    Year = Convert.ToInt32(cboYears.Text);
                    LoadData();
                };

            foreach (var str in Store.Default.arrDept.Split(','))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    cboDepartment.Items.Add(str);
                }
            }

            if (cboDepartment.Items.Count > 0)
            {
                cboDepartment.SelectedIndex = 0;
            }

            cboYears.SelectedIndex = cboYears.FindString(DateTime.Now.Year.ToString());
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;

            LoadData();

            FormClosing += delegate
                {
                    lblSavedInfo.Visible = true;
                    lblSavedInfo.Text = "Reloading...";

                    var m = new Central();
                    m.GetBase();
                    };

            base.OnLoad(e);
            }

        private bool _firstRead = true;

        private void LoadData()
            {
            if (_firstRead)
                {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
                }

            _dataTable = new DataTable();

            if (dgvCheck.DataSource != null) dgvCheck.DataSource = null;

            _dataTable.Columns.Add("Linea");

            for (var date = new DateTime(Year, Month, 1); date <= new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)); date = date.AddDays(+1))
                {
                var col = new DataColumn()
                    {
                    ColumnName = date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture),
                    };

                col.DefaultValue = null;
                _dataTable.Columns.Add(col.ColumnName, typeof(string));
                }

            //var dept = Store.Default.sectorId == 1 ? "Confezione B" : "Stiro";
            var lstLines = (from lines in Models.Tables.Lines
                            where lines.Department == cboDepartment.Text
                            select lines).ToList();

            foreach (var item in lstLines)
                {
                var newRow = _dataTable.NewRow();

                newRow[0] = item.Line;

                _dataTable.Rows.Add(newRow);
                }

            dgvCheck.DataSource = _dataTable;

            //customize datagrid
            dgvCheck.Columns[0].Width = 80;
            dgvCheck.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCheck.Columns[0].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 9);

            for (var i = 1; i <= dgvCheck.Columns.Count - 1; i++)
                {
                dgvCheck.Columns[i].Width = 40;
                dgvCheck.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCheck.Columns[i].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 9);
                dgvCheck.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                var dt = Convert.ToDateTime(dgvCheck.Columns[i].Name);
                dgvCheck.Columns[i].HeaderText = dt.Day.ToString();

                if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                    {
                    dgvCheck.Columns[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    dgvCheck.Columns[i].HeaderCell.Style.BackColor = Color.LightBlue;
                    }
                for (var r = 0; r <= dgvCheck.Rows.Count - 1; r++)
                    {
                    dgvCheck.Rows[r].Cells[i].Value = "0";
                    }
                }

            ReloadData();

            pnControlSave.Visible = false;

            _firstRead = false;

            dgvCheck.Focus();
            }

        private Image ReturnImageByState(bool check)
            {
            if (check)
                {
                var bmp = new Bitmap(40, 25);
                bmp = Properties.Resources.holiday_button_16;
                return bmp;
                }
            else
                {
                return EmptyBitmap();
                }            
            }

        private Image EmptyBitmap()
            {
            return new Bitmap(40, 25);
            }
        private void dgvCheck_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
          
            }
        private void SaveData()
            {
            if (cboDepartment.Text == string.Empty)
            {
                MessageBox.Show("Please select one department.");
                return;
            }

            var q = "delete from holidays where month='" + Month + "' and year='" + Year + "' and department='" + cboDepartment.Text + "'"; // and department='" + Store.Default.selDept + "'";
            var nQ = "insert into holidays (line,hdate,note,month,year,department,idsector) values (@param1,@param2,@param3,@param4,@param5,@param6,@param7)";

            using (var con = new SqlConnection(Central.SpecialConnStr))
                {
                var cmd = new SqlCommand(q, con);
                con.Open();
                cmd.ExecuteNonQuery();
                cmd = null;
 
                foreach (DataGridViewRow row in dgvCheck.Rows)
                    {
                    cmd = new SqlCommand(nQ, con);
                    cmd.Parameters.Clear();

                    var line = row.Cells[0].Value.ToString();

                    var strBuil = new StringBuilder();

                    foreach (DataGridViewCell cell in row.Cells)
                        if (cell.Value.ToString() == "1")
                            strBuil.Append("," + Convert.ToDateTime(dgvCheck.Columns[cell.ColumnIndex].Name)
                                .ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

                    if (strBuil.ToString() == string.Empty) continue;
                    var dateArr = strBuil.ToString();
                    dateArr = dateArr.Remove(0, 1);
                    cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = line;
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = dateArr;
                    cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = "hld";
                    cmd.Parameters.Add("@param4", SqlDbType.Int).Value = Month;
                    cmd.Parameters.Add("@param5", SqlDbType.Int).Value = Year;
                    cmd.Parameters.Add("@param6", SqlDbType.NVarChar).Value = cboDepartment.Text;
                    cmd.Parameters.Add("@param7", SqlDbType.Int).Value = Store.Default.sectorId;

                    cmd.ExecuteNonQuery();
                    }

                con.Close();
                }
            }

        private List<LineHolidays> lst = new List<LineHolidays>();
        private void ReloadData()
            {
            var hld = new LineHolidays();
            lst = new List<LineHolidays>();
            var q = "select line,hdate from holidays where month='" + Month + "' and year='" + Year + "' and department='" + cboDepartment.Text + "'";
            using (var con = new SqlConnection(Central.SpecialConnStr))
                {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    {
                    while (dr.Read())
                        {
                        hld.AddLinePeriodClassification(dr[0].ToString(), dr[1].ToString(), lst);
                        }
                    }
                con.Close();
                dr.Close();
                }           
            foreach (DataGridViewRow row in dgvCheck.Rows)
                {
                foreach (var item in lst)
                    {
                    if (item.Line != row.Cells[0].Value.ToString()) continue;
                    foreach (DataGridViewCell cell in row.Cells)
                        {
                        if (cell.ColumnIndex < 1) continue;
                        var dt = Convert.ToDateTime(dgvCheck.Columns[cell.ColumnIndex].Name);
                        if (dt != item.Holiday) continue;
                        cell.Value = "1";
                        cell.ToolTipText = "Holiday";
                        }
                    }
                }
            for (var c = 1; c<= dgvCheck.ColumnCount - 1; c++)
            {
                var cl = dgvCheck.Columns[c];
                cl.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }           
        }
        private void dgvCheck_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {   
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;

            if (dgvCheck.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "1")
                {
                var rect = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
                e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), rect);
                
                var img = Properties.Resources.close_circle;
                var imgx = (rect.Width / 2 - img.Width / 2);
                var imgy = (rect.Height / 2 - img.Height / 2);
                e.Graphics.DrawImage(img, rect.X + imgx, rect.Y + imgy);
            }
            else
                {
                e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);         
                }
            e.Graphics.DrawLine(Pens.Silver, e.CellBounds.Left,
                   e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                   e.CellBounds.Bottom - 1);
            e.Graphics.DrawLine(Pens.Silver, e.CellBounds.Right - 1,
                e.CellBounds.Top, e.CellBounds.Right - 1,
                e.CellBounds.Bottom);
            e.Handled = true;
            }
        private void dgvCheck_CellClick(object sender, DataGridViewCellEventArgs e)
            {
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
            DateTime.TryParse(dgvCheck.Columns[e.ColumnIndex].Name, out var dt);
            if ((dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday) && Store.Default.sectorId!=7)
                {
                dgvCheck.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                return;
                }
            if (dgvCheck.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "0")
                {
                dgvCheck.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                }
            else
                {
                dgvCheck.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                }
            pnControlSave.Visible = true;
            }
        private void pbCheck_Click_1(object sender, EventArgs e)
            {
            lblSavedInfo.Visible = true;
            lblSavedInfo.Refresh();

            SaveData();

            pnControlSave.Visible = false;
            lblSavedInfo.Visible = false;
            lblSavedInfo.Refresh();
            }
        private void pbDiscard_Click_1(object sender, EventArgs e)
            {
            pnControlSave.Visible = false;
            LoadData();
            }
        private void PnControlSave_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Holidays_Load(object sender, EventArgs e)
        {

        }

        private void cboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
    public class LineHolidays
        {
        public LineHolidays()
            {
            }
        public string Line { get; set; }
        public DateTime Holiday { get; set; }
        public string Department { get; set; }
        public LineHolidays(string line, DateTime hd)
            {
            Line = line;
            Holiday = hd;
            }
        public void AddLinePeriodClassification(string line, string date, List<LineHolidays> lst)
        {
            var strDate = date.Split(',').ToList();
            foreach (var item in strDate)
            {
                var dt = DateTime.ParseExact(item.ToString(), 
                    "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                lst.Add(new LineHolidays(line, Convert.ToDateTime(dt)));
            }
        }       
    }    
}
