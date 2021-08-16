using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1
{
    internal class Ganttchart : UserControl
    {
        private const int HeaderHeight = 50;
        private int _widthPerItem;
        private int _barStartLeft = 100;
        private const int BarStartTop = 49;
        private const int BarStartRight = 20;
        private int _barSpace = 8;
        private int _barsViewable = -1;
        private const int HeaderTimeStartTop = 20;
        private int _lastLineStop;
        private int _mouseHoverBarIndex = -1;
        private bool _mouseHoverScrollBar;
        private bool _mouseHoverScrollBarArea;
        private Bitmap _objBmp;
        private Graphics _objGraphics;
        private Rectangle _scroll, _scrollBarArea;

        private enum MouseOver
        {
            Empty,
            Bar,
            BarLeftSide,
            BarRightSide
        }
        private MouseOver _mouseHover = MouseOver.Empty;

        public Ganttchart()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);

            var resX = Screen.FromControl(this).Bounds.Width;
            var resY = Screen.FromControl(this).Bounds.Height;

            _objBmp = new Bitmap(resX, resY);
            _objGraphics = Graphics.FromImage(_objBmp);

            BarsCount = GetBarIdxByRowText();
            if (Store.Default.sectorId == 7)
            {
                BarHeight = 30;
            }
            else
            {
                BarHeight = 40;
            }

        }

        public List<BarProperty> Bars = new List<BarProperty>();
        public List<LineLabels> LineList = new List<LineLabels>();
        public int ScrollPosition;
        public int BarsCount = 0;
        public int BarHeight = 0;
        public List<Header> HeaderList;

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        private bool AllowManualEditBar { get; set; }
        private bool AllowChange { get; set; }
        public bool HideDelay { get; set; }
        public bool HideClosedTask { get; set; }
        public bool AllowRowSelection { get; set; }
        public bool IsMouseOverRowText { get; set; }
        public bool IsMouseOverRowLine { get; set; }
        public bool IsMouseOverToggle { get; private set; }
        public bool IsMouseOverProductionBar { get; set; }
        public string MouseOverRowText { get; set; }
        public object MouseOverRowValue { get; set; }
        public object MouseOverToggleValue { get; private set; }
        public int MouseOverRowIndex { get; private set; }
        public bool RectangleSelectorActivated { get; set; }
        public object MouseOverNextValue { get; private set; }
        public List<string> MouseOverRowElements { get; private set; }
        public DateTime MouseOverColumnDate { get; set; }
        public object MouseOverProductionBar { get; set; }
        public string FilteredRowText { get; set; }
        public string FilteredRowAtt { get; set; }
        private Pen GridColor { get; } = Pens.LightGray;
        private Pen GridColor1 { get; } = Pens.Red;
        private Color ChartBackColor { get; } = Color.WhiteSmoke;     
        private Brush ChartBackBrush { get; } = new SolidBrush(Color.WhiteSmoke);   
        private readonly Font _barFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
        private Font TimeFont { get; } = new Font("Bahnschrift", 8, FontStyle.Regular);
        private Font RowFont { get; } = new Font("Bahnschrift", 9, FontStyle.Regular);
        private Font DateFont { get; } = new Font("Bahnschrift", 9, FontStyle.Regular);

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

                            if (ScrollPosition + _barsViewable > GetBarIdxByRowText())
                                ScrollPosition = GetBarIdxByRowText() - _barsViewable;

                            PaintChart();
                            return;
                        }
                    }

                    distanceFromLastPosition = distanceFromCurrentPosition;

                    index += 1;
                }
            }
        }

        private Color SelectedBarColor { get; set; }

        private int Direction { get; set; }

        private int SelectedBarIndex { get; set; }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ScrollPosition = 0;

            PaintChart();
        }

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
                }
            }


            return result;
        }

        private void DrawHeader(Graphics gfx, List<Header> headerList)
        {
            while (true)
            {
                if (headerList == null) headerList = GetFullHeaderList();

                if (headerList.Count == 0) return;

                dynamic availableWidth = Width - 10 - _barStartLeft - BarStartRight;
                _widthPerItem = availableWidth / headerList.Count;

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
                gfx.FillRectangle(new SolidBrush(Color.WhiteSmoke), new Rectangle(0, 0, Width - 40, HeaderHeight));

                gfx.DrawString("  Linea", TimeFont, Brushes.Black, 1, 16);
                var curMonth = FromDate.Month.ToString("MMM");    

                foreach (var header in headerList)
                {
                    var startPos = _barStartLeft + index * _widthPerItem;

                    header.StartLocation = startPos;

                    var day = header.HeaderTextInsteadOfTime.Length > 0
                            ? header.HeaderTextInsteadOfTime
                            : header.Time.ToString("dd");

                    var month = header.HeaderTextInsteadOfTime.Length > 0
                            ? header.HeaderTextInsteadOfTime
                            : header.Time.ToString("MMMMM");

                    var mesHText = gfx.MeasureString(day, DateFont);
                    var hgHText = mesHText.Height;

                    dynamic fontSize = FromDate.Day == dayMax ? 7 : 10;
                    var lineWdth = 2;
                    if (fontSize == 8) lineWdth = 1;

                    while (curMonth != month)
                    {
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

        public int GetScrollHeigh()
        {
            _barsViewable = (Height - BarStartTop) / (BarHeight + _barSpace);

            var barCount = GetBarIdxByRowText();
            if (barCount == 0)
                return 0;

            var maxHeight = Height;
            var scrollHeight = maxHeight / barCount * _barsViewable;

            return scrollHeight;
        }

        private void DrawScrollBar(Graphics grfx)
        {

            _barsViewable = (Height - BarStartTop) / (BarHeight + _barSpace);
            var barCount = GetBarIdxByRowText();
            if (barCount == 0)
                return;

            var maxHeight = Height;
            var scrollHeight = GetScrollHeigh();

            if (scrollHeight >= maxHeight || scrollHeight > Height)
                return;

            decimal divideBy = barCount - _barsViewable;
            if (divideBy == 0) divideBy = 1;

            var scrollSpeed = (maxHeight - scrollHeight) / divideBy;

            _scrollBarArea = new Rectangle(Width - 15, 19, 15, maxHeight + 10);
            grfx.FillRectangle(ChartBackBrush, _scrollBarArea);

            _scroll = new Rectangle(Width - 15, (int)(ScrollPosition * scrollSpeed), 15, scrollHeight);

            if (_scroll.Height == 0) return;

            grfx.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), _scroll);
        }

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

        private void SetBarStartLeft(string rowText)
        {
            var gfx = CreateGraphics();

            var length = (int)gfx.MeasureString(rowText, RowFont, 500).Width;

            if (length > _barStartLeft) _barStartLeft = length;
        }

        public void AddBars(string rowText, string chain, object barValue,
            DateTime fromTime, DateTime toTime, DateTime prodFromTime, DateTime prodToTime, DateTime delayFromTime, DateTime delayToTime,
            Color color, Color hoverColor, int rowIndex, bool isRoot, string tag, bool expanded, Image toggle, int fixQty, int dailyQty, int prodQty,
            DateTime prodOverStartValue, DateTime prodOverEndValue, bool locked, bool prodLocked, bool closed, Color prodColor, string art, string dept, bool launched,
            string operation, int idx, int parentIdx, int loadedQty, int members, double qtyH, int id, DateTime dvc, DateTime rdd)
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
                Department = dept,
                Launched = launched,
                Operation = operation,
                Idx = idx,
                ParentIdx = parentIdx,
                LoadedQty = loadedQty,
                Members = members,
                QtyH = qtyH,
                Id = id,
                Rdd= rdd,
                DvcValue=dvc
            };

            Bars.Add(bar);

            SetBarStartLeft(rowText);
        }

        private int GetBarStartLocation(DateTime start)
        {
            var timeBetween = HeaderList[1].Time - HeaderList[0].Time;
            var minutesBetween = (int)timeBetween.TotalMinutes;
            dynamic widthBetween = HeaderList[1].StartLocation - HeaderList[0].StartLocation;

            var perMinute = (float)widthBetween / minutesBetween;

            var startTimeSpan = start - FromDate;
            var startMinutes = startTimeSpan.Days * 1440 + startTimeSpan.Hours * 60 + startTimeSpan.Minutes;
            var startLocation = (int)(perMinute * startMinutes);

            return startLocation;
        }

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

        private void DrawBars(Graphics grfx, bool ignoreScrollAndMousePosition = false)
        {

            grfx.SmoothingMode = SmoothingMode.AntiAlias;
            var geo = new Geometry();
            foreach (var bar in Bars)
            {
               
                if (FilteredRowText != string.Empty && bar.RowText != FilteredRowText) continue;

                if (Central.IsActiveOrdersSelection && bar.ProdQty == bar.FixQty || Central.IsActiveOrdersSelection && bar.ProdQty == 0) continue;

                // explicit -> do not load orders older then 3 months
                if (bar.StartValue < DateTime.Now.AddMonths(-3)) continue;  

                var index = bar.RowIndex;
                var scrollPos = ScrollPosition;

                var x = _barStartLeft + GetBarStartLocation(bar.StartValue);
                var y = BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 5;
                var w = GetBarEndLocation(bar.StartValue, bar.EndValue);
                int h = BarHeight - 25;
                var dx = _barStartLeft + GetBarStartLocation(bar.DelayStartValue);
                var dy = BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 5;
                var dw = GetBarEndLocation(bar.DelayStartValue, bar.DelayEndValue);
                int dh = BarHeight - 25;

                var pw = GetBarEndLocation(bar.ProdStartValue, bar.ProdEndValue);
                var px = _barStartLeft + GetBarStartLocation(bar.ProdStartValue);
                var py = y + 17;    
                var ph = BarHeight - 30;
                if (ph <= 0)
                {
                    ph = 2; py -= 7;
                }
                var ow = GetBarEndLocation(bar.ProdOverStartValue, bar.ProdOverEndValue);
                var ox = _barStartLeft + GetBarStartLocation(bar.ProdOverStartValue) + 1;
                var oy = y + 20;    
                var oh = BarHeight - 30;

                var dvcx = _barStartLeft + GetBarStartLocation(bar.DvcValue);
                var dvcy = BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos);
                var dvcw= GetBarEndLocation(bar.StartValue, bar.DvcValue);
                int dvch = BarHeight - 20;
                var dvcpen = new Pen(Brushes.Red, 1.8F);

                var rddx = _barStartLeft + GetBarStartLocation(bar.Rdd);
                var rddy = BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos);
                var rddw = GetBarEndLocation(bar.StartValue, bar.Rdd);
                int rddh = BarHeight - 20;
                var rddpen = new Pen(Brushes.DarkSlateGray, 1.8F);




                var findCenterByY = py + (ph / 2 - 5);

                var barColor = bar.Color;

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

                    if (bar.Launched == true) barColor = Color.DarkOrchid;

                    grfx.FillPath(new SolidBrush(barColor), geo.RoundedRectanglePath(barRect, 7));
                    grfx.DrawPath(borderPen, geo.RoundedRectanglePath(barRect, 7));

                    if (!HideDelay)
                    {
                        if (bar.DelayStartValue != DateTime.MinValue && bar.DelayEndValue != DateTime.MinValue && bar.DelayStartValue < bar.DelayEndValue)
                        {
                            grfx.FillPath(new SolidBrush(Color.FromArgb(255,155,63)), geo.RoundedRectanglePath(delayBarRect, dw > 0 ? 7 : 1));
                            grfx.DrawPath(borderPen, geo.RoundedRectanglePath(delayBarRect, 7));
                        }
                    }
                    grfx.FillPath(new SolidBrush(bar.ProdColor), geo.RoundedRectanglePath(prodBarRect, 7));
                    grfx.DrawPath(borderPen, geo.RoundedRectanglePath(prodBarRect, 7));

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

                    if (BarHeight >= 23)
                    {
                        if (!Central.IsArticleSelection)
                        {
                            if (w > 25)
                                grfx.DrawString(bar.EndValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, (x + w) - 34, y + 2);
                            if (w > 60)
                                grfx.DrawString(bar.StartValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, x, y + 2);
                            if (!HideDelay)
                            {
                                if (dw > 25)
                                    grfx.DrawString(bar.DelayEndValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, (dx + dw) - 34, dy + 2);
                                if (dw > 60)
                                    grfx.DrawString(bar.DelayStartValue.ToString("dd/MM"), _barFont, Brushes.WhiteSmoke, dx, dy + 2);
                            }
                            if (pw > 25)
                                grfx.DrawString(bar.ProdEndValue.ToString("dd/MM"), _barFont, brshProdText, (px + pw) - 34, findCenterByY);
                            if (pw > 60)
                                grfx.DrawString(bar.ProdStartValue.ToString("dd/MM"), _barFont, brshProdText, px, findCenterByY);
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

                        if (ow > 160)
                            grfx.DrawString(rowTxt, _barFont, Brushes.OrangeRed,
                                new PointF(ox + (ow / 2) - (barTextWidth / 2), findCenterByY));
                    }
                    Rectangle lineRect = new Rectangle();
                    if (Store.Default.sectorId != 7)
                    {
                        lineRect = new Rectangle(new Point(0,
                                  BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1), new Size(100, BarHeight + 7));
                    }
                    else
                    {
                        lineRect = new Rectangle(new Point(0,
                                 BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1), new Size(80, BarHeight + 7));
                    }
                    grfx.FillRectangle(new SolidBrush(Color.FromArgb(50, 52, 68)), lineRect);


                    var desc = (from line in Central.ListOfLines
                                where line.Line == bar.Tag && line.Department == bar.Department
                                select line).SingleOrDefault().Description;
                    if (bar.ProdQty > 0 && bar.ClosedOrd == false && bar.ProdQty < bar.FixQty && (bar.EndValue> bar.DvcValue || bar.DelayEndValue>bar.DvcValue))
                    {
                        grfx.DrawLine(dvcpen, dvcx, dvcy+3, dvcx, (dvcy + dvch)+3);
                    }
                    if (bar.ProdQty > 0 && bar.ClosedOrd == false && bar.ProdQty < bar.FixQty && (bar.EndValue > bar.Rdd || bar.DelayEndValue > bar.Rdd))
                    {
                        grfx.DrawLine(rddpen, rddx, rddy + 3, rddx, (rddy + dvch) + 3);
                    }
                    if (Store.Default.sectorId == 1)
                    {
                        var p2 = new Pen(Brushes.WhiteSmoke, 2);
                        var p3 = new Pen(Brushes.WhiteSmoke, 3);
                        grfx.DrawLine(p2, lineRect.X, lineRect.Y + lineRect.Height, lineRect.Width, lineRect.Y + lineRect.Height);
                        grfx.DrawLine(p3, lineRect.X + lineRect.Width, lineRect.Y, lineRect.X + lineRect.Width, lineRect.Y + lineRect.Height);
                        p2.Dispose();
                        p3.Dispose();
                        using (Image taskImg = Properties.Resources.folder_icon_32_gold ??
                                              throw new ArgumentNullException(nameof(grfx)))
                        {
                            var destRect1 = new Rectangle(7,
                                (BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 - 7, 20, 20);
                            const GraphicsUnit units = GraphicsUnit.Pixel;

                            grfx.DrawImage(taskImg, destRect1, 0, 0, 30, 30, units);
                        }
                        var strLine = "LINEA ";
                        var lineNum = bar.Tag.Remove(0, 5);
                        var deptChar = bar.Department == "Confezione A" 
                            || bar.Department == "Confezione B" || bar.Department == "Confezione C" ? bar.Department.Split(' ')[1] : "";
                        var rowTitle = strLine + lineNum + deptChar;

                        grfx.DrawString(new string(' ', 10) + rowTitle, new Font("Bahnschrift", 9, FontStyle.Regular),
                            new SolidBrush(Color.FromArgb(242,242,242)), 1,
                            ((BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 - 2));
                        
                        grfx.DrawString(new string(' ', 16) + desc, new Font("Bahnschrift", 6, FontStyle.Regular),
                       new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                       ((BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 + 12));

                    }
                    else if (Store.Default.sectorId == 7)
                    {

                        foreach (var line in Models.Tables.Lines.Where(a => a.Department == "Tessitura").ToList())
                        {

                            var strLine = "LINEA ";
                            var lineNum = /*line.Line.Remove(0, 5); //*/bar.Tag.Remove(0, 5);
                            if (lineNum.Contains('.')) lineNum = lineNum.Remove(1, 2);
                            //var deptChar = bar.Department == "Confezione A"
                            //    || bar.Department == "Confezione B" || bar.Department == "Confezione C" ? bar.Department.Split(' ')[1] : "";
                            var rowTitle = strLine + lineNum;// + deptChar;
                            var linelbl = new LineLabels() { Line = rowTitle, Index = index };
                            var intlinnr = Convert.ToInt32(lineNum);
                            if (!LineList.Contains(linelbl))
                            {
                                intlinnr -= 1;
                                if (intlinnr != 0)
                                {
                                    intlinnr = intlinnr * 3;
                                }
                                if (intlinnr == linelbl.Index && rowTitle == linelbl.Line)
                                {

                                    //using (Image taskImg = Properties.Resources.folder_icon_32_gold ??
                                    //      throw new ArgumentNullException(nameof(grfx)))
                                    //{
                                    //    var destRect1 = new Rectangle(7,
                                    //        (BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 - 7, 20, 20);
                                    //    const GraphicsUnit units = GraphicsUnit.Pixel;

                                    //    grfx.DrawImage(taskImg, destRect1, 0, 0, 30, 30, units);
                                    //}
                                    //        //var btn = new Button();
                                    //btn.Location = lineRect.Location;
                                    //btn.Text = rowTitle;
                                    //btn.Size = new Size(100, BarHeight + 7);
                                    //btn.BackColor = Color.FromArgb(50, 52, 68);
                                    //btn.ForeColor = Color.FromArgb(242, 242, 242);
                                    //btn.Anchor = AnchorStyles.Top;
                                    //btn.Click += Btn_Click;
                                    //btn.CreateGraphics();
                                    //Controls.Add(btn);
                                    grfx.DrawString(new string(' ', 10) + rowTitle, new Font("Bahnschrift", 9, FontStyle.Regular),
                                                new SolidBrush(Color.FromArgb(242, 242, 242)), -10,
                                                ((BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 - 2));
                                    LineList.Add(linelbl);
                                //    grfx.DrawString(new string(' ', 16) + desc, new Font("Bahnschrift", 6, FontStyle.Regular),
                                //                new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                                //                ((BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 + 12));
                               
                                }
                            }
                        }
                    }
                    else
                    {
                        var p2 = new Pen(Brushes.WhiteSmoke, 2);
                        var p3 = new Pen(Brushes.WhiteSmoke, 3);
                        grfx.DrawLine(p2, lineRect.X, lineRect.Y + lineRect.Height, lineRect.Width, lineRect.Y + lineRect.Height);
                        grfx.DrawLine(p3, lineRect.X + lineRect.Width, lineRect.Y, lineRect.X + lineRect.Width, lineRect.Y + lineRect.Height);
                        p2.Dispose();
                        p3.Dispose();
                        using (Image taskImg = Properties.Resources.folder_icon_32_gold ??
                                              throw new ArgumentNullException(nameof(grfx)))
                        {
                            var destRect1 = new Rectangle(7,
                                (BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 - 7, 20, 20);
                            const GraphicsUnit units = GraphicsUnit.Pixel;

                            grfx.DrawImage(taskImg, destRect1, 0, 0, 30, 30, units);
                        }
                        grfx.DrawString(new string(' ', 10) + bar.Tag, new Font("Bahnschrift", 9, FontStyle.Regular),
                          new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                          (BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 - 2);


                        grfx.DrawString(new string(' ', 16) + desc, new Font("Bahnschrift", 6, FontStyle.Regular),
                      new SolidBrush(Color.FromArgb(242, 242, 242)), 1,
                      ((BarStartTop + BarHeight * (index - scrollPos) + _barSpace * (index - scrollPos) + 1) + BarHeight / 2 + 12));

                    }
                }

                var maxHeight = Height;
                _scrollBarArea = new Rectangle(Width - 57, 0, 57, maxHeight + 10);
                grfx.FillRectangle(new SolidBrush(Color.WhiteSmoke), _scrollBarArea);
            }

            grfx.SmoothingMode = SmoothingMode.HighSpeed;
            grfx.CompositingMode = CompositingMode.SourceCopy;
            grfx.CompositingQuality = CompositingQuality.HighSpeed;
            grfx.PageScale = 1;
        }

        private void PaintChart()
        {
            Invalidate();
        }      
        private void PaintChart(Graphics gfx)
        {
            gfx.Clear(ChartBackColor);

            DrawScrollBar(gfx);
            DrawHeader(gfx, null);
            DrawNetHorizontal(gfx);
            DrawNetVertical(gfx);
            DrawBars(gfx);

            gfx.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            gfx.CompositingMode = CompositingMode.SourceOver;
            gfx.SmoothingMode = SmoothingMode.AntiAlias;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintChart(e.Graphics);
            base.OnPaint(e);
        }

        private void DrawNetVertical(Graphics grfx)
        {
            if (HeaderList == null)
                return;
            if (HeaderList.Count == 0)
                return;

            var index = 0;
            var ColHeaderWeekendColor = Color.FromArgb(150, 248, 220, 63);
            var ColumnWeekendColor = Color.FromArgb(70, 201, 201, 200);

            foreach (var header in HeaderList)
            {
                var headerLocationY = HeaderTimeStartTop;

                if (header.Time.DayOfWeek == DayOfWeek.Saturday | header.Time.DayOfWeek == DayOfWeek.Sunday)
                {
                    grfx.FillRectangle(new SolidBrush(ColHeaderWeekendColor),
                       _barStartLeft + index * _widthPerItem + 1,  
                      headerLocationY,    
                       _widthPerItem,         
                       HeaderHeight - 20); 

                    grfx.FillRectangle(new SolidBrush(ColumnWeekendColor),
                        _barStartLeft + index * _widthPerItem + 1,  
                        headerLocationY + HeaderHeight - 20,    
                        _widthPerItem,         
                        _lastLineStop - 50); 
                }

                if (header.Time.Date == DateTime.Now.Date)
                {
                    var pen = new Pen(Brushes.Coral, 2)
                    {
                        DashStyle = DashStyle.Solid,
                        DashOffset = 0.5f
                    };

                    grfx.DrawRectangle(pen,
                        _barStartLeft + index * _widthPerItem + 1,  
                        headerLocationY + HeaderHeight - 20,    
                        _widthPerItem,         
                        _lastLineStop - 50); 
                }

                var drawHldCount = 0;
                foreach (var item in Central.ListOfHolidays)
                {
                    var isHoliday = header.Time.Date == item.Holiday;

                    foreach (var bar in Bars)
                    {
                        if (isHoliday)    
                        {
                            if (drawHldCount > 0) break;
                            grfx.FillRectangle(new SolidBrush(Color.FromArgb(100, 249, 139, 49)),
                          _barStartLeft + index * _widthPerItem + 1,  
                          HeaderTimeStartTop + HeaderHeight - 20,    
                          _widthPerItem - 2,         
                           _lastLineStop - 50);      
                            drawHldCount++;
                        }
                    }
                }

                grfx.DrawLine(GridColor, _barStartLeft + index * _widthPerItem, headerLocationY,
                   _barStartLeft + index * _widthPerItem, _lastLineStop);

                index += 1;

                Header lastHeader = header;
            }

            grfx.DrawLine(GridColor, _barStartLeft + index * _widthPerItem, HeaderTimeStartTop,
                _barStartLeft + index * _widthPerItem, _lastLineStop);
        }

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
                        grfx.DrawLine(GridColor, 0, BarStartTop + BarHeight * index + _barSpace * index, width,
                        BarStartTop + BarHeight * index + _barSpace * index);
                    }
            }
            _lastLineStop = BarStartTop + BarHeight * (index - 1) + _barSpace * (index - 1);
        }

        public void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (HeaderList == null)
                return;
            if (HeaderList.Count == 0)
                return;

            _mouseHoverBarIndex = -1;

            var LocalMousePosition = PointToClient(Cursor.Position);

            var timeBetween = HeaderList[1].Time - HeaderList[0].Time;
            var minutesBetween = timeBetween.Days * 1440 + timeBetween.Hours * 60 + timeBetween.Minutes;
            dynamic widthBetween = HeaderList[1].StartLocation - HeaderList[0].StartLocation;
            var perMinute = (float)widthBetween / minutesBetween;

            if (perMinute == 0) perMinute = 1;

            if (LocalMousePosition.X > _barStartLeft)
            {
                var minutesAtCursor = (int)((LocalMousePosition.X - _barStartLeft) / perMinute);
                MouseOverColumnDate = FromDate.AddMinutes(minutesAtCursor);
            }
            else
            {
                MouseOverColumnDate = DateTime.MinValue;
            }

            var rowText = "";
            object rowValue = null;
            var scrollBarStatusChanged = false;

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

            if (_mouseHoverScrollBarArea == false)
                if ((LocalMousePosition.X > _scrollBarArea.Left) & (LocalMousePosition.Y < _scrollBarArea.Right) &
                    (LocalMousePosition.Y < _scrollBarArea.Bottom) & (LocalMousePosition.Y > _scrollBarArea.Top))
                    _mouseHoverScrollBarArea = false;

            var index = 0;
            IsMouseOverProductionBar = false;

            foreach (var bar in Bars)
            {
                if ((LocalMousePosition.Y > bar.ProdTopLocation.Left.Y) &
                    (LocalMousePosition.Y < bar.ProdBottomLocation.Left.Y))
                {
                    if ((LocalMousePosition.X > bar.ProdTopLocation.Left.X) &
                        (LocalMousePosition.X < bar.ProdTopLocation.Right.X))
                    {
                        if ((_mouseHover != MouseOver.BarLeftSide) &
                            (_mouseHover != MouseOver.BarRightSide)) _mouseHover = MouseOver.Bar;
                        IsMouseOverProductionBar = true;
                        rowText = bar.RowText;
                        rowValue = bar.Value;
                        _mouseHoverBarIndex = index;
                    }
                }
                
                if ((LocalMousePosition.Y > bar.TopLocation.Left.Y) &
                (LocalMousePosition.Y < bar.BottomLocation.Left.Y))
                {
                    if ((LocalMousePosition.X > bar.TopLocation.Left.X) &
                        (LocalMousePosition.X < bar.TopLocation.Right.X))
                    {
                        rowText = bar.RowText;
                        rowValue = bar.Value;
                        _mouseHoverBarIndex = index;
                        IsMouseOverProductionBar = false;
                        if ((_mouseHover != MouseOver.BarLeftSide) &
                            (_mouseHover != MouseOver.BarRightSide)) _mouseHover = MouseOver.Bar;
                    }
                }

                index += 1;
            }

            MouseOverRowText = rowText;
            MouseOverRowValue = rowValue;
            MouseOverToggleValue = rowValue;
            IsMouseOverToggle = false;

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
                    }
                }
            }
            if (((MouseOverRowValue == null) &
                (rowValue != null)) |
                ((MouseOverRowValue != null) &
                (rowValue == null)) |
                scrollBarStatusChanged)
                PaintChart();
        }

        public void Chart_MouseLeave(object sender, EventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            MouseOverRowText = null;
            MouseOverRowValue = null;

            _mouseHover = MouseOver.Empty;

            PaintChart();
        }

        public void Chart_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mouseHoverBarIndex > -1)
            {
                SelectedBarColor = Bars[_mouseHoverBarIndex].Color;
                Direction = e.X;
                SelectedBarIndex = _mouseHoverBarIndex;
            }
        }

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "Ganttochart";
            this.Size = new System.Drawing.Size(10, 10);
            this.ResumeLayout(false);
        }

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

    public class LineLabels
    {
        public string Line { get; set; }
        public int Index { get; set; }
    }

    public class Header
    {
        public string HeaderText { get; set; }

        public int StartLocation { get; set; }

        public string HeaderTextInsteadOfTime { get; set; }

        public DateTime Time { get; set; }
    }

    public class BarProperty
    {
        public DateTime StartValue { get; set; }

        public DateTime EndValue { get; set; }

        public DateTime ProdStartValue { get; set; }

        public DateTime ProdEndValue { get; set; }

        public DateTime ProdOverStartValue { get; set; }

        public DateTime ProdOverEndValue { get; set; }

        public DateTime DelayStartValue { get; set; }

        public DateTime DelayEndValue { get; set; }

        public bool Locked { get; set; }

        public bool LockedProd { get; set; }

        public bool ClosedOrd { get; set; }

        public DateTime Rdd { get; set; }

        public DateTime DvcValue { get; set; }

        public Color Color { get; set; }

        public Color HoverColor { get; set; }

        public string RowText { get; set; }

        public string Chain { get; set; }

        public object Value { get; set; }

        public int RowIndex { get; set; }

        public int FixQty { get; set; }

        public int DailyQty { get; set; }

        public int ProdQty { get; set; }

        public Color ProdColor { get; set; }

        public string Article { get; set; }

        public bool IsRoot { get; set; }

        public string Tag { get; set; }
        public string Department { get; set; }
        public bool Launched { get; set; }
        public bool Expanded { get; set; }

        public Image Toggle { get; set; }

        public string Operation { get; set; }
         
        public int Idx { get; set; }
        public int ParentIdx { get; set; }

        public int LoadedQty { get; set; }
        public int Members { get; set; }
        public double QtyH { get; set; }
        public int Id { get; set; }

        internal Location TopLocation { get; set; } = new Location();

        internal Location BottomLocation { get; set; } = new Location();

        internal Location DelayTopLocation { get; set; } = new Location();

        internal Location DelayBottomLocation { get; set; } = new Location();

        internal Location ProdTopLocation { get; set; } = new Location();

        internal Location ProdBottomLocation { get; set; } = new Location();


        internal class Location
        {
            public Point Left { get; set; }

            public Point Right { get; set; }
        }
    }

    public class Bar
    {
        public Bar()
        {
        }

        public Bar(string rowText, string chain, DateTime fromTime, DateTime toTime, DateTime prodFromTime, DateTime prodToTime, DateTime delayStart, DateTime delayEnd,
            Color color, Color hoverColor, int index, bool isRoot, string tag,
            int fixQty, int dailyQty, int prodQty, DateTime prodOverFromTime, DateTime prodOverToTime, bool locked, bool prodLocked, bool closed, Color prodColor, string art,string dept, bool launched,
            string operation, int idx, int parentIdx, int loadedQty, int members, double qtyH, int id, DateTime dvc, DateTime rdd)
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
            Launched = launched;
            Operation = operation;
            Idx = idx;
            ParentIdx = parentIdx;
            LoadedQty = loadedQty;
            Members = members;
            QtyH = qtyH;
            Id = id;
            ToDvc = dvc;
            ToRdd = rdd;

        }

        public string RowText { get; set; }

        public string Chain { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public DateTime ProdFromTime { get; set; }

        public DateTime ProdToTime { get; set; }

        public DateTime ProdOverFromTime { get; set; }

        public DateTime ProdOverToTime { get; set; }

        public DateTime DelayFromTime { get; set; }

        public DateTime DelayToTime { get; set; }

        public DateTime ToRealTime { get; set; }

        public DateTime ToDvc { get; set; }
        public DateTime ToRdd { get; set; }

        public Color Color { get; set; }

        public Color HoverColor { get; set; }

        public int Index { get; set; }

        public int FixedQty { get; set; }

        public int ProductionQty { get; set; }

        public int DailyQty { get; set; }

        public bool IsRoot { get; set; }

        public string Tag { get; set; }
        public string Department { get; set; }

        public bool Launched { get; set; }

        public string Article { get; set; }

        public bool Expanded { get; set; }

        public Image Toggle { get; set; }

        public bool Locked { get; set; }

        public bool LockedProd { get; set; }

        public bool ClosedOrd { get; set; }

        public Color ProdColor { get; set; }

        public string Operation { get; set; }
        public int Idx { get; set; }
        public int ParentIdx { get; set; }
        public int LoadedQty { get; set; }
        public int Members { get; set; }
        public double QtyH { get; set; }
        public int Id { get; set; }
    }

    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        public T Data { get; set; }

        public TreeNode<T> Parent { get; set; }

        public ICollection<TreeNode<T>> Children { get; set; }

        public Boolean IsRoot
        {
            get { return Parent == null; }
        }

        public Boolean IsLeaf
        {
            get { return Children.Count == 0; }
        }

        public int Level
        {
            get
            {
                if (IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }

        public TreeNode(T data)
        {
            Data = data;
            Children = new LinkedList<TreeNode<T>>();

            ElementsIndex = new LinkedList<TreeNode<T>>();
            ElementsIndex.Add(this);
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            Children.Add(childNode);

            RegisterChildForSearch(childNode);

            return childNode;
        }

        public override string ToString()
        {
            return Data != null ? Data.ToString() : "[data null]";
        }

        private ICollection<TreeNode<T>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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
