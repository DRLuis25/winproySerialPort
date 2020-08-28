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

namespace winproySerialPort
{
    class classTransRecep
    {
        public delegate void HandlerTxRx(object oo, string mensRec);
        public event HandlerTxRx LlegoMensaje;
        //Nuevo
        private ClassArchivoEnviando archivoEnviar;
        private FileStream FlujoArchivoEnviar;
        private BinaryReader LeyendoArchivo;
        //
        //Nuevo 2
        private ClassArchivoEnviando archivoRecibir;
        private FileStream FlujoArchivoRecibir;
        private BinaryWriter EscribiendoArchivo;
        //
        Thread procesoEnvio;
        Thread procesoVerificaSalida;
        Thread procesoRecibirMensaje;
        //Nuevo
        Thread procesoEnvioArchivo;
        Thread procesoConstruyeArchivo;
        //
        private SerialPort puerto;
        //Enviar
        private string mensajeEnviar;
        private string mensRecibido;

        private Boolean BufferSalidaVacio;

        byte[] TramaEnvio;
        byte[] TramaCabeceraEnvio;
        byte[] TramaRelleno;
        //Recibir
        byte[] TramaRecibida;
        public classTransRecep()
        {
            TramaEnvio = new byte[1024];
            TramaCabeceraEnvio = new byte[5];
            TramaRelleno= new byte[1024];//Completa el tamaño de la trama con algo xD
            TramaRecibida = new byte[1024];
            for (int i = 0; i < 1024; i++)
                TramaRelleno[i] = 64;
            BufferSalidaVacio = true;
        }
        public void Inicializa(string NombrePuerto,int baudrate)
        {
            try
            {
                puerto = new SerialPort(NombrePuerto, baudrate, Parity.Even, 8, StopBits.Two);
                puerto.ReceivedBytesThreshold = 1024;
                puerto.DataReceived += new SerialDataReceivedEventHandler(puerto_DataReceived);//Aquí se instancia el evento LlegoMensaje
                puerto.Open();
                //MessageBox.Show("apertura del puerto" + puerto.PortName);
                procesoVerificaSalida = new Thread(VerificandoSalida);
                procesoVerificaSalida.Start();
                //Nuevo
                archivoEnviar = new ClassArchivoEnviando();
                archivoRecibir = new ClassArchivoEnviando();
            }
            catch (Exception e)
            {
                //MessageBox.Show("Error al inicializar el puerto "+ NombrePuerto);
                throw e;
            }
        }
        private void puerto_DataReceived(object o, SerialDataReceivedEventArgs sd)
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
                        /*
                         * caso "AC"=letra "F":Debe instanciar los flujos y el binary de escritura(InicioConstruirArchivo)
                         * 
                         */
                    case "A":
                        procesoConstruyeArchivo = new Thread(ConstruirArchivo);
                        procesoConstruyeArchivo.Start();
                        //ConstruirArchivo();//Comentar? YES
                        break;
                    case "I":
                        break;
                    default:
                        MessageBox.Show("Trama no reconocida");
                        break;
                }
            }
        }
        private void RecibiendoMensaje()
        {
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            int LongMensRec = Convert.ToInt16(CabRec);
            mensRecibido = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
            OnLlegoMensaje();//Evento de esta clase
        }
        protected virtual void OnLlegoMensaje()
        {
            if (LlegoMensaje != null)
            {
                LlegoMensaje(this, mensRecibido);//El metodo no está aquí F
            }
                
        }
        public void Enviar(string mens)
        {
            mensajeEnviar = mens;
            //Prueba
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
            puerto.Write(TramaCabeceraEnvio, 0, 5);
            puerto.Write(TramaEnvio, 0, TramaEnvio.Length);
            puerto.Write(TramaRelleno, 0, 1019 - TramaEnvio.Length);
            //MessageBox.Show("mensaje terminado de enviar");
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
        //Nuevo
        public void IniciaEnvioArchivo(string nombre)
        {
            //abrirlo, manejar el control de errores(excepciones)
            FlujoArchivoEnviar=new FileStream(nombre, FileMode.Open,FileAccess.Read);
            LeyendoArchivo=new BinaryReader(FlujoArchivoEnviar);
            archivoEnviar.Nombre = nombre;
            archivoEnviar.Tamaño = FlujoArchivoEnviar.Length;
            MessageBox.Show("Tamaño archivo: " + archivoEnviar.Tamaño);
            archivoEnviar.Avance = 0;
            archivoEnviar.Num = 1;
            //leerlo en stream
            //iniciar una hebra de envio
            procesoEnvioArchivo = new Thread(EnviandoArchivo);
            procesoEnvioArchivo.Start();
        }
        //
        private void EnviandoArchivo()
        {
            byte[] TramaEnvioArchivo;
            byte[] TramaCabeceraEnvioArchivo;
            //byte[] TramaRellenoArchivo; //No es necesario
            TramaEnvioArchivo = new byte[1019];
            TramaCabeceraEnvioArchivo = new byte[5];
            //Enviar la primera trama con el nombre del archivo (tipo mensaje?)
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("AC001");
            //Enviar las tramas de información
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("AI001");
            while (archivoEnviar.Avance<=(archivoEnviar.Tamaño-1019))
            {
                LeyendoArchivo.Read(TramaEnvioArchivo, 0, 1019);
                archivoEnviar.Avance += 1019;
                //Envío de una trama llena de 1019 bytes del archivo
                while (!BufferSalidaVacio)
                {
                    //Esperamos a que se envie 
                }
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioArchivo, 0, TramaEnvioArchivo.Length);
            }
            int tamanito = Convert.ToInt16(archivoEnviar.Tamaño - archivoEnviar.Avance);
            LeyendoArchivo.Read(TramaEnvioArchivo, 0, tamanito);
            //Envío de lo que queda del archivo + el relleno
            while (!BufferSalidaVacio)
            {
                //Esperamos a que se envie 
            }
            puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
            puerto.Write(TramaEnvioArchivo, 0, tamanito);
            puerto.Write(TramaRelleno, 0, 1019 - tamanito);
            //Cerrar el flujo
            LeyendoArchivo.Close();
            FlujoArchivoEnviar.Close();
        }
        public void InicioConstruirArchivo(string nombre)//faltan parametros del archivo //Deberia ser privado
        {
            FlujoArchivoRecibir = new FileStream(nombre, FileMode.Create, FileAccess.Write);//Manejar excepcion
            EscribiendoArchivo = new BinaryWriter(FlujoArchivoRecibir);
            archivoRecibir.Nombre = nombre;
            archivoRecibir.Num = 1;
            archivoRecibir.Tamaño = 135671;//Obviamente obtener tamaño
            archivoRecibir.Avance = 0;
        }
        private void ConstruirArchivo()
        {
            //Debe realizarse en funcion del tamaño 1019 y la ultima será tamanito(lo sacamos del avance)
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
                MessageBox.Show("Archivo cerrado");
            }
            //Debe actualizarse el tamaño de la recepción en archivoRecibir
        }
    }
}
