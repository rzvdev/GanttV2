using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class MiniTitle : UserControl
        {
        public MiniTitle()
            {
            InitializeComponent();
            Dock = DockStyle.Top;
            SendToBack();
            Height = 40;
            }

        [Description("Text displayed as a sub-title.")]
        [Category("Data")]
        public string TitleText
            {
            get => lbl_Title.Text;
            set => lbl_Title.Text = value;
            }

        private void ScanproSubTitleBar_Load(object sender, EventArgs e)
            {
            BackColor = Color.FromArgb(250, 250, 250);

            lbl_Title.Text = TitleText;
            lbl_Title.Width = Parent.Width - 40;
            lbl_Title.BackColor = Color.White;
            lbl_Title.ForeColor = Color.DimGray;
            lbl_Title.TextAlign = ContentAlignment.MiddleLeft;
            }

        protected override void OnResize(EventArgs e)
            {
            if (Parent == null) return;

            lbl_Title.Width = Parent.Width - 40;
            base.OnResize(e);
            }
        }
    }