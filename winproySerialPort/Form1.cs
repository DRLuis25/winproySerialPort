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
        int baudrate;
        string x;
        Thread envioarchivo;
        public Form1()
        {
            InitializeComponent();
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            objTrRX.Enviar(rchMensajes.Text.Trim());
            rchConversacion.Text += "Tú: " + rchMensajes.Text.Trim() + "\n";
            rchMensajes.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {    
            //MessageBox.Show(ConfigurationManager.AppSettings["Path"]);
            RutaDescarga();
            //MessageBox.Show(ConfigurationManager.AppSettings["Path"]);
            btnEnviar.Enabled = false;
            objTrRX = new classTransRecep();
            objTrRX.LlegoMensaje += new classTransRecep.HandlerTxRx(objTrRx_LlegoMensaje);//Se adiciona el delegado
            delegadoMostrar = new MostrarOtroProceso(MostrandoMensaje);
        }
        /*
        private void btnRecibir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("faltan por salir a enviar: " + objTrRX.BytesPorSalir().ToString());
        }
        */
        //Este metodo se desencadena cuando se dispare el evento LlegoMensaje
        private void objTrRx_LlegoMensaje(object o, string mm)
        {
            //MessageBox.Show("Se disparó: "+mm);
            Invoke(delegadoMostrar, mm);
        }
        private void MostrandoMensaje(string textMens)
        {
            rchConversacion.Text += "Otro: " + textMens + "\n";
        }

        private void cbmPuerto_SelectedIndexChanged(object sender, EventArgs e)
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
                return;
            }
            MessageBox.Show("Puerto " + x + " abierto");
            btnCerrarPuerto.Text = "CERRAR PUERTO " + x;
            btnCerrarPuerto.Enabled = true;
            btnEnviar.Enabled = true;
            grpParametros.Enabled = false;//Así no deshabilito uno por uno xD
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Environment.Exit(Environment.ExitCode);
            objTrRX.CerrarPuerto();
        }

        private void btmCerrarPuerto_Click(object sender, EventArgs e)
        {
            objTrRX.CerrarPuerto();
            btnCerrarPuerto.Text = "CERRAR PUERTO";
            cbmPuerto.SelectedIndex = -1;
            btnEnviar.Enabled = false;
            btnCerrarPuerto.Enabled = false;
            grpParametros.Enabled = true;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Archivo Seleccionado");
                //label3.Text=ofdOpenFile.FileName;//Ruta completa
                MessageBox.Show(Path.GetFileName(ofdOpenFile.FileName));//Nombre archivo
                objTrRX.IniciaEnvioArchivo(ofdOpenFile.FileName);
                MessageBox.Show("Se empezó a enviar");
                envioarchivo = new Thread(Progreso);
                envioarchivo.Start();
            }
        }
        private void Progreso()
        {
            //while()
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void RutaDescarga()
        {
            string path = ConfigurationManager.AppSettings["Path"];     
            if(path=="")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                foreach(XmlElement element in xmlDoc.DocumentElement)
                {
                    if (element.Name.Equals("appSettings"))
                    {
                        foreach(XmlNode node in element.ChildNodes)
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

        private void rchConversacion_TextChanged(object sender, EventArgs e)
        {
            rchConversacion.SelectionStart = rchConversacion.Text.Length;
            rchConversacion.ScrollToCaret();
        }
    }
}
