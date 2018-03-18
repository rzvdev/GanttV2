using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
{
    public partial class FrmLineListChecker : Form
    {
        public FrmLineListChecker()
        {
            InitializeComponent();
        }
        private void FrmLineListChecker_Load(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var line in  (from line in ObjectModels.Tables.Lines
                                   orderby Convert.ToInt32(line.Line.Remove(0,5))
                                   select line).ToList())
            {
                if (WorkflowController.ListOfLinesSelected != null && 
                    WorkflowController.ListOfLinesSelected.Contains(line.Line))
                    checkedListBox1.Items.Add(line.Line,true);               
                else
                    checkedListBox1.Items.Add(line.Line, false);
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            WorkflowController.ListOfLinesSelected = new List<string>();
            
            int i;
            for (i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    WorkflowController.ListOfLinesSelected.Add(checkedListBox1.Items[i].ToString());
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
