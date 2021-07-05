using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class MyMessage : Form
        {
        private System.Timers.Timer _tmCloser = new System.Timers.Timer();
        private readonly Geometry _geometry = new Geometry();

        public string Title { get; set; }
        public string Message { get; set; }

        public MyMessage()
            {
            InitializeComponent();
            }

        public MyMessage(string title, string message)
            {
            Title = new string (' ', 3) + title;
            Message = message;
            InitializeComponent();
            }

        private void FrmMyInfo_Load(object sender, EventArgs e)
            {
            lblTitle.Text = Title;
            lblInfo.Text = Message;

            pbRobotic.Image = MessageIcon;

            _tmCloser = new System.Timers.Timer();
            _tmCloser.Elapsed += _tmCloser_Elapsed;
            _tmCloser.Interval = 5000;
            _tmCloser.Enabled = true;
            }
        
        private void _tmCloser_Elapsed(object s, System.Timers.ElapsedEventArgs e)
            {
            Invoke((MethodInvoker)delegate
                {
                    Close();
                    });
            }

        protected override void OnPaint(PaintEventArgs e)
            {
            var pen = new Pen(new SolidBrush(Color.LightGray), 1);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, Width, Height), 5))
                {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Region = new Region(path);
                e.Graphics.SmoothingMode = old;
                }

            base.OnPaint(e);
            }

        protected override void OnLostFocus(EventArgs e)
            {
            Close();
            base.OnLostFocus(e);
            }
        protected override void OnClosing(CancelEventArgs e)
            {
            if (_tmCloser != null) _tmCloser.Dispose();
            base.OnClosing(e);
            }
        
        private void lblInfo_Click_1(object sender, EventArgs e)
            {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));

            Color colorhtm = ColorTranslator.FromHtml(color);

            lblInfo.BackColor = colorhtm;
            }

        [Description("Display image")]
        [Category("Data")]
        public Image MessageIcon
            {
            get => pbRobotic.Image;
            set => pbRobotic.Image = value;
            }
        }
    }
