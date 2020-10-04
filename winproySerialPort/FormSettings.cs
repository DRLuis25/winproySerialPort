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
using System.Xml;

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
                RutaDescarga(fbdUbicacion.SelectedPath);
                MessageBox.Show("Ruta actualizada");
                lblRuta.Text = ConfigurationManager.AppSettings["Path"];
                //    try
                //    {
                //        ConfigurationManager.AppSettings["Path"] = fbdUbicacion.SelectedPath;
                //        MessageBox.Show("Ruta actualizada");
                //        lblRuta.Text = ConfigurationManager.AppSettings["Path"];
                //    }
                //    catch (Exception ex)
                //    {

                //        MessageBox.Show("Error: " + ex.Message);
                //    }
            }

        }
        private void RutaDescarga(string path)
        {
            if (path != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                foreach (XmlElement element in xmlDoc.DocumentElement)
                {
                    if (element.Name.Equals("appSettings"))
                    {
                        foreach (XmlNode node in element.ChildNodes)
                        {
                            if (node.Attributes[0].Value == "Path")
                                node.Attributes[1].Value = path;
                        }
                    }
                }
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection("appSettings");
            }

        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
