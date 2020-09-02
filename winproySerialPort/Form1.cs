using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Threading;

namespace winproySerialPort
{
    public partial class Form1 : Form
    {
        classTransRecep objTrRX;
        delegate void MostrarOtroProceso(string mensaje);
        MostrarOtroProceso delegadoMostrar;
        //Delegado proceso Envío
        delegate void MostrarEnvio(long x, long y);
        MostrarEnvio delegadoEnvio;
        int baudrate;
        string x;
        public Form1()
        {
            InitializeComponent();
        }
        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            string msje = rchMensajes.Text.Trim();
            if(msje!="" && msje.Length<=1019)
            {
                objTrRX.Enviar(msje);
                rchConversacion.Text += "Tú: " + rchMensajes.Text.Trim() + "\n";
                rchMensajes.Text = "";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RutaDescarga();
            objTrRX = new classTransRecep();
            objTrRX.LlegoMensaje += new classTransRecep.HandlerTxRx(ObjTrRx_LlegoMensaje);//Se adiciona el delegado
            objTrRX.Proceso += new classTransRecep.HandlerProceso(ObjTrRX_proceso);
            delegadoMostrar = new MostrarOtroProceso(MostrandoMensaje);
            delegadoEnvio = new MostrarEnvio(MostrandoProceso);
        }

        private void MostrandoProceso(long x, long y)
        {
            prgEnvio.Maximum = (int)x;
            prgEnvio.Value = (int)y;
            if (x == y)
            {
                btnCerrarPuerto.Enabled = true;
                prgEnvio.Value = 0;
            }
                
        }

        private void ObjTrRX_proceso(long tam, long avance)
        {
            Invoke(delegadoEnvio, tam, avance);
        }
        //Este metodo se desencadena cuando se dispare el evento LlegoMensaje
        private void ObjTrRx_LlegoMensaje(object o, string mm)
        {
            //MessageBox.Show("Se disparó: "+mm);
            Invoke(delegadoMostrar, mm);
        }
        private void MostrandoMensaje(string textMens)
        {
            rchConversacion.Text += "Otro: " + textMens + "\n";
        }

        private void CbmPuerto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbmPuerto.SelectedIndex == -1)//No queria :c
                return;
            try
            {
                x = cbmPuerto.Text.ToString();
                baudrate = (int)nudBaudRate.Value;
                //Inicializa puerto
                objTrRX.Inicializa(x,baudrate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir puerto, " +ex.Message);
                cbmPuerto.SelectedIndex = -1;
                btnEnviar.Enabled = false;
                btnSelectFile.Enabled = false;
                return;
            }
            MessageBox.Show("Puerto " + x + " abierto");
            btnCerrarPuerto.Text = "CERRAR PUERTO " + x;
            CambiarEstado(true);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTrRX.CerrarPuerto();
        }
        private void BtnCerrarPuerto_Click(object sender, EventArgs e)
        {
            objTrRX.CerrarPuerto();
            btnCerrarPuerto.Text = "CERRAR PUERTO";
            cbmPuerto.SelectedIndex = -1;
            CambiarEstado(false);
        }
        private void CambiarEstado(bool estado)
        {
            btnEnviar.Enabled = estado;
            btnCerrarPuerto.Enabled = estado;
            btnSelectFile.Enabled = estado;
            grpParametros.Enabled = !estado;
        }
        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    objTrRX.IniciaEnvioArchivo(ofdOpenFile.FileName);
                    MessageBox.Show("Enviando Archivo");
                    btnCerrarPuerto.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar el archivo: " + ex.Message);
                    return;
                }
            }
        }
        private void RchConversacion_TextChanged(object sender, EventArgs e)
        {
            rchConversacion.SelectionStart = rchConversacion.Text.Length;
            rchConversacion.ScrollToCaret();
        }
        private void RutaDescarga()
        {
            string path = ConfigurationManager.AppSettings["Path"];
            if (path == "")
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
                                node.Attributes[1].Value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        }
                    }
                }
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection("appSettings");
            }

        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }//Para el final

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objTrRX.CerrarPuerto();
            Application.Exit();
        }
    }
}
