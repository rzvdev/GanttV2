using System;
using System.Windows.Forms;

namespace ganntproj1.Views
{
    public partial class OperationProgram : Form
    {
        public OperationProgram()
        {
            InitializeComponent();
        }

        public string Operation { get; set; }
        public int OperationId { get; set; }

        private void OperationProgram_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Operation = comboBox1.Text;

            if(comboBox1.SelectedIndex == 1)
            {
                OperationId = 313;
            }
            else
            {
                OperationId = 314;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
