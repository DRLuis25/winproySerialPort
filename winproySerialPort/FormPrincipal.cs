using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
//using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace winproySerialPort
{
    public partial class FormPrincipal : Form
    {
        
        public ClassTransRecep objTrRX;
        private int baudrate;
        private string x;
        private FormChat form2;
        private FormEnviando form3;
        private FormRecibiendo form4;
        private FormSettings form5;
        public FormPrincipal()
        {
            InitializeComponent();
            hideSubMenu();
        }
        
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            btnChat.Enabled = false;
            btnTransferencias.Enabled = false;
            RutaDescarga();
            form2 = new FormChat();
            form2.Enviarmsje += new FormChat.HandlerEnviarmsje(objTrRX_Enviarmsje);
            form2.EnviarArchivo += new FormChat.HandlerEnviarArchivo(objTrRX_EnviarArchivo);
            form3 = new FormEnviando();
            form4 = new FormRecibiendo();
            form5 = new FormSettings();
            objTrRX = new ClassTransRecep();
            objTrRX.LlegoMensaje += new ClassTransRecep.HandlerTxRx(form2.ObjTrRx_LlegoMensaje);//Se adiciona el delegado
            objTrRX.Proceso += new ClassTransRecep.HandlerProceso(form3.ObjTrRX_proceso);
            objTrRX.Proceso += new ClassTransRecep.HandlerProceso(form4.ObjTrRX_proceso);
            objTrRX.InicioProceso += new ClassTransRecep.HandlerInicioProceso(form3.ObjTrRX_InicioProceso);
            objTrRX.InicioProceso += new ClassTransRecep.HandlerInicioProceso(form4.ObjTrRX_InicioProceso);
        }

        private void objTrRX_EnviarArchivo(string[] path)
        {
            Thread envio = new Thread(() => Enviar(path));
            envio.Start();
            openChildForm(form3);
        }
        private void Enviar(string[] paths)
        {
            foreach (string item in paths)
            {
                objTrRX.IniciaEnvioArchivo(item);
                Thread.Sleep(100);
            }
        }
        private void objTrRX_Enviarmsje(string msje)
        {
            objTrRX.Enviar(msje);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTrRX.CerrarPuerto();
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
        private void Form1_DragEnter(object sender, DragEventArgs e)//NO SE USA
        {
            e.Effect = DragDropEffects.All;
        }//Para el final

        private void cbmPuerto_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbmPuerto.SelectedIndex == -1)//No queria :c
                return;
            try
            {
                x = cbmPuerto.Text.ToString();
                baudrate = (int)nudBaudRate.Value;
                //Inicializa puerto
                objTrRX.Inicializa(x, baudrate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir puerto, " + ex.Message);
                cbmPuerto.SelectedIndex = -1;
                //btnEnviar.Enabled = false;
                //btnSelectFile.Enabled = false;
                return;
            }
            MessageBox.Show("Puerto " + x + " abierto");
            btnCerrarPuerto.Text = "Cerrar Puerto " + x;
            CambiarEstado(true);
            openChildForm(form3);
            form3.Hide();
            openChildForm(form4);
            form4.Hide();
            openChildForm(form2);
        }
        private void CambiarEstado(bool estado)
        {
            //btnEnviar.Enabled = estado;
            btnCerrarPuerto.Enabled = estado;
            //btnSelectFile.Enabled = estado;
            grpParametros.Enabled = !estado;
            btnChat.Enabled = estado;
            btnTransferencias.Enabled = estado;
        }
        //private void ajustesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Form ajustes = new FormSettings();
        //    ajustes.Show();
        //}

        //Codigo visual
        private void openChildForm(Form childForm)
        {
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void btnTransferencias_Click(object sender, EventArgs e)
        {
            showSubMenu(panelTransferenciasSubMenu);
        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }
        private void hideSubMenu()
        {
            panelTransferenciasSubMenu.Visible = false;
        }

        private void btnEnviando_Click(object sender, EventArgs e)
        {
            openChildForm(form3);
            hideSubMenu();
        }

        private void btnRecibiendo_Click(object sender, EventArgs e)
        {
            openChildForm(form4);
            hideSubMenu();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            openChildForm(form2);
            hideSubMenu();
        }
        private void btnCerrarPuerto_Click(object sender, EventArgs e)
        {
            objTrRX.CerrarPuerto();
            btnCerrarPuerto.Text = "Cerrar Puerto";
            cbmPuerto.SelectedIndex = -1;
            CambiarEstado(false);
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTrRX.CerrarPuerto();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            objTrRX.CerrarPuerto();
            Application.Exit();
        }

        private void btnAjustes_Click(object sender, EventArgs e)
        {
            openChildForm(form5);
        }
    }
}
