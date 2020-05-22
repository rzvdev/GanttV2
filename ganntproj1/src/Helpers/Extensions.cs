namespace ganntproj1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="ExtensionMethods" />
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// The DoubleBuffered
        /// </summary>
        /// <param name="dgv">The dgv<see cref="DataGridView"/></param>
        /// <param name="setting">The setting<see cref="bool"/></param>
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
        /// <summary>
        /// The DoubleBuffered
        /// </summary>
        /// <param name="ctl">The ctl<see cref="UserControl"/></param>
        /// <param name="setting">The setting<see cref="bool"/></param>
        public static void DoubleBuffered(this UserControl ctl, bool setting)
        {
            Type pnType = ctl.GetType();
            PropertyInfo pi = pnType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctl, setting, null);
        }
        /// <summary>
        /// The DoubleBuffered
        /// </summary>
        /// <param name="ctl">The ctl<see cref="Form"/></param>
        /// <param name="setting">The setting<see cref="bool"/></param>
        public static void DoubleBuffered(this Form ctl, bool setting)
        {
            Type pnType = ctl.GetType();
            PropertyInfo pi = pnType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctl, setting, null);
        }
        public static void DoubleBuffered(this Label ctl, bool setting)
        {
            Type pnType = ctl.GetType();
            PropertyInfo pi = pnType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctl, setting, null);
        }
        /// <summary>
        /// The ExportToExcel
        /// </summary>
        /// <param name="dgv">The dgv<see cref="DataGridView"/></param>
        /// <param name="filename">The filename<see cref="string"/></param>
        public static void ExportToExcel(this DataGridView dgv, string filename)
        {
            var exp = new ExcelExport();
            exp.ExportToExcel(dgv, filename);
        }
        /// <summary>
        /// The DoubleBuffered
        /// </summary>
        /// <param name="ctl">The control.</param>
        /// <param name="setting">if set to <c>true</c> [setting].</param>
        public static void DoubleBuffered(this Panel ctl, bool setting)
        {
            Type pnType = ctl.GetType();
            PropertyInfo pi = pnType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctl, setting, null);
        }
        public static void DoubleBuffered(this CheckBox ctl, bool setting)
        {
            Type pnType = ctl.GetType();
            PropertyInfo pi = pnType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctl, setting, null);
        }

        /// <summary>Represents a t source.</summary>
        /// <typeparam name="TArg0">The type of the arg0.</typeparam>
        /// <param name="element">The element.</param>
        public delegate void Func<TArg0>(TArg0 element);

        /// <summary>
        /// The Update
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="update">The update.</param>
        /// <returns></returns>
        public static int Update<TSource>(this IEnumerable<TSource> source, Func<TSource> update)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (update == null)
            {
                throw new ArgumentNullException("update");
            }
            if (typeof(TSource).IsValueType)
            {
                throw new NotSupportedException("Value type elements are not supported by update.");
            }
            int count = 0;
            foreach (TSource element in source)
            {
                update(element);
                count++;
            }
            return count;
        }
        /// <summary>
        /// The Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null)) 
                return default;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        /// <summary>
        /// The GetWindowLong
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="nIndex">The nIndex<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// The SetWindowLong
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="nIndex">The nIndex<see cref="int"/></param>
        /// <param name="dwNewLong">The dwNewLong<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        /// <summary>
        /// The SetWindowPos
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="hWndInsertAfter">The hWndInsertAfter<see cref="IntPtr"/></param>
        /// <param name="X">The X<see cref="int"/></param>
        /// <param name="Y">The Y<see cref="int"/></param>
        /// <param name="cx">The cx<see cref="int"/></param>
        /// <param name="cy">The cy<see cref="int"/></param>
        /// <param name="uFlags">The uFlags<see cref="uint"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// Defines the GWL_EXSTYLE
        /// </summary>
        private const int GWL_EXSTYLE = -20;

        /// <summary>
        /// Defines the WS_EX_CLIENTEDGE
        /// </summary>
        private const int WS_EX_CLIENTEDGE = 0x200;

        /// <summary>
        /// Defines the SWP_NOSIZE
        /// </summary>
        private const uint SWP_NOSIZE = 0x0001;

        /// <summary>
        /// Defines the SWP_NOMOVE
        /// </summary>
        private const uint SWP_NOMOVE = 0x0002;

        /// <summary>
        /// Defines the SWP_NOZORDER
        /// </summary>
        private const uint SWP_NOZORDER = 0x0004;

        /// <summary>
        /// Defines the SWP_NOREDRAW
        /// </summary>
        private const uint SWP_NOREDRAW = 0x0008;

        /// <summary>
        /// Defines the SWP_NOACTIVATE
        /// </summary>
        private const uint SWP_NOACTIVATE = 0x0010;

        /// <summary>
        /// Defines the SWP_FRAMECHANGED
        /// </summary>
        private const uint SWP_FRAMECHANGED = 0x0020;

        /// <summary>
        /// Defines the SWP_SHOWWINDOW
        /// </summary>
        private const uint SWP_SHOWWINDOW = 0x0040;

        /// <summary>
        /// Defines the SWP_HIDEWINDOW
        /// </summary>
        private const uint SWP_HIDEWINDOW = 0x0080;

        /// <summary>
        /// Defines the SWP_NOCOPYBITS
        /// </summary>
        private const uint SWP_NOCOPYBITS = 0x0100;

        /// <summary>
        /// Defines the SWP_NOOWNERZORDER
        /// </summary>
        private const uint SWP_NOOWNERZORDER = 0x0200;

        /// <summary>
        /// Defines the SWP_NOSENDCHANGING
        /// </summary>
        private const uint SWP_NOSENDCHANGING = 0x0400;

        /*
         *downloaded from:
         * https://social.msdn.microsoft.com/Forums/en-US/b74fba49-16c4-463c-b5a2-2654078bd21a/mdi-child-has-some-border-around?forum=winformsdesigner 
         */
        /// <summary>
        /// The SetBevel
        /// </summary>
        /// <param name="form">The form<see cref="Form"/></param>
        /// <param name="show">The show<see cref="bool"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool SetBevel(this Form form, bool show)
        {
            foreach (Control c in form.Controls)
            {
                if (c is MdiClient client)
                {
                    int windowLong = GetWindowLong(c.Handle, GWL_EXSTYLE);

                    if (show) { windowLong |= WS_EX_CLIENTEDGE; } else { windowLong &= ~WS_EX_CLIENTEDGE; }

                    SetWindowLong(c.Handle, GWL_EXSTYLE, windowLong);

                    SetWindowPos(client.Handle, IntPtr.Zero, 0, 0, 0, 0,
                        SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER |
                        SWP_NOOWNERZORDER | SWP_FRAMECHANGED);
                    
                    client.BackColor = System.Drawing.Color.WhiteSmoke;

                    return true;
                }
            }
            return false;
        }
    }
}
