using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
{
    public partial class Carico : Form
    {
        private readonly Geometry _geometry = new Geometry();
        public string Commessa { get; set; }
        public string Id { get; set; }

        #region FormMovementService
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        public Carico()
        {
            InitializeComponent();
        }
        public Carico(string comm, string id)
        {
            Commessa = comm;
            Id = id;
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, Width, Height), 8))
            {               
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;               
            }

            base.OnPaint(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            pnHeader.MouseMove += (s, mv) =>
            {
                if (mv.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
            LoadCarico();
            LoadDistinte();
            dgvCarico.ClearSelection();
            dgvDistinta.ClearSelection();
            dgvCarico.SelectionChanged += delegate
            {
                dgvCarico.ClearSelection();
            };

            dgvDistinta.SelectionChanged += delegate
            {
                dgvDistinta.ClearSelection();
            };
            lblComTxt.Text = Commessa;
            base.OnLoad(e);
        }
        private void LoadCarico()
        {
            var dt = new DataTable();
            dt.Columns.Add("Carico");
            dt.Columns.Add("Date");
            var query = "select value,date from Carico where commessaId='" + Id + "'";
            using (var c = new System.Data.SqlClient.SqlConnection(Central.ConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(query, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DataRow row = dt.NewRow();
                        row[0] = dr[0].ToString();
                        row[1] = dr[1].ToString();
                        dt.Rows.Add(row);
                    }
                }
                c.Close();
            }
            dgvCarico.DataSource = dt;
            foreach (DataGridViewColumn c in dgvDistinta.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (var i = 0; i <= dgvCarico.ColumnCount - 1; i++)
            {
                dgvCarico.Columns[i].Width = 150;
            }
        }
        private void LoadDistinte()
        {
            var query = "select Color,Total,0Y,1Y,2Y,XX,XS,S,M,L,XL,EL,XXL,XXXL from Distinta where CommessaId='" + Id + "'";                
            var dt = new DataTable();           
            using (var c = new System.Data.SqlClient.SqlConnection(Central.ConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(query, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                c.Close();
                dr.Close();
            }
            dgvDistinta.DataSource = dt;
            foreach (DataGridViewColumn c in dgvDistinta.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;                
            }
                
            dgvDistinta.Columns[0].Width = 150;
            dgvDistinta.Columns[1].Width = 70;
            dgvDistinta.Columns[2].HeaderText = @"Y0";

            for (var i = 2; i <= dgvDistinta.ColumnCount - 1; i++)
            {
                dgvDistinta.Columns[i].Width = 30;
            }
        }
        private void LblClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LblCommTit_Paint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;

            var pen = new Pen(new SolidBrush(Color.White), 1);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 5))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                lbl.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }
    }
}
