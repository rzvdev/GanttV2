using ganntproj1.src.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1.src.Views
{
    public partial class FractionateOrder : Form
    {
        private readonly Bar _bar;

        public FractionateOrder()
        {
            InitializeComponent();
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnResetSuggDate, "Reset to suggested date and time");
        }

        public FractionateOrder(Bar bar)
        {
            InitializeComponent();
            _bar = bar;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnResetSuggDate, "Reset to suggested date and time");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Text = "Reprogramm order: " + _bar.RowText;

            if (_bar != null)
            {
                lblOrder.Text = _bar.RowText;
                lblArticle.Text = _bar.Article;
                lblLine.Text = _bar.Tag;
                lblTotQty.Text = (_bar.LoadedQty - _bar.ProductionQty).ToString();
                lblMaxQty.Text = "Max: " + (_bar.LoadedQty - _bar.ProductionQty).ToString();
                lblMaxMembers.Text = "Max: " + _bar.Members.ToString();
                txtPersons.Text = _bar.Members.ToString();
            }

            txtQty.Text = "0";
            dtpStart.ShowCheckBox = Store.Default.manualDate;
            dtpStart.Enabled = Store.Default.manualDate;
            txtPersons.Enabled = Store.Default.manualMembers;

            LoadLines();

            if (!string.IsNullOrEmpty(_bar.Operation))
            {
                cbOperation.Visible = false;
                cbOperation.SelectedIndex = cbOperation.FindStringExact(_bar.Operation);
            }

            if (_bar.ProductionQty > 0 || !_bar.RowText.Contains('_'))
            {
                btnUndo.Visible = false;
            }

            txtQty.Focus();
        }

        #region PrivateMethods

        private void LoadLines()
        {
            try
            {
                var q = $"select line from Lines where department='{_bar.Department}';";

                using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cboLine.Items.Add(dr[0].ToString());
                        }
                    }
                    con.Close();
                    dr.Close();
                }

                if (cboLine.Items.Count > 0)
                {
                    cboLine.SelectedItem = _bar.Tag;
                }
                else
                {
                    MessageBox.Show("No lines detected");
                    lblSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Block orders form error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSave.Enabled = false;
            }
        }

        private bool FractionateOrders()
        {
            int.TryParse(txtQty.Text, out var newQty);
            int.TryParse(txtPersons.Text, out var newMembers);

            var operation = cbOperation.Text;

            if (newQty <= 0 || newMembers <= 0)
            {
                MessageBox.Show("Invalid qty or members input.");
                return false;
            }

            if (newQty < 0 || newQty > _bar.LoadedQty - _bar.ProductionQty)
            {
                MessageBox.Show("Value must be between 0 and maximum qty by 'commessa'");
                return false;
            }

            if (_bar.ProductionQty == 0 && newQty == _bar.LoadedQty)
            {
                DeleteExsistingOrder();
            }
            else
            {
                UpdateExsistingOrder(_bar.LoadedQty - newQty, 0, cboLine.Text, _bar.FromTime);
                InsertFractionatedOrder(newQty, newMembers);
            }

            var queryOn = $"{_bar.RowText}-{cboLine.Text}-{_bar.Department}";
            Config.InsertOperationLog("manual_fractioning", queryOn, "fractionate");

            var c = new Central();
            c.GetBase();

            return true;
        }

        private void UpdateExsistingOrder(int newQty, int id = 0, string line = "", DateTime date = default(DateTime))
        {
            var jobModel = new JobModel();
            if (line == string.Empty) line = _bar.Tag;
            if (date == default(DateTime)) date = _bar.FromTime;
            var duration = jobModel.CalculateJobDuration(line, newQty, _bar.QtyH, _bar.Department, _bar.Members);
            var dailyProd = jobModel.CalculateDailyQty(line, _bar.QtyH, _bar.Department, _bar.Members, newQty);

            var durationTick = TimeSpan.FromDays(duration).Ticks;
            var eDate = date.AddTicks(durationTick);

            var shift = new ShiftRecognition();
            eDate = shift.GetEndTimeInShift(date, eDate);

            //eDate = new DateTime(eDate.Year, eDate.Date.Month, eDate.Day, eDate.Hour, eDate.Minute, 0, 0);

            using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var updateQuery = @"
update objects set loadedQty=@LoadedQty, duration=@Duration, endDate=@EndDate, dailyProd=@DailyProd, closedord=@ClosedOrd
where Id=@Id;";

                var cmd = new System.Data.SqlClient.SqlCommand(updateQuery, con);
                cmd.Parameters.Add("@LoadedQty", SqlDbType.Int).Value = newQty;
                cmd.Parameters.Add("@Duration", SqlDbType.Float).Value = duration;
                cmd.Parameters.Add("@EndDate", SqlDbType.BigInt).Value = eDate.Ticks;
                cmd.Parameters.Add("@DailyProd", SqlDbType.Int).Value = dailyProd;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id == 0 ? _bar.Id : id;

                if (_bar.ProductionQty > 0 && _bar.LoadedQty - _bar.ProductionQty == newQty)
                {
                    cmd.Parameters.Add("@ClosedOrd", SqlDbType.Bit).Value = true;
                }
                else
                {
                    cmd.Parameters.Add("@ClosedOrd", SqlDbType.Bit).Value = false;
                }
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void InsertFractionatedOrder(int newQty, int newMembers)
        {
            var loadingJob = new LoadingJob(false);

            var jobModel = new JobModel();
            var task = Central.TaskList.Where(x => x.Id == _bar.Id).FirstOrDefault();

            var duration = jobModel.CalculateJobDuration(cboLine.Text, newQty, _bar.QtyH, _bar.Department, newMembers);
            var startDate = !Store.Default.manualDate ? JobModel.GetLineNextDate(cboLine.Text, _bar.Department) : dtpStart.Value;

            var dailyProd = jobModel.CalculateDailyQty(cboLine.Text, _bar.QtyH, _bar.Department, newMembers, newQty);

            var newRowText = string.Empty;
            if (_bar.RowText.Contains('_'))
            {
                int.TryParse(_bar.RowText.Split('_')[1], out var newIndex);
                newRowText = _bar.RowText.Split('_')[0] + '_' + (newIndex + 1).ToString();

                var newTask = Central.TaskList.Where(x => x.Name == newRowText && x.Department == _bar.Department).FirstOrDefault();

                if (newTask != null)
                {
                    int.TryParse(newTask.Name.Split('_')[1], out var n);
                    newRowText = _bar.RowText.Split('_')[0] + '_' + (n + 1).ToString();
                }
            }
            else
            {
                newRowText = _bar.RowText + "_1";
            }

            loadingJob.InsertNewProgram(newRowText, cboLine.Text, _bar.Article, newQty,
                _bar.QtyH, startDate, duration, dailyProd,
                task.ArtPrice, _bar.Department, newMembers, dtpStart.Checked, false);
        }

        private void DeleteExsistingOrder()
        {
            using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
            {
                ctx.ExecuteCommand("delete from objects where id={0}", _bar.Id);
            }
        }

        private void GetLineNextDate()
        {
            var suggDate = JobModel.GetLineNextDate(cboLine.Text, _bar.Department);
            dtpStart.Value = suggDate;
        }

        private void UndoFraction()
        {
            var order = _bar.RowText.Split('_')[0];

            if (_bar.RowText.Split('_')[1] == "1")
            {
                var task = Central.TaskList.Where(x => x.Name == order &&
                    x.Department == _bar.Department).FirstOrDefault();

                UpdateExsistingOrder(_bar.LoadedQty + task.LoadedQty, task.Id, task.Aim, task.StartDate);
            }
            else
            {
                int.TryParse(_bar.RowText.Split('_')[1], out var index);

                var task = Central.TaskList.Where(x => x.Name == order + '_' + (index - 1).ToString() &&
                    x.Department == _bar.Department).FirstOrDefault();

                UpdateExsistingOrder(_bar.LoadedQty + task.LoadedQty, task.Id, task.Aim, task.StartDate);
            }

            DeleteExsistingOrder();

            this.DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region EventHandlers

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtQty.Text, out var qty);
            var maxQty = _bar.LoadedQty - _bar.ProductionQty;

            if (qty > maxQty)
            {
                txtQty.Text = maxQty.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtPersons.Text, out var qty);

            if (qty > _bar.Members)
            {
                txtPersons.Text = _bar.Members.ToString();
            }
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            if (FractionateOrders())
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLineNextDate();
        }

        private void btnResetSuggDate_Click(object sender, EventArgs e)
        {
            GetLineNextDate();
        }

        private void btnUndoFraction_Click(object sender, EventArgs e)
        {
            UndoFraction();
        }

        private void lblSave_MouseEnter(object sender, EventArgs e)
        {
            lblSave.BackColor = Color.Gainsboro;
        }

        private void lblSave_MouseLeave(object sender, EventArgs e)
        {
            lblSave.BackColor = Color.Transparent;
        }

        #endregion
    }
}
