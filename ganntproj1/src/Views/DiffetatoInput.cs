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

namespace ganntproj1
{
    public partial class DiffetatoInput : Form
    {
        private int CommessaId { get; set; }
        public DiffetatoInput(int commId)
        {
            InitializeComponent();
            CommessaId = commId;
            btnOk.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
        }

        private void SaveData()
        {
            try
            {
                var q = @"insert into ComenziDiffetato ([NrComandaId]
      ,[Tessitura]
      ,[Confezione]
      ,[Stiro]
      ,[MateriaPrima]
      ,[Taglio]
      ,[Stampa]
      ,[Tintoria]
      ,[ApplicazioneAccessori]) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)";

                using (var c = new SqlConnection(Central.ConnStr))
                {
                    var cmd = new SqlCommand(q, c);
                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = CommessaId;
                    cmd.Parameters.Add("@p2", SqlDbType.Int).Value = tess.Text;
                    cmd.Parameters.Add("@p3", SqlDbType.Int).Value = conf.Text;
                    cmd.Parameters.Add("@p4", SqlDbType.Int).Value = stiro.Text;
                    cmd.Parameters.Add("@p5", SqlDbType.Int).Value = Prima.Text;
                    cmd.Parameters.Add("@p6", SqlDbType.Int).Value = taglio.Text;
                    cmd.Parameters.Add("@p7", SqlDbType.Int).Value = stampa.Text;
                    cmd.Parameters.Add("@p8", SqlDbType.Int).Value = tint.Text;
                    cmd.Parameters.Add("@p9", SqlDbType.Int).Value = access.Text;

                    c.Open();
                    cmd.ExecuteNonQuery();
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, 
                    "Insert error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
