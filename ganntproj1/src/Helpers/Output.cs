using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ganntproj1
{
    public class Output
    {
        public Output()
            {

            }

        /// <summary>
        /// Static table that can be read-only, or downloadable.
        /// It's used when the data and behavior of a class do not depend on object identity
        /// </summary>
        public static DataTable ProcessingTable { get; set; }

        public IEnumerable<string[]> Content()
        {
            var file = new string[] { };

            IEnumerable<string[]> csvFile;

            try
                {
                file = File.ReadAllLines(Store.Default.targetFile);

                csvFile = from lines in file
                          select lines.Split(',').ToArray();
                }
            catch
                {
                //reset output file
                Store.Default.outputDir = string.Empty;
                Store.Default.Save();

                MessageBox.Show("Output file is broken or may be corrupted.");

                var outputFileDialog = new Config.OpenDialog();
                outputFileDialog.ShowDialog();
                outputFileDialog.Dispose();

                file = File.ReadAllLines(Store.Default.outputDir);

                csvFile = from lines in file
                          select lines.Split(',').ToArray();
                }

            return csvFile;
        }

        /// <summary>
        /// Formated array of resource data.
        /// </summary>
        /// <returns>Array</returns>
        public List<string> OutputFormater()
        {
            var lst = new List<string>();
            
            var sb = new StringBuilder();
            foreach (var arr in Content())
            {
                sb.Clear();

                for (var len = 0; len <= arr.Length - 1; len++)
                    sb.Append(arr[len] + " ");

                //add environmental record
                lst.Add(sb + "\n");    
            }

            return lst;
        }

        /// <summary>
        /// Constant string array of all existent columns in .xls file.
        /// </summary>
        public string[] FileColumns { get; set; } = new string[] {"Commessa", "Articolo", "Finezza", "Capi commessa", 
            "Stagione", "Capi usciti da tessitura", "Diff(CapiComm-CapiTess)", "Diff% 1", "Data Arrivo Filato", 
            "Data Inizio Tessitura", "GG Staz Pre-Tess", "Data Fine Tessitura", "GG Lav 1", "Data Consegna Tessitura", 
            "GG Staz Post-Tess", "GG Staz TOT-Tess", "Rdd Tessitura", "Destinazione", "Data Arrivo Conf", 
            "Data Inizio Confezione (1°lett cart)", "GGStaz Pre-Conf", "Data Fine Conf", "GG Lav 2", "Data Cons Confezione", 
            "GG Staz Post-Conf", "GG Staz TOT-Conf", "Rdd Confezione", "Capi usciti da confezione", "Diff 1", "Diff% 2", "Diff (tess+conf)", 
            "GG Staz TINTORIA", "Data Arrivo Stiro", "Data Inizio Stiro", "GG Staz Pre-Stiro", "Data Fine Stiro", "GG Lav 3", 
            "Data Cons Stiro", "GG Staz Post-Stiro", "GG Staz TOT-Stiro", "Rdd Stiro", "Capi usciti da stiro", "Diff 2", "DifF %", "Diff (tess+conf+stiro)", 
            "Tot GG Lav 4", "Tot GG STAZ", "Note", "Data Dvc","none1", "none2", "none3", "none4", "none5", };
        }
}