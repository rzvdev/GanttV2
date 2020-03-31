using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1
{
    public partial class LineList : Form
    {
        public LineList()
        {
            InitializeComponent();
        }
        private void FrmLineListChecker_Load(object sender, EventArgs e)
        {
            var dept = Store.Default.sectorId == 1 ? "Confezione B" : "Stiro";
 
            foreach (var line in (from line in Models.Tables.Lines
                                  where line.Department == dept
                                   orderby Convert.ToInt32(line.Line.Remove(0,5))
                                   select line).ToList())
            {
                if (Workflow.ListOfLinesSelected != null && 
                    Workflow.ListOfLinesSelected.Contains(line.Line))
                    checkedListBox1.Items.Add(line.Line,true);               
                else
                    checkedListBox1.Items.Add(line.Line, false);
            }
        }
        private void BtnAccept_Click(object sender, EventArgs e)
        {
            Workflow.ListOfLinesSelected = new List<string>();
            
            int i;
            for (i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    Workflow.ListOfLinesSelected.Add(checkedListBox1.Items[i].ToString());
                }
            }

            Close();
        }
        private void BtnDiscard_Click(object sender, EventArgs e)
        {            
            Close();
        }
    }
}
