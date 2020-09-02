using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO; //Nuevo
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace winproySerialPort
{
    class classTransRecep
    {
        private static object control = new object();

        public delegate void HandlerTxRx(object oo, string mensRec);
        public event HandlerTxRx LlegoMensaje;
        //Delegado para envío archivo
        public delegate void HandlerProceso(long tam, long avance);
        public event HandlerProceso Proceso;
        //archivoEnviar
        private ClassArchivoEnviando archivoEnviar;
        private FileStream FlujoArchivoEnviar;
        private BinaryReader LeyendoArchivo;
        //archivoRecibir
        private ClassArchivoEnviando archivoRecibir;
        private FileStream FlujoArchivoRecibir;
        private BinaryWriter EscribiendoArchivo;
        //Hilos envío Mensaje
        Thread procesoEnvio;
        Thread procesoVerificaSalida;
        Thread procesoRecibirMensaje;
        //Hilos envío Archivo
        Thread procesoEnvioArchivo;
        Thread procesoConstruyeArchivo;
        private SerialPort puerto;
        //Mensaje
        private string mensajeEnviar;
        private string mensRecibido;

        private Boolean BufferSalidaVacio;
        //Envío trama
        byte[] TramaEnvio;
        byte[] TramaCabeceraEnvio;
        readonly byte[] TramaRelleno;
        //Recibir
        byte[] TramaRecibida;
        private bool rarchivo;
        public classTransRecep()
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
                //MessageBox.Show("Error al inicializar el puerto "+ NombrePuerto);
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
        private void RecibiendoMensaje()
        {
            int LongMensRec;
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            try
            {
                LongMensRec = Convert.ToInt16(CabRec);
                mensRecibido = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
                OnLlegoMensaje();
            }
            catch (Exception e)
            {

                MessageBox.Show("Ha ocurrido un error al recibir un mensaje: " + e.Message);
            }
        }
        protected virtual void OnLlegoMensaje()
        {
            if (LlegoMensaje != null)
            {
                LlegoMensaje(this, mensRecibido);//No está aquí F
            }
        }
        protected virtual void OnProcesoEnvio()
        {
            if (Proceso != null)
            {
                Proceso(archivoEnviar.Tamaño,archivoEnviar.Avance);//No está aquí F
            }
        }
        public void Enviar(string mens)
        {
            mensajeEnviar = mens;
            int l = mensajeEnviar.Length;
            //Añadir ceros a la izq
            string LongitudMensaje = "M"+l.ToString("D4");
            TramaEnvio = ASCIIEncoding.UTF8.GetBytes(mensajeEnviar);
            TramaCabeceraEnvio = ASCIIEncoding.UTF8.GetBytes(LongitudMensaje);
            procesoEnvio = new Thread(Enviando);
            procesoEnvio.Start();
        }
        private void Enviando()
        {
            lock (control) { 
            puerto.Write(TramaCabeceraEnvio, 0, 5);
            puerto.Write(TramaEnvio, 0, TramaEnvio.Length);
            puerto.Write(TramaRelleno, 0, 1019 - TramaEnvio.Length);
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
        public void IniciaEnvioArchivo(string path)
        {
            try
            {
                FlujoArchivoEnviar = new FileStream(path, FileMode.Open, FileAccess.Read);
                LeyendoArchivo = new BinaryReader(FlujoArchivoEnviar);
                archivoEnviar.Nombre = Path.GetFileName(path);
                archivoEnviar.Tamaño = FlujoArchivoEnviar.Length;
                archivoEnviar.Avance = 0;
                archivoEnviar.Num = 1;
                archivoEnviar.Activo = true;
                EnviarCabecera();               //If(Llegó la cabecera){Enviar Información} (Falta x'D)
                procesoEnvioArchivo = new Thread(EnviandoArchivo);
                procesoEnvioArchivo.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR!: " + e.Message);
                throw;
            }
        }
        private void EnviarCabecera()
        {
            //Envia una trama mensaje que contiene el nombre del archivo a enviar y su tamaño
            byte[] TramaEnvioArchivo = new byte[1019];
            byte[] TramaCabeceraEnvioArchivo = new byte[5];
            string cabeceraArchivo = "C" + archivoEnviar.Nombre.Length.ToString("D4");
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(cabeceraArchivo);
            TramaEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(archivoEnviar.Nombre + archivoEnviar.Tamaño.ToString("D19"));
            lock (control)
            {
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioArchivo, 0, TramaEnvioArchivo.Length);
                puerto.Write(TramaRelleno, 0, 1019 - TramaEnvioArchivo.Length);
            }
        }
        private void EnviandoArchivo()
        {
            //Envia las tramas de información
            byte[] TramaEnvioArchivo = new byte[1019];
            byte[] TramaCabeceraEnvioArchivo = new byte[5];
            //Se deberia esperar la confirmación para enviar el archivo(En la final será, idk)
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("I0001");
            while (archivoEnviar.Avance<=(archivoEnviar.Tamaño-1019))
            {
                LeyendoArchivo.Read(TramaEnvioArchivo, 0, 1019);
                archivoEnviar.Avance += 1019;
                while (!BufferSalidaVacio)
                {
                    //Esperamos a que se envie 
                }
                lock (control) { 
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioArchivo, 0, 1019);
                }
                OnProcesoEnvio();
            }
            int tamanito = Convert.ToInt16(archivoEnviar.Tamaño - archivoEnviar.Avance);
            LeyendoArchivo.Read(TramaEnvioArchivo, 0, tamanito);
            //Envío de lo que queda del archivo + un relleno
            lock (control)
            {
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioArchivo, 0, tamanito);
                puerto.Write(TramaRelleno, 0, 1019 - tamanito);
            }
            //Cerrar el flujo
            archivoEnviar.Avance = archivoEnviar.Tamaño;
            archivoEnviar.Activo = false;
            LeyendoArchivo.Close();
            FlujoArchivoEnviar.Close();
            OnProcesoEnvio();
            MessageBox.Show("Archivo Enviado");
        }
        private void InicioConstruirArchivo()
        {
            string nombreArchivo;
            long tamarchivo;
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            int LongMensRec = Convert.ToInt16(CabRec);
            nombreArchivo = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
            tamarchivo= long.Parse(ASCIIEncoding.UTF8.GetString(TramaRecibida, LongMensRec+5, 19));
            try
            {
                //Crea un archivo en el disco
                string filepath = Path.Combine(ConfigurationManager.AppSettings["Path"], nombreArchivo);
                //filepath = ChangeFileName(filepath);
                FlujoArchivoRecibir = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);//Manejar excepcion
                EscribiendoArchivo = new BinaryWriter(FlujoArchivoRecibir);
                archivoRecibir.Nombre = nombreArchivo;
                archivoRecibir.Num = 1;
                archivoRecibir.Tamaño = tamarchivo;//Obviamente obtener tamaño
                archivoRecibir.Avance = 0;
                archivoRecibir.Activo = true;
                rarchivo = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR!: No se ha podido crear el archivo: " + e.Message);
                rarchivo = false;
                //throw;
            }
        }
        private string ChangeFileName(string fullpath)
        {
            if (System.IO.File.Exists(fullpath))
            {
                //MessageBox.Show("El archivo existe");
                int n = 0;
                string temp = fullpath;
                do
                {
                    n++;
                    fullpath = Path.Combine(Path.GetDirectoryName(temp), Path.GetFileNameWithoutExtension(temp) + "(" + n + ")" + Path.GetExtension(temp));
                } while (System.IO.File.Exists(fullpath));
                
            }
            return fullpath;
        }
        private void ConstruirArchivo()
        {
            if (archivoRecibir.Avance <= archivoRecibir.Tamaño - 1019)
            {
                EscribiendoArchivo.Write(TramaRecibida,5,1019);
                archivoRecibir.Avance += 1019;
            }
            else
            {
                int tamanito = Convert.ToInt16(archivoRecibir.Tamaño - archivoRecibir.Avance);
                EscribiendoArchivo.Write(TramaRecibida,5,tamanito);
                EscribiendoArchivo.Close();
                FlujoArchivoRecibir.Close();
                archivoRecibir.Activo = false;
                MessageBox.Show("Archivo recibido");
            }
        }
    }
}
