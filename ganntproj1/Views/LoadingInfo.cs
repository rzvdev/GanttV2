using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
{
    internal class LoadingInfo : Form
    {
        private static LoadingInfo _formBlock;
        private static Label _lbl;
        private readonly System.Windows.Forms.Timer _tmDots = new System.Windows.Forms.Timer();
        private static ProgressBar _loadingProgressBar = new ProgressBar();
        public static string InfoText { get; set; }
        private static int ProgressMax => 0;
        private static Thread _threadB;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public LoadingInfo()
        {
        }
        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        public static void ShowLoading()
        {
            _threadB = new Thread(delegate ()
            {
                _formBlock = new LoadingInfo
                {
                    FormBorderStyle = FormBorderStyle.None,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    WindowState = FormWindowState.Normal,
                    ShowIcon = false,
                    ControlBox = false,
                    ShowInTaskbar = false,
                    Size = new Size(500, 50),
                    StartPosition = FormStartPosition.CenterScreen,
                    //Location = new Point(_formBlock.Parent.Right - _formBlock.Width - 10, _formBlock.Parent.Bottom - _formBlock.Height - 10),
                };
                _formBlock.DoubleBuffered(true);
                _formBlock.ShowDialog();
                _formBlock.Dispose();
            });
            _threadB.SetApartmentState(ApartmentState.STA);
            _threadB.IsBackground = true;
            _threadB.Start();
        } 
        public static void CloseLoading()
        {
            if (_formBlock == null) return;
            _formBlock.Dispose();
        }
        //private readonly System.Windows.Forms.Timer _tm = new System.Windows.Forms.Timer();
        protected override void OnLoad(EventArgs e)
        {
            _lbl = new Label
            {
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                BorderStyle = BorderStyle.None,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.AliceBlue,
                AutoSize=true
            };
            _lbl.MouseMove += (s, mv) =>
            {
                if (mv.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                _lbl.Invalidate();
            };
            _lbl.Paint += (sender,args) => 
            {
                var geo = new Geometry();
                args.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = geo.RoundedRectanglePath(new Rectangle(-1, -1, _lbl.Width, _lbl.Height), 10))
                {
                    args.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    args.Graphics.DrawPath(new Pen(new SolidBrush(Color.SeaGreen), 6), geo.RoundedRectanglePath(new Rectangle(-1, -1, _lbl.Width, _lbl.Height), 10));
                    Region = new Region(path);
                    args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
                args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            };
            _lbl.Dock = DockStyle.Fill;
            _formBlock.Controls.Add(_lbl);
            Task.Factory.StartNew(() => { UpdateWaiting(); });
            base.OnLoad(e);
        }
        private void UpdateWaiting()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    UpdateProgress();
                    UpdateText(InfoText);
                }));
            }
            else
            {
                UpdateProgress();
                UpdateText(InfoText);
            }
        }
        public static void UpdateProgress()
        {
            _loadingProgressBar.Maximum = 211;

            if (_loadingProgressBar.InvokeRequired)
            {
                _loadingProgressBar.BeginInvoke((MethodInvoker)delegate { _loadingProgressBar.Value = _loadingProgressBar.Value + 1; });
            }
            else
            {
                _loadingProgressBar.Value = _loadingProgressBar.Value + 1;
            }
            _loadingProgressBar.Refresh();
            _loadingProgressBar.Update();
        }
        public static void ResetProgress()
        {
            if (_loadingProgressBar.InvokeRequired)
            {
                _loadingProgressBar.BeginInvoke((MethodInvoker)delegate { _loadingProgressBar.Value = 0; });
            }
            else
            {
                _loadingProgressBar.Value = 0;
            }

            _loadingProgressBar.Refresh();
            _loadingProgressBar.Update();
        }
        public static void UpdateText(string txt)
        {
            //_formBlock.Size = new Size(500, 50);
            //_formBlock.Refresh();
            _lbl.Text = new string(' ', 5) + txt;
            _lbl.Refresh();
            _formBlock.Width = _lbl.Width + 50;
            _formBlock.Height = _lbl.Height + 50;
            _formBlock.Refresh();
            _formBlock.CenterToScreen();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _tmDots.Dispose();
            base.OnFormClosing(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString("ONLYOU", new Font("Tahoma", 8, FontStyle.Bold), Brushes.Orange, 2, 2);
            base.OnPaint(e);
        }       
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var geo = new Geometry();
            using (GraphicsPath path = geo.RoundedRectanglePath(new Rectangle(-1, -1, Width - 1, Height-1), 10))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }

            base.OnPaintBackground(e);
        }
    }
}
