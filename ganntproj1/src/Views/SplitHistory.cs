using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class SplitHistory : Form
        {
        public string Order { get; set; }
        public string Dept { get; set; }

        public SplitHistory(string order, string dept)
            {
            Order = order;
            Dept = dept;
            InitializeComponent();
            }

        public bool IsFromSplit { get; set; }
        private void SplitHistory_Load(object sender, EventArgs e)
            {
            Text = Text + " (Commessa:" + Workflow.TargetOrder + ")";

            FormClosing += delegate
                 {
                     if (!IsFromSplit)
                         {
                         Workflow.TargetOrder = string.Empty;
                         Workflow.TargetLine = string.Empty;
                         Workflow.TargetDepartment = string.Empty;
                         }
                     };

            var splitQuery = (from split in Central.TaskList
                              where split.Name == Order || split.Name == Order+".1" && split.Department == Dept
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
                var lst = new ListViewItem(new[] { split.Aim, split.LoadedQty.ToString(), 
                    split.StartDate.ToString("dd/MM/yyyy"), 
                    split.EndDate.ToString("dd/MM/yyyy"), 
                    split.IsBase.ToString() });
                
                listView1.Items.Add(lst);
                }
            }

        private void btnClose_Click(object sender, EventArgs e)
            {
            Close();
            }
        }
    }
