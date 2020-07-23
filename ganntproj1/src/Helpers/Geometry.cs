using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace ganntproj1
    {
    class Geometry
        {
        public GraphicsPath RoundedRectanglePath(Rectangle bounds, int rad)
            {
            var diameter = rad * 2;
            var size = new Size(diameter, diameter);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();

            if (rad == 0)
                {
                path.AddRectangle(bounds);
                return path;
                }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();      

            return path;
            }
        
        private Color HexToColor(string hexColor)
            {
            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");

            byte red = 0;
            byte green = 0;
            byte blue = 0;

            if (hexColor.Length == 8)
                {
                hexColor = hexColor.Substring(2);
                }

            if (hexColor.Length == 6)
                {
                red = byte.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = byte.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = byte.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                }
            else if (hexColor.Length == 3)
                {
                red = byte.Parse(hexColor[0].ToString() + hexColor[0].ToString(), NumberStyles.AllowHexSpecifier);
                green = byte.Parse(hexColor[1].ToString() + hexColor[1].ToString(), NumberStyles.AllowHexSpecifier);
                blue = byte.Parse(hexColor[2].ToString() + hexColor[2].ToString(), NumberStyles.AllowHexSpecifier);
                }

            return Color.FromArgb(red, green, blue);
            }

        public Color InvertColor(Color cl)
            {
               return Color.FromArgb(0, 0, 0);
            }
        }
    }
