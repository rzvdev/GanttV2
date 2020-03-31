using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ganntproj1
    {
    public class TableView : DataGridView
        {
        public TableView()
            {
            //dissalow user access to data architecture

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = false;
            AllowUserToResizeRows = false;
            AllowUserToResizeColumns = false;
            ReadOnly = true; //disallow user to change data

            BackgroundColor = Color.WhiteSmoke;

            MultiSelect = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataBindingComplete += delegate
            {
                foreach (DataGridViewColumn c in Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                ColumnHeadersHeight = 50;
                RowTemplate.Height = 22;
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                CellBorderStyle = DataGridViewCellBorderStyle.None;
                ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(125, 141, 161);
                ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(242, 242, 242);
                CellBorderStyle = DataGridViewCellBorderStyle.Single;
                DefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);
                RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(125, 141, 161);
                RowsDefaultCellStyle.SelectionForeColor = Color.AliceBlue;
                RowHeadersVisible = false;
                GridColor = Color.FromArgb(181,181,181);
                for (var i = 0; i <= Columns.Count - 1; i++)
                {
                    var c = Columns[i];
                    c.DefaultCellStyle.Font = new Font("Bahnschrift", 9);
                    c.HeaderCell.Style.Font = new Font("Bahnschrift", 9, FontStyle.Bold);
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft;
                }
            };
        }
        
        #region Formating

        private static string ToTitleCase(string str)
            {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToUpper());
            }

        private void DataGridViewUpperCaseValues(object sender, DataGridViewEditingControlShowingEventArgs e)
            {
            if (e.Control is TextBox box) box.CharacterCasing = CharacterCasing.Upper;
            }

        private bool CheckExistedData(DataGridView dgv)
            {
            var check = Columns.Count > 0;

            return check;
            }

        private void Dgv_CellFormation(object sender, DataGridViewCellFormattingEventArgs e)
            {
            if (!CheckExistedData(this)) return;

            for (var index = 0; index <= ColumnCount - 1; index++)
                {
                //add grid headers to title case
                Columns[index].HeaderText = ToTitleCase(Columns[index].HeaderText);

                //repleacements
                if (Columns[index].HeaderText.Contains("_"))
                    {
                    //repleace undersocres with empty space
                    var replace = Columns[index].HeaderText.Replace("_", " ");
                    //set repleacment to header text
                    Columns[index].HeaderText = replace;
                    }
                }
            }

        #endregion

        public class DataGridViewRolloverCell : DataGridViewTextBoxCell
            {
            protected override void Paint(
                Graphics graphics,
                Rectangle clipBounds,
                Rectangle cellBounds,
                int rowIndex,
                DataGridViewElementStates cellState,
                object value,
                object formattedValue,
                string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
                {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,
                    value, formattedValue, errorText, cellStyle,
                    advancedBorderStyle, paintParts);

                Point cursorPosition = DataGridView.PointToClient(Cursor.Position);

                if (cellBounds.Contains(cursorPosition))
                    {
                    Rectangle newRect = new Rectangle(cellBounds.X + 1,
                        cellBounds.Y + 1, cellBounds.Width - 4,
                        cellBounds.Height - 4);
                    graphics.DrawRectangle(Pens.Red, newRect);
                    }
                }
            protected override void OnMouseEnter(int rowIndex)
                {
                DataGridView.InvalidateCell(this);
                }
            protected override void OnMouseLeave(int rowIndex)
                {
                DataGridView.InvalidateCell(this);
                }

            }

        public class DataGridViewRolloverCellColumn : DataGridViewColumn
            {
            public DataGridViewRolloverCellColumn()
                {
                CellTemplate = new DataGridViewRolloverCell();
                }
            }
        }
    }