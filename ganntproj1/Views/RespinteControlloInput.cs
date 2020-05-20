using System;
using System.Windows.Forms;

namespace ganntproj1.Views
{
    public partial class RespinteControlloInput : Form
    {
        public RespinteControlloInput()
        {
            InitializeComponent();
        }

        public int Id { get; set; }
        public DateTime Dates { get; set; }
        public string Motivo { get; set; }

        public RespinteControlloInput(int id, DateTime controlloDate, string motivo)
        {
            InitializeComponent();
            Id = id;
            Dates = controlloDate;
            Motivo = motivo;

            //dtpFrom.Value = controlloDate;
            textBox1.Text = motivo;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dates = dtpFrom.Value;
            Motivo = textBox1.Text;

            using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
            {
                ctx.ExecuteCommand("update comenzi set DateControlled={0}, Motivo={1} where Id={2}", Dates,Motivo,Id);
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
