using System;
using System.Windows.Forms;

namespace ganntproj1.src.Views
{
    public partial class PinInput : Form
    {
        public PinInput()
        {
            InitializeComponent();
            
        }
        
        protected override void OnLoad(EventArgs e)
        {
            PinCorrect = false;

            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPin.Text != "8452")
            {
                label1.Visible = true;
                PinCorrect = false;
                txtPin.Clear();
                return;
            }
            else
            {
                button1.DialogResult = DialogResult.OK;
                PinCorrect = true;
                Close();
            }
        }

        public bool PinCorrect { get; set; }

        private void txtPin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
