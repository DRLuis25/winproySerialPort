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
                //MessageBox.Show("Tarea: "+tarea);
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
                    case "I":
                        procesoConstruyeArchivo = new Thread(ConstruirArchivo);
                        procesoConstruyeArchivo.Start();
                        //ConstruirArchivo();//Comentar? YES
                        break;
                    case "C":
                        /*
                        if(MessageBox.Show("Recibir Archivo?", "Archivo entrante", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                        }*/
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
            lock (control) { 
            puerto.Write(TramaCabeceraEnvio, 0, 5);
            puerto.Write(TramaEnvio, 0, TramaEnvio.Length);
            puerto.Write(TramaRelleno, 0, 1019 - TramaEnvio.Length);
            //MessageBox.Show("mensaje terminado de enviar");
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
        //Nuevo
        public void IniciaEnvioArchivo(string path)
        {
            //abrirlo, manejar el control de errores(excepciones)
            FlujoArchivoEnviar=new FileStream(path, FileMode.Open,FileAccess.Read);
            LeyendoArchivo=new BinaryReader(FlujoArchivoEnviar);
            archivoEnviar.Nombre = Path.GetFileName(path);//Aqui se envía el nombre del archivo)
            archivoEnviar.Tamaño = FlujoArchivoEnviar.Length;
            MessageBox.Show("Tamaño archivo: " + archivoEnviar.Tamaño);
            archivoEnviar.Avance = 0;
            archivoEnviar.Num = 1;
            archivoEnviar.Activo = true;
            //leerlo en stream
            //iniciar una hebra de envio
            EnviarCabecera();
            procesoEnvioArchivo = new Thread(EnviandoArchivo);
            procesoEnvioArchivo.Start();
        }
        private void EnviarCabecera()
        {
            byte[] TramaEnvioArchivo;
            byte[] TramaCabeceraEnvioArchivo;
            TramaEnvioArchivo = new byte[1019];
            TramaCabeceraEnvioArchivo = new byte[5];
            //Enviar la primera trama con el nombre del archivo (tipo mensaje?)

            //Enviar cabecera de que es archivo con un nro N
            string cabeceraArchivo = "C" + archivoEnviar.Nombre.Length.ToString("D4"); //+ archivoEnviar.Num.ToString("D4");
            //MessageBox.Show("Cabecera Archivo: "+cabeceraArchivo);
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(cabeceraArchivo);
            //Enviar nombre archivo
            //MessageBox.Show("Trama Archivo: " + archivoEnviar.Nombre + archivoEnviar.Tamaño.ToString("D19"));
            TramaEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(archivoEnviar.Nombre + archivoEnviar.Tamaño.ToString("D19"));
            while (!BufferSalidaVacio)//Primero así luego veo como quitarlo
            {
                //Esperamos a que se envie 
            }
            //Enviar Archivo
            puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
            puerto.Write(TramaEnvioArchivo, 0, TramaEnvioArchivo.Length);
            puerto.Write(TramaRelleno, 0, 1019 - TramaEnvioArchivo.Length);
        }
        private void EnviandoArchivo()
        {
            byte[] TramaEnvioArchivo;
            byte[] TramaCabeceraEnvioArchivo;
            TramaEnvioArchivo = new byte[1019];
            TramaCabeceraEnvioArchivo = new byte[5];
            //Se deberia esperar la confirmación para enviar el archivo(En la final será, idk)
            //Enviar las tramas de información
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("I0001");
            while (archivoEnviar.Avance<=(archivoEnviar.Tamaño-1019))
            {
                //MessageBox.Show("Enviar Avance: " + archivoEnviar.Avance);
                LeyendoArchivo.Read(TramaEnvioArchivo, 0, 1019);
                archivoEnviar.Avance += 1019;
                //Envío de una trama llena de 1019 bytes del archivo
                while (!BufferSalidaVacio)
                {
                    //Esperamos a que se envie 
                }
                lock (control) { 
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioArchivo, 0, 1019);
                }
            }
            int tamanito = Convert.ToInt16(archivoEnviar.Tamaño - archivoEnviar.Avance);
            LeyendoArchivo.Read(TramaEnvioArchivo, 0, tamanito);
            //Envío de lo que queda del archivo + el relleno
            while (!BufferSalidaVacio)
            {
                //Esperamos a que se envie 
            }
            lock (control)
            {
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioArchivo, 0, tamanito);
                puerto.Write(TramaRelleno, 0, 1019 - tamanito);
            }
            //Cerrar el flujo
            archivoEnviar.Activo = false;
            LeyendoArchivo.Close();
            FlujoArchivoEnviar.Close();
            
        }
        private void InicioConstruirArchivo()//faltan parametros del archivo //Deberia ser privado
        {
            string nombreArchivo;
            long tamarchivo;
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            int LongMensRec = Convert.ToInt16(CabRec);
            nombreArchivo = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
            //MessageBox.Show("Ruta recibida: "+Path.Combine(ConfigurationManager.AppSettings["Path"], nombreArchivo));
            tamarchivo= long.Parse(ASCIIEncoding.UTF8.GetString(TramaRecibida, LongMensRec+5, 19));
            //MessageBox.Show("Tamaño Archivo: " + tamarchivo);
            FlujoArchivoRecibir = new FileStream(Path.Combine(ConfigurationManager.AppSettings["Path"], nombreArchivo), FileMode.Create, FileAccess.Write);//Manejar excepcion
            EscribiendoArchivo = new BinaryWriter(FlujoArchivoRecibir);
            archivoRecibir.Nombre = nombreArchivo;
            archivoRecibir.Num = 1;
            archivoRecibir.Tamaño = tamarchivo;//Obviamente obtener tamaño
            archivoRecibir.Avance = 0;
            archivoRecibir.Activo = true;
        }
        private void ConstruirArchivo()
        {
            //Debe realizarse en funcion del tamaño 1019 y la ultima será tamanito(lo sacamos del avance)
            if (archivoRecibir.Avance <= archivoRecibir.Tamaño - 1019)
            {
                EscribiendoArchivo.Write(TramaRecibida,5,1019);
                archivoRecibir.Avance += 1019;
                //MessageBox.Show("Recibir Avance: " + archivoRecibir.Avance);
            }
            else
            {
                int tamanito = Convert.ToInt16(archivoRecibir.Tamaño - archivoRecibir.Avance);
                EscribiendoArchivo.Write(TramaRecibida,5,tamanito);
                EscribiendoArchivo.Close();
                FlujoArchivoRecibir.Close();
                archivoRecibir.Activo = false;
                MessageBox.Show("Archivo recibido y cerrado");
            }
            //Debe actualizarse el tamaño de la recepción en archivoRecibir
        }
    }
}
