using ganntproj1.ObjectModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class Settings : Form
        {
        private readonly JobModel _viewport = new JobModel();

        /// <summary>
        /// 
        /// </summary>
        public Settings()
            {
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

        private void rbStiro_CheckedChanged(object sender, EventArgs e)
            {
            }

        private void Settings_Load(object sender, EventArgs e)
            {
            var arrDept = Store.Default.arrDept.Split(',');
            foreach (var dpt in arrDept)
            {
                if (dpt == string.Empty) continue;
                cbDept.Items.Add(dpt);
            }
            if (cbDept.Items.Count > 0)
            {
                cbDept.SelectedIndex = cbDept.FindString(Store.Default.selDept);
            }

            var dept = Store.Default.arrDept;
            if (dept.Contains(lblConfA.Text)) cbConfA.Checked = true;
            if (dept.Contains(lblConfB.Text)) cbConfB.Checked = true;


            _suggest = Store.Default.suggestData;
            _autoSync = Store.Default.autoSync;
            _backupData = Store.Default.backupData;

            //cbSuggest.Checked = _suggest;
            cbAutoSync.Checked = _autoSync;
            cbBackupData.Checked = _backupData;

            txtConn1.Text = System.Configuration.ConfigurationManager.ConnectionStrings["Ganttproj"].ConnectionString;
            txtConn2.Text = System.Configuration.ConfigurationManager.ConnectionStrings["ONLYOU"].ConnectionString;

            txtConn1.ReadOnly = true;
            txtConn2.ReadOnly = true;

            npdComplet.Value = Store.Default.daysToFinish;

            LoadLines();

            AddButtonEvents();

            LoadShifts();

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
            else
                {
                tc1.SelectedIndex = 0;
                }
            }
        private void LoadLines()
            {
            var vp = new JobModel();
            var linesQuery = from lin in ObjectModels.Tables.Lines
                             where lin.Department == Store.Default.selDept
                             select lin;

            dgvLines.DataSource = linesQuery;
            }
        private void button1_Click_1(object sender, EventArgs e)
            {
            SaveDepartments();
            var c = new Central();
            c.GetBase(null);

            Close();
            }
        private void btnOk_Click(object sender, EventArgs e)
            {
            //Store.Default.suggestData = cbSuggest.Checked;
            Store.Default.autoSync = cbAutoSync.Checked;
            Store.Default.backupData = cbBackupData.Checked;
            Store.Default.daysToFinish = Convert.ToInt32(npdComplet.Value);
            Store.Default.Save();
            SaveDepartments();
            var c = new Central();
            c.GetBase(null);
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
            else
                {
                Close();
                }
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
                //insert new record
                { 
                try
                    {
                    Lines lines = new Lines();
                    lines.Line = txtLine.Text;
                    lines.Members = nrp;
                    lines.Abatimento = ab;
                    lines.Department = Store.Default.selDept;

                    Tables.Lines.InsertOnSubmit(lines);

                    Config.GetGanttConn().SubmitChanges();

                    dgvLines.Enabled = true;
                    txtLine.BackColor = Color.White;
                    txtMembers.BackColor = Color.White;
                    txtAbatimentoEff.BackColor = Color.White;

                    _isNew = false;
                    }
                catch 
                    {
                    MessageBox.Show("Cannot add the line that is already in use.");
                    } 
                }
            else
            //update record
                {
                var dr = MessageBox.Show("Do you want to update " + _selectedLine + "?", "Line update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) return;

                var query = (from line in ObjectModels.Tables.Lines
                             where line.Line == _selectedLine &&
                             line.Department == Store.Default.selDept
                             select line).Single();

                Tables.Lines.DeleteOnSubmit(query);
                
                Lines lines = new Lines();
                lines.Line = txtLine.Text;
                lines.Members = nrp;
                lines.Abatimento = ab;
                lines.Department = Store.Default.selDept;

                Tables.Lines.InsertOnSubmit(lines);
                Config.GetGanttConn().SubmitChanges();
                }

            txtLine.Text = "";
            txtMembers.Text = "";
            txtAbatimentoEff.Text = "";

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

        private void LoadShifts()
            {
            var dt = new DataTable();
            var q = "select shift,starttime,endtime,department from shifts " +
                "where department='" + Store.Default.selDept + "'";
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
                            sh.Department = Store.Default.selDept;
                            sh.Specialnote = txtSpecNote.Text;
                            
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

                        var q = "delete from shifts where shift='" + _selectedShift + "' and department='" + Store.Default.selDept + "'";
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
                            Department = Store.Default.selDept,
                            Specialnote = txtSpecNote.Text
                            };

                        Tables.Shifts.InsertOnSubmit(shf);
                        Config.GetGanttConn().SubmitChanges();
                        }
                    
                    txtShiftName.Text = "";
                    mtxtEnd.Text = "";
                    mtxtStart.Text = "";

                    LoadShifts();
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

                        LoadShifts();
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

                    LoadShifts();
                    };
            }

        #endregion Shifts

        private void tc1_SelectedIndexChanged(object sender, EventArgs e)
            {

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
            var m = new MyMessage();
            if (string.IsNullOrEmpty(_selectedShift))
                {
                m = new MyMessage("Error", "No shift detection.");
                m.MessageIcon = Properties.Resources.discard_changes_48;
                m.Show();
                
                return;
                }

            if (dgvShifts.Rows.Count <= 0)
                {
                m = new MyMessage("Error", "No shift detection.");
                m.MessageIcon = Properties.Resources.discard_changes_48;
                m.Show();

                return;
                }

            Store.Default.selShift = _selectedShift;

            m = new MyMessage("Shifts", _selectedShift + " has been saved as default shift.");
            m.MessageIcon = Properties.Resources.inform_16;
            m.Show();
            }
        private void SaveDepartments()
        {
            if (!cbConfA.Checked && !cbConfB.Checked)
            {
                MessageBox.Show("Invalid department definition.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var str = "";
            var sb = new System.Text.StringBuilder();
            if (cbConfA.Checked)
            {
                sb.Append(',' + lblConfA.Text);
            }
            if (cbConfB.Checked)
            {
                sb.Append(',' + lblConfB.Text);
            }
            str = sb.ToString() + ',';

            Store.Default.arrDept = str;
            Store.Default.Save();
        }
        private void CbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            Store.Default.selDept = cbDept.Text;
            Store.Default.Save();
            LoadLines();
            LoadShifts();
        }
    } 
}
