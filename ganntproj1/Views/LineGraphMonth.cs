using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ganntproj1.Views
{
    public partial class LineGraphMonth : Form
    {

        private string Line { get; set; }

        private string Department { get; set; }

        private int Month { get; set; }

        private int Year { get; set; }

        private double Media { get; set; }

        private List<LineProductionData> lineProductionDatas = new List<LineProductionData>();

        public LineGraphMonth()
        {
            InitializeComponent();
        }

        public LineGraphMonth(string line, string department, int month, int year, double media)
        {
            InitializeComponent();
            this.DoubleBuffered(true);
            
            Line = line;
            Department = department;
            Month = month;
            Year = year;
            Media = media;

            lblLine.Text = Line;
            lblDept.Text = Department;    
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                LoadData();
                LoadGraph();

                lblMedia.Text = "Media " + Math.Round(Media,1).ToString() + "%";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Width+=1;
        }

        private void LoadData()
        {
            lineProductionDatas = new List<LineProductionData>();

            var q = @"select round(cast(sum(capi) as float) / cast(sum(dailyQty) as float) * 100, 1), datepart(day,data),count(1) from produzione 
where line=@line and department=@department and datepart(month,data) = @month and datepart(year,data) = @year 
group by line,department,datepart(day,data) 
order by datepart(day,data)";

            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@line", SqlDbType.NVarChar).Value = Line;
                cmd.Parameters.Add("@department", SqlDbType.NVarChar).Value = Department;
                cmd.Parameters.Add("@month", SqlDbType.Int).Value = Month;
                cmd.Parameters.Add("@year", SqlDbType.Int).Value = Year;

                con.Open();

                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        double.TryParse(dr[0].ToString(), out var eff);
                        int.TryParse(dr[1].ToString(), out var day);
                        int.TryParse(dr[2].ToString(), out var ordersCount);

                        lineProductionDatas.Add(
                            new LineProductionData(eff * ordersCount, day));
                    }

                con.Close();
                dr.Close();
            }
        }

        private void LoadGraph()
        {
            if (lineProductionDatas.Count <= 0)
            {
                MessageBox.Show("Unable to load data from server.", "Line efficiency daily graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pane = new ZedGraph.GraphPane();

            //graph customization
            pane.Title.Text = "EFF con " + Line + "  (" + Department + ")";
            pane.YAxis.Title.Text = "EFF %";
            pane.XAxis.Title.Text = Month.ToString() + "/" + Year.ToString();
            pane.XAxis.MajorTic.IsAllTics = true;
            pane.XAxis.Scale.MajorUnit = ZedGraph.DateUnit.Day;
            pane.XAxis.Scale.MajorStep = 1;
            pane.XAxis.Scale.Min = 1;
            pane.XAxis.Scale.Max = 31;

            pane.Fill = new ZedGraph.Fill(Brushes.WhiteSmoke);
            
            ZedGraph.PointPairList list = new ZedGraph.PointPairList();

            foreach (var lineProduction in lineProductionDatas)
            {
                list.Add(lineProduction.Day, lineProduction.Eff);
            }

            var curve = new ZedGraph.LineItem("EFF %", list, Color.SteelBlue, ZedGraph.SymbolType.Circle);
            curve.Line.IsVisible = true;
            curve.Symbol.Fill.Color = Color.SteelBlue;
            curve.Symbol.Fill.Type = ZedGraph.FillType.Solid;
            curve.Symbol.Size = 10;
            curve.Line.Width = 4;
            curve.Symbol.IsAntiAlias = true;
            curve.Line.IsSmooth = false;
            curve.Line.IsAntiAlias = true;
            curve.Line.Fill = new ZedGraph.Fill(Color.White,
                        Color.LightSkyBlue, -45F);

            curve.Symbol.Size = 8.0F;
            curve.Symbol.Fill = new ZedGraph.Fill(Color.White);
            curve.Line.Width = 2.0F;

            pane.XAxis.MajorTic.IsBetweenLabels = true;

            pane.Chart.Fill = new ZedGraph.Fill(Color.White,Color.FromArgb(250, 250, 250), 90F);
            pane.Fill = new ZedGraph.Fill(Color.FromArgb(250, 250, 250));
            
            zedGraph.GraphPane = pane;

            for (var i = 0; i <= curve.Points.Count - 1; i++)
            {
                ZedGraph.PointPair pt = curve.Points[i];

                ZedGraph.TextObj text = new ZedGraph.TextObj(pt.Y.ToString("f1"), pt.X, pt.Y + 2,
                    ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center);
                text.ZOrder = ZedGraph.ZOrder.D_BehindAxis;
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.IsVisible = false;
                text.FontSpec.Angle = 90;
                pane.GraphObjList.Add(text);
            }

            zedGraph.GraphPane.CurveList.Add(curve);

            zedGraph.AxisChange();
            zedGraph.IsShowPointValues = true;
            zedGraph.PointValueFormat = "0";
            zedGraph.Invalidate();
        }

        internal class LineProductionData
        {
            public LineProductionData(double eff, int day)
            {
                Eff = eff;
                Day = day;
            }

            public double Eff { get; set; }

            public int Day { get; set; }
        }
    }
}
