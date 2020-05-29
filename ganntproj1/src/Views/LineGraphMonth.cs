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
        
        private List<DataCollection> lineProductionDatas = new List<DataCollection>();
        private List<ArticleProductionData> articleProductions = new List<ArticleProductionData>();

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
            lineProductionDatas = new List<DataCollection>();
            articleProductions = new List<ArticleProductionData>();

            var qx = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float, capi int, dept nvarchar(50)) ";
            qx += "insert into tmpTable ";
            qx += "select convert(date,data,101),line,qtyH,members,case when @paramAb = 0 then (cast(abatim as float)/100) else 1 end, ";
            qx += "capi,department from viewproduction ";
            qx += "where line=@line and datepart(month,data) = @month and datepart(year,data) = @year and department=@department ";
            qx += "order by datepart(day,data) ";
            qx += "create table tmpSum (datas date,line nvarchar(50),produc float,prevent float,qty int,dept nvarchar(50),cnt int) ";
            qx += "insert into tmpSum ";
            qx += "select datas,line,sum((qtyH * members))producibili, ";
            qx += "sum((qtyH * members * abat))prevent, sum(capi)qty,dept,count(1) from tmpTable ";
            qx += "group by datas,line,dept order by dept,len(line),line,datas ";
            qx += "select datas,line, (produc / cnt * '" + Store.Default.confHour + "')producibili,(prevent / cnt * '" + Store.Default.confHour + "'),qty,dept from tmpSum ";
            qx += "drop table tmpTable drop table tmpSum";

            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand(qx, con);
                cmd.Parameters.Add("@line", SqlDbType.NVarChar).Value = Line;
                cmd.Parameters.Add("@department", SqlDbType.NVarChar).Value = Department;
                cmd.Parameters.Add("@month", SqlDbType.Int).Value = Month;
                cmd.Parameters.Add("@year", SqlDbType.Int).Value = Year;
                cmd.Parameters.Add("@paramAb", SqlDbType.BigInt).Value = false;

                con.Open();

                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        DateTime.TryParse(dr[0].ToString(), out var dt);
                        double.TryParse(dr[2].ToString(), out var produc);
                        double.TryParse(dr[3].ToString(), out var prevent);
                        int.TryParse(dr[4].ToString(), out var qty);
                        var dpt = dr[5].ToString();
                        lineProductionDatas.Add(new DataCollection(
                            dt,
                            dr[1].ToString(), "", produc, prevent, qty, dpt, 0.0)
                            );
                    }

                dr.Close();


                qx = @"use Ganttproj
select o.article, datepart(day,p.data) from produzione p
inner join objects o on p.commessa = o.ordername and o.department = p.department
where p.line=@line and p.department=@department and datepart(month,p.data) = @month and datepart(year,p.data) = @year
group by o.article,datepart(day,p.data) 
order by datepart(day,p.data)";
                cmd = new SqlCommand(qx, con);
                cmd.Parameters.Add("@line", SqlDbType.NVarChar).Value = Line;
                cmd.Parameters.Add("@department", SqlDbType.NVarChar).Value = Department;
                cmd.Parameters.Add("@month", SqlDbType.Int).Value = Month;
                cmd.Parameters.Add("@year", SqlDbType.Int).Value = Year;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        var art = dr[0].ToString();
                        int.TryParse(dr[1].ToString(), out var day);

                        articleProductions.Add(
                            new ArticleProductionData(art, day));
                    }
                dr.Close();
                con.Close();
            }
        }

        private void LoadGraph()
        {
            if (lineProductionDatas.Count <= 0)
            {
                MessageBox.Show("Unable to load data from server.", "Line efficiency daily graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var bufferList = new List<ArticleProductionData>();

            var pane = new ZedGraph.GraphPane();
            
            pane.Title.Text = "EFF con " + Line + "  (" + Department + ")";
            pane.YAxis.Title.Text = "EFF %";
            pane.XAxis.Title.Text = Month.ToString() + "/" + Year.ToString();
            pane.XAxis.MajorTic.IsAllTics = true;
            pane.XAxis.Scale.MajorStep = 1;
            pane.XAxis.Scale.Min = 1;
            pane.XAxis.Scale.Max = 31;

            pane.Fill = new ZedGraph.Fill(Brushes.WhiteSmoke);
            
            ZedGraph.PointPairList list = new ZedGraph.PointPairList();

            foreach (var lineProduction in lineProductionDatas)
            {
                var workEff = Math.Round((lineProduction.Qty / lineProduction.Producibili) * 100.0, 1);

                list.Add(lineProduction.Datex.Day, workEff);
            }

            pane.GraphObjList.Clear();
            zedGraph.GraphPane.CurveList.Clear();

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
            pane.Legend.IsVisible = false;
            ZedGraph.PointPairList articleRangeList = new ZedGraph.PointPairList();
            ZedGraph.LineItem articleVertCurve = new ZedGraph.LineItem("");
            
            for (var i = 0; i <= curve.Points.Count - 1; i++)
            {
                ZedGraph.PointPair pt = curve.Points[i];

                ZedGraph.TextObj text = new ZedGraph.TextObj(pt.Y.ToString("f1"), pt.X, pt.Y,
                    ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center);
                text.ZOrder = ZedGraph.ZOrder.D_BehindAxis;
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.IsVisible = false;
                text.FontSpec.Angle = 90;
                pane.GraphObjList.Add(text);

                var art = articleProductions.LastOrDefault(x => x.Day == pt.X);
                var buf = bufferList.FirstOrDefault(x => x.Article == art.Article || x.Day == art.Day);


                if (art != null && buf == null)
                {
                    bufferList.Add(art);

                    ZedGraph.TextObj textArt = new ZedGraph.TextObj(art.Article, pt.X + 0.2f, pane.YAxis.Scale.Min + pt.Y / 2,
                                            ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center);

                    textArt.ZOrder = ZedGraph.ZOrder.D_BehindAxis;
                    textArt.FontSpec.Border.IsVisible = false;
                    textArt.FontSpec.Fill.IsVisible = false;
                    textArt.FontSpec.Size = 9;
                    textArt.FontSpec.FontColor = Color.Black;

                    var lastArt = articleProductions.LastOrDefault(x => x.Day == pt.X - 1);
                    var nextArt = articleProductions.FirstOrDefault(x => x.Day == pt.X + 2);

                    if (lastArt != null && lastArt.Article != art.Article || nextArt != null && nextArt.Article != art.Article)
                    {
                        textArt.FontSpec.Angle = 90;
                    }
                    else
                    {
                        textArt.FontSpec.Angle = 0;
                    }

                    pane.GraphObjList.Add(textArt);

                    articleRangeList = new ZedGraph.PointPairList();
                    articleRangeList.Add(pt.X, pt.Y);
                    articleRangeList.Add(pt.X, pane.YAxis.Scale.Min);

                    var ac = new ZedGraph.LineItem(art.Article);
                    ac.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                    ac = pane.AddCurve(art.Article, articleRangeList, Color.Orange, ZedGraph.SymbolType.None);                  
                }
            }
            
            zedGraph.GraphPane.CurveList.Add(curve);
            zedGraph.AxisChange();
            zedGraph.Refresh();

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

        internal class ArticleProductionData
        {
            public ArticleProductionData(string art, int day)
            {
                Article = art;
                Day = day;
            }

            public string Article { get; set; }

            public int Day { get; set; }
        }
    }
}
