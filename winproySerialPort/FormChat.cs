using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winproySerialPort
{
    public partial class FormChat : Form
    {
        public delegate void HandlerEnviarmsje(string msje);
        public event HandlerEnviarmsje Enviarmsje;

        public delegate void HandlerEnviarArchivo(string[] path);
        public event HandlerEnviarArchivo EnviarArchivo;

        public FormChat()
        {
            InitializeComponent();
            
            delegadoMostrar = new MostrarOtroProceso(MostrandoMensaje);
        }
        //Nuevos delegados
        //Pasarle mensaje al form principal

        delegate void MostrarOtroProceso(string mensaje);
        MostrarOtroProceso delegadoMostrar;
        private void rchConversacion_TextChanged(object sender, EventArgs e)
        {
            rchConversacion.SelectionStart = rchConversacion.Text.Length;
            rchConversacion.ScrollToCaret();
        }

        internal void ObjTrRx_LlegoMensaje(object oo, string mm)
        {
            Invoke(delegadoMostrar, mm);
        }


        private void MostrandoMensaje(string textMens)
        {
            rchConversacion.Text += "Otro: " + textMens + "\n";
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string msje = rchMensajes.Text.Trim();
            if (msje != "" && msje.Length <= 1019)
            {
                //objTrRX.Enviar(msje); //CORREGIDO
                Enviarmsje(msje);
                rchConversacion.Text += "Tú: " + rchMensajes.Text.Trim() + "\n";
                rchMensajes.Text = "";
            }
        }
        
        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //objTrRX.IniciaEnvioArchivo(ofdOpenFile.FileName);//FIXED
                    EnviarArchivo(ofdOpenFile.FileNames);
                    //btnCerrarPuerto.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar el archivo: " + ex.Message);
                    return;
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void rchMensajes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string msje = rchMensajes.Text.Trim();
                if (msje != "" && msje.Length <= 1019)
                {
                    //objTrRX.Enviar(msje); //CORREGIDO
                    Enviarmsje(msje);
                    rchConversacion.Text += "Tú: " + rchMensajes.Text.Trim() + "\n";
                    rchMensajes.Text = "";
                }
            }
        }
    }
}
