namespace ganntproj1
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="MyCheckBox" />
    /// </summary>
    public class MyCheckBox : CheckBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyCheckBox"/> class.
        /// </summary>
        public MyCheckBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            Padding = new Padding(6);
        }

        /// <summary>
        /// The OnPaint
        /// </summary>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            OnPaintBackground(e);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            var geo = new Geometry();

            var rectn = new Rectangle(0, 3, Width, Height - 6);

            using (var path = geo.RoundedRectanglePath(rectn, 9))
            {
                e.Graphics.FillPath(Checked ? Brushes.Silver : Brushes.LightGray, path);
            }

            int r = Height - 1;

            var rect = Checked ? new Rectangle(Width - r - 1, 0, r, r)
                           : new Rectangle(0, 0, r, r);

            var brs = new SolidBrush(Color.FromArgb(27, 98, 124));
            e.Graphics.FillEllipse(Checked ? brs : Brushes.Gray, rect);
        }
    }
}
