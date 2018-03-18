using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class SplitHistory : Form
        {
        public SplitHistory()
            {
            InitializeComponent();
            }

        public bool IsFromSplit { get; set; }
        private void SplitHistory_Load(object sender, EventArgs e)
            {
            Text = Text + " (Commessa:" + WorkflowController.TargetOrder + ")";

            FormClosing += delegate
                 {
                     if (!IsFromSplit)
                         {
                         WorkflowController.TargetOrder = string.Empty;
                         WorkflowController.TargetLine = string.Empty;
                         }
                     };

            var splitQuery = (from split in ObjectModels.Tables.ProductionSplits
                              where split.Commessa == WorkflowController.TargetOrder
                              select split).ToList();

            listView1.Columns.Add("Line", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Qty", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("Start date", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("End  date", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Base", 50, HorizontalAlignment.Left);
            //listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Regular);

            foreach (var split in splitQuery)
                {
                var lst = new ListViewItem(new[] { split.Line, split.Qty.ToString(), "-", "-", "-" });
                
                listView1.Items.Add(lst);
                }
            }

        private void btnClose_Click(object sender, EventArgs e)
            {
            Close();
            }
        }
    }
