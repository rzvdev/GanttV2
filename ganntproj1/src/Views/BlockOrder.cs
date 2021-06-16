using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ganntproj1.src.Views
{
    public partial class BlockOrder : Form
    {
        private readonly Bar _bar;

        public BlockOrder()
        {
            InitializeComponent();
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnResetSuggDate, "Reset to suggested date and time");
        }

        public BlockOrder(Bar bar)
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
                lblTotQty.Text = _bar.LoadedQty.ToString();
                lblMaxQty.Text = "Max: " + _bar.LoadedQty.ToString();
                lblMaxMembers.Text = "Max: " + _bar.Members.ToString();
                txtPersons.Text = _bar.Members.ToString();
            }

            txtQty.Text = "0";
            dtpStart.ShowCheckBox = Store.Default.manualDate;
            dtpStart.Enabled = Store.Default.manualDate;
            txtPersons.Enabled = Store.Default.manualMembers;

            LoadLines();

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

        private bool FractionateOrder()
        {
            int.TryParse(txtQty.Text, out var newQty);
            int.TryParse(txtPersons.Text, out var newMembers);

            if (newQty <= 0 || newMembers <= 0)
            {
                MessageBox.Show("Invalid qty or members input.");
                return false;
            }

            if (newQty < 0 || newQty > _bar.LoadedQty)
            {
                MessageBox.Show("Value must be between 0 and maximum qty by 'commessa'");
                return false;
            }


            // Delete exsisting order only in case when inserted qty is equals original qty

            if (newQty.Equals(_bar.LoadedQty))
            {
                var dr = MessageBox.Show("You have added the same qty as qty from selected order.\n" +
                    "Are you sure you want to reprogramm and delete selected order?", "Fractionate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    DeleteExsistingOrder();
                }
                else
                {
                    return false;
                }
            }

            // Update exsisting order only in case when inserted qty is less then original qty

            if (newQty < _bar.LoadedQty)
            {
                UpdateExsistingOrder(newQty);
            }

            InsertFractionatedOrder(newQty, newMembers);

            var queryOn = $"{_bar.RowText}-{cboLine.Text}-{_bar.Department}";
            Config.InsertOperationLog("manual_fractioning", queryOn, "fractionate");

            var c = new Central();
            c.GetBase();

            return true;
        }

        private void UpdateExsistingOrder(int newQty)
        {
            var jobModel = new JobModel();
            var task = Central.TaskList.Where(x => x.Name == _bar.RowText && 
                x.Aim == _bar.Tag && _bar.Department == _bar.Department && x.Idx == _bar.Idx).FirstOrDefault();

            var duration = jobModel.CalculateJobDuration(_bar.Tag, _bar.LoadedQty - newQty, _bar.QtyH, _bar.Department, _bar.Members);
            var dailyProd = jobModel.CalculateDailyQty(_bar.Tag, _bar.QtyH, _bar.Department, _bar.Members, newQty);
            var endDate = _bar.FromTime.AddDays(+duration);

            if (_bar.ProductionQty > 0)
            {
                dailyProd = _bar.DailyQty;
            }

            using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var updateQuery = @"
update objects set loadedQty=@LoadedQty, duration=@Duration, endDate=@EndDate, dailyProd=@DailyProd
where Id=@Id;";

                var cmd = new System.Data.SqlClient.SqlCommand(updateQuery, con);
                cmd.Parameters.Add("@LoadedQty", SqlDbType.Int).Value = _bar.LoadedQty - newQty;
                cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = duration;
                cmd.Parameters.Add("@EndDate", SqlDbType.BigInt).Value = endDate.Ticks;
                cmd.Parameters.Add("@DailyProd", SqlDbType.Int).Value = dailyProd;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = task.Id;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void DeleteExsistingOrder()
        {
            using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var deleteQuery = $"delete from objects where Id={_bar.Id}";
                var cmd = new System.Data.SqlClient.SqlCommand(deleteQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void InsertFractionatedOrder(int newQty, int newMembers)
        {
            var loadingJob = new LoadingJob(false);
           
            var jobModel = new JobModel();
            var task = Central.TaskList.Where(x => x.Name == _bar.RowText && 
                x.Aim == _bar.Tag && _bar.Department == _bar.Department && x.Idx == _bar.Idx).FirstOrDefault();

            var duration = jobModel.CalculateJobDuration(cboLine.Text, newQty, _bar.QtyH, _bar.Department, _bar.Members);
            var startDate = JobModel.GetLineNextDate(cboLine.Text, _bar.Department);
            var endDate = startDate.AddDays(+duration);
            var idx = jobModel.GetObjectNextIndex(_bar.RowText, cboLine.Text, _bar.Department);
            var dailyProd = jobModel.CalculateDailyQty(_bar.Tag, _bar.QtyH, _bar.Department, newMembers, newQty);

            loadingJob.InsertNewProgram(_bar.RowText, cboLine.Text, _bar.Article, newQty, 
                _bar.QtyH, startDate, duration, dailyProd, 
                task.ArtPrice, _bar.Department, newMembers, dtpStart.Checked, false, idx);
        }

        private void GetLineNextDate()
        {
            var suggDate = JobModel.GetLineNextDate(cboLine.Text, _bar.Department);
            dtpStart.Value = suggDate;
        }

        #endregion

        #region EventHandlers

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtQty.Text, out var qty);

            if (qty > _bar.LoadedQty)
            {
                txtQty.Text = _bar.LoadedQty.ToString();
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
            if (FractionateOrder())
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

        #endregion
    }
}
