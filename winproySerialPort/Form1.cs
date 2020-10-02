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
        private static readonly object control = new object();
        ClassTransRecep objTrRX;
        delegate void MostrarOtroProceso(string mensaje);
        MostrarOtroProceso delegadoMostrar;
        //Delegado proceso Envío
        delegate void MostrarEnvio(long tam, long avance, int num, bool ED);
        MostrarEnvio delegadoEnvio;
        delegate void MostrarInicioEnvio(int num, string nombreArchivo, bool ED);
        MostrarInicioEnvio delegadoInicioEnvio;
        int baudrate;
        string x;
        private int y;
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
            
            //if (Path.IsPathRooted("F"))
            //    MessageBox.Show("yey");
            //else
            //    MessageBox.Show("no :c");
            RutaDescarga();
            objTrRX = new ClassTransRecep();
            objTrRX.LlegoMensaje += new ClassTransRecep.HandlerTxRx(ObjTrRx_LlegoMensaje);//Se adiciona el delegado
            objTrRX.Proceso += new ClassTransRecep.HandlerProceso(ObjTrRX_proceso);
            objTrRX.InicioProceso += new ClassTransRecep.HandlerInicioProceso(ObjTrRX_InicioProceso);
            delegadoMostrar = new MostrarOtroProceso(MostrandoMensaje);
            delegadoEnvio = new MostrarEnvio(MostrandoProceso);
            delegadoInicioEnvio = new MostrarInicioEnvio(MostrandoInicioProceso);
            y = 3;
        }
        private void MostrandoProceso(long tam, long avance, int num, bool ED)
        {
            lock (control)
            {
                if (ED)
                {
                    GroupBox group = flpEnviando.Controls.OfType<GroupBox>().FirstOrDefault(b => b.Name.Equals("grpArchivoN" + num.ToString("D4")));
                    //Label etiqueta = group.Controls.OfType<Label>().FirstOrDefault(b => b.Name.Equals("lblArchivoN" + num.ToString("D4")));
                    ProgressBar proceso = group.Controls.OfType<ProgressBar>().FirstOrDefault(b => b.Name.Equals("prgArchivoN" + num.ToString("D4")));
                    //Label button = groupBox1.Controls.OfType<Button>().FirstOrDefault(b => b.Name.Equals("btn"));
                    //etiqueta.Text = "Changed...";
                    proceso.Maximum = (int)tam;
                    proceso.Value = (int)avance;
                    //VIEJO
                    //prgEnvio.Maximum = (int)x;
                    //prgEnvio.Value = (int)y;
                    //if (x == y)
                    //{
                    //    btnCerrarPuerto.Enabled = true;
                    //    prgEnvio.Value = 0;
                    //}
                }
                else
                {
                    GroupBox group = flpDescargando.Controls.OfType<GroupBox>().FirstOrDefault(b => b.Name.Equals("grpArchivoN" + num.ToString("D4")));
                    //Label etiqueta = group.Controls.OfType<Label>().FirstOrDefault(b => b.Name.Equals("lblArchivoN" + num.ToString("D4")));
                    ProgressBar proceso = group.Controls.OfType<ProgressBar>().FirstOrDefault(b => b.Name.Equals("prgArchivoN" + num.ToString("D4")));
                    //Label button = groupBox1.Controls.OfType<Button>().FirstOrDefault(b => b.Name.Equals("btn"));
                    //etiqueta.Text = "Changed...";
                    proceso.Maximum = (int)tam;
                    proceso.Value = (int)avance;
                }
            }
        }
        private void MostrandoInicioProceso(int num, string nombreArchivo, bool ED)
        {
            lock (control)
            {
                if (ED)
                {
                    GroupBox insertar = ArchivoNuevo(num, nombreArchivo,"E");
                    flpEnviando.Controls.Add(insertar);
                }
                else
                {
                    GroupBox insertar = ArchivoNuevo(num, nombreArchivo,"D");
                    flpDescargando.Controls.Add(insertar);
                }
            }
        }
        private GroupBox ArchivoNuevo(int num, string nombreArchivo,string x)
        {
            GroupBox grpArchivoN = new GroupBox();
            Button btnCerrarArchivoN = new Button();
            ProgressBar prgArchivoN = new ProgressBar();
            Label lblArchivoN = new Label();
            Label lbltemp = new Label();
            // 
            // lblArchivoN
            // 
            lblArchivoN.AutoSize = true;
            lblArchivoN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblArchivoN.Location = new System.Drawing.Point(30, 30);
            lblArchivoN.Name = "lblArchivoN" + num.ToString("D4");
            lblArchivoN.Size = new System.Drawing.Size(146, 20);
            lblArchivoN.TabIndex = 0;
            lblArchivoN.Text = nombreArchivo.Length>15? nombreArchivo.Substring(0, 12)+"...":nombreArchivo;
            // 
            // lbltemp
            // 
            lbltemp.Name = "lblTemp" + num.ToString("D4");
            lbltemp.Text = x;
            //lbltemp.Visible = false;
            // 
            // prgArchivoN
            // 
            prgArchivoN.Location = new System.Drawing.Point(220, 25);
            prgArchivoN.Name = "prgArchivoN" + num.ToString("D4");
            prgArchivoN.Size = new System.Drawing.Size(300, 30);
            prgArchivoN.TabIndex = 1;
            // 
            // btnCerrarArchivoN
            // 
            btnCerrarArchivoN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnCerrarArchivoN.Location = new System.Drawing.Point(630, 25);
            btnCerrarArchivoN.Name = "btnCerrarArchivoN" + num.ToString("D4");
            btnCerrarArchivoN.Size = new System.Drawing.Size(30, 30);
            btnCerrarArchivoN.TabIndex = 2;
            btnCerrarArchivoN.Text = "X";
            btnCerrarArchivoN.UseVisualStyleBackColor = true;
            btnCerrarArchivoN.Click += new System.EventHandler(Borrar);
            // 
            // grpArchivoN
            // 
            grpArchivoN.Controls.Add(btnCerrarArchivoN);
            grpArchivoN.Controls.Add(prgArchivoN);
            grpArchivoN.Controls.Add(lblArchivoN);
            grpArchivoN.Controls.Add(lbltemp);
            grpArchivoN.Location = new System.Drawing.Point(y, 3);
            y += 86;
            grpArchivoN.Name = "grpArchivoN" + num.ToString("D4");
            grpArchivoN.Size = new System.Drawing.Size(694, 80);
            grpArchivoN.TabIndex = 0;
            grpArchivoN.TabStop = false;
            return grpArchivoN;
        }
        private void Borrar(object sender, EventArgs e)
        {
            GroupBox group;
            string num;
            Button button = sender as Button;
            num = button.Name.Substring(17, 4);
            group = flpEnviando.Controls.OfType<GroupBox>().FirstOrDefault(b => b.Name.Equals("grpArchivoN" + num));
            if(group == null)
                group = flpDescargando.Controls.OfType<GroupBox>().FirstOrDefault(b => b.Name.Equals("grpArchivoN" + num));
            Label etiqueta = group.Controls.OfType<Label>().FirstOrDefault(b => b.Name.Equals("lblTemp" + num));
            string x = etiqueta.Text;
            ProgressBar proceso = group.Controls.OfType<ProgressBar>().FirstOrDefault(b => b.Name.Equals("prgArchivoN" + num));
            if (proceso.Value == proceso.Maximum )
            {
                if(x=="E")
                    flpEnviando.Controls.Remove(flpEnviando.Controls.Find("grpArchivoN" + num, true)[0]);
                else
                    flpDescargando.Controls.Remove(flpDescargando.Controls.Find("grpArchivoN" + num, true)[0]);
            }
        }
        private void ObjTrRX_InicioProceso(int num, string nombreArchivo, bool ED)
        {
            Invoke(delegadoInicioEnvio, num, nombreArchivo, ED);
        }
        private void ObjTrRX_proceso(long tam, long avance, int num, bool ED)
        {

            Invoke(delegadoEnvio, tam, avance, num, ED);
        }
        
        private void ObjTrRx_LlegoMensaje(object o, string mm)
        {
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
                    //Añadir la progressbar
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
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programa Elaborado por \nDelgado Rodríguez Luis Guillermo");
        }

        private void ajustesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form ajustes = new FormSettings();
            ajustes.Show();
        }
    }
}
