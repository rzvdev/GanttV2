using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class SummaryReport : Form
        {
        public SummaryReport()
            {
            InitializeComponent();

            typeof(SummaryReport).InvokeMember("DoubleBuffered",
              BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this,
              new object[] { true });
            }

        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private int Filter { get; set; }

        private void LoadReport()
            {
            StartDate = dtpDateFrom.Value;
            EndDate = dtpDateTo.Value;

            var tmpTable = Output.ProcessingTable.Clone();
            tmpTable.Merge(Output.ProcessingTable);

            tmpTable.AcceptChanges();

            for (var c = 5; c <= dgvReport.Columns.Count - 2; c++)
                {
                dgvReport.Columns[c].Visible = false;
                }

            foreach (DataRow row in tmpTable.Rows)
                {
                var rdd = new DateTime();

                if (string.IsNullOrEmpty(row[16].ToString())) continue;

                if (Filter == 1)
                    {
                    rdd = Convert.ToDateTime(row[16]);
                    }

                if (rbData.Checked)
                    {
                    if (rdd <= StartDate && rdd >= EndDate)
                        {
                        row.Delete();
                        }
                    }
                else if (rbAnticipi.Checked)
                    {
                    if (rdd >= EndDate)
                        {
                        row.Delete();
                        }
                    }
                else if (rbRitardi.Checked)
                    {
                    if (rdd <= EndDate)
                        {
                        row.Delete();
                        }
                    }
                }

            tmpTable.AcceptChanges();

            var bs = new BindingSource();
            bs.DataSource = tmpTable;
            dgvReport.DataSource = bs;

            if (Filter == 1)
                {
                dgvReport.Columns[5].Visible = true;
                dgvReport.Columns[6].Visible = true;
                dgvReport.Columns[7].Visible = true;
                dgvReport.Columns[10].Visible = true;
                dgvReport.Columns[12].Visible = true;
                dgvReport.Columns[14].Visible = true;
                dgvReport.Columns[15].Visible = true;
                dgvReport.Columns[16].Visible = true;

                IntegrateTotalFields();
                }
            else if (Filter == 2)
                {
                //do not
                }
            else if (Filter == 3)
                {
                //do not
                }
            }

        private void Form1_Load(object sender, EventArgs e)
            {
            Filter = 1;
            LoadReport();
            }

        private void btnReload_Click(object sender, EventArgs e)
            {
            LoadReport();
            }

        private Label _txt = new Label();
        private void IntegrateTotalFields()
            {
            for (int i = pnFields.Controls.Count - 1; i >= 0; i--)
                {
                if ((pnFields.Controls[i] is Label label) && (label.Name != "XXXXXXX"))
                    {
                    pnFields.Controls.RemoveAt(i);
                    label.Dispose();
                    }
                }

            var articleTotal = 0;
            var totalSum = dgvReport.ColumnCount - 2;

            for (var i = 0; i <= dgvReport.Columns.Count - 1; i++)
                {
                _txt = new Label
                    {
                    Name = dgvReport.Columns[i].Name,
                    Parent = pnFields,
                    //ReadOnly = true,
                    BackColor = Color.White,
                    ForeColor = Color.Crimson,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                    BorderStyle = BorderStyle.None,
                    Visible = true,
                    TabStop = false,
                    TabIndex = TabIndex + 1,
                    Text = "0"
                    };

                if (i == 0)
                    {
                    _txt.Text = "Totali";
                    }
                else if (i == 1)
                    {
                    var distinctRows = (from DataGridViewRow row in dgvReport.Rows
                                        select row.Cells[1]
                   ).Distinct().Count();

                    _txt.Text = distinctRows.ToString();
                    articleTotal = distinctRows;
                    }
                else if (i == 3)
                    {
                    int sumtot = 0;

                    var rows = from row in dgvReport.Rows.Cast<DataGridViewRow>()
                               where row.Cells[3].Value != DBNull.Value
                               select row;

                    rows.ToList().ForEach(row =>
                    {
                        sumtot += Convert.ToInt32(row.Cells[3].Value);
                    });

                    _txt.Text = sumtot.ToString();
                    }
                else if (i == 10)
                    {
                    double sumtot = 0;

                    var rows = from row in dgvReport.Rows.Cast<DataGridViewRow>()
                               where row.Cells[10].Value != DBNull.Value
                               select row;

                    rows.ToList().ForEach(row =>
                    {

                        sumtot += Convert.ToInt32(row.Cells[10].Value);
                    });

                    _txt.Text = Math.Round(sumtot / totalSum, 2).ToString();
                    }
                else if (i == 12)
                    {
                    double sumtot = 0;

                    var rows = from row in dgvReport.Rows.Cast<DataGridViewRow>()
                               where row.Cells[12].Value != DBNull.Value
                               select row;

                    rows.ToList().ForEach(row =>
                    {
                        sumtot += Convert.ToInt32(row.Cells[12].Value);
                    });

                    _txt.Text = Math.Round(sumtot / totalSum, 2).ToString();
                    }

                PlaceField(dgvReport, _txt, i);

                pnFields.Controls.Add(_txt);
                }
            }

        private void btn_Tess_Click(object sender, EventArgs e)
            {

            }

        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
            {
            var dgv = (DataGridView)sender;
            IntegrateTotalFields();
            }

        private void PlaceField(DataGridView dgv, Control c, int index)
            {
            var headerRect = dgv.GetColumnDisplayRectangle(index, true);
            c.Location = new Point(headerRect.Location.X, 35 - c.Height - 3);
            c.Size = new Size(headerRect.Width, dgv.ColumnHeadersHeight);

            dgv.Invalidate();
            }
        }
    }
