using System;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ganntproj1
    {
    class ExcelExport
        {
        private void CopyAlltoClipboard(DataGridView dgv)
            {
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgv.SelectAll();
            System.Windows.Forms.DataObject dataObj = dgv.GetClipboardContent();

            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
            }

        public void ExportToExcel(DataGridView dgv, string fileName)
            {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = fileName + "_" + DateTime.Now.ToString("yyyyMMdd-ffff");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                CopyAlltoClipboard(dgv);

                object misValue = System.Reflection.Missing.Value;
                Excel.Application xlexcel = new Excel.Application();

                xlexcel.DisplayAlerts = false;          
                Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                xlWorkSheet.Columns.AutoFit();

                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlexcel.DisplayAlerts = true;
                xlWorkBook.Close(true, misValue, misValue);
                xlexcel.Quit();

                RelComObject(xlWorkSheet);
                RelComObject(xlWorkBook);
                RelComObject(xlexcel);

                Clipboard.Clear();
                dgv.ClearSelection();

                try
                {
                    if (System.IO.File.Exists(sfd.FileName))
                        System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void RelComObject(object obj)
            {
            try
                {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
                }
            catch (Exception ex)
                {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
                }
            finally
                {
                GC.Collect();
                }
            }
        }
    }