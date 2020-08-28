using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace winproySerialPort
{
    public partial class Form1 : Form
    {
        classTransRecep objTrRX;
        delegate void MostrarOtroProceso(string mensaje);
        MostrarOtroProceso delegadoMostrar;
        int baudrate;
        string x;
        public Form1()
        {
            InitializeComponent();
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            objTrRX.Enviar(rchMensajes.Text.Trim());
            rchMensajes.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnviar.Enabled = false;
            objTrRX = new classTransRecep();
            objTrRX.LlegoMensaje += new classTransRecep.HandlerTxRx(objTrRx_LlegoMensaje);//Se adiciona el delegado
            delegadoMostrar = new MostrarOtroProceso(MostrandoMensaje);
        }

        private void btnRecibir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("faltan por salir a enviar: " + objTrRX.BytesPorSalir().ToString());
        }
        //Este metodo se desencadena cuando se dispare el evento LlegoMensaje
        private void objTrRx_LlegoMensaje(object o, string mm)
        {
            //MessageBox.Show("Se disparó: "+mm);
            Invoke(delegadoMostrar, mm);
        }
        private void MostrandoMensaje(string textMens)
        {
            rchConversacion.Text += textMens + "\n";
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

        private void btnEnviarArchivo_Click(object sender, EventArgs e)
        {
            objTrRX.IniciaEnvioArchivo("D:\\prueba\\archivo1.pdf");
            MessageBox.Show("Se empezó a enviar");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            objTrRX.InicioConstruirArchivo("D:\\prueba\\archivo2.pdf");//Ver si añadir los parametros aqui(creo q nel)
            
        }
    }
}
