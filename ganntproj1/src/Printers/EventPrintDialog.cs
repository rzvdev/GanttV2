namespace ganntproj1
{
    using System;
    using System.Management;

    /// <summary>
    /// Defines the <see cref="EventPrintDialog" />
    /// </summary>
    public class EventPrintDialog
    {
        /// <summary>
        /// Gets or sets the Formater
        /// </summary>
        public System.Drawing.Printing.PaperKind Formater { get; set; }

        /// <summary>
        /// Gets or sets the DocumentTitle
        /// Gets or sets main document title, defined by user choise.
        /// </summary>
        public string DocumentTitle { get; set; } = "Test document";

        /// <summary>
        /// Gets or sets the Notification
        /// Gets or sets optional user note, that references to the document.
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// Gets or sets the Dedication
        /// Gets or sets dedication description inserted by user.
        /// </summary>
        public string Dedication { get; set; } = "Demo 1.0";

        /// <summary>
        /// Gets or sets the PrintDate
        /// Gets or sets current date, that is the print date.
        /// </summary>
        public string PrintDate { get; set; } = DateTime.Now.ToLongDateString();

        /// <summary>
        /// Gets or sets the Printer
        /// Gets or sets local printer that will be used to print document.
        /// </summary>
        public string Printer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Printing
        /// Gets or sets dialog result;
        /// </summary>
        public bool Printing { get; set; }

        /// <summary>
        /// The LoadPrintersList
        /// </summary>
        /// <param name="cbo">The cbo<see cref="System.Windows.Forms.ComboBox"/></param>
        private void LoadPrintersList(System.Windows.Forms.ComboBox cbo)
        {
            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

            //list my installed printers to list view
            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = printer.GetPropertyValue("Network");

                string[] row = { name.ToString(), status.ToString(), isDefault.ToString(), isNetworkPrinter.ToString() };
                var listViewItem = new System.Windows.Forms.ListViewItem(row);
                cbo.Items.Add(listViewItem.Text);
            }
        }

        /// <summary>
        /// The Show
        /// </summary>
        public void Show()
        {
            Printing = false;
            var startX = 30;
            var startY = 40;

            var f = new System.Windows.Forms.Form
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                ControlBox = false,
                WindowState = System.Windows.Forms.FormWindowState.Normal,
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen,
                Size = new System.Drawing.Size(300, 400),
                Text = "Gantt print dialog",
                ShowIcon = false
            };

            f.Load += PrintDialog_Load;
            f.Paint += PrintDialog_Paint;
            f.LostFocus += PrintDialog_LostFocus;

            var intA = 0;

            //title label
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            lbl.Text = "Chose some format to print focused content";
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(startX, startY);

            f.Controls.Add(lbl);

            System.Windows.Forms.RadioButton radio;
            System.Windows.Forms.Label txtTit = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox(); ;
            System.Windows.Forms.Label lblDedicat = new System.Windows.Forms.Label();
            System.Windows.Forms.Button btnPrint = new System.Windows.Forms.Button();
            System.Windows.Forms.Button btnClose = new System.Windows.Forms.Button();
            System.Windows.Forms.Label cboTit = new System.Windows.Forms.Label();
            System.Windows.Forms.ComboBox cboPrinters = new System.Windows.Forms.ComboBox();

            var radioTop = 60;

            //create radio formats

            for (intA = 2; intA <= 4; intA++)
            {
                radio = new System.Windows.Forms.RadioButton
                {
                    Text = "A" + intA.ToString(),
                    Location = new System.Drawing.Point(startX, radioTop),
                    AutoSize = true
                };

                //change format enumareation on check even
                radio.CheckedChanged += null;

                radio.CheckedChanged += RadioFormat_Check;

                radioTop += 20;

                f.Controls.Add(radio);
            }

            txtTit.Text = "Description (Optional)";
            txtTit.Location = new System.Drawing.Point(startX, radioTop + 30);
            txtTit.AutoSize = true;
            f.Controls.Add(txtTit);

            txt.Location = new System.Drawing.Point(startX, radioTop + 50);
            txt.Width = 240;
            f.Controls.Add(txt);

            cboTit.Text = "Choose a printer";
            cboTit.Location = new System.Drawing.Point(startX, txt.Location.Y + 30);
            cboTit.AutoSize = true;
            f.Controls.Add(cboTit);

            cboPrinters.Location = new System.Drawing.Point(startX, cboTit.Location.Y + 20);
            cboPrinters.Width = 200;
            f.Controls.Add(cboPrinters);
            LoadPrintersList(cboPrinters);  //load printers to combo

            btnPrint.Text = "&Print";
            btnPrint.Location = new System.Drawing.Point(70, txtTit.Location.Y + 120);
            f.Controls.Add(btnPrint);
            btnPrint.DialogResult = System.Windows.Forms.DialogResult.Yes;  //accept only
            btnPrint.Click += delegate
                {
                    Printer = cboPrinters.Text; //get selected printer
                    Notification = txt.Text;    //get inserted text by user
                    Printing = true;
                    f.Close();
                };
            btnClose.Text = "&Close";
            btnClose.Location = new System.Drawing.Point(btnPrint.Location.X + btnPrint.Width + 10, btnPrint.Location.Y);
            btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            f.Controls.Add(btnClose);
            btnClose.Click += delegate
                {
                    Printing = false;
                    f.Close();
                };

            f.ShowDialog();
            f.Dispose();
        }

        /// <summary>
        /// The PrintDialog_Load
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="EventArgs"/></param>
        private void PrintDialog_Load(object sender, EventArgs eventArgs)
        {
        }

        /// <summary>
        /// The PrintDialog_LostFocus
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="EventArgs"/></param>
        private void PrintDialog_LostFocus(object sender, EventArgs eventArgs)
        {
            var d = (System.Windows.Forms.Form)sender;
            d.Close();
        }

        /// <summary>
        /// The RadioFormat_Check
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="EventArgs"/></param>
        private void RadioFormat_Check(object sender, EventArgs eventArgs)
        {
            var rb = sender as System.Windows.Forms.RadioButton;    //direct cast radios

            if (rb.Text == "A2")
            { Formater = System.Drawing.Printing.PaperKind.A2; }
            else if (rb.Text == "A3")
            { Formater = System.Drawing.Printing.PaperKind.A3; ; }
            else if (rb.Text == "A4")
            { Formater = System.Drawing.Printing.PaperKind.A4; }
        }

        /// <summary>
        /// The PrintDialog_Paint
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="System.Windows.Forms.PaintEventArgs"/></param>
        private void PrintDialog_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var lnb = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.PointF(0, 10), new System.Drawing.PointF(300, 10), color1: System.Drawing.Color.Gainsboro, color2: System.Drawing.Color.Silver);

            var pen = new System.Drawing.Pen(lnb, 5)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                DashCap = System.Drawing.Drawing2D.DashCap.Round
            };

            e.Graphics.DrawRectangle(pen, -2, -2, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
        }
    }
}
