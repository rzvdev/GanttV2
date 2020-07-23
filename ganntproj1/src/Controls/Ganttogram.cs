namespace ganntproj1
{
    using ganntproj1.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Chart class that encapsulates GDI+ drawing surfaces.
    /// </summary>
    internal class Ganttogram : UserControl
    {
        /// <summary>
        /// Defines the Bars
        /// </summary>
        public List<BarProperty> Bars = new List<BarProperty>();

        /// <summary>
        /// Defines the HeaderHeight
        /// </summary>
        private const int HeaderHeight = 50;

        /// <summary>
        /// Defines the _widthPerItem
        /// </summary>
        private int _widthPerItem;

        /*
         ** Bar start-location declerations
         */
        /// <summary>
        /// Defines the _barStartLeft
        /// </summary>
        private int _barStartLeft = 100;

        /// <summary>
        /// Defines the BarStartTop
        /// </summary>
        private const int BarStartTop = 49;

        /// <summary>
        /// Defines the BarStartRight
        /// </summary>
        private const int BarStartRight = 20;

        /// <summary>
        /// Defines the _barSpace
        /// </summary>
        private int _barSpace = 8;

        /// <summary>
        /// Defines the _barHeight
        /// </summary>
        public int _barHeight = 40;

        /// <summary>
        /// Defines the _barIsChanging
        /// </summary>
        //private int _barIsChanging = -1;

        /*
         ** Scroll bar declerations
         */
        /// <summary>
        /// Defines the _barsViewable
        /// </summary>
        private int _barsViewable = -1;

        /// <summary>
        /// Defines the HeaderTimeStartTop
        /// </summary>
        private const int HeaderTimeStartTop = 20;

        /// <summary>
        /// Defines the _lastLineStop
        /// </summary>
        private int _lastLineStop;

        /// <summary>
        /// Defines the _mouseHoverBarIndex
        /// </summary>
        private int _mouseHoverBarIndex = -1;

        /*
         ** Mouse work
         */
        private enum MouseOver
        {
            /// <summary>
            /// Defines the Empty
            /// </summary>
            Empty,

            /// <summary>
            /// Defines the Bar
            /// </summary>
            Bar,

            /// <summary>
            /// Defines the BarLeftSide
            /// </summary>
            BarLeftSide,

            /// <summary>
            /// Defines the BarRightSide
            /// </summary>
            BarRightSide
        }

        /// <summary>
        /// Defines the MouseDragged
        /// </summary>
        //public event MouseEventHandler MouseDragged;

        //public event EventHandler MouseClicked;
        /// <summary>
        /// Defines the BarChanged
        /// </summary>
        //public event EventHandler BarChanged;

        /// <summary>
        /// Defines the _mouseHover
        /// </summary>
        private MouseOver _mouseHover = MouseOver.Empty;

        /// <summary>
        /// Defines the _mouseHoverScrollBar
        /// </summary>
        private bool _mouseHoverScrollBar;

        /// <summary>
        /// Defines the _mouseHoverScrollBarArea
        /// </summary>
        private bool _mouseHoverScrollBarArea;

        /// <summary>
        /// Defines the _objBmp
        /// </summary>
        private Bitmap _objBmp;

        /// <summary>
        /// Defines the _objGraphics
        /// </summary>
        private Graphics _objGraphics;

        /// <summary>
        /// Defines the _scroll, _scrollBarArea
        /// </summary>
        private Rectangle _scroll, _scrollBarArea;

        /// <summary>
        /// Defines the ScrollPosition
        /// </summary>
        public int ScrollPosition;

        /// <summary>
        /// Defines the HeaderList
        /// </summary>
        public List<Header> HeaderList;

        /// <summary>
        /// Defines the BarsCount
        /// </summary>
        public int BarsCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ganttogram"/> class.
        /// </summary>
        public Ganttogram()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);

            var resX = Screen.FromControl(this).Bounds.Width;
            var resY = Screen.FromControl(this).Bounds.Height;

            _objBmp = new Bitmap(resX, resY);
            _objGraphics = Graphics.FromImage(_objBmp);

            BarsCount = GetBarIdxByRowText();
        }

        /// <summary>
        /// Gets or sets the FromDate
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets the ToDate
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AllowManualEditBar
        /// </summary>
        private bool AllowManualEditBar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AllowChange
        /// </summary>
        private bool AllowChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HideDelay
        /// </summary>
        public bool HideDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HideClosedTask
        /// </summary>
        public bool HideClosedTask { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AllowRowSelection
        /// </summary>
        public bool AllowRowSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseOverRowText
        /// </summary>
        public bool IsMouseOverRowText { get; set; }

        public bool IsMouseOverRowLine { get; set; }
        /// <summary>
        /// Gets a value indicating whether IsMouseOverToggle
        /// </summary>
        public bool IsMouseOverToggle { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseOverProductionBar
        /// </summary>
        public bool IsMouseOverProductionBar { get; set; }

        //public bool MouseIsChangingBarColor { get; set; }
        /// <summary>
        /// Gets or sets the MouseOverRowText
        /// </summary>
        public string MouseOverRowText { get; set; }

        /// <summary>
        /// Gets the MouseOverRowValue
        /// </summary>
        public object MouseOverRowValue { get; set; }

        /// <summary>
        /// Gets the MouseOverToggleValue
        /// </summary>
        public object MouseOverToggleValue { get; private set; }

        /// <summary>
        /// Gets the MouseOverRowIndex
        /// </summary>
        public int MouseOverRowIndex { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether RectangleSelectorActivated
        /// </summary>
        public bool RectangleSelectorActivated { get; set; }

        //public object MouseOverRdd { get; set; }

        //public object MouseOverDvc { get; set; }
        /// <summary>
        /// Gets the MouseOverNextValue
        /// </summary>
        public object MouseOverNextValue { get; private set; }

        /// <summary>
        /// Gets the MouseOverRowElements
        /// </summary>
        public List<string> MouseOverRowElements { get; private set; }

        /// <summary>
        /// Gets or sets the MouseOverColumnDate
        /// </summary>
        public DateTime MouseOverColumnDate { get; set; }

        /// <summary>
        /// Gets or sets the MouseOverProductionBar
        /// </summary>
        public object MouseOverProductionBar { get; set; }

        /// <summary>
        /// Gets or sets the single usage filter for a rowText
        /// </summary>
        public string FilteredRowText { get; set; }

        /// <summary>
        /// Gets or sets the single usage filter on a rowText attribute
        /// </summary>
        public string FilteredRowAtt { get; set; }
        /// <summary>
        /// Gets the GridColor
        /// </summary>
        private Pen GridColor { get; } = Pens.LightGray;

        /// <summary>
        /// Gets the ChartBackColor
        /// </summary>
        private Color ChartBackColor { get; } = Color.WhiteSmoke; //Color.FromArgb(245, 245, 245);//Color.FromArgb(250, 250, 250);

        /// <summary>
        /// Gets the ChartBackBrush
        /// </summary>
        private Brush ChartBackBrush { get; } = new SolidBrush(Color.WhiteSmoke); //Color.FromArgb(245, 245, 245));

        /// <summary>
        /// Defines the _barFont
        /// </summary>
        private readonly Font _barFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);

        /// <summary>
        /// Gets the TimeFont
        /// </summary>
        private Font TimeFont { get; } = new Font("Bahnschrift", 8, FontStyle.Regular);

        /// <summary>
        /// Gets the RowFont
        /// </summary>
        private Font RowFont { get; } = new Font("Bahnschrift", 9, FontStyle.Regular);

        /// <summary>
        /// Gets the DateFont
        /// </summary>
        private Font DateFont { get; } = new Font("Bahnschrift", 9, FontStyle.Regular);

        /// <summary>
        /// Sets the ScrollPosY
        /// </summary>
        private int ScrollPosY
        {
            set
            {
                var barCount = GetBarIdxByRowText();
                var maxHeight = Height - 10;
                decimal scrollHeight = maxHeight / barCount * _barsViewable;

                decimal divideBy = barCount - _barsViewable;
                if (divideBy == 0) divideBy = 1;

                var scrollSpeed = (maxHeight - scrollHeight) / divideBy;
                var index = 0;
                dynamic distanceFromLastPosition = 9999;

                // Tests to see what scrollposition is the closest to the set position

                while (index < barCount)
                {
                    var newPositionTemp = index * (int)scrollSpeed + (int)scrollHeight / 2 + 30 / 2;
                    dynamic distanceFromCurrentPosition = newPositionTemp - value;

                    if (distanceFromLastPosition < 0)
                    {
                        if (distanceFromCurrentPosition < distanceFromLastPosition)
                        {
                            ScrollPosition = index - 1;
                            PaintChart();
                            return;
                        }
                    }
                    else
                    {
                        if (distanceFromCurrentPosition > distanceFromLastPosition)
                        {
                            ScrollPosition = index - 1;

                            // A prudence to make sure the scroll bar doesn't go too far down

                            if (ScrollPosition + _barsViewable > GetBarIdxByRowText())
                                ScrollPosition = GetBarIdxByRowText() - _barsViewable;

                            PaintChart();
                            return;
                        }
                    }

                    // If the scroll area is filled there's no need to show the scrollbar

                    distanceFromLastPosition = distanceFromCurrentPosition;

                    index += 1;
                }
            }
        }

        /// <summary>
        /// Gets or sets the SelectedBarColor
        /// </summary>
        private Color SelectedBarColor { get; set; }

        /// <summary>
        /// Gets or sets the Direction
        /// </summary>
        private int Direction { get; set; }

        /// <summary>
        /// Gets or sets the SelectedBarIndex
        /// </summary>
        private int SelectedBarIndex { get; set; }

        /// <summary>
        /// The OnResize
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ScrollPosition = 0;

            //if (_lastLineStop > 0)
            //    {
            //    _objBmp = new Bitmap(Width - _barStartRight, _lastLineStop);
            //    _objGraphics = Graphics.FromImage(_objBmp);
            //    }

            PaintChart();
        }

        /// <summary>
        /// The GetFullHeaderList
        /// </summary>
        /// <returns>The <see cref="List{Header}"/></returns>
        private List<Header> GetFullHeaderList()
        {
            List<Header> result = new List<Header>();
            DateTime newFromTime = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);
            string item = null;

            TimeSpan interval = ToDate - FromDate;

            if (interval.TotalDays < 3)
            {
                DateTime tmpTime = newFromTime;
                newFromTime = tmpTime.AddHours(FromDate.Hour);

                if (FromDate.Minute < 59 & ToDate.Minute > 29)
                {
                    tmpTime = newFromTime;
                    newFromTime = tmpTime.AddMinutes(30);
                }
                else
                {
                    tmpTime = newFromTime;
                    newFromTime = tmpTime.AddMinutes(0);
                }

                while (newFromTime <= ToDate)
                {
                    item = newFromTime.Hour + ":";

                    if (newFromTime.Minute < 10)
                    {
                        item += "0" + newFromTime.Minute;
                    }
                    else
                    {
                        item += "" + newFromTime.Minute;
                    }

                    Header header = new Header
                    {
                        HeaderText = item,
                        HeaderTextInsteadOfTime = "",
                        Time = new DateTime(newFromTime.Year, newFromTime.Month, newFromTime.Day, newFromTime.Hour, newFromTime.Minute, 0)
                    };
                    result.Add(header);

                    newFromTime = newFromTime.AddMinutes(5);
                    // The minimum interval of time between the headers
                }
            }
            else if (interval.TotalDays < 60)
            {
                while (newFromTime <= ToDate)
                {
                    Header header = new Header();

                    header.HeaderText = "";
                    header.HeaderTextInsteadOfTime = "";
                    header.Time = new System.DateTime(newFromTime.Year, newFromTime.Month, newFromTime.Day, 0, 0, 0);
                    result.Add(header);

                    newFromTime = newFromTime.AddDays(1);
                    // The minimum interval of time between the headers
                }
            }
            else
            {
                while (newFromTime <= ToDate)
                {
                    Header header = new Header();

                    header.HeaderText = "";
                    header.Time = new System.DateTime(newFromTime.Year, newFromTime.Month, newFromTime.Day, 0, 0, 0);
                    header.HeaderTextInsteadOfTime = newFromTime.ToString("MMM");
                    result.Add(header);

                    newFromTime = newFromTime.AddMonths(1);
                    // The minimum interval of time between the headers
                }
            }


            return result;
        }

        /// <summary>
        /// The DrawHeader
        /// </summary>
        /// <param name="gfx">The gfx<see cref="Graphics"/></param>
        /// <param name="headerList">The headerList<see cref="List{Header}"/></param>
        private void DrawHeader(Graphics gfx, List<Header> headerList)
        {
            while (true)
            {
                if (headerList == null) headerList = GetFullHeaderList();

                if (headerList.Count == 0) return;

                dynamic availableWidth = Width - 10 - _barStartLeft - BarStartRight;
                _widthPerItem = availableWidth / headerList.Count;

                //if (_widthPerItem < 40)
                //    {
                //    List<Header> newHeaderList = new List<Header>();

                //    bool showNext = true;

                //    // If there's not enough room for all headers remove 50%

                //    foreach (Header header in headerList)
                //        {
                //        if (showNext == true)
                //            {
                //            newHeaderList.Add(header);
                //            showNext = false;
                //            }
                //        else
                //            {
                //            showNext = true;
                //            }
                //        }

                //    DrawHeader(gfx, newHeaderList);
                //    return;
                //    }

                //int index = 0;
                //Header lastHeader = null;

                //foreach (Header header in headerList)
                //    {
                //    int startPos = _barStartLeft + (index * _widthPerItem);
                //    bool showDateHeader = false;

                //    header.StartLocation = startPos;

                //    // Checks whether to show the date or not

                //    if (lastHeader == null)
                //        {
                //        showDateHeader = true;
                //        }
                //    else if (header.Time.Hour < lastHeader.Time.Hour)
                //        {
                //        showDateHeader = true;
                //        }
                //    else if (header.Time.Minute == lastHeader.Time.Minute)
                //        {
                //        showDateHeader = true;
                //        }

                //    // Show date

                //    if (showDateHeader == true)
                //        {
                //        string str = "";

                //        if (header.HeaderTextInsteadOfTime.Length > 0)
                //            {
                //            str = header.HeaderTextInsteadOfTime;
                //            }
                //        else
                //            {
                //            str = header.Time.ToString("d-MMM");
                //            }

                //        gfx.DrawString(str, TimeFont, Brushes.Black, startPos, 0);
                //        }

                //    // Show time

                //    gfx.DrawString(header.HeaderText, TimeFont, Brushes.Black, startPos, HeaderTimeStartTop);
                //    index += 1;

                //    lastHeader = header;
                //    }

                //HeaderList = headerList;
                //_widthPerItem = (Width - 10 - _barStartLeft - BarStartRight) / HeaderList.Count;

                if (_widthPerItem < 30)
                {
                    var newHeaderList = new List<Header>();

                    var showNext = true;

                    foreach (var header in headerList)
                        if (showNext)
                        {
                            newHeaderList.Add(header);
                            showNext = false;
                        }
                        else
                        {
                            showNext = true;
                        }

                    headerList = newHeaderList;
                    continue;
                }

                var index = 0;
                var dayMax = DateTime.DaysInMonth(FromDate.Year, FromDate.Month);
                //Header lastHeader = null;

                // Draws an optimal graphic chapter in relation to the row header
                gfx.FillRectangle(new SolidBrush(Color.WhiteSmoke), new Rectangle(0, 0, Width - 40, HeaderHeight));

                gfx.DrawString("  Linea", TimeFont, Brushes.Black, 1, 16);
                //    "Commessa             " + 
                //    "Duration", TimeFont, Brushes.Black, 1, 32);

                var curMonth = FromDate.Month.ToString("MMM");  //get start month

                foreach (var header in headerList)
                {
                    var startPos = _barStartLeft + index * _widthPerItem;

                    header.StartLocation = startPos;

                    //bool showDateHeader = false;

                    ////Show date or hide
                    //if (lastHeader == null)
                    //    showDateHeader = true;
                    //else if (header.Time.Hour < lastHeader.Time.Hour)
                    //    showDateHeader = true;
                    //else if (header.Time.Minute == lastHeader.Time.Minute) showDateHeader = true;

                    //Show date

                    var day = header.HeaderTextInsteadOfTime.Length > 0
                            ? header.HeaderTextInsteadOfTime
                            : header.Time.ToString("dd");

                    var month = header.HeaderTextInsteadOfTime.Length > 0
                            ? header.HeaderTextInsteadOfTime
                            : header.Time.ToString("MMMMM");

                    //measure header text
                    var mesHText = gfx.MeasureString(day, DateFont);
                    var hgHText = mesHText.Height;

                    dynamic fontSize = FromDate.Day == dayMax ? 7 : 10;
                    var lineWdth = 2;
                    if (fontSize == 8) lineWdth = 1;

                    // Searching for next month, draws current month

                    while (curMonth != month)
                    {
                        //Draws month title

                        var pen = new Pen(Color.LightGray, lineWdth);

                        gfx.DrawLine(pen, startPos, 10, startPos, HeaderHeight);
                        gfx.DrawLine(pen, startPos, 10, startPos + 5, 10);

                        gfx.DrawString(month, new Font("Bahnschrift", fontSize, FontStyle.Regular), 
                            Brushes.Black, startPos + 5, HeaderHeight - hgHText - 36);

                        curMonth = month;

                        break;
                    }

                    gfx.DrawString(day, DateFont, Brushes.Black, startPos + 6, HeaderHeight - hgHText - 2);

                    gfx.DrawString(header.HeaderText,
                        TimeFont,
                        Brushes.Black,
                        startPos, HeaderTimeStartTop);

                    index += 1;
                }

                HeaderList = headerList;
                _widthPerItem = (Width - 10 - _barStartLeft - BarStartRight) / HeaderList.Count;

                break;
            }
        }

        /// <summary>
        /// The GetScrollHeigh
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public int GetScrollHeigh()
        {
            _barsViewable = (Height - BarStartTop) / (_barHeight + _barSpace);

            var barCount = GetBarIdxByRowText();
            if (barCount == 0)
                return 0;

            var maxHeight = Height;
            var scrollHeight = maxHeight / barCount * _barsViewable;

            return scrollHeight;
        }

        /// <summary>
        /// The DrawScrollBar
        /// </summary>
        /// <param name="grfx">The grfx<see cref="Graphics"/></param>
        private void DrawScrollBar(Graphics grfx)
        {

            _barsViewable = (Height - BarStartTop) / (_barHeight + _barSpace);
            var barCount = GetBarIdxByRowText();
            if (barCount == 0)
                return;

            var maxHeight = Height;
            var scrollHeight = GetScrollHeigh();

            // If the scroll area is filled there's no need to show the scrollbar

            if (scrollHeight >= maxHeight || scrollHeight > Height)
                return;

            decimal divideBy = barCount - _barsViewable;
            if (divideBy == 0) divideBy = 1;

            var scrollSpeed = (maxHeight - scrollHeight) / divideBy;

            // Draws the scroll area
            _scrollBarArea = new Rectangle(Width - 15, 19, 15, maxHeight + 10);
            grfx.FillRectangle(ChartBackBrush, _scrollBarArea);

            _scroll = new Rectangle(Width - 15, (int)(ScrollPosition * scrollSpeed), 15, scrollHeight);

            if (_scroll.Height == 0) return;

            // Draws the actual scrollbar
            grfx.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), _scroll);
        }

        /// <summary>
        /// The GetBarIdxByRowText
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public int GetBarIdxByRowText()
        {
            var index = -1;

            for (var i = 0; i <= Bars.Count - 1; i++)
            {
                var bar = Bars[i];

                if (bar.RowIndex > index) index = bar.RowIndex;
            }

            return index + 1;
        }

        /// <summary>
        /// The SetBarStartLeft
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        private void SetBarStartLeft(string rowText)
        {
            var gfx = CreateGraphics();

            var length = (int)gfx.MeasureString(rowText, RowFont, 500).Width;

            if (length > _barStartLeft) _barStartLeft = length;
        }

        /// <summary>
        /// Adds the bars.
        /// </summary>
        /// <param name="rowText">The row text.</param>
        /// <param name="chain">The chain<see cref="string"/></param>
        /// <param name="barValue">The bar value.</param>
        /// <param name="fromTime">From time.</param>
        /// <param name="toTime">To time.</param>
        /// <param name="prodFromTime">The product from time.</param>
        /// <param name="prodToTime">The product to time.</param>
        /// <param name="delayFromTime">The delayFromTime<see cref="DateTime"/></param>
        /// <param name="delayToTime">The delayToTime<see cref="DateTime"/></param>
        /// <param name="color">The color.</param>
        /// <param name="hoverColor">Color of the hover.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="isRoot">if set to <c>true</c> [is root].</param>
        /// <param name="tag">The tag.</param>
        /// <param name="expanded">if set to <c>true</c> [expanded].</param>
        /// <param name="toggle">The toggle.</param>
        /// <param name="fixQty">The fixQty<see cref="int"/></param>
        /// <param name="dailyQty">The dailyQty<see cref="int"/></param>
        /// <param name="prodQty">The prodQty<see cref="int"/></param>
        /// <param name="prodOverStartValue">The prodOverStartValue<see cref="DateTime"/></param>
        /// <param name="prodOverEndValue">The prodOverEndValue<see cref="DateTime"/></param>
        /// <param name="locked">The locked<see cref="bool"/></param>
        /// <param name="prodLocked">The prodLocked<see cref="bool"/></param>
        /// <param name="closed">The closed<see cref="bool"/></param>
        /// <param name="prodColor">The prodColor<see cref="Color"/></param>
        /// <param name="art">The art<see cref="string"/></param>
        public void AddBars(string rowText, string chain, object barValue,
            DateTime fromTime, DateTime toTime, DateTime prodFromTime, DateTime prodToTime, DateTime delayFromTime, DateTime delayToTime,
            Color color, Color hoverColor, int rowIndex, bool isRoot, string tag, bool expanded, Image toggle, int fixQty, int dailyQty, int prodQty,
            DateTime prodOverStartValue, DateTime prodOverEndValue, bool locked, bool prodLocked, bool closed, Color prodColor, string art, string dept)
        {
            if (string.IsNullOrEmpty(rowText)) return;

            var bar = new BarProperty
            {
                RowText = rowText,
                Chain = chain,
                Value = barValue,
                StartValue = fromTime,
                EndValue = toTime,
                ProdStartValue = prodFromTime,
                ProdEndValue = prodToTime,
                DelayStartValue = delayFromTime,
                DelayEndValue = delayToTime,
                Color = color,
                HoverColor = hoverColor,
                RowIndex = rowIndex,
                IsRoot = isRoot,
                Tag = tag,
                Expanded = expanded,
                Toggle = toggle,
                FixQty = fixQty,
                DailyQty = dailyQty,
                ProdQty = prodQty,
                ProdOverStartValue = prodOverStartValue,
                ProdOverEndValue = prodOverEndValue,
                Locked = locked,
                LockedProd = prodLocked,
                ClosedOrd = closed,
                ProdColor = prodColor,
                Article = art,
                Department=dept,
            };

            Bars.Add(bar);

            SetBarStartLeft(rowText);
        }

        /// <summary>
        /// Gets the bar start location.
        /// </summary>
        /// <param name="start">Start datetime parameter.</param>
        /// <returns>bar start location if the start parameter is not null</returns>
        private int GetBarStartLocation(DateTime start)
        {
            var timeBetween = HeaderList[1].Time - HeaderList[0].Time;
            var minutesBetween = (int)timeBetween.TotalMinutes;
            dynamic widthBetween = HeaderList[1].StartLocation - HeaderList[0].StartLocation;

            var perMinute = (float)widthBetween / minutesBetween;

            // Calculates where the bar start point should be located
            var startTimeSpan = start - FromDate;
            var startMinutes = startTimeSpan.Days * 1440 + startTimeSpan.Hours * 60 + startTimeSpan.Minutes;
            var startLocation = (int)(perMinute * startMinutes);

            return startLocation;
        }

        /// <summary>
        /// Gets the bar end location.
        /// </summary>
        /// <param name="start">The start<see cref="DateTime"/></param>
        /// <param name="end">End datetime parameter.</param>
        /// <returns>bar end location if the end parameter is not null</returns>
        private int GetBarEndLocation(DateTime start, DateTime end)
        {
            var timeBetween = HeaderList[1].Time - HeaderList[0].Time;
            var minutesBetween = (int)timeBetween.TotalMinutes;
            dynamic widthBetween = HeaderList[1].StartLocation - HeaderList[0].StartLocation;
            var perMinute = (float)widthBetween / minutesBetween;

            var endTimeSpan = end - start;
            var lengthMinutes = endTimeSpan.Days * 1440 + endTimeSpan.Hours * 60 + endTimeSpan.Minutes;
            var endLocation = (int)(perMinute * lengthMinutes);

            return endLocation;
        }

        /// <summary>
        /// The DrawBars
        /// </summary>
        /// <param name="grfx">The grfx<see cref="Graphics"/></param>
        /// <param name="ignoreScrollAndMousePosition">The ignoreScrollAndMousePosition<see cref="bool"/></param>
        private void DrawBars(Graphics grfx, bool ignoreScrollAndMousePosition = false)
        {
            grfx.SmoothingMode = SmoothingMode.AntiAlias;
            var geo = new Geometry();
            foreach (var bar in Bars)
            {                
                if (FilteredRowText != string.Empty && bar.RowText != FilteredRowText) continue;

                if (Central.IsActiveOrdersSelection && bar.ProdQty == bar.FixQty || Central.IsActiveOrdersSelection && bar.ProdQty == 0) continue;

                var index = bar.RowIndex;
                var scrollPos = ScrollPosition;

                //progbar
                var x = _barStartLeft + GetBarStartLocation(bar.StartValue);
                var y = BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 5;
                var w = GetBarEndLocation(bar.StartValue, bar.EndValue);
                int h = _barHeight - 25;
                var dx = _barStartLeft + GetBarStartLocation(bar.DelayStartValue);
                var dy = BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 5;
                var dw = GetBarEndLocation(bar.DelayStartValue, bar.DelayEndValue);
                int dh = _barHeight - 25;

                //var bh = 0;
                //if (_barHeight < 31) bh = 31 - _barHeight; else bh = _barHeight - 31;
                //prodqty
                var pw = GetBarEndLocation(bar.ProdStartValue, bar.ProdEndValue);
                var px = _barStartLeft + GetBarStartLocation(bar.ProdStartValue);
                var py = y + 17;// + bh + 1;
                var ph = _barHeight - 30;
                if (ph <= 0)
                {
                    ph = 2; py -= 7;
                }
                //overqty
                var ow = GetBarEndLocation(bar.ProdOverStartValue, bar.ProdOverEndValue);
                var ox = _barStartLeft + GetBarStartLocation(bar.ProdOverStartValue) + 1;
                var oy = y + 20;// + bh + 1;
                var oh = _barHeight - 30;

                //prod + overqty
                var findCenterByY = py + (ph / 2 - 5);

                var barColor = bar.Color;

                // Test to see does mouse is over bar to set hovercolor
                if ((MouseOverRowText == bar.RowText) & (bar.StartValue <= MouseOverColumnDate) &
                    (bar.EndValue >= MouseOverColumnDate))
                {
                    barColor = bar.HoverColor;
                }                  

                bar.TopLocation.Left = new Point(x, y);
                bar.TopLocation.Right = new Point(x + w, y);
                bar.BottomLocation.Left = new Point(x, y + h);
                bar.BottomLocation.Right = new Point(x, y + h);
                bar.DelayTopLocation.Left = new Point(dx, dy);
                bar.DelayTopLocation.Right = new Point(dx + dw, dy);
                bar.DelayBottomLocation.Left = new Point(dx, dy + dh);
                bar.DelayBottomLocation.Right = new Point(dx, dy + dh);
                bar.ProdTopLocation.Left = new Point(px, py);
                bar.ProdTopLocation.Right = new Point(px + pw, py);
                bar.ProdBottomLocation.Left = new Point(px, py + ph);
                bar.ProdBottomLocation.Right = new Point(px, py + ph);

                // Creates bars' rectangles
                if (w <= 0) continue;

                var barRect = new Rectangle(x, y, w, h + 1);
                var delayBarRect = new Rectangle(dx, dy, dw, dh + 1);
                var prodBarRect = new Rectangle(px, py, pw, ph + 2);
                var prodOverBarRect = new Rectangle(ox, oy, ow, oh + 2);

                if (((index >= scrollPos) & (index < _barsViewable + scrollPos)) |
                         ignoreScrollAndMousePosition)
                {
                    var borderPen = new Pen(Brushes.White, 1);

                    if (HideClosedTask && barColor != Color.FromArgb(80, 144, 169) && barColor != Color.FromArgb(27, 98, 124)) continue;

                    grfx.FillPath(new SolidBrush(barColor), geo.RoundedRectanglePath(barRect, 7));
                    grfx.DrawPath(borderPen, geo.RoundedRectanglePath(barRect, 7));

                    if (!HideDelay)
                    {
                        if (bar.DelayStartValue != DateTime.MinValue && bar.DelayEndValue != DateTime.MinValue && bar.DelayStartValue < bar.DelayEndValue)
                        {
                            grfx.FillPath(new SolidBrush(Color.FromArgb(255,155,63)), geo.RoundedRectanglePath(delayBarRect, 7));
                            grfx.DrawPath(borderPen, geo.RoundedRectanglePath(delayBarRect, 7));
                        }
                    }
                    grfx.FillPath(new SolidBrush(bar.ProdColor), geo.RoundedRectanglePath(prodBarRect, 7));
                    grfx.DrawPath(borderPen, geo.RoundedRectanglePath(prodBarRect, 7));

                    // Draws bar text
                    string rowTxt;
                    string prodRowTxt;
                    if (Central.IsArticleSelection)
                        rowTxt = bar.Article;
                    else
                        rowTxt = bar.RowText;

                    prodRowTxt = rowTxt;

                    if (Central.IsQtySelection)
                    {
                        rowTxt = bar.FixQty.ToString();
                        prodRowTxt = bar.ProdQty.ToString();
                    }
                    else
                    {
                        rowTxt = bar.RowText;
                        prodRowTxt = rowTxt;
                    }
                    
                    var barTextWidth = grfx.MeasureString(rowTxt, _barFont).Width;

                    var brshProdText = bar.ProdColor == Color.FromArgb(175, 175, 175) ? Brushes.DimGray : Brushes.WhiteSmoke;

                    if (_barHeight >= 36)
                    {
                        if (!Central.IsArticleSelection)
                        {
                            //bar
                            if (w > 25)
                                grfx.DrawString(bar.EndValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, (x + w) - 34, y + 2);
                            if (w > 60)
                                grfx.DrawString(bar.StartValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, x, y + 2);
                            //delay
                            if (!HideDelay)
                            {
                                if (dw > 25)
                                    grfx.DrawString(bar.DelayEndValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, (dx + dw) - 34, dy + 2);
                                if (dw > 60)
                                    grfx.DrawString(bar.DelayStartValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, dx, dy + 2);
                            }
                            //production
                            if (pw > 25)
                                grfx.DrawString(bar.ProdEndValue.ToString("dd/MM"), _barFont, brshProdText, (px + pw) - 34, findCenterByY);
                            if (pw > 60)
                                grfx.DrawString(bar.ProdStartValue.ToString("dd/MM"), _barFont, brshProdText, px, findCenterByY);
                            //production over
                            if (ow > 25)
                                grfx.DrawString(bar.ProdOverEndValue.ToString("dd/MM"), _barFont, Brushes.OrangeRed, (ox + ow) - 34, findCenterByY);
                            if (ow > 60)
                                grfx.DrawString(bar.ProdOverStartValue.AddDays(+1).ToString("dd/MM"), _barFont, Brushes.OrangeRed, ox, findCenterByY);
                        }
                        if (w > 100)
                        {
                            grfx.DrawString(rowTxt, _barFont, Brushes.WhiteSmoke, new PointF(x + (w / 2) - (barTextWidth / 2), y + 2));

                            if (bar.Locked && bar.LockedProd && bar.Color != Color.DarkOrange)
                                using (Image imgLock = Properties.Resources.padlock_icon_d20 ?? throw new ArgumentNullException(nameof(grfx)))
                                {
                                    var destRect1 = new Rectangle(Convert.ToInt32(x + (w / 2) + (barTextWidth / 2)), y - 1, 20, 20);
                                    const GraphicsUnit units = GraphicsUnit.Pixel;
                                    grfx.DrawImage(imgLock, destRect1, 0, 0, 20, 20, units);
                                }
                        }
                        if (dw > 100)
                        {
                            grfx.DrawString(rowTxt, _barFont, Brushes.WhiteSmoke, new PointF(dx + (dw / 2) - (barTextWidth / 2), dy + 2));
                        }

                        if (pw > 160)
                            grfx.DrawString(prodRowTxt, _barFont, brshProdText,
                                new PointF(px + (pw / 2) - (barTextWidth / 2), findCenterByY));

                        //prod over
                        if (ow > 160)
                            grfx.DrawString(rowTxt, _barFont, Brushes.OrangeRed,
                                new PointF(ox + (ow / 2) - (barTextWidth / 2), findCenterByY));
                    }

                    var lineRect = new Rectangle(new Point(0,
                              BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1), new Size(100, _barHeight + 7));

                    grfx.FillRectangle(new SolidBrush(Color.FromArgb(50, 52, 68)), lineRect);
                    var p2 = new Pen(Brushes.WhiteSmoke, 2);
                    var p3 = new Pen(Brushes.WhiteSmoke, 3);
                    grfx.DrawLine(p2, lineRect.X, lineRect.Y + lineRect.Height, lineRect.Width, lineRect.Y + lineRect.Height);
                    grfx.DrawLine(p3, lineRect.X + lineRect.Width, lineRect.Y, lineRect.X + lineRect.Width, lineRect.Y + lineRect.Height);
                    p2.Dispose();
                    p3.Dispose();

                    // Draws sub-task image
                    using (Image taskImg = Properties.Resources.folder_icon_32_gold ??
                                           throw new ArgumentNullException(nameof(grfx)))
                    {
                        var destRect1 = new Rectangle(7,
                            (BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + _barHeight / 2 - 7, 20, 20);
                        const GraphicsUnit units = GraphicsUnit.Pixel;

                        grfx.DrawImage(taskImg, destRect1, 0, 0, 30, 30, units);
                    }

                    var desc = (from line in Central.ListOfLines
                                where line.Line == bar.Tag && line.Department == bar.Department
                                select line).SingleOrDefault().Description;

                    //var i = 0;
                    if (Store.Default.sectorId == 1)
                    {
                        var strLine = "LINEA ";
                        var lineNum = bar.Tag.Remove(0, 5);
                        var deptChar = bar.Department == "Confezione A" 
                            || bar.Department == "Confezione B" ? bar.Department.Split(' ')[1] : "";
                        var rowTitle = strLine + lineNum + deptChar;

                        // Draws strings as row texts/values
                        grfx.DrawString(new string(' ', 10) + rowTitle, new Font("Bahnschrift", 9, FontStyle.Regular),
                            new SolidBrush(Color.FromArgb(242,242,242)), 1,
                            ((BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + _barHeight / 2 - 2));
                        
                        grfx.DrawString(new string(' ', 16) + desc, new Font("Bahnschrift", 6, FontStyle.Regular),
                       new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                       ((BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + _barHeight / 2 + 12));

                    }
                    else
                    {
                        grfx.DrawString(new string(' ', 10) + bar.Tag, new Font("Bahnschrift", 9, FontStyle.Regular),
                           new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                           (BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + _barHeight / 2 - 2);


                        grfx.DrawString(new string(' ', 16) + desc, new Font("Bahnschrift", 6, FontStyle.Regular),
                      new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                      ((BarStartTop + _barHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + _barHeight / 2 + 12));
                    }
                }

                // Draws the scroll area
                var maxHeight = Height;
                _scrollBarArea = new Rectangle(Width - 57, 0, 57, maxHeight + 10);
                grfx.FillRectangle(new SolidBrush(Color.WhiteSmoke), _scrollBarArea);
            }

            grfx.SmoothingMode = SmoothingMode.HighSpeed;
            grfx.CompositingMode = CompositingMode.SourceCopy;
            grfx.CompositingQuality = CompositingQuality.HighSpeed;
        }

        /// <summary>
        /// The PaintChart
        /// </summary>
        private void PaintChart()
        {
            Invalidate();
        }      
        /// <summary>
        /// The PaintChart
        /// </summary>
        /// <param name="gfx">The gfx<see cref="Graphics"/></param>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        private void PaintChart(Graphics gfx)
        {
            gfx.Clear(ChartBackColor);

            DrawScrollBar(gfx);
            DrawHeader(gfx, null);
            DrawNetHorizontal(gfx);
            DrawNetVertical(gfx);
            DrawBars(gfx);

            //~2300ms cpu cycles
            //gfx.CompositingMode = CompositingMode.SourceOver;
            //gfx.CompositingQuality = CompositingQuality.HighSpeed;
            gfx.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            gfx.CompositingMode = CompositingMode.SourceOver;
            gfx.SmoothingMode = SmoothingMode.AntiAlias;
        }

        /// <summary>
        /// The OnPaint
        /// </summary>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            PaintChart(e.Graphics);
            base.OnPaint(e);
        }

        /// <summary>
        /// The DrawNetVertical
        /// </summary>
        /// <param name="grfx">The grfx<see cref="Graphics"/></param>
        private void DrawNetVertical(Graphics grfx)
        {
            if (HeaderList == null)
                return;
            if (HeaderList.Count == 0)
                return;

            var index = 0;
            //_ = Color.FromArgb(150, 136, 239, 92);
            var ColHeaderWeekendColor = Color.FromArgb(150, 248, 220, 63);
            var ColumnWeekendColor = Color.FromArgb(70, 201, 201, 200);

            foreach (var header in HeaderList)
            {
                var headerLocationY = HeaderTimeStartTop;

                //if (lastHeader == null) headerLocationY = 0;
                //else if (header.Time.Hour < lastHeader.Time.Hour)
                //    headerLocationY = 0;
                //else
                //    headerLocationY = HeaderTimeStartTop;

                if (header.Time.DayOfWeek == DayOfWeek.Saturday | header.Time.DayOfWeek == DayOfWeek.Sunday)
                {
                    grfx.FillRectangle(new SolidBrush(ColHeaderWeekendColor),
                       _barStartLeft + index * _widthPerItem + 1,  //x
                      headerLocationY,    //y
                       _widthPerItem,         //width
                       HeaderHeight - 20); //height

                    grfx.FillRectangle(new SolidBrush(ColumnWeekendColor),
                        _barStartLeft + index * _widthPerItem + 1,  //x
                        headerLocationY + HeaderHeight - 20,    //y
                        _widthPerItem,         //width
                        _lastLineStop - 50); //height
                }

                if (header.Time.Date == DateTime.Now.Date)
                {
                    var pen = new Pen(Brushes.Coral, 2)
                    {
                        DashStyle = DashStyle.Solid,
                        DashOffset = 0.5f
                    };

                    grfx.DrawRectangle(pen,
                        _barStartLeft + index * _widthPerItem + 1,  //x
                        headerLocationY + HeaderHeight - 20,    //y
                        _widthPerItem,         //width
                        _lastLineStop - 50); //height
                }

                var drawHldCount = 0;
                foreach (var item in Central.ListOfHolidays)
                {
                    var isHoliday = header.Time.Date == item.Holiday;

                    foreach (var bar in Bars)
                    {
                        if (isHoliday) //&& bar.Tag == item.Line)
                        {
                            //if (item.Line != bar.Tag) continue; //|| item.Line != "LINEA 1B") continue;
                            if (drawHldCount > 0) break;
                            grfx.FillRectangle(new SolidBrush(Color.FromArgb(100, 249, 139, 49)),
                          _barStartLeft + index * _widthPerItem + 1,  //x
                          HeaderTimeStartTop + HeaderHeight - 20,    //y
                          _widthPerItem - 2,         //width
                           _lastLineStop - 50); //_barHeight + _lastLineStop - 80); //height
                            drawHldCount++;
                        }
                    }
                }

                grfx.DrawLine(GridColor, _barStartLeft + index * _widthPerItem, headerLocationY,
                   _barStartLeft + index * _widthPerItem, _lastLineStop);

                index += 1;

                //int availableWidth = Width - 10 - barStartLeft - barStartRight;
                Header lastHeader = header;
            }

            grfx.DrawLine(GridColor, _barStartLeft + index * _widthPerItem, HeaderTimeStartTop,
                _barStartLeft + index * _widthPerItem, _lastLineStop);
        }

        /// <summary>
        /// The DrawNetHorizontal
        /// </summary>
        /// <param name="grfx">The grfx<see cref="Graphics"/></param>
        private void DrawNetHorizontal(Graphics grfx)
        {
            if (HeaderList == null)
                return;
            if (HeaderList.Count == 0)
                return;

            int index;
            var width = _widthPerItem * HeaderList.Count + _barStartLeft;

            for (index = 0; index <= GetBarIdxByRowText(); index++)
            {
                foreach (var bar in Bars)
                {
                    grfx.DrawLine(GridColor, 0, BarStartTop + _barHeight * index + _barSpace * index, width,
                    BarStartTop + _barHeight * index + _barSpace * index);
                }
            }

            _lastLineStop = BarStartTop + _barHeight * (index - 1) + _barSpace * (index - 1);
        }

        /// <summary>
        /// The Chart_MouseMove
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="MouseEventArgs"/></param>
        public void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (HeaderList == null)
                return;
            if (HeaderList.Count == 0)
                return;

            //if (e.Button != MouseButtons.Left)
            //{
            //    // If bar has changed manually, but left mouse button is no longer pressed the BarChanged event will be raised

            //    if (AllowManualEditBar)
            //        if (_barIsChanging >= 0)
            //        {
            //            if (AllowChange)
            //            {
            //                BarChanged?.Invoke(Bars[_barIsChanging].Value, new EventArgs());

            //                var barInfo = Bars[_barIsChanging].Value as Bar;
            //                var tempValue = MouseOverColumnDate;
            //                switch (_mouseHover)
            //                {
            //                    case MouseOver.BarLeftSide:
            //                        if (barInfo != null) barInfo.FromTime = tempValue;
            //                        break;
            //                    case MouseOver.BarRightSide:
            //                        if (barInfo != null) barInfo.ToTime = tempValue;
            //                        break;
            //                    case MouseOver.Empty:
            //                        break;
            //                    case MouseOver.Bar:
            //                        break;
            //                    default:
            //                        throw new ArgumentOutOfRangeException();
            //                }
            //            }

            //            _barIsChanging = -1;
            //        }

            //    _mouseHover = MouseOver.Empty;
            //}

            _mouseHoverBarIndex = -1;

            var LocalMousePosition = PointToClient(Cursor.Position);

            // Finds pixels per minute

            var timeBetween = HeaderList[1].Time - HeaderList[0].Time;
            var minutesBetween = timeBetween.Days * 1440 + timeBetween.Hours * 60 + timeBetween.Minutes;
            dynamic widthBetween = HeaderList[1].StartLocation - HeaderList[0].StartLocation;
            var perMinute = (float)widthBetween / minutesBetween;

            if (perMinute == 0) perMinute = 1;

            // Finds the time at mousepointer

            if (LocalMousePosition.X > _barStartLeft)
            {
                var minutesAtCursor = (int)((LocalMousePosition.X - _barStartLeft) / perMinute);
                MouseOverColumnDate = FromDate.AddMinutes(minutesAtCursor);
            }
            else
            {
                MouseOverColumnDate = DateTime.MinValue;
            }

            // Finds the row at mousepointer

            var rowText = "";
            object rowValue = null;
            //object prodRowValue = null;

            // Tests to see if the mouse pointer is hovering above the scrollbar

            var scrollBarStatusChanged = false;

            // Tests to see if the mouse is hovering over the scroll

            if ((LocalMousePosition.X > _scroll.Left) & (LocalMousePosition.Y < _scroll.Right) &
                (LocalMousePosition.Y < _scroll.Bottom) & (LocalMousePosition.Y > _scroll.Top))
            {
                if (_mouseHoverScrollBar == false) scrollBarStatusChanged = true;

                _mouseHoverScrollBar = true;
                _mouseHoverScrollBarArea = true;
            }
            else
            {
                if (_mouseHoverScrollBar == false) scrollBarStatusChanged = true;

                _mouseHoverScrollBar = false;
                _mouseHoverScrollBarArea = false;
            }

            // If the mouse is not above the scroll, test if it's over the scroll area (no need to test if it's not above the scroll)

            if (_mouseHoverScrollBarArea == false)
                if ((LocalMousePosition.X > _scrollBarArea.Left) & (LocalMousePosition.Y < _scrollBarArea.Right) &
                    (LocalMousePosition.Y < _scrollBarArea.Bottom) & (LocalMousePosition.Y > _scrollBarArea.Top))
                    _mouseHoverScrollBarArea = false;

            // Tests to see if the mouse pointer is hovering above a bar

            var index = 0;
            IsMouseOverProductionBar = false;

            foreach (var bar in Bars)
            {
                // If the bar is set to be hidden from mouse move, the current bar will be ignored

                if ((LocalMousePosition.Y > bar.ProdTopLocation.Left.Y) &
                    (LocalMousePosition.Y < bar.ProdBottomLocation.Left.Y))
                {
                    if ((LocalMousePosition.X > bar.ProdTopLocation.Left.X) &
                        (LocalMousePosition.X < bar.ProdTopLocation.Right.X))
                    {
                        // If the current bar is the one where the mouse is above, the rowText and rowValue needs to be set correctly

                        if ((_mouseHover != MouseOver.BarLeftSide) &
                            (_mouseHover != MouseOver.BarRightSide)) _mouseHover = MouseOver.Bar;
                        IsMouseOverProductionBar = true;
                        rowText = bar.RowText;
                        rowValue = bar.Value;
                        _mouseHoverBarIndex = index;
                    }
                }
                
                // Mouse pointer needs to be inside the X and Y positions of the bar
                if ((LocalMousePosition.Y > bar.TopLocation.Left.Y) &
                (LocalMousePosition.Y < bar.BottomLocation.Left.Y))
                {
                    if ((LocalMousePosition.X > bar.TopLocation.Left.X) &
                        (LocalMousePosition.X < bar.TopLocation.Right.X))
                    {
                        // If the current bar is the one where the mouse is above, the rowText and rowValue needs to be set correctly
                        //prodRowValue = null;
                        rowText = bar.RowText;
                        rowValue = bar.Value;
                        _mouseHoverBarIndex = index;
                        IsMouseOverProductionBar = false;
                        if ((_mouseHover != MouseOver.BarLeftSide) &
                            (_mouseHover != MouseOver.BarRightSide)) _mouseHover = MouseOver.Bar;

                    }

                    // If mouse pointer is near the edges of the bar it will open up for editing the bar

                    //if (AllowManualEditBar)
                    //{
                    //    var areaSize = 0;

                    //    if (e.Button == MouseButtons.Left) areaSize = 40;

                    //    if ((LocalMousePosition.X > bar.TopLocation.Left.X - areaSize) &
                    //        (LocalMousePosition.X < bar.TopLocation.Left.X + areaSize) &
                    //        (_mouseHover != MouseOver.BarRightSide))
                    //    {
                    //        _mouseHover = MouseOver.BarLeftSide;
                    //        _mouseHoverBarIndex = index;
                    //    }
                    //    else if ((LocalMousePosition.X > bar.TopLocation.Right.X - areaSize) &
                    //             (LocalMousePosition.X < bar.TopLocation.Right.X + areaSize) &
                    //             (_mouseHover != MouseOver.BarLeftSide))
                    //    {
                    //        _mouseHover = MouseOver.BarRightSide;
                    //        _mouseHoverBarIndex = index;
                    //    }
                    //}
                }

                index += 1;
            }

            // Sets the mouseover row value and text

            MouseOverRowText = rowText;
            MouseOverRowValue = rowValue;
            MouseOverToggleValue = rowValue;
            //MouseOverProductionBar = rowValue;
            //Tests to see if the mouse is hovering over toggle space

            IsMouseOverToggle = false;

            //from top-left: loc+9
            //from right: -22
            //from bottom: loc-5

            if ((LocalMousePosition.X >= 0) & (LocalMousePosition.X <= 100)
            & (LocalMousePosition.Y > 0))
            {
                foreach (var bar in Bars)
                {
                    if (!((LocalMousePosition.Y > bar.TopLocation.Left.Y) &
                          (LocalMousePosition.Y < bar.BottomLocation.Left.Y))) continue;
                    if ((LocalMousePosition.X > 0) &
                        (LocalMousePosition.X < 100))
                    {
                        IsMouseOverRowLine = true;
                        //MouseClicked?.Invoke(sender, e);
                    }
                }
                IsMouseOverToggle = true;
            }
            MouseOverNextValue = "";
            MouseOverRowElements = new List<string>();
            if ((LocalMousePosition.X >= 100) & (LocalMousePosition.X <= Width)
                & (LocalMousePosition.Y > 50))
            {
                foreach (var bar in Bars)
                {
                    if (!((LocalMousePosition.Y > bar.TopLocation.Left.Y) &
                          (LocalMousePosition.Y < bar.BottomLocation.Left.Y + 25))) continue;
                    if ((LocalMousePosition.X > 100) &
                        (LocalMousePosition.X < Width))
                    {
                        MouseOverNextValue = bar.Value;
                        if (!MouseOverRowElements.Contains(((Bar)bar.Value).RowText))
                        {
                            MouseOverRowElements.Add(((Bar)bar.Value).RowText);
                        }
                        //MouseClicked?.Invoke(sender, e);
                    }
                }
            }
            //if (e.Button == MouseButtons.Left)
            //{
            //    MouseDragged?.Invoke(sender, e);
            //}
            //else
            //{
            if (((MouseOverRowValue == null) &
                (rowValue != null)) |
                ((MouseOverRowValue != null) &
                (rowValue == null)) |
                scrollBarStatusChanged)
                PaintChart();
            //}
        }

        //public object RowTextGlobal { get; set; }
        /// <summary>
        /// The Chart_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        public void Chart_MouseLeave(object sender, EventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            MouseOverRowText = null;
            MouseOverRowValue = null;

            _mouseHover = MouseOver.Empty;

            PaintChart();
        }

        /// <summary>
        /// The Chart_MouseDragged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="MouseEventArgs"/></param>
        //public void Chart_MouseDragged(object sender, MouseEventArgs e)
        //{
        //    if (_mouseHoverScrollBarArea) ScrollPosY = e.Location.Y;

        //    if (AllowManualEditBar)
        //    {
        //        if (_mouseHoverBarIndex > -1)
        //        {
        //            if (Bars[_mouseHoverBarIndex].Color != SelectedBarColor ||
        //                SelectedBarIndex != _mouseHoverBarIndex) return;

        //            if (_mouseHover == MouseOver.BarLeftSide)
        //            {
        //                //exception

        //                if (Bars[_mouseHoverBarIndex].Color == Color.Silver) return;

        //                //get bar time span
        //                var startD = Bars[_mouseHoverBarIndex].EndValue.Subtract(Bars[_mouseHoverBarIndex].StartValue).Days;
        //                if (startD < 1 && e.X > Direction)
        //                {
        //                    return;
        //                }

        //                _barIsChanging = _mouseHoverBarIndex;
        //                Bars[_mouseHoverBarIndex].StartValue = MouseOverColumnDate;
        //            }
        //            else if (_mouseHover == MouseOver.BarRightSide)
        //            {
        //                if (Bars[_mouseHoverBarIndex].EndValue <= Bars[_mouseHoverBarIndex].StartValue.AddDays(+1) && e.X < Direction) return;

        //                _barIsChanging = _mouseHoverBarIndex;

        //                Bars[_mouseHoverBarIndex].EndValue = MouseOverColumnDate;
        //            }

        //            PaintChart();
        //        }
        //    }
        //}

        /// <summary>
        /// The Chart_MouseDown
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="MouseEventArgs"/></param>
        public void Chart_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mouseHoverBarIndex > -1)
            {
                SelectedBarColor = Bars[_mouseHoverBarIndex].Color;
                Direction = e.X;
                SelectedBarIndex = _mouseHoverBarIndex;
            }
        }

        /// <summary>
        /// The SaveImage
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        public void SaveImage(string filePath)
        {
            _objGraphics.SmoothingMode = SmoothingMode.HighQuality;
            _objGraphics.InterpolationMode = InterpolationMode.High;
            _objGraphics.Clear(ChartBackColor);

            DrawHeader(_objGraphics, null);
            DrawNetVertical(_objGraphics);
            DrawNetHorizontal(_objGraphics);

            DrawBars(_objGraphics, true);

            var encoderParameters =
                new EncoderParameters(1) { Param = { [0] = new EncoderParameter(Encoder.Quality, 100L) } };

            _objBmp.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        /// <summary>
        /// The InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Ganttochart
            // 
            this.Name = "Ganttochart";
            this.Size = new System.Drawing.Size(10, 10);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// The GetEncoder
        /// </summary>
        /// <param name="format">The format<see cref="ImageFormat"/></param>
        /// <returns>The <see cref="ImageCodecInfo"/></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Defines the <see cref="Header" />
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Gets or sets the HeaderText
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the StartLocation
        /// </summary>
        public int StartLocation { get; set; }

        /// <summary>
        /// Gets or sets the HeaderTextInsteadOfTime
        /// </summary>
        public string HeaderTextInsteadOfTime { get; set; }

        /// <summary>
        /// Gets or sets the Time
        /// </summary>
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="BarProperty" />
    /// </summary>
    public class BarProperty
    {
        /// <summary>
        /// Gets or sets the StartValue
        /// </summary>
        public DateTime StartValue { get; set; }

        /// <summary>
        /// Gets or sets the EndValue
        /// </summary>
        public DateTime EndValue { get; set; }

        /// <summary>
        /// Gets or sets the ProdStartValue
        /// </summary>
        public DateTime ProdStartValue { get; set; }

        /// <summary>
        /// Gets or sets the ProdEndValue
        /// </summary>
        public DateTime ProdEndValue { get; set; }

        /// <summary>
        /// Gets or sets the ProdOverStartValue
        /// </summary>
        public DateTime ProdOverStartValue { get; set; }

        /// <summary>
        /// Gets or sets the ProdOverEndValue
        /// </summary>
        public DateTime ProdOverEndValue { get; set; }

        /// <summary>
        /// Gets or sets the DelayStartValue
        /// </summary>
        public DateTime DelayStartValue { get; set; }

        /// <summary>
        /// Gets or sets the DelayEndValue
        /// </summary>
        public DateTime DelayEndValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Locked
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether LockedProd
        /// </summary>
        public bool LockedProd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ClosedOrd
        /// </summary>
        public bool ClosedOrd { get; set; }

        /// <summary>
        /// Gets or sets the Rdd
        /// </summary>
        public DateTime Rdd { get; set; }

        /// <summary>
        /// Gets or sets the DvcValue
        /// </summary>
        public DateTime DvcValue { get; set; }

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the HoverColor
        /// </summary>
        public Color HoverColor { get; set; }

        /// <summary>
        /// Gets or sets the RowText
        /// </summary>
        public string RowText { get; set; }

        /// <summary>
        /// Gets or sets the Chain
        /// </summary>
        public string Chain { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the RowIndex
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the FixQty
        /// </summary>
        public int FixQty { get; set; }

        /// <summary>
        /// Gets or sets the DailyQty
        /// </summary>
        public int DailyQty { get; set; }

        /// <summary>
        /// Gets or sets the ProdQty
        /// </summary>
        public int ProdQty { get; set; }

        /// <summary>
        /// Gets or sets the ProdColor
        /// </summary>
        public Color ProdColor { get; set; }

        /// <summary>
        /// Gets or sets the Article
        /// </summary>
        public string Article { get; set; }

        //public bool HideFromMouseMove { get; set; } = false;
        /// <summary>
        /// Gets or sets a value indicating whether IsRoot
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether Expanded
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets the Toggle
        /// </summary>
        public Image Toggle { get; set; }

        /// <summary>
        /// Gets or sets the TopLocation
        /// </summary>
        internal Location TopLocation { get; set; } = new Location();

        /// <summary>
        /// Gets or sets the BottomLocation
        /// </summary>
        internal Location BottomLocation { get; set; } = new Location();

        /// <summary>
        /// Gets or sets the DelayTopLocation
        /// </summary>
        internal Location DelayTopLocation { get; set; } = new Location();

        /// <summary>
        /// Gets or sets the DelayBottomLocation
        /// </summary>
        internal Location DelayBottomLocation { get; set; } = new Location();

        /// <summary>
        /// Gets or sets the ProdTopLocation
        /// </summary>
        internal Location ProdTopLocation { get; set; } = new Location();

        /// <summary>
        /// Gets or sets the ProdBottomLocation
        /// </summary>
        internal Location ProdBottomLocation { get; set; } = new Location();

        /// <summary>
        /// Defines the <see cref="Location" />
        /// </summary>
        internal class Location
        {
            /// <summary>
            /// Gets or sets the Left
            /// </summary>
            public Point Left { get; set; }

            /// <summary>
            /// Gets or sets the Right
            /// </summary>
            public Point Right { get; set; }
        }
    }

    /// <summary>
    /// Defines the <see cref="Bar" />
    /// </summary>
    public class Bar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        public Bar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        /// <param name="isRoot">The isRoot<see cref="bool"/></param>
        /// <param name="expanded">The expanded<see cref="bool"/></param>
        /// <param name="toggle">The toggle<see cref="Image"/></param>
        public Bar(string rowText, int index, bool isRoot, bool expanded, Image toggle)
        {
            RowText = rowText;
            Index = index;
            IsRoot = isRoot;
            Expanded = expanded;
            Toggle = toggle;
        }

        //comesa, tessYarn, tessInit, tessRdd, Color.Silver, Color.WhiteSmoke, index
        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        /// <param name="fromTime">The fromTime<see cref="DateTime"/></param>
        /// <param name="toTime">The toTime<see cref="DateTime"/></param>
        /// <param name="rdd">The rdd<see cref="DateTime"/></param>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <param name="hoverColor">The hoverColor<see cref="Color"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        public Bar(string rowText, DateTime fromTime, DateTime toTime, DateTime rdd, Color color, Color hoverColor, int index)
        {
            RowText = rowText;
            FromTime = fromTime;
            ToTime = toTime;
            ToRealTime = rdd;
            Color = color;
            HoverColor = hoverColor;
            Index = index;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        /// <param name="fromTime">The fromTime<see cref="DateTime"/></param>
        /// <param name="toTime">The toTime<see cref="DateTime"/></param>
        /// <param name="rdd">The rdd<see cref="DateTime"/></param>
        /// <param name="dvc">The dvc<see cref="DateTime"/></param>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <param name="hoverColor">The hoverColor<see cref="Color"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        public Bar(string rowText, DateTime fromTime, DateTime toTime, DateTime rdd, DateTime dvc, Color color, Color hoverColor, int index)
        {
            RowText = rowText;
            FromTime = fromTime;
            ToTime = toTime;
            ToRealTime = rdd;
            ToDvc = dvc;
            Color = color;
            HoverColor = hoverColor;
            Index = index;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        /// <param name="chain">The chain<see cref="string"/></param>
        /// <param name="fromTime">The fromTime<see cref="DateTime"/></param>
        /// <param name="toTime">The toTime<see cref="DateTime"/></param>
        /// <param name="prodFromTime">The prodFromTime<see cref="DateTime"/></param>
        /// <param name="prodToTime">The prodToTime<see cref="DateTime"/></param>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <param name="hoverColor">The hoverColor<see cref="Color"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        /// <param name="isRoot">The isRoot<see cref="bool"/></param>
        /// <param name="tag">The tag<see cref="string"/></param>
        /// <param name="fixQty">The fixQty<see cref="int"/></param>
        /// <param name="dailyQty">The dailyQty<see cref="int"/></param>
        /// <param name="prodQty">The prodQty<see cref="int"/></param>
        public Bar(string rowText, string chain, DateTime fromTime, DateTime toTime, DateTime prodFromTime, DateTime prodToTime, Color color, Color hoverColor, int index, bool isRoot, string tag, int fixQty, int dailyQty, int prodQty)
        {
            RowText = rowText;
            Chain = chain;
            FromTime = fromTime;
            ToTime = toTime;
            ProdFromTime = prodFromTime;
            ProdToTime = prodToTime;
            Color = color;
            HoverColor = hoverColor;
            Index = index;
            IsRoot = isRoot;
            Tag = tag;
            FixedQty = fixQty;
            DailyQty = dailyQty;
            ProductionQty = prodQty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        /// <param name="chain">The chain<see cref="string"/></param>
        /// <param name="fromTime">The fromTime<see cref="DateTime"/></param>
        /// <param name="toTime">The toTime<see cref="DateTime"/></param>
        /// <param name="prodFromTime">The prodFromTime<see cref="DateTime"/></param>
        /// <param name="prodToTime">The prodToTime<see cref="DateTime"/></param>
        /// <param name="delayStart">The delayStart<see cref="DateTime"/></param>
        /// <param name="delayEnd">The delayEnd<see cref="DateTime"/></param>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <param name="hoverColor">The hoverColor<see cref="Color"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        /// <param name="isRoot">The isRoot<see cref="bool"/></param>
        /// <param name="tag">The tag<see cref="string"/></param>
        /// <param name="fixQty">The fixQty<see cref="int"/></param>
        /// <param name="dailyQty">The dailyQty<see cref="int"/></param>
        /// <param name="prodQty">The prodQty<see cref="int"/></param>
        /// <param name="prodOverFromTime">The prodOverFromTime<see cref="DateTime"/></param>
        /// <param name="prodOverToTime">The prodOverToTime<see cref="DateTime"/></param>
        /// <param name="locked">The locked<see cref="bool"/></param>
        /// <param name="prodLocked">The prodLocked<see cref="bool"/></param>
        /// <param name="closed">The closed<see cref="bool"/></param>
        /// <param name="prodColor">The prodColor<see cref="Color"/></param>
        /// <param name="art">The art<see cref="string"/></param>
        public Bar(string rowText, string chain, DateTime fromTime, DateTime toTime, DateTime prodFromTime, DateTime prodToTime, DateTime delayStart, DateTime delayEnd,
            Color color, Color hoverColor, int index, bool isRoot, string tag,
            int fixQty, int dailyQty, int prodQty, DateTime prodOverFromTime, DateTime prodOverToTime, bool locked, bool prodLocked, bool closed, Color prodColor, string art,string dept)
        {
            RowText = rowText;
            Chain = chain;
            FromTime = fromTime;
            ToTime = toTime;
            ProdFromTime = prodFromTime;
            ProdToTime = prodToTime;
            DelayFromTime = delayStart;
            DelayToTime = delayEnd;
            Color = color;
            HoverColor = hoverColor;
            Index = index;
            IsRoot = isRoot;
            Tag = tag;
            FixedQty = fixQty;
            DailyQty = dailyQty;
            ProductionQty = prodQty;
            ProdOverFromTime = prodOverFromTime;
            ProdOverToTime = prodOverToTime;
            Locked = locked;
            LockedProd = prodLocked;
            ClosedOrd = closed;
            ProdColor = prodColor;
            Article = art;
            Department = dept;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bar"/> class.
        /// </summary>
        /// <param name="rowText">The rowText<see cref="string"/></param>
        /// <param name="chain">The chain<see cref="string"/></param>
        /// <param name="fromTime">The fromTime<see cref="DateTime"/></param>
        /// <param name="toTime">The toTime<see cref="DateTime"/></param>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <param name="hoverColor">The hoverColor<see cref="Color"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        /// <param name="isRoot">The isRoot<see cref="bool"/></param>
        /// <param name="tag">The tag<see cref="string"/></param>
        public Bar(string rowText, string chain, DateTime fromTime, DateTime toTime, Color color, Color hoverColor, int index, bool isRoot, string tag)
        {
            RowText = rowText;
            Chain = chain;
            FromTime = fromTime;
            ToTime = toTime;
            Color = color;
            HoverColor = hoverColor;
            Index = index;
            IsRoot = isRoot;
            Tag = tag;
        }

        /// <summary>
        /// Gets or sets the RowText
        /// </summary>
        public string RowText { get; set; }

        /// <summary>
        /// Gets or sets the Chain
        /// </summary>
        public string Chain { get; set; }

        /// <summary>
        /// Gets or sets the FromTime
        /// </summary>
        public DateTime FromTime { get; set; }

        /// <summary>
        /// Gets or sets the ToTime
        /// </summary>
        public DateTime ToTime { get; set; }

        /// <summary>
        /// Gets or sets the ProdFromTime
        /// </summary>
        public DateTime ProdFromTime { get; set; }

        /// <summary>
        /// Gets or sets the ProdToTime
        /// </summary>
        public DateTime ProdToTime { get; set; }

        /// <summary>
        /// Gets or sets the ProdOverFromTime
        /// </summary>
        public DateTime ProdOverFromTime { get; set; }

        /// <summary>
        /// Gets or sets the ProdOverToTime
        /// </summary>
        public DateTime ProdOverToTime { get; set; }

        /// <summary>
        /// Gets or sets the DelayFromTime
        /// </summary>
        public DateTime DelayFromTime { get; set; }

        /// <summary>
        /// Gets or sets the DelayToTime
        /// </summary>
        public DateTime DelayToTime { get; set; }

        /// <summary>
        /// Gets or sets the ToRealTime
        /// </summary>
        public DateTime ToRealTime { get; set; }

        /// <summary>
        /// Gets or sets the ToDvc
        /// </summary>
        public DateTime ToDvc { get; set; }

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the HoverColor
        /// </summary>
        public Color HoverColor { get; set; }

        /// <summary>
        /// Gets or sets the Index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the FixedQty
        /// </summary>
        public int FixedQty { get; set; }

        /// <summary>
        /// Gets or sets the ProductionQty
        /// </summary>
        public int ProductionQty { get; set; }

        /// <summary>
        /// Gets or sets the DailyQty
        /// </summary>
        public int DailyQty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsRoot
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Gets or sets the Article
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Expanded
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets the Toggle
        /// </summary>
        public Image Toggle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Locked
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether LockedProd
        /// </summary>
        public bool LockedProd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ClosedOrd
        /// </summary>
        public bool ClosedOrd { get; set; }

        /// <summary>
        /// Gets or sets the ProdColor
        /// </summary>
        public Color ProdColor { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="TreeNode{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        /// <summary>
        /// Gets or sets the Children
        /// </summary>
        public ICollection<TreeNode<T>> Children { get; set; }

        /// <summary>
        /// Gets the IsRoot
        /// </summary>
        public Boolean IsRoot
        {
            get { return Parent == null; }
        }

        /// <summary>
        /// Gets the IsLeaf
        /// </summary>
        public Boolean IsLeaf
        {
            get { return Children.Count == 0; }
        }

        /// <summary>
        /// Gets the Level
        /// </summary>
        public int Level
        {
            get
            {
                if (IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/></param>
        public TreeNode(T data)
        {
            Data = data;
            Children = new LinkedList<TreeNode<T>>();

            ElementsIndex = new LinkedList<TreeNode<T>>();
            ElementsIndex.Add(this);
        }

        /// <summary>
        /// The AddChild
        /// </summary>
        /// <param name="child">The child<see cref="T"/></param>
        /// <returns>The <see cref="TreeNode{T}"/></returns>
        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            Children.Add(childNode);

            RegisterChildForSearch(childNode);

            return childNode;
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return Data != null ? Data.ToString() : "[data null]";
        }

        /// <summary>
        /// Gets or sets the ElementsIndex
        /// </summary>
        private ICollection<TreeNode<T>> ElementsIndex { get; set; }

        /// <summary>
        /// The RegisterChildForSearch
        /// </summary>
        /// <param name="node">The node<see cref="TreeNode{T}"/></param>
        private void RegisterChildForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        /// <summary>
        /// The FindTreeNode
        /// </summary>
        /// <param name="predicate">The predicate<see cref="Func{TreeNode{T}, bool}"/></param>
        /// <returns>The <see cref="TreeNode{T}"/></returns>
        public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate);
        }

        /// <summary>
        /// The GetEnumerator
        /// </summary>
        /// <returns>The <see cref="IEnumerator"/></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// The GetEnumerator
        /// </summary>
        /// <returns>The <see cref="IEnumerator{TreeNode{T}}"/></returns>
        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            yield return this;

            foreach (var directChild in Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }
    }
}
