using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Permissions;

namespace ganntproj1
    {
    public partial class Popup : Form
        {
        public int Y1;
        public int X1;
        public Rectangle WR;

        protected virtual CreateParams CreateParam
            {
            [SecurityPermissionAttribute(SecurityAction.InheritanceDemand,
                Flags = SecurityPermissionFlag.UnmanagedCode)]
            [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get;
            }

        private Control ownerControl;

        public string Order { get; set; }            
        public string Art { get; set; }
        public string Qty { get; set; }

        public string Stag { get; set; }
        public string Fin { get; set; }
        public string TotDays { get; set; }

        public string IniT { get; set; }
        public string IniC { get; set; }
        public string IniS { get; set; }

        public string ConT { get; set; }
        public string ConC { get; set; }
        public string ConS { get; set; }

        public string RddT { get; set; }
        public string RddC { get; set; }
        public string RddS { get; set; }

        public string gnT { get; set; }
        public string gnC { get; set; } 
        public string gnS { get; set; }

        public string Diff { get; set; }

        public string Dvc { get; set; }
        //public Color BackgroundColor { get; set; }

        public Popup(Control owner) : base()
            {       
            InitializeComponent();

            //this.DoubleBuffered(true);

            ownerControl = owner;
            }

        protected override void OnCreateControl()
            {
            lblCom.Text = Order;
            lblArt.Text = Art;
            lblCapi.Text = Qty;
            lblStag.Text = Stag;
            lblFin.Text = Fin;
            lblDiff.Text = Diff;

            lblIniT.Text = IniT;
            lblIniC.Text = IniC;
            lblIniS.Text = IniS;
            lblConT.Text = ConT;
            lblConC.Text = ConC;
            lblConS.Text = ConS;
            lblRddT.Text = RddT;
            lblRddC.Text = RddC;
            lblRddS.Text = RddS;

            lblGnTot.Text = TotDays;
            lblGnTotT.Text = gnT;
            lblGnTotC.Text = gnC;
            lblGnTotS.Text = gnS;

            lblDvcfin.Text = Dvc;

            //var newX = 0;
            //var newY = 0;
            //newX = (ParentForm.Width / 2 - Width / 2);
            //newY = (ParentForm.Height / 2 - Height / 2);

            //Location = new Point(newX, newY);
            BackColor = SystemColors.Control;

            StartPosition = FormStartPosition.CenterScreen;
            //ShowInTaskbar = false;
            BringToFront();

            base.OnCreateControl();
            }

        public Control OwnerControl
            {
            get
                {
                return (ownerControl as Control);
                }
            set
                {
                ownerControl = value;
                }
            }

        protected override void OnLoad(EventArgs e)
            {
            //X1 = MousePosition.X;
            //Y1 = MousePosition.Y;
            //WR = Screen.GetWorkingArea(ownerControl);
            
            ////put color here if it's necessary
            ////-->

            //var rightFromCursor = ownerControl.Left + X1 + Width;    //get right side
            //var bottomFromCursor = ownerControl.Bottom + Y1 + Height; //get bottom

            //var x = MousePosition.X + 2;
            //var y = MousePosition.Y + 2;

            ////Tests to see if control will go over owner's right side

            //if (rightFromCursor > WR.Width)
            //    {
            //    x = X1 - Width - 2;
            //    y = Y1 - Height - 10;
            //    }

            ////Tests to see if control will go over owner's down side

            //if (bottomFromCursor > WR.Height)
            //    {
            //    x = X1 - Width - 2;
            //    y = Y1 - Height - 10;
            //    }
            
            base.OnLoad(e);
            }

        protected override void OnLostFocus(EventArgs e)
            {
            Close();
            base.OnLostFocus(e);
            }

        private void label1_Click(object sender, EventArgs e)
            {
            //Dispose();
            Close();
            }
        }
    }
