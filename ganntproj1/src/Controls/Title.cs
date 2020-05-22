using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class Title : UserControl
        {
        public Title()
            {
            InitializeComponent();

            //setup
            Dock = DockStyle.Top;
            SendToBack();
            Height = 70;
            }

        /// <summary>
        ///     Property that can be found in 'Property grid'
        /// </summary>
        [Description("Text displayed as a title.")]
        [Category("Data")]
        public string TitleText
            {
            get => lbl_Title.Text;
            set => lbl_Title.Text = value;
            }

        /// <summary>
        ///     Property that can be found in 'Property grid'
        /// </summary>
        [Description("Title back color.")]
        [Category("Data")]
        public Color TitleBackColor
            {
            get => lbl_Title.BackColor;
            set => lbl_Title.BackColor = value;
            }

        private void ScanproTitleBar_Load(object sender, EventArgs e)
            {
            //title appereance

            //lbl_Title.BackColor = Color.DimGray;
            lbl_Title.ForeColor = Color.Orange;
            lbl_Title.Text = TitleText;
            lbl_Title.TextAlign = ContentAlignment.MiddleLeft; //software default
            }

        protected override void OnPaint(PaintEventArgs e)
        {
           
        }
    }
    }