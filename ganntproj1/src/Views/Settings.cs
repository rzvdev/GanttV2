using ganntproj1.Models;
using ganntproj1.src.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class Settings : Form
        {
        private readonly JobModel _viewport = new JobModel();

        /// <summary>
        /// The SendMessage
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="msg">The msg<see cref="int"/></param>
        /// <param name="wp">The wp<see cref="IntPtr"/></param>
        /// <param name="lp">The lp<see cref="IntPtr"/></param>
        /// <returns>The <see cref="IntPtr"/></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        /// <summary>
        /// 
        /// </summary>
        public Settings()
            {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            dgvLines.EditingControlShowing += DataGridViewUpperCaseValues;
            dgvLines.DoubleBuffered(true);
            dataGridView2.DoubleBuffered(true);
            dataGridView2.MultiSelect = true;
            }

        private DataTable _tbl_lines = new DataTable();
        
        private bool _suggest = false;
        private bool _autoSync = false;
        private bool _backupData = false;
        private int _startWdth;
        private bool _fast = false;

        private void Settings_Load(object sender, EventArgs e)
            {            
            LoadProductData();

            AddClearFilterButton(txtDownloadSource, "btnDownloadSourcePath");

            if (Store.Default.sectorId==1)
            {
                rbConfezione.Checked = true;
                grpStiro.Enabled = false;
                grpStiro.Enabled = false;
                grpSart.Enabled = false;
            }
            else if (Store.Default.sectorId == 2)
            {
                rbStiro.Checked = true;
                grpSart.Enabled = false;
                grpConfezione.Enabled = false;
                grpTess.Enabled=false;
            }
            else if (Store.Default.sectorId == 7)
            {
                grpStiro.Enabled = false;
                grpSart.Enabled = false;
                grpConfezione.Enabled = false;
                radioButton1.Checked = true;
            }
            else if (Store.Default.sectorId == 8)
            {
                grpStiro.Enabled = false;
                radioButton2.Checked = true;
                grpConfezione.Enabled = false;
                grpTess.Enabled = false;
            }

            var dept = Store.Default.arrDept;
            if (dept.Contains(lblConfA.Text)) cbConfA.Checked = true;
            if (dept.Contains(lblConfB.Text)) cbConfB.Checked = true;
            if (dept.Contains(lblConfC.Text)) cbConfC.Checked = true;
            _suggest = Store.Default.suggestData;
            _autoSync = Store.Default.autoSync;
            _backupData = Store.Default.backupData;
            _fast = Store.Default.fastStart;

            txtDownloadSource.Text = Store.Default.downloadSource;
            cbAutoSync.Checked = _autoSync;
            cbBackupData.Checked = _backupData;
            cbFast.Checked = _fast;
            cbUpdateRuntime.Checked = Store.Default.updateCheckRuntime;

            txtConn1.Text = System.Configuration.ConfigurationManager.ConnectionStrings["Ganttproj"].ConnectionString;
            txtConn2.Text = System.Configuration.ConfigurationManager.ConnectionStrings["ONLYOU"].ConnectionString;
            txtConn1.ReadOnly = true;
            txtConn2.ReadOnly = true;
            npdComplet.Value = Store.Default.daysToFinish;

            var dpt = Store.Default.arrDept.Split(',');
            for (var i = 0; i <= dpt.Length - 1; i++)
            {
                if (dpt[i] == string.Empty) continue;
                cbDept.Items.Add(dpt[i]);
            }
            if (cbDept.Items.Count > 0)
            {
                cbDept.SelectedIndex = 0;
            }

            LoadLines();
            AddButtonEvents();
            var table = Central.ListOfModels;
            dataGridView2.DataSource = table;
            dgvLines.ReadOnly = true;
            dgvLines.AllowUserToAddRows = false;
            dgvLines.DefaultCellStyle.BackColor = Color.White;
            dgvLines.DefaultCellStyle.ForeColor = Color.Black;
            dgvLines.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DefaultCellStyle.BackColor = Color.White;
            dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.DefaultCellStyle.BackColor = SystemColors.Control;
            txtLine.CharacterCasing = CharacterCasing.Upper;
            txtShiftName.CharacterCasing = CharacterCasing.Upper;
            _startWdth = Width;

            if (Central.SettingsCompleted == Central.SettingsSys.Shift)
                {
                tc1.SelectedIndex = 4;
                }
            else if (Central.SettingsCompleted == Central.SettingsSys.NewUpdate)
                {
                lblpop.Visible = true;
                tc1.SelectedIndex = 6;
                }
            else
            {
                tc1.SelectedIndex = 0;
               
            }

            var settings = new SettingsDom();
            settings.SaveHoursToSettings();

            txtHoursConf.Text = Store.Default.confHour.ToString();
            txtHoursStiro.Text = Store.Default.stiroHour.ToString();
            txtHoursTess.Text = Store.Default.tessHour.ToString();
            txtHoursSart.Text = Store.Default.sartHour.ToString();

            txtHoursConfW.Text = Store.Default.confHourW.ToString();
            txtHoursStiroW.Text = Store.Default.stioHourW.ToString();
            txtHoursTessW.Text = Store.Default.tessHourW.ToString();
            txtHoursSartW.Text = Store.Default.sartHourW.ToString();

            if (Central.SettingsCompleted == Central.SettingsSys.Department)
            {
                tc1.SelectedIndex = 2;
            }

            Text = "Settings - " + tc1.SelectedTab.Text;

            txt1.Text = Central.LowEff.ToString();
            txt2.Text = Central.MediumEff.ToString();
            txt3.Text = Central.HighEff.ToString();
            lbl1.BackColor = Central.LowColor;
            lbl2.BackColor = Central.MediumColor;
            lbl3.BackColor = Central.HighColor;

            lblcolor1.Text = "#" + Central.LowColor.ToArgb().ToString("x").Substring(2, 6);
            lblcolor2.Text = "#" + Central.MediumColor.ToArgb().ToString("x").Substring(2, 6);
            lblcolor3.Text = "#" + Central.HighColor.ToArgb().ToString("x").Substring(2, 6);

            toggleCheckBox1.Checked = Store.Default.manualMembers;
            toggleCheckBox2.Checked = Store.Default.manualDate;
            cbOrderArrivo.Checked = Store.Default.arrivoOrder;
        }

        private void LoadLines()
            {
            var vp = new JobModel();
            var linesQuery = from lin in Models.Tables.Lines
                             where lin.Department == cbDept.Text
                             select new { lin.Line, lin.Members, lin.Abatimento, lin.Description, lin.Groupby};
            dgvLines.DataSource = linesQuery;
            dgvLines.Columns[0].Width = 80;
            dgvLines.Columns[1].HeaderText = "Memb/\nMac";
            dgvLines.Columns[1].Width = 60;
            dgvLines.Columns[2].HeaderText = "Ab%";
            dgvLines.Columns[2].Width = 40;
            dgvLines.Columns[3].Width = 160;
            dgvLines.Columns[4].Width = 80;
        }

        private void button1_Click_1(object sender, EventArgs e)
            {
            Store.Default.autoSync = cbAutoSync.Checked;
            Store.Default.backupData = cbBackupData.Checked;
            Store.Default.fastStart = cbFast.Checked;
            Store.Default.daysToFinish = Convert.ToInt32(npdComplet.Value);
            //double.TryParse(txtHoursConf.Text, out var confh);
            //Store.Default.confHour = confh;
            //double.TryParse(txtHoursStiro.Text, out var stiroh);
            //Store.Default.stiroHour = stiroh;
            //double.TryParse(txtHoursTess.Text, out var tessh);
            //Store.Default.tessHour = tessh;
            //double.TryParse(txtHoursSart.Text, out var sarth);
            //Store.Default.sartHour = sarth;
            //double.TryParse(txtHoursConfW.Text, out var confhw);
            //Store.Default.confHourW = confhw;
            //double.TryParse(txtHoursStiroW.Text, out var stirohw);
            //Store.Default.stioHourW = stirohw;
            //double.TryParse(txtHoursTessW.Text, out var tesshw);
            //Store.Default.tessHourW = tesshw;
            //double.TryParse(txtHoursSartW.Text, out var sarthw);
            UpdateDepartmentHours();
            //Store.Default.sartHourW = sarthw;
            Store.Default.downloadSource = txtDownloadSource.Text;
            Store.Default.updateCheckRuntime = cbUpdateRuntime.Checked;
            Store.Default.Save();
            SaveDepartments();
            Close();
            }

        private void btnOk_Click(object sender, EventArgs e)
            {
            //Store.Default.suggestData = cbSuggest.Checked;
            Store.Default.autoSync = cbAutoSync.Checked;
            Store.Default.backupData = cbBackupData.Checked;
            Store.Default.fastStart = cbFast.Checked;
            Store.Default.daysToFinish = Convert.ToInt32(npdComplet.Value);

            //double.TryParse(txtHoursConf.Text, out var confh);
            //Store.Default.confHour = confh;
            //double.TryParse(txtHoursStiro.Text, out var stiroh);
            //Store.Default.stiroHour = stiroh;
            //double.TryParse(txtHoursTess.Text, out var tessh);
            //Store.Default.tessHour = tessh;
            //double.TryParse(txtHoursSart.Text, out var sarth);
            //Store.Default.sartHour = sarth;

            //double.TryParse(txtHoursConfW.Text, out var confhw);
            //Store.Default.confHourW = confhw;
            //double.TryParse(txtHoursStiroW.Text, out var stirohw);
            //Store.Default.stioHourW = stirohw;
            //double.TryParse(txtHoursTessW.Text, out var tesshw);
            //Store.Default.tessHourW = tesshw;
            //double.TryParse(txtHoursSartW.Text, out var sarthw);
            //Store.Default.sartHourW = sarthw;

            UpdateDepartmentHours();

            Store.Default.downloadSource = txtDownloadSource.Text;
            Store.Default.updateCheckRuntime = cbUpdateRuntime.Checked;

            Store.Default.Save();
            
            SaveDepartments();
            SaveProductionEff();
            var c = new Central();
            c.GetBase();
            c.GetProductionColor();

            var table = Central.ListOfModels;
            dataGridView2.DataSource = table;
            }

        private void btnCancel_Click(object sender, EventArgs e)
            {
            if (Store.Default.selShift == string.Empty)
            {
                MessageBox.Show("Must define shift.");
                return;
            }
            else if (Store.Default.sectorId <= 0 || Store.Default.arrDept == string.Empty)
            {
                MessageBox.Show("Department must be defined.",
                    Application.ProductName + " Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Close();            
        }
        #region Lines
        private bool FindLineIndex(int rowIdx)
            {
            var buf_list = new List<string>();

            for (var i = 0; i <= dgvLines.Rows.Count - 2; i++)
                {
                var row = dgvLines.Rows[i];

                if (i == rowIdx) continue;

                buf_list.Add(row.Cells[1].Value.ToString());
                }

            if (buf_list.Contains(dgvLines.Rows[rowIdx].Cells[0].Value.ToString()))
                {
                return true;
                } 
            else
                return false;
            }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
            {
            if (FindLineIndex(e.RowIndex))
                {
                MessageBox.Show("Line exist");
                dgvLines.Rows[e.RowIndex].Cells[0].Value = "";
                }
            }

        private static void DataGridViewUpperCaseValues(object sender, DataGridViewEditingControlShowingEventArgs e)
            {
            if (e.Control is TextBox box) box.CharacterCasing = CharacterCasing.Upper;
            }

        private string _selectedLine;
        private void dgvLines_SelectionChanged(object sender, EventArgs e)
            {
            if (dgvLines.SelectedRows.Count <= 0) return;

            _selectedLine = dgvLines.SelectedRows[0].Cells[0].Value.ToString();
            txtLine.Text = _selectedLine;
            txtMembers.Text = dgvLines.SelectedRows[0].Cells[1].Value.ToString();
            txtAbatimentoEff.Text = dgvLines.SelectedRows[0].Cells[2].Value.ToString();
            txtDescriptionLine.Text = dgvLines.SelectedRows[0].Cells[3].Value.ToString();

            if (dgvLines.SelectedRows[0].Cells[4].Value != null)
            {
                txtGroup.Text = dgvLines.SelectedRows[0].Cells[4].Value.ToString();
            }

        }

        private bool _isNew = false;
        private bool _isNewShift = false;
        private void pbAdd_Click(object sender, EventArgs e)
            {
            dgvLines.Enabled = false;
            _isNew = true;

            txtLine.Text = "";
            txtMembers.Text = "";
            txtAbatimentoEff.Text = "";
            txtLine.BackColor = Color.LightYellow;
            txtMembers.BackColor = Color.LightYellow;
            txtAbatimentoEff.BackColor = Color.LightYellow;
            txtLine.Focus();
            txtLine.Text = "LINEA";
            txtLine.SelectionStart = txtLine.Text.Length;
            txtLine.SelectionLength = 0;
            }

        private void pbSave_Click(object sender, EventArgs e)
            {
            int.TryParse(txtMembers.Text, out int nrp);
            int.TryParse(txtAbatimentoEff.Text, out int ab);

            if (_isNew)
                //insert new line record
                { 
                try
                   {
                    var checkLine = (from l in Tables.Lines
                                     where l.Line == txtLine.Text && l.Department == cbDept.Text
                                     select l).SingleOrDefault();

                    if (checkLine != null)
                    {
                        MessageBox.Show("Line already exist.", "Ganttproj - Lines", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Lines lines = new Lines();
                    lines.Line = txtLine.Text;
                    lines.Members = nrp;
                    lines.Abatimento = ab;
                    lines.Department = cbDept.Text;
                    lines.Description = txtDescriptionLine.Text;
                    lines.Groupby = txtGroup.Text != string.Empty ? txtGroup.Text : null;

                    Tables.Lines.InsertOnSubmit(lines);
                    Config.GetGanttConn().SubmitChanges();

                    dgvLines.Enabled = true;
                    txtLine.BackColor = Color.White;
                    txtMembers.BackColor = Color.White;
                    txtAbatimentoEff.BackColor = Color.White;

                    _isNew = false;
                  }
                catch (Exception ex)
                  {
                  MessageBox.Show(ex.Message);
                  } 
                }
            else
            //update record
                {
                var dr = MessageBox.Show("Do you want to update " + _selectedLine + "?", "Line update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) return;

                var query = (from line in Models.Tables.Lines
                             where line.Line == _selectedLine &&
                             line.Department == cbDept.Text
                             select line).Single();

                Tables.Lines.DeleteOnSubmit(query);

                Lines lines = new Lines
                {
                    Line = txtLine.Text,
                    Members = nrp,
                    Abatimento = ab,
                    Department = cbDept.Text,
                    Description =txtDescriptionLine.Text,
                    Groupby = txtGroup.Text != string.Empty ? txtGroup.Text : null
                };

                Tables.Lines.InsertOnSubmit(lines);
                Config.GetGanttConn().SubmitChanges();
                }

            txtLine.Text = "";
            txtMembers.Text = "";
            txtAbatimentoEff.Text = "";
            txtDescriptionLine.Text = "";
            txtGroup.Text = "";

            LoadLines();
            }

        private void pbDiscard_Click(object sender, EventArgs e)
            {
            if (_isNew)
                {
                txtLine.Text = "";
                txtMembers.Text = "";
                txtAbatimentoEff.Text = "";
                txtLine.BackColor = Color.White;
                txtMembers.BackColor = Color.White;
                txtAbatimentoEff.BackColor = Color.White;
                dgvLines.Enabled = true;
                LoadLines();
                _isNew = false;
                return;
                }
            var dr = MessageBox.Show("Are you sure you want to delete " + txtLine.Text +"?", "Lines", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            var query = from line in Tables.Lines
                        where line.Line == _selectedLine 
                        && line.Department == cbDept.Text
                        select line;
            var lines = query.ToList();
            foreach (var line in lines)
                {
                Tables.Lines.DeleteOnSubmit(line);
                }
            Config.GetGanttConn().SubmitChanges();
            LoadLines();
            }

        #endregion Lines
       
        #region Shifts

        private void LoadShiftsBySector()
            {
            var dt = new DataTable();
            var q = "select shift,starttime,endtime from shifts " +
                "where sectorId='" + Store.Default.sectorId + "'";
            using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
                }

            dgvShifts.DataSource = dt;

            if (dgvShifts.Rows.Count <= 0) return;

            foreach (DataGridViewRow row in dgvShifts.Rows)
                {
                var sRow = row.Cells[0].Value.ToString();

                if (sRow == Store.Default.userShifts)
                    {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }

        private void AddButtonEvents()
            {
            pbAddShift.Click += delegate 
            {
                dgvShifts.Enabled = false;
                _isNewShift = true;

                txtShiftName.Text = "";
                mtxtStart.Text = "";
                mtxtEnd.Text = "";
                txtSpecNote.Text = "<note>";

                txtShiftName.BackColor = Color.LightYellow;
                mtxtStart.BackColor = Color.LightYellow;
                mtxtEnd.BackColor = Color.LightYellow;

                txtShiftName.Focus();
                txtShiftName.Text = "TURNO";
                txtShiftName.SelectionStart = txtLine.Text.Length;
                txtShiftName.SelectionLength = 0;
                };

            pbSaveShift.Click += delegate
                {
                    var start = TimeSpan.Parse(mtxtStart.Text);
                    var end = TimeSpan.Parse(mtxtEnd.Text);
                    
                    if (_isNewShift)
                    //insert new record
                        {
                        try
                            {                            
                            Shiftx sh = new Shiftx();
                            sh.Shift = txtShiftName.Text;
                            sh.Starttime = new DateTime(1900, 1, 1, start.Hours, start.Minutes, 0);
                            sh.Endtime = new DateTime(1900, 1, 1, end.Hours, end.Minutes, 0);
                            sh.Department = cbDept.Text;
                            sh.Specialnote = txtSpecNote.Text;
                            sh.Sectorid = Store.Default.sectorId;
                            
                            Tables.Shifts.InsertOnSubmit(sh);

                            Config.GetGanttConn().SubmitChanges();
                            
                            dgvShifts.Enabled = true;
                            txtShiftName.BackColor = Color.White;
                            mtxtStart.BackColor = Color.White;
                            mtxtEnd.BackColor = Color.White;

                            _isNewShift = false;
                            }
                        catch (Exception ex)
                            {
                            MessageBox.Show(ex.Message);
                            }
                        }
                    else
                    //update record
                        {
                        var dr = MessageBox.Show("Do you want to update selected shift?", "Shift update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.No) return;

                        var q = "delete from shifts where shift='" + _selectedShift + "' and sectorId='" + Store.Default.sectorId + "'";
                        using   (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                            {
                            var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            }

                        Shiftx shf = new Shiftx
                        {
                            Shift = txtShiftName.Text,
                            Starttime = new DateTime(1900, 1, 1, start.Hours, start.Minutes, 0),
                            Endtime = new DateTime(1900, 1, 1, end.Hours, end.Minutes, 0),
                            Department = cbDept.Text,
                            Specialnote = txtSpecNote.Text,
                            Sectorid = Store.Default.sectorId
                            };

                        Tables.Shifts.InsertOnSubmit(shf);
                        Config.GetGanttConn().SubmitChanges();
                        }
                    
                    txtShiftName.Text = "";
                    mtxtEnd.Text = "";
                    mtxtStart.Text = "";

                    LoadShiftsBySector();
                    };

            pbDiscardShift.Click += delegate
                {
                    if (_isNewShift)
                        {
                        txtShiftName.Text = "";
                        mtxtStart.Text = "";
                        mtxtEnd.Text = "";
                        txtSpecNote.Text = "<note>";
                        txtShiftName.BackColor = Color.White;
                        mtxtStart.BackColor = Color.White;
                        mtxtEnd.BackColor = Color.White;

                        dgvShifts.Enabled = true;

                        LoadShiftsBySector();
                        _isNewShift = false;
                        return;
                        }

                    var dr = MessageBox.Show("Are you sure you want to delete selected shift?", "Delete shift", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.No) return;

                    var q = "delete from shifts where shift='" + _selectedShift + "'";
                    using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                        {
                        var cmd = new System.Data.SqlClient.SqlCommand(q, c);
                        c.Open();
                        cmd.ExecuteNonQuery();
                        c.Close();
                        }

                    LoadShiftsBySector();
                    };
            }

        #endregion Shifts
        private void tc1_SelectedIndexChanged(object sender, EventArgs e)
            {
            if (tc1.SelectedIndex == 4)
            {

                LoadShiftsBySector();
            }

            Text = "Settings - " + tc1.SelectedTab.Text;
            }
        private string _selectedShift;
        private void dgvShifts_SelectionChanged(object sender, EventArgs e)
            {
            if (dgvShifts.SelectedRows.Count <= 0) return;

            _selectedShift = dgvShifts.SelectedRows[0].Cells[0].Value.ToString();
            txtShiftName.Text = _selectedShift;
            mtxtStart.Text = dgvShifts.SelectedRows[0].Cells[1].Value.ToString();
            mtxtEnd.Text = dgvShifts.SelectedRows[0].Cells[2].Value.ToString();
            }
        private void button2_Click_1(object sender, EventArgs e)
            {
            MyMessage m;
            if (string.IsNullOrEmpty(_selectedShift))
            {
                m = new MyMessage("Error", "No shift detection.")
                {
                    MessageIcon = Properties.Resources.discard_changes_48
                };
                m.Show();
                return;
            }

            if (dgvShifts.Rows.Count <= 0)            
            {
                m = new MyMessage("Error", "No shift detection.")
                {
                    MessageIcon = Properties.Resources.discard_changes_48
                };
                m.Show();
                return;
                }

            Store.Default.selShift = _selectedShift;
            m = new MyMessage("Shifts", _selectedShift + " has been saved as default shift.")
            {
                MessageIcon = Properties.Resources.inform_16
            };
            m.Show();
            }

        private void SaveDepartments()
        {
            var sb = new System.Text.StringBuilder();
            if (Store.Default.sectorId == 1)
            {
                if (!cbConfA.Checked && !cbConfB.Checked && !cbConfC.Checked)
                {
                    MessageBox.Show("Invalid department definition.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cbConfA.Checked)
                {
                    sb.Append(',' + lblConfA.Text);
                }
                if (cbConfB.Checked)
                {
                    sb.Append(',' + lblConfB.Text);
                }
                if (cbConfC.Checked)
                {
                    sb.Append(',' + lblConfC.Text);
                }
            }
            else if (Store.Default.sectorId == 2)
            {
                sb.Append(",Stiro");
            }
            else if (Store.Default.sectorId == 7)
            {
                sb.Append(",Tessitura");
            }
            else if (Store.Default.sectorId == 8)
            {
                sb.Append(",Sartoria");
            }

            string str = sb.ToString() + ',';

            if (Store.Default.sectorId == 1)
            {
                if (rbConfezione.Checked)
                {
                    Store.Default.selSector = rbConfezione.Text;
                }
                else
                {
                    Store.Default.selSector = rbStiro.Text;
                }
            }
            else if (Store.Default.sectorId == 2)
            {
                Store.Default.selSector = "Stiro";
            }
            else if (Store.Default.sectorId == 7)
            {
                Store.Default.selSector = "Tessitura";
            }
            else if (Store.Default.sectorId == 8)
            {
                Store.Default.selSector = "Sartoria";
            }

            Store.Default.arrDept = str;
            Store.Default.Save();
        }

        private void CbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void RbConfezione_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.sectorId = 1;
            Store.Default.Save();
            grpStiro.Enabled = false;
            grpTess.Enabled = false;
            grpSart.Enabled = false;
            grpConfezione.Enabled = true;
        }

        private void RbStiro_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.sectorId = 2;
            Store.Default.Save();
            grpConfezione.Enabled = false;
            grpTess.Enabled = false;
            grpSart.Enabled = false;
            grpStiro.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.sectorId = 7;
            Store.Default.Save();
            grpConfezione.Enabled = false;
            grpTess.Enabled = true;
            grpStiro.Enabled = false;
            grpSart.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.sectorId = 8;
            Store.Default.Save();
            grpConfezione.Enabled = false;
            grpSart.Enabled = true;
            grpStiro.Enabled = false;
            grpTess.Enabled = false;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var pathMain = AppDomain.CurrentDomain.BaseDirectory;

                linkLabel1.Text = "Checking for updates...";
                linkLabel1.Refresh();
                SuspendLayout();
                var strAssembOld = "";
                foreach (var file in Directory.GetFiles(pathMain))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembOld = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var strAssembNew = "";
                foreach (var file in Directory.GetFiles(txtDownloadSource.Text))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembNew = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var newVr = Config.ReturnAssemblyNumber(strAssembNew);
                var oldVr = Config.ReturnAssemblyNumber(strAssembOld);

                if (newVr <= oldVr)
                {
                    linkLabel1.Text = "0 results";
                    linkLabel1.ForeColor = Color.Red;
                    linkLabel1.Refresh();

                    ResumeLayout(true);
                    MessageBox.Show("No updates available.",
                        Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    linkLabel1.Text = "";
                    linkLabel1.Refresh();
                    return;
                }

                linkLabel1.Text = "1 results";
                linkLabel1.ForeColor = Color.SeaGreen;
                linkLabel1.Refresh();

                var diag = MessageBox.Show("New version Ganntproj1 " + strAssembNew + " is available. Do you want to update Ganntproj1 " + strAssembOld + "?",
                    Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (diag == DialogResult.Yes)
                {
                    linkLabel1.Text = "Updaing " + strAssembNew;
                    linkLabel1.ForeColor = Color.DodgerBlue;
                    linkLabel1.Refresh();

                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Backup"))
                    {
                        //create 'Backup folder' before update
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Backup");
                    }
                    else
                    {
                        //if exist clear backup folder before update
                        foreach (var f in Directory.GetFiles(pathMain + "\\Backup"))
                        {
                            File.Delete(f);
                        }
                    }

                    var sourceFileName = AppDomain.CurrentDomain.BaseDirectory + "\\ganntproj1.exe";
                    var sourceFileConfing = AppDomain.CurrentDomain.BaseDirectory + "\\ganntproj1.exe.config";
                    var destinationDirApp = AppDomain.CurrentDomain.BaseDirectory + "\\Backup";

                    //perform moving current version to backup directory
                    File.Move(sourceFileName, destinationDirApp + "\\exebck");
                    File.Move(sourceFileConfing, destinationDirApp + "\\configbck");

                    var copySourceDir = txtDownloadSource.Text;
                    // copy new version from server to local user directory
                    File.Copy(copySourceDir + "\\ganntproj1.exe", AppDomain.CurrentDomain.BaseDirectory + "\\ganntproj1.exe");
                    File.Copy(copySourceDir + "\\ganntproj1.exe.config", AppDomain.CurrentDomain.BaseDirectory + "\\ganntproj1.exe.config");

                    ResumeLayout(true);
                    linkLabel1.Text = "";
                    linkLabel1.ForeColor = Color.Black;
                    linkLabel1.Refresh();

                    var restart = MessageBox.Show("Your software is up to date. Do you want to restart application to continue on ganntproj1 " + strAssembNew + "?",
                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (restart == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                }
                else
                {
                    ResumeLayout(true);
                    linkLabel1.Text = "";
                    linkLabel1.Refresh();
                }
            }
            catch
            {
                MessageBox.Show("Invalid path or network connection.", Application.ProductName + " Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ResumeLayout(true);
                linkLabel1.Text = "";
                linkLabel1.Refresh();
            }
        }

        private void LoadProductData()
        {
            var pathMain = AppDomain.CurrentDomain.BaseDirectory;

            lblProductName.Text = Application.ProductName;
            lblProduct.Text = Application.ProductName + ".sector";
           
            foreach (var file in Directory.GetFiles(pathMain))
            {
                if (Path.GetExtension(file) != ".exe") continue;

                lblRelDate.Text = File.GetLastWriteTime(file).ToString();
                lblVersion.Text = Assembly.LoadFile(file).GetName().Version.ToString() + "; " + Application.ProductVersion.ToString();
            }

            lblCopyright.Text = Application.CompanyName;
            lblCulture.Text = "CI: " + Application.CurrentCulture.ToString();
        }

        private void AddClearFilterButton(TextBox txt, string name)
        {
            var _browseDownloadPath = new Button
            {
                Size = new Size(24, txt.ClientSize.Height + 2)
            };
            _browseDownloadPath.Location = new Point(txt.ClientSize.Width - _browseDownloadPath.Width, -1);
            _browseDownloadPath.BackgroundImage = Properties.Resources.folder;
            _browseDownloadPath.BackgroundImageLayout = ImageLayout.Zoom;
            _browseDownloadPath.Cursor = Cursors.Default;
            _browseDownloadPath.BackColor = default;
            _browseDownloadPath.Name = name;
            _browseDownloadPath.Parent = txt;

            _browseDownloadPath.Click += (s, g) =>
            {
                var srcPath = new FolderBrowserDialog();
                srcPath.Description = "Search download path";
                if (srcPath.ShowDialog() == DialogResult.OK)
                {
                    txt.Text = srcPath.SelectedPath;
                }
            };

            SendMessage(txt.Handle, 0xd3, (IntPtr)2, (IntPtr)(_browseDownloadPath.Width << 16));
        }

        private void CbUpdateRuntime_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                toolTip1.Show("Update silent listener that will gives information about new version when is available.", (CheckBox)sender);
            }
            else
            {
                toolTip1.Hide((CheckBox)sender);
            }
        }

        private string _color1;
        private string _color2;
        private string _color3;

        private void SaveProductionEff()
        {
            var q = "delete from produzioneRelation";
            var cmd = new SqlCommand();
            using (var c = new SqlConnection(Central.SpecialConnStr))
            {
                c.Open();
                cmd = new SqlCommand(q, c);
                cmd.ExecuteNonQuery();
                c.Close();
                cmd = null;
            }

             _color1 = "#" + lbl1.BackColor.ToArgb().ToString("x").Substring(2, 6);
             _color2 = "#" + lbl2.BackColor.ToArgb().ToString("x").Substring(2, 6);
             _color3 = "#" + lbl3.BackColor.ToArgb().ToString("x").Substring(2, 6);

            if (txt1.Text == string.Empty) txt1.Text = "82";
            if (txt2.Text == string.Empty) txt2.Text = "90";
            if (txt3.Text == string.Empty) txt3.Text = "120";

            q = "insert into produzioneRelation (id,mode,item,value) values (@param,@param1, @param2, @param3)";
            using (var c = new SqlConnection(Central.SpecialConnStr))
            {
                c.Open();
                cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@param", SqlDbType.NVarChar).Value = 1;
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = "production";
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = txt1.Text;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = _color1;

                cmd.ExecuteNonQuery();
                c.Close();
                cmd = null;
            }
            using (var c = new SqlConnection(Central.SpecialConnStr))
            {
                c.Open();
                cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@param", SqlDbType.NVarChar).Value = 2;
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = "production";
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = txt2.Text;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = _color2;

                cmd.ExecuteNonQuery();
                c.Close();
                cmd = null;
            }
            using (var c = new SqlConnection(Central.SpecialConnStr))
            {
                c.Open();
                cmd = new SqlCommand(q, c);
                cmd.Parameters.Add("@param", SqlDbType.NVarChar).Value = 3;
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = "production";
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = txt3.Text;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = _color3;
                cmd.ExecuteNonQuery();
                c.Close();
                cmd = null;
            }
        }

        private void lbl1_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();

            if (col.ShowDialog() == DialogResult.OK)
            {
                string color = col.Color.ToArgb().ToString("x");
                color = color.Substring(2, 6);                
                lbl1.BackColor = col.Color;
                lblcolor1.Text = color;
            }
        }

        private void lbl2_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();

            if (col.ShowDialog() == DialogResult.OK)
            {
                string color = col.Color.ToArgb().ToString("x");
                color = color.Substring(2, 6);
                lbl2.BackColor = col.Color;
                lblcolor2.Text = color;
            }
        }

        private void lbl3_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();

            if (col.ShowDialog() == DialogResult.OK)
            {
                string color = col.Color.ToArgb().ToString("x");
                color = color.Substring(2, 6);                
                lbl3.BackColor = col.Color;
                lblcolor3.Text = color;
            }
        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void toggleCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.manualMembers = toggleCheckBox1.Checked;
            Store.Default.Save();
        }

        private void toggleCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.manualDate = toggleCheckBox2.Checked;
            Store.Default.Save();
        }

        private void btnUpdate_MouseEnter(object sender, EventArgs e)
        {

        }

        private void cbOrderArrivo_CheckedChanged(object sender, EventArgs e)
        {
            Store.Default.arrivoOrder = cbOrderArrivo.Checked;
            Store.Default.Save();
        }

        private void UpdateDepartmentHours()
        {
            var lst = new List<string>
            {
                "Confezione",
                "Stiro",
                "Tessitura",
                "Sartoria"
            };

            double.TryParse(txtHoursConf.Text, out var confh);
            Store.Default.confHour = confh;
            double.TryParse(txtHoursStiro.Text, out var stiroh);
            Store.Default.stiroHour = stiroh;
            double.TryParse(txtHoursTess.Text, out var tessh);
            Store.Default.tessHour = tessh;
            double.TryParse(txtHoursSart.Text, out var sarth);
            Store.Default.sartHour = sarth;
            double.TryParse(txtHoursConfW.Text, out var confhw);
            Store.Default.confHourW = confhw;
            double.TryParse(txtHoursStiroW.Text, out var stirohw);
            Store.Default.stioHourW = stirohw;
            double.TryParse(txtHoursTessW.Text, out var tesshw);
            Store.Default.tessHourW = tesshw;
            double.TryParse(txtHoursSartW.Text, out var sarthw);
            Store.Default.sartHourW = sarthw;

            var settings = new SettingsDom();

            foreach (var item in lst)
            {
                switch (item)
                {
                    case "Confezione":
                        settings.UpdateSettingsHours(item, confh.ToString(), confhw.ToString());
                        break;
                    case "Stiro":
                        settings.UpdateSettingsHours(item, stiroh.ToString(),stirohw.ToString());
                        break;
                    case "Tessitura":
                        settings.UpdateSettingsHours(item, tessh.ToString(), tesshw.ToString());
                        break;
                    case "Sartoria":
                        settings.UpdateSettingsHours(item, sarth.ToString(), sarthw.ToString());
                        break;
                }
            }
        }
    }
}
