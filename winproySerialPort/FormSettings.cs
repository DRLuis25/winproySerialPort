using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winproySerialPort
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            string filepath = ConfigurationManager.AppSettings["Path"];
            lblRuta.Text = filepath;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCambiarUbicacion_Click(object sender, EventArgs e)
        {
            if (fbdUbicacion.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ConfigurationManager.AppSettings["Path"] = fbdUbicacion.SelectedPath;
                    MessageBox.Show("Ruta actualizada");
                    lblRuta.Text = ConfigurationManager.AppSettings["Path"];
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
