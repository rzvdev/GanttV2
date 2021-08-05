namespace ganntproj1
{
    using ganntproj1.Properties;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Linq;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Config
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan StartShift { get; set; }

        public TimeSpan EndShift { get; set; }

        public static List<string> FileColumns { get; set; }

        public string GlobalDir { get; set; } = @"C:\GanttOutcomes";
        private SqlConnection _sql_conn1;
        private static DataContext _ganttConn;
        private static DataContext _olyConn;
        public SqlConnection Get_sql_conn()
        {
            return _sql_conn1;
        }
        public void Set_sql_conn(SqlConnection value)
        {
            _sql_conn1 = value;
        }

        public static DataContext GetGanttConn()
        {
            return _ganttConn;
        }

        public static void SetGanttConn(DataContext value)
        {
            _ganttConn = value;
        }

        public static DataContext GetOlyConn()
        {
            return _olyConn;
        }

        public static void SetOlyConn(DataContext value)
        {
            _olyConn = value;
        }

        public static void InsertOperationLog(string operation, string query, string program)
        {
            var username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var pcname = System.Environment.MachineName;

            using (var context = new System.Data.Linq.DataContext(Central.SpecialConnStr))
            {
                context.ExecuteCommand("insert into log (id,username,computername,logdate,operation,queryon,program)" +
                    " values ({0},{1},{2},{3},{4},{5},{6})", Guid.NewGuid(), username, pcname, DateTime.Now.Subtract(MinimalDate).Ticks, operation, query, program);
            }
            
        }

        public string ReadSqlConnectionString(int conIdx)
        {
            var strConn = "";

            if (conIdx == 1)
            {
                strConn = ConfigurationManager.ConnectionStrings["Ganttproj"].ConnectionString;
            }
            else if (conIdx == 2)
            {
                strConn = ConfigurationManager.ConnectionStrings["ONLYOU"].ConnectionString;
            }

            return strConn;
        }

        public static DateTime MinimalDate = new DateTime(2015, 1, 1, 0, 0, 0, 0);

        public void CreateOutcomeDir()
        {
            try
            {
                if (!Directory.Exists(GlobalDir))
                {
                    var di = Directory.CreateDirectory(GlobalDir);

                    Store.Default.globalDir = GlobalDir;
                    Store.Default.Save();

                    Console.WriteLine("The directory was created successfully at {0}.",
                        Directory.GetCreationTime(GlobalDir));
                }
                else
                {
                    if (string.IsNullOrEmpty(Store.Default.globalDir))
                    {
                        Store.Default.globalDir = GlobalDir;
                        Store.Default.Save();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Directory process failed: {0}", e);
            }

            string path = GlobalDir + "\\output.csv";

            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);

                    Console.WriteLine("File successfully created.");
                    Store.Default.targetFile = path;
                    Store.Default.Save();
                }
                else
                {
                    Console.WriteLine("File confirmed.");
                    if (string.IsNullOrEmpty(Store.Default.targetFile))
                    {
                        Store.Default.targetFile = path;
                        Store.Default.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Target file process failed: {0}", ex);
            }
        }

        public static void DoContextAction(string command, object[] param, int target)
        {
        }

        public static int ReturnAssemblyNumber(string assemblyText)
        {
            var sb = new System.Text.StringBuilder();
            foreach (char c in assemblyText)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }
            int.TryParse(sb.ToString(), out var assembluNumber);
            return assembluNumber;
        }

        public void ExcelToCSVConversion(string sourceFile, string worksheetName, string targetFile)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + @";Extended Properties=""Excel 12.0 Xml;HDR=YES""";
            OleDbConnection connection = null;
            StreamWriter writer = null;
            OleDbCommand command = null;
            OleDbDataAdapter dataAdapter = null;

            try
            {
                if (Store.Default.outputDir == string.Empty) return;

                connection = new OleDbConnection(connectionString);
                connection.Open();

                command = new OleDbCommand("SELECT * FROM [" + worksheetName + "$]", connection)
                {
                    CommandType = CommandType.TableDirect,
                };

                Output.ProcessingTable = new DataTable("ganttOutFile");
                Output.ProcessingTable.Clear();

                writer = new StreamWriter(targetFile);

                dataAdapter = new OleDbDataAdapter(command);
                dataAdapter.Fill(Output.ProcessingTable);
                dataAdapter.Dispose();

                Output.ProcessingTable.Rows.Remove(Output.ProcessingTable.Rows[0]);
                Output.ProcessingTable.Rows.Remove(Output.ProcessingTable.Rows[1]);
                Output.ProcessingTable.Rows.Remove(Output.ProcessingTable.Rows[2]);

                Output.ProcessingTable.AcceptChanges();

                foreach (DataRow row in Output.ProcessingTable.Rows)
                {
                    if (string.IsNullOrEmpty(row.ItemArray.GetValue(0).ToString())) row.Delete();
                }

                Output.ProcessingTable.AcceptChanges();

                for (int row = 0; row < Output.ProcessingTable.Rows.Count; row++)
                {
                    var tmpStr = string.Empty;

                    for (int column = 0; column < Output.ProcessingTable.Columns.Count; column++)
                    {
                        var strToCollect = Output.ProcessingTable.Rows[row][column];

                        var typeOfStr = strToCollect.GetType();

                        if (typeOfStr == typeof(DateTime))
                            strToCollect = Convert.ToDateTime(Output.ProcessingTable.Rows[row][column]).ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);

                        tmpStr += strToCollect + ",";
                    }

                    writer.WriteLine(tmpStr);
                }
                Console.WriteLine();
                Console.WriteLine("The XLS file " + sourceFile + " has been converted to CSV " +
                                  "into " + targetFile + ".");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.ReadLine();
                Console.WriteLine("File may be corrupted or already running.");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
                writer.Close();
                writer.Dispose();

                Output.ProcessingTable.AcceptChanges();

                Output g = new Output();
                var columns = g.FileColumns;

                for (var c = 0; c <= Output.ProcessingTable.Columns.Count - 6; c++)
                {
                    Output.ProcessingTable.Columns[c].ColumnName = columns[c].ToString();
                }
            }

            Output.ProcessingTable.AcceptChanges();
        }

        public void PopulateTableUsingCsv(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                        Output.ProcessingTable.Columns.Add(header);
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        var newRow = Output.ProcessingTable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            newRow[i] = rows[i];
                        }
                        Output.ProcessingTable.Rows.Add(newRow);
                    }
                }
            }

            Output.ProcessingTable.AcceptChanges();

            Output g = new Output();
            var columns = g.FileColumns;

            for (var c = 0; c <= Output.ProcessingTable.Columns.Count - 6; c++)
            {
                Output.ProcessingTable.Columns[c].ColumnName = columns[c].ToString();
            }

            Output.ProcessingTable.AcceptChanges();
        }

        public void ExportTableToDba()
        {
            var cmd = new SqlCommand("delete from avanzamento", Get_sql_conn());
            Get_sql_conn().Open();
            cmd.ExecuteNonQuery();
            Get_sql_conn().Close();

            using (var sbc = new SqlBulkCopy(Get_sql_conn()))
            {
                sbc.DestinationTableName = "avanzamento";
                Get_sql_conn().Open();
                sbc.WriteToServer(Output.ProcessingTable);
                Get_sql_conn().Close();
                sbc.Close();
            }
        }

        public DateTime ConvertToUniTime(object obj)
        {
            DateTime.TryParse(obj.ToString(), out DateTime dtc);

            return dtc;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        internal class OpenDialog : Form
        {
            private Button _btnDiag, _btnNext;

            private Label _label, _lblWroksheetTitle, _lblTargetTitle;

            private TextBox _textBox, _txtWorksheet, _txtTarget;

            private LinkLabel _link;

            private readonly Config _config = new Config();

            public OpenDialog()
            {
                Width = 400;
                Height = 300;
                Text = "File dialog";
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MinimizeBox = false;
                MaximizeBox = false;
            }

            protected override void OnLoad(EventArgs e)
            {
                _config.CreateOutcomeDir();     

                _label = new Label
                {
                    Text = ".XLS resource file",
                    Width = 200,
                    Font = default(Font),
                    Location = new Point(20, 8)
                };
                Controls.Add(_label);

                _textBox = new TextBox
                {
                    Text = Store.Default.outputDir,
                    Width = 350,
                    Location = new Point(20, 32),
                    Font = default(Font),
                    ForeColor = default(Color),
                    BackColor = default(Color)
                };
                Controls.Add(_textBox);

                _btnDiag = new Button();
                _btnDiag.Size = new Size(25, _textBox.ClientSize.Height + 2);
                _btnDiag.Location = new Point(_textBox.ClientSize.Width - _btnDiag.Width, -1);
                _btnDiag.Image = Resources.open;
                _btnDiag.Cursor = Cursors.Default;
                _btnDiag.BackColor = default(Color);

                _btnDiag.Click += delegate
                {
                    var openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileAr = openFileDialog.FileName.Split('\\').Last();      
                        var validExtent = fileAr.Split('.')[1];  

                        if (validExtent != "xls")
                        {
                            DialogResult = DialogResult.None;
                            MessageBox.Show("File extension isn't valid. Please choose another file.");
                        }
                        else
                        {
                            _textBox.Text = openFileDialog.FileName;
                        }
                    }
                };
                _textBox.Controls.Add(_btnDiag);

                SendMessage(_textBox.Handle, 0xd3, (IntPtr)2, (IntPtr)(_btnDiag.Width << 16));

                _lblWroksheetTitle = new Label
                {
                    Text = ".XLS worksheet",
                    AutoSize = true,
                    Location = new Point(20, 60)
                };
                Controls.Add(_lblWroksheetTitle);

                _txtWorksheet = new TextBox
                {
                    Text = Store.Default.worksheetName,
                    Width = 200,
                    Location = new Point(20, 80),
                };
                Controls.Add(_txtWorksheet);

                _lblTargetTitle = new Label
                {
                    Text = ".CSV target resource",
                    AutoSize = true,
                    Location = new Point(20, 110)
                };
                Controls.Add(_lblTargetTitle);

                _txtTarget = new TextBox
                {
                    Text = Store.Default.targetFile,
                    Width = 300,
                    Location = new Point(20, 130)
                };
                Controls.Add(_txtTarget);

                _link = new LinkLabel
                {
                    Text = "Server configuration",
                    AutoSize = true,
                    Location = new Point(20, 170)
                };

                _link.Click += delegate
                    {
                        var serverConfig = new ServerConfiguration();
                        serverConfig.ShowDialog();
                        serverConfig.Dispose();
                    };

                Controls.Add(_link);

                _btnNext = new Button
                {
                    DialogResult = DialogResult.None,
                    Size = new Size(150, 25),
                    ImageAlign = ContentAlignment.MiddleLeft,
                    Image = Resources.export,
                    Location = new Point(115, 210),
                    Text = @"&Next",
                    UseMnemonic = true
                };

                _btnNext.Click += delegate
                    {
                        Store.Default.outputDir = _textBox.Text;
                        Store.Default.worksheetName = _txtWorksheet.Text;
                        Store.Default.targetFile = _txtTarget.Text;
                        Store.Default.Save();

                        if (Store.Default.outputDir == string.Empty)
                        {
                            MessageBox.Show("Please choose one file with .xls extention to extract.");
                            _btnNext.DialogResult = DialogResult.None;
                            return;
                        }

                        if (Store.Default.worksheetName == string.Empty)
                        {
                            MessageBox.Show("Please instert worksheet name from file that you want to extract.");
                            _btnNext.DialogResult = DialogResult.None;
                            return;
                        }

                        if (Store.Default.targetFile == string.Empty)
                        {
                            MessageBox.Show("Please choose or inster file that you want to be you extraction end-point.");
                            _btnNext.DialogResult = DialogResult.None;
                            return;
                        }

                        DialogResult = DialogResult.OK;
                        Close();
                    };

                Controls.Add(_btnNext);

                base.OnLoad(e);
            }
        }
    }

    public class Filter
    {
        public Filter()
        {
        }

        public Filter(string aim, string key)
        {
            SetFilterAim(aim);
            SetFilterKey(key);
        }

        private string filterAim;

        public string GetFilterAim()
        {
            return filterAim;
        }

        public void SetFilterAim(string value)
        {
            filterAim = value;
        }

        private string filterKey;

        public string GetFilterKey()
        {
            return filterKey;
        }

        public void SetFilterKey(string value)
        {
            filterKey = value;
        }

        private static string filteredKey;

        public static string GetFilteredKey()
        {
            return filteredKey;
        }

        public static void SetFilteredKey(string value)
        {
            filteredKey = value;
        }

        private static string filteredChannel;

        public static string GetFilteredChannel()
        {
            return filteredChannel;
        }

        public static void SetFilteredChannel(string value)
        {
            filteredChannel = value;
        }

        private static List<string> filteredKeys;

        public static List<string> GetFilteredKeys()
        {
            return filteredKeys;
        }

        public static void SetFilteredKeys(List<string> value)
        {
            filteredKeys = value;
        }

        private static bool allChannels;

        public static bool GetAllChannels()
        {
            return allChannels;
        }

        public static void SetAllChannels(bool value)
        {
            allChannels = value;
        }
    }

    internal class FilterSet : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private Button _btnFilter, _btnOk, _btnCancel;

        private CheckBox _chk;

        public AutoCompleteStringCollection _filterSource;

        private Label _label;

        private Panel _pnChannelFilters;

        private RadioButton _radioDepthFilter;

        private TextBox _textBox;

        public FilterSet(AutoCompleteStringCollection source)
        {
            Width = 400;
            Height = 280;
            Text = "Source for filtering";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;

            _filterSource = source;     
        }

        protected override void OnLoad(EventArgs e)
        {
            _label = new Label
            {
                Text = "Insert some value to obtain releations",
                Width = 300,
                Font = default(Font),
                Location = new Point(20, 15)
            };
            Controls.Add(_label);

            _textBox = new TextBox
            {
                Text = string.Empty,
                Width = 300,
                Location = new Point(20, 40),
                Font = default(Font),
                ForeColor = default(Color),
                BackColor = default(Color)
            };

            _textBox.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);

            _textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            _textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            var f = new Filter();
            _textBox.AutoCompleteCustomSource = _filterSource;

            Controls.Add(_textBox);

            _btnFilter = new Button();
            _btnFilter.Size = new Size(25, _textBox.ClientSize.Height + 2);
            _btnFilter.Location = new Point(_textBox.ClientSize.Width - _btnFilter.Width, -1);
            _btnFilter.Image = Resources.search;
            _btnFilter.Cursor = Cursors.Default;
            _btnFilter.BackColor = default(Color);
            _btnFilter.DialogResult = DialogResult.OK;

            _btnFilter.Click += delegate
                {
                    Filter.SetFilteredKey(_textBox.Text);
                    Filter.SetAllChannels(_chk.Checked);

                    Close();
                };

            _textBox.Controls.Add(_btnFilter);
            SendMessage(_textBox.Handle, 0xd3, (IntPtr)2, (IntPtr)(_btnFilter.Width << 16));

            var lblPnDepthFilters = new Label
            {
                Text = "Choose depth",
                Font = default(Font),
                Location = new Point(20, 70),
                AutoSize = true
            };
            Controls.Add(lblPnDepthFilters);

            _pnChannelFilters = new Panel
            {
                Width = 330,
                Height = 70,
                Location = new Point(20, 90),
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = default(Color),
                AutoScroll = true,
                AutoScrollMargin = new Size(10, 10)
            };

            Controls.Add(_pnChannelFilters);

            var depth = new Channels();
            var radioLeft = 5;

            foreach (var dpt in depth.ListOfChannels())
                BeginInvoke((Action)(() =>
                {
                    _radioDepthFilter = new RadioButton();

                    _radioDepthFilter.Location = new Point(radioLeft,
                        _pnChannelFilters.Height / 2 - _radioDepthFilter.Height / 2 - 5);

                    _radioDepthFilter.Text = dpt.Channel;

                    if (dpt.Channel == Filter.GetFilteredChannel()) _radioDepthFilter.Checked = true;

                    _radioDepthFilter.CheckedChanged += _radioFilter_CheckedChange;
                    _pnChannelFilters.Controls.Add(_radioDepthFilter);

                    radioLeft += _radioDepthFilter.Width + 5;
                }));

            _chk = new CheckBox
            {
                Checked = Filter.GetAllChannels(),
                Text = @"Gannt full preview",
                Width = 200,
                Location = new Point(20, 170),
                AutoSize = true
            };
            Controls.Add(_chk);

            _btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Size = new Size(90, 25),
                Location = new Point(170, 190),
                Text = "&Filter",
                UseMnemonic = true
            };
            _btnOk.Click += delegate
                {
                    Filter.SetFilteredKey(_textBox.Text);
                    Filter.SetAllChannels(_chk.Checked ? true : false);

                    Close();
                };
            Controls.Add(_btnOk);

            _btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Size = new Size(60, 25),
                Location = new Point(280, 190),
                Text = "&Cancel",
                UseMnemonic = true
            };
            _btnCancel.Click += delegate { Close(); };

            Controls.Add(_btnCancel);

            base.OnLoad(e);
        }

        private void _radioFilter_CheckedChange(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;

            if (rb.Checked) Filter.SetFilteredChannel(rb.Text);
        }
    }

    public class ServerConfiguration : Form
    {
        public ServerConfiguration()
        {
            Text = "Configre connection";
            Width = 700;
            Height = 250;
            ShowIcon = false;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private string GetSetting(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        internal Label _lbl;

        internal TextBox _txt;

        internal Button _btnOk, _btnCancel;

        internal Configuration _config;

        protected override void OnLoad(EventArgs e)
        {
            var title = ConfigurationManager.AppSettings["App.config"];
            var source = ConfigurationManager.ConnectionStrings["GanttprojEntities"].ConnectionString;
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            _lbl = new Label
            {
                Text = "Provider connection string",
                AutoSize = true,
                Location = new Point(20, 10)
            };

            Controls.Add(_lbl);

            _txt = new TextBox
            {
                Text = source,
                Multiline = true,
                Location = new Point(20, 30),
                Width = 650,
                Height = 100,
                Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold)
            };

            Controls.Add(_txt);

            _btnOk = new Button
            {
                Text = "&OK",
                Size = new Size(90, 25),
                Location = new Point(450, 150)
            };

            _btnOk.Click += delegate
                {
                    try
                    {
                        var newConnection = _txt.Text;
                        var newConfig = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        newConfig.ConnectionStrings.ConnectionStrings["Ganttproj"].ConnectionString = newConnection;
                        newConfig.Save(ConfigurationSaveMode.Full);

                        MessageBox.Show("Server configuration changed.");
                        Close();

                        Application.Restart();
                    }
                    catch
                    {
                        MessageBox.Show("Config file doesn't exist or may be corrupted.");
                    }
                };

            Controls.Add(_btnOk);

            _btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Size = new Size(90, 25),
                Location = new Point(560, 150),
                Text = "&Cancel",
                UseMnemonic = true
            };
            _btnCancel.Click += delegate { Close(); };

            Controls.Add(_btnCancel);

            base.OnLoad(e);
        }
    }

    public class UpdateList : Form
    {
        public string TextInput { get; set; }

        private Label _lbl1, _lbl2;

        private TextBox _txt;

        private DateTimePicker _dtp;

        private RadioButton _rb1, _rb2;

        private Button _btnOk, _btnCancel;

        public UpdateList()
        {
            Width = 400;
            Height = 300;
            Text = "Update selected field";
            ShowIcon = false;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        protected override void OnLoad(EventArgs e)
        {
            _lbl1 = new Label
            {
                Text = "Input text",
                AutoSize = true,
                Location = new Point(20, 10)
            };
            Controls.Add(_lbl1);

            _txt = new TextBox
            {
                Width = 240,
                Location = new Point(20, 30)
            };
            Controls.Add(_txt);

            _lbl2 = new Label
            {
                Text = "Input date",
                AutoSize = true,
                Location = new Point(20, 60)
            };
            Controls.Add(_lbl2);

            _dtp = new DateTimePicker
            {
                CustomFormat = "MM/dd/yyyy",
                Format = DateTimePickerFormat.Custom,
                Location = new Point(20, 80)
            };
            Controls.Add(_dtp);

            _rb1 = new RadioButton
            {
                Text = "Textual update",
                Checked = false,
                Location = new Point(20, 120)
            };
            Controls.Add(_rb1);

            _rb2 = new RadioButton
            {
                Text = "Time update",
                Checked = false,
                Location = new Point(140, 120)
            };
            Controls.Add(_rb2);

            _btnOk = new Button
            {
                Text = "&OK",
                Size = new Size(90, 25),
                Location = new Point(80, 180),
                DialogResult = DialogResult.OK
            };

            _btnOk.Click += delegate
                {
                    if (_rb1.Checked == false && _rb2.Checked == false)
                    {
                        _btnOk.DialogResult = DialogResult.None;
                    }
                    else
                    {
                        _btnOk.DialogResult = DialogResult.OK;

                        if (_rb1.Checked == true)
                            TextInput = _txt.Text;
                        else
                            TextInput = _dtp.Value.ToString("M/d/yyyy", CultureInfo.InvariantCulture);

                        Close();
                    }
                };
            Controls.Add(_btnOk);
            _btnCancel = new Button
            {
                Text = "&Cancel",
                Size = new Size(90, 25),
                Location = new Point(190, 180),
                DialogResult = DialogResult.Cancel
            };
            _btnCancel.Click += delegate
                {
                    Close();
                };
            Controls.Add(_btnCancel);
            base.OnLoad(e);
        }        
    }
}
