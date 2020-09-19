using System;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace winproySerialPort
{
    public partial class ClassTransRecep
    {
        private static readonly object control = new object();
        private SerialPort puerto;
        private Boolean BufferSalidaVacio;
        //Envío trama
        byte[] TramaEnvio;
        byte[] TramaCabeceraEnvio;
        readonly byte[] TramaRelleno;
        //Recibir
        byte[] TramaRecibida;
        //Lista Enviando
        LinkedList<ClassArchivoEnviando> listaEnviando = new LinkedList<ClassArchivoEnviando>();
        LinkedList<ClassArchivoEnviando> listaRecibiendo = new LinkedList<ClassArchivoEnviando>();
        public ClassTransRecep()
        {
            TramaEnvio = new byte[1024];
            TramaCabeceraEnvio = new byte[5];
            TramaRelleno= new byte[1024];
            TramaRecibida = new byte[1024];
            for (int i = 0; i < 1024; i++)
                TramaRelleno[i] = 64;
            BufferSalidaVacio = true;
            rarchivo = true;
        }
        public void Inicializa(string NombrePuerto,int baudrate)
        {
            try
            {
                puerto = new SerialPort(NombrePuerto, baudrate, Parity.Even, 8, StopBits.Two);
                puerto.ReceivedBytesThreshold = 1024;
                puerto.DataReceived += new SerialDataReceivedEventHandler(Puerto_DataReceived);
                puerto.Open();
                procesoVerificaSalida = new Thread(VerificandoSalida);
                procesoVerificaSalida.Start();
                archivoEnviar = new ClassArchivoEnviando();
                archivoRecibir = new ClassArchivoEnviando();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void Puerto_DataReceived(object o, SerialDataReceivedEventArgs sd)
        {
            if(puerto.BytesToRead>=1024)
            {
                puerto.Read(TramaRecibida, 0, 1024);
                //Decodificar tarea
                string tarea = ASCIIEncoding.UTF8.GetString(TramaRecibida, 0, 1);
                switch (tarea)
                {
                    case "M":
                        procesoRecibirMensaje = new Thread(RecibiendoMensaje);
                        procesoRecibirMensaje.Start();
                        break;
                    case "I":
                        if (rarchivo)
                        {
                            procesoConstruyeArchivo = new Thread(ConstruirArchivo);
                            procesoConstruyeArchivo.Start();
                        }
                        //ConstruirArchivo();//Comentar? YES
                        break;
                    case "C":
                        /*
                        if(MessageBox.Show("Recibir Archivo?", "Archivo entrante", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {}*/
                        InicioConstruirArchivo();
                        break;
                    default:
                        MessageBox.Show("Trama no reconocida");
                        break;
                }
            }
        }
        private void VerificandoSalida()
        {
            while (puerto.IsOpen)
            {
                if (puerto.BytesToWrite > 0)//Buffer de salida no vacío
                    BufferSalidaVacio = false;
                else
                    BufferSalidaVacio = true;
            }
        }
        public int BytesPorSalir()
        {
            int cantBytes = 0;
            if (BufferSalidaVacio == false)
                cantBytes = puerto.BytesToWrite;
            return cantBytes;
        }
        public void CerrarPuerto()
        {
            if(puerto!=null)
            puerto.Close();
        }
    }
}
