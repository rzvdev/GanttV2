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

    /// <summary>
    /// Contains configuration structure for the display.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets the StartDate
        /// Gets or sets start date, controlled by user.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate
        /// Gets or sets end date, controlled by user.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the StartShift
        /// </summary>
        public TimeSpan StartShift { get; set; }

        /// <summary>
        /// Gets or sets the EndShift
        /// </summary>
        public TimeSpan EndShift { get; set; }

        /// <summary>
        /// Gets or sets the FileColumns
        /// Gets or sets obtained XLS or XLSX source file.
        /// </summary>
        public static List<string> FileColumns { get; set; }

        /// <summary>
        /// Gets or sets the GlobalDir
        /// </summary>
        public string GlobalDir { get; set; } = @"C:\GanttOutcomes";
        /// <summary>
        /// Defines the _sql_conn1
        /// </summary>
        private SqlConnection _sql_conn1;
        /// <summary>
        /// Defines the _ganttConn
        /// </summary>
        private static DataContext _ganttConn;
        /// <summary>
        /// Defines the _olyConn
        /// </summary>
        private static DataContext _olyConn;
        /// <summary>
        /// The Get_sql_conn
        /// </summary>
        /// <returns>The <see cref="SqlConnection"/></returns>
        public SqlConnection Get_sql_conn()
        {
            return _sql_conn1;
        }
        /// <summary>
        /// The Set_sql_conn
        /// </summary>
        /// <param name="value">The value<see cref="SqlConnection"/></param>
        public void Set_sql_conn(SqlConnection value)
        {
            _sql_conn1 = value;
        }

        /// <summary>
        /// The GetGanttConn
        /// </summary>
        /// <returns>The <see cref="DataContext"/></returns>
        public static DataContext GetGanttConn()
        {
            return _ganttConn;
        }

        /// <summary>
        /// The SetGanttConn
        /// </summary>
        /// <param name="value">The value<see cref="DataContext"/></param>
        public static void SetGanttConn(DataContext value)
        {
            _ganttConn = value;
        }

        /// <summary>
        /// The GetOlyConn
        /// </summary>
        /// <returns>The <see cref="DataContext"/></returns>
        public static DataContext GetOlyConn()
        {
            return _olyConn;
        }

        /// <summary>
        /// The SetOlyConn
        /// </summary>
        /// <param name="value">The value<see cref="DataContext"/></param>
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
                // delete existing records
                context.ExecuteCommand("insert into log (id,username,computername,logdate,operation,queryon,program)" +
                    " values ({0},{1},{2},{3},{4},{5},{6})", Guid.NewGuid(), username, pcname, DateTime.Now.Subtract(MinimalDate).Ticks, operation, query, program);
            }
        }

        /// <summary>
        /// The ReadSqlConnectionString
        /// </summary>
        /// <param name="conIdx">The conIdx<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
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

        /// <summary>
        /// The CreateOutcomeDir
        /// </summary>
        public void CreateOutcomeDir()
        {
            try
            {
                // Determines whether the directory exists.
                if (!Directory.Exists(GlobalDir))
                {
                    // Creates the directory.
                    var di = Directory.CreateDirectory(GlobalDir);

                    // Save direcotry path in my store
                    Store.Default.globalDir = GlobalDir;
                    Store.Default.Save();

                    Console.WriteLine("The directory was created successfully at {0}.",
                        Directory.GetCreationTime(GlobalDir));
                }
                else
                {
                    // Save target file path
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
                // Delete the file if it exists.
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

        /// <summary>
        /// The DoContextAction
        /// </summary>
        /// <param name="command">The command<see cref="string"/></param>
        /// <param name="param">The param<see cref="object[]"/></param>
        /// <param name="target">The target<see cref="int"/></param>
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

        /// <summary>
        /// Converts worksheet from XLS source to target CSV file.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="worksheetName"></param>
        /// <param name="targetFile"></param>
        public void ExcelToCSVConversion(string sourceFile, string worksheetName, string targetFile)
        {
            //LoadingInfo.InfoText = "Converting " + sourceFile + " to .csv format";
            //LoadingInfo.ShowLoading();

            /*Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\temp\test.xls;" + 
  @"Extended Properties='Excel 8.0;HDR=Yes;'";   */

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + @";Extended Properties=""Excel 12.0 Xml;HDR=YES""";
            OleDbConnection connection = null;
            StreamWriter writer = null;
            OleDbCommand command = null;
            OleDbDataAdapter dataAdapter = null;

            try
            {
                if (Store.Default.outputDir == string.Empty) return;

                //LoadingInfo.InfoText = "Opening connection to " + sourceFile;
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

                //Removes the first 3 illogical rows. 
                Output.ProcessingTable.Rows.Remove(Output.ProcessingTable.Rows[0]);
                Output.ProcessingTable.Rows.Remove(Output.ProcessingTable.Rows[1]);
                Output.ProcessingTable.Rows.Remove(Output.ProcessingTable.Rows[2]);

                //Removes rows with nullable root
                Output.ProcessingTable.AcceptChanges();

                foreach (DataRow row in Output.ProcessingTable.Rows)
                {
                    if (string.IsNullOrEmpty(row.ItemArray.GetValue(0).ToString())) row.Delete();
                }

                Output.ProcessingTable.AcceptChanges();

                //writes data to CSV 

                for (int row = 0; row < Output.ProcessingTable.Rows.Count; row++)
                {
                    var tmpStr = string.Empty;

                    //skip empty comesa root
                    //if (string.IsNullOrEmpty(Output.ProcessingTable.Rows[row][0].ToString())) continue;

                    for (int column = 0; column < Output.ProcessingTable.Columns.Count; column++)
                    {
                        var strToCollect = Output.ProcessingTable.Rows[row][column];

                        var typeOfStr = strToCollect.GetType();

                        //converts values to representable date format
                        if (typeOfStr == typeof(DateTime))
                            strToCollect = Convert.ToDateTime(Output.ProcessingTable.Rows[row][column]).ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);

                        //collects strindg
                        tmpStr += strToCollect + ",";
                    }

                    //writes collection to the CSV file
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
                //LoadingInfo.CloseLoading();
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

        /// <summary>
        /// The PopulateTableUsingCsv
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        public void PopulateTableUsingCsv(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //here only read
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    // Skip first 3 unnecessary lines
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    string[] headers = sr.ReadLine().Split(',');
                    // Creates datatable columns from CSV file
                    foreach (string header in headers)
                        Output.ProcessingTable.Columns.Add(header);
                    while (!sr.EndOfStream)
                    // Add rows to datatable from CSV file
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        var newRow = Output.ProcessingTable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            //insert original values in string
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

        /// <summary>
        /// The ExportTableToDba
        /// </summary>
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

        /// <summary>
        /// Confirms the correct value of date parameters, that will be forwarded to the drawing process.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns></returns>
        public DateTime ConvertToUniTime(object obj)
        {
            DateTime.TryParse(obj.ToString(), out DateTime dtc);

            return dtc;
        }

        /// <summary>
        /// The SendMessage
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="msg">The msg<see cref="int"/></param>
        /// <param name="wp">The wp<see cref="IntPtr"/></param>
        /// <param name="lp">The lp<see cref="IntPtr"/></param>
        /// <returns>The <see cref="IntPtr"/></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        /// <summary>
        /// Defines the <see cref="OpenDialog" />
        /// </summary>
        internal class OpenDialog : Form
        {
            /// <summary>
            /// Defines the _btnDiag, _btnNext
            /// </summary>
            private Button _btnDiag, _btnNext;

            /// <summary>
            /// Defines the _label, _lblWroksheetTitle, _lblTargetTitle
            /// </summary>
            private Label _label, _lblWroksheetTitle, _lblTargetTitle;

            /// <summary>
            /// Defines the _textBox, _txtWorksheet, _txtTarget
            /// </summary>
            private TextBox _textBox, _txtWorksheet, _txtTarget;

            /// <summary>
            /// Defines the _link
            /// </summary>
            private LinkLabel _link;

            /// <summary>
            /// Defines the _config
            /// </summary>
            private readonly Config _config = new Config();

            /// <summary>
            /// Initializes a new instance of the <see cref="OpenDialog"/> class.
            /// </summary>
            public OpenDialog()
            {
                Width = 400;
                Height = 300;
                Text = "File dialog";
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MinimizeBox = false;
                MaximizeBox = false;
            }

            /// <summary>
            /// The OnLoad
            /// </summary>
            /// <param name="e">The e<see cref="EventArgs"/></param>
            protected override void OnLoad(EventArgs e)
            {
                _config.CreateOutcomeDir();   //create default directory

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

                //! do not simplify with object initializaton
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
                        //declare validation parameters
                        var fileAr = openFileDialog.FileName.Split('\\').Last(); //get file from the address string
                        var validExtent = fileAr.Split('.')[1]; //get extension

                        //gets only .xls assoiciation
                        if (validExtent != "xls")
                        {
                            DialogResult = DialogResult.None;
                            MessageBox.Show("File extension isn't valid. Please choose another file.");
                            //return;
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

                //Test

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

    /// <summary>
    /// Represents an interactive method, with keys and their habitats, that could be filtered.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="aim">Habitat where their childs and key mutually interact.</param>
        /// <param name="key">Filtering keyword connected with aim value.</param>
        public Filter(string aim, string key)
        {
            SetFilterAim(aim);
            SetFilterKey(key);
        }

        /// <summary>
        /// Defines the filterAim
        /// </summary>
        private string filterAim;

        /// <summary>
        /// Gets target habitat.
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string GetFilterAim()
        {
            return filterAim;
        }

        /// <summary>
        /// Sets target habitat.
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        public void SetFilterAim(string value)
        {
            filterAim = value;
        }

        /// <summary>
        /// Defines the filterKey
        /// </summary>
        private string filterKey;

        /// <summary>
        /// Gets key combination that makes some tree.
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string GetFilterKey()
        {
            return filterKey;
        }

        /// <summary>
        /// Sets key combination that makes some tree.
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        public void SetFilterKey(string value)
        {
            filterKey = value;
        }

        /// <summary>
        /// Defines the filteredKey
        /// </summary>
        private static string filteredKey;

        /// <summary>
        /// Gets key, that were filtered.
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public static string GetFilteredKey()
        {
            return filteredKey;
        }

        /// <summary>
        /// Sets key, that were filtered.
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        public static void SetFilteredKey(string value)
        {
            filteredKey = value;
        }

        /// <summary>
        /// Defines the filteredChannel
        /// </summary>
        private static string filteredChannel;

        /// <summary>
        /// Gets depth, that were filtered.
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public static string GetFilteredChannel()
        {
            return filteredChannel;
        }

        /// <summary>
        /// Sets depth, that were filtered.
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        public static void SetFilteredChannel(string value)
        {
            filteredChannel = value;
        }

        /// <summary>
        /// Defines the filteredKeys
        /// </summary>
        private static List<string> filteredKeys;

        /// <summary>
        /// Gets all multiplied selected keys from a list.
        /// </summary>
        /// <returns>The <see cref="List{string}"/></returns>
        public static List<string> GetFilteredKeys()
        {
            return filteredKeys;
        }

        /// <summary>
        /// Sets all multiplied selected keys from a list.
        /// </summary>
        /// <param name="value">The value<see cref="List{string}"/></param>
        public static void SetFilteredKeys(List<string> value)
        {
            filteredKeys = value;
        }

        /// <summary>
        /// Defines the allChannels
        /// </summary>
        private static bool allChannels;

        /// <summary>
        /// Gets status, that passes the signal to the function, to open all channel views.
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetAllChannels()
        {
            return allChannels;
        }

        /// <summary>
        /// Sets status, that passes the signal to the function, to open all channel views.
        /// </summary>
        /// <param name="value">The value<see cref="bool"/></param>
        public static void SetAllChannels(bool value)
        {
            allChannels = value;
        }
    }

    /// <summary>
    /// Defines the <see cref="FilterSet" />
    /// </summary>
    internal class FilterSet : Form
    {
        /// <summary>
        /// The SendMessage
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="msg">The msg<see cref="int"/></param>
        /// <param name="wp">The wp<see cref="IntPtr"/></param>
        /// <param name="lp">The lp<see cref="IntPtr"/></param>
        /// <returns>The <see cref="IntPtr"/></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        /// <summary>
        /// Defines the _btnFilter, _btnOk, _btnCancel
        /// </summary>
        private Button _btnFilter, _btnOk, _btnCancel;

        /// <summary>
        /// Defines the _chk
        /// </summary>
        private CheckBox _chk;

        /// <summary>
        /// Defines the _filterSource
        /// </summary>
        public AutoCompleteStringCollection _filterSource;

        /// <summary>
        /// Defines the _label
        /// </summary>
        private Label _label;

        /// <summary>
        /// Defines the _pnChannelFilters
        /// </summary>
        private Panel _pnChannelFilters;

        /// <summary>
        /// Defines the _radioDepthFilter
        /// </summary>
        private RadioButton _radioDepthFilter;

        /// <summary>
        /// Defines the _textBox
        /// </summary>
        private TextBox _textBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSet"/> class.
        /// </summary>
        /// <param name="source">The source<see cref="AutoCompleteStringCollection"/></param>
        public FilterSet(AutoCompleteStringCollection source)
        {
            Width = 400;
            Height = 280;
            Text = "Source for filtering";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;

            _filterSource = source; //source is a class owner
        }

        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

            //my custom source from csv file - direct by owner
            _textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            _textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //use owner as a general
            var f = new Filter();
            _textBox.AutoCompleteCustomSource = _filterSource;

            Controls.Add(_textBox);

            //add search button to textbox
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

            //create dynamic list of channels' radio filters

            var depth = new Channels();
            var radioLeft = 5;

            foreach (var dpt in depth.ListOfChannels())
                BeginInvoke((Action)(() =>
                {
                    _radioDepthFilter = new RadioButton();

                    //set up the Center in relation to the height of the parent
                    _radioDepthFilter.Location = new Point(radioLeft,
                        _pnChannelFilters.Height / 2 - _radioDepthFilter.Height / 2 - 5);

                    _radioDepthFilter.Text = dpt.Channel;

                    //auto-check radio which one is equals to the current depth definition
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

            //dialog confirmation buttons
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

        /// <summary>
        /// The _radioFilter_CheckedChange
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void _radioFilter_CheckedChange(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;

            if (rb.Checked) Filter.SetFilteredChannel(rb.Text);
        }
    }

    /// <summary>
    /// Defines the <see cref="ServerConfiguration" />
    /// </summary>
    public class ServerConfiguration : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfiguration"/> class.
        /// </summary>
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

        /// <summary>
        /// The GetSetting
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string GetSetting(string key)
        {
            //Return connection string config
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        /// Defines the _lbl
        /// </summary>
        internal Label _lbl;

        /// <summary>
        /// Defines the _txt
        /// </summary>
        internal TextBox _txt;

        /// <summary>
        /// Defines the _btnOk, _btnCancel
        /// </summary>
        internal Button _btnOk, _btnCancel;

        /// <summary>
        /// Defines the _config
        /// </summary>
        internal Configuration _config;

        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        protected override void OnLoad(EventArgs e)
        {
            //Load entity connection string config
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
                        //Save entity configuration changes
                        //LoadingInfo.InfoText = "Trying to configure server connection...";
                        //LoadingInfo.ShowLoading();

                        var newConnection = _txt.Text;
                        var newConfig = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        newConfig.ConnectionStrings.ConnectionStrings["Ganttproj"].ConnectionString = newConnection;
                        newConfig.Save(ConfigurationSaveMode.Full);

                        //LoadingInfo.CloseLoading();

                        MessageBox.Show("Server configuration changed.");
                        Close();

                        Application.Restart();
                    }
                    catch
                    {
                        //LoadingInfo.CloseLoading();
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

    /// <summary>
    /// Defines the <see cref="UpdateList" />
    /// </summary>
    public class UpdateList : Form
    {
        /// <summary>
        /// Gets or sets the TextInput
        /// </summary>
        public string TextInput { get; set; }

        /// <summary>
        /// Defines the _lbl1, _lbl2
        /// </summary>
        private Label _lbl1, _lbl2;

        /// <summary>
        /// Defines the _txt
        /// </summary>
        private TextBox _txt;

        /// <summary>
        /// Defines the _dtp
        /// </summary>
        private DateTimePicker _dtp;

        /// <summary>
        /// Defines the _rb1, _rb2
        /// </summary>
        private RadioButton _rb1, _rb2;

        /// <summary>
        /// Defines the _btnOk, _btnCancel
        /// </summary>
        private Button _btnOk, _btnCancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateList"/> class.
        /// </summary>
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

        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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
                        //MessageBox.Show("Please check one textual option.");
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
