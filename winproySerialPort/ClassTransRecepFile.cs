using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
namespace winproySerialPort
{
    public partial class ClassTransRecep
    {
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
        //Hilos envío Archivo
        Thread procesoEnvioArchivo;
        Thread procesoConstruyeArchivo;
        private bool rarchivo;
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
                EnviarNameyTam();               //If(Llegó la cabecera){Enviar Información} (Falta x'D)
                procesoEnvioArchivo = new Thread(EnviandoArchivo);
                procesoEnvioArchivo.Start();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void EnviarNameyTam()
        {
            //Envia una trama mensaje que contiene el nombre del archivo a enviar y su tamaño
            byte[] TramaEnvioNameyTam = new byte[1019];
            byte[] TramaCabeceraEnvioArchivo = new byte[5];
            string cabeceraArchivo = "C" + archivoEnviar.Nombre.Length.ToString("D4");
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(cabeceraArchivo);
            TramaEnvioNameyTam = ASCIIEncoding.UTF8.GetBytes(archivoEnviar.Nombre + archivoEnviar.Tamaño.ToString("D19"));
            lock (control)
            {
                try
                {
                    puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                    puerto.Write(TramaEnvioNameyTam, 0, TramaEnvioNameyTam.Length);
                    puerto.Write(TramaRelleno, 0, 1019 - TramaEnvioNameyTam.Length);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }
        private void EnviandoArchivo()
        {
            try
            {
                //Envia las tramas de información
                byte[] TramaEnvioArchivo = new byte[1019];
                byte[] TramaCabeceraEnvioArchivo = new byte[5];
                //Se deberia esperar la confirmación para enviar el archivo(En la final será, idk)
                TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("I0001");
                while (archivoEnviar.Avance <= (archivoEnviar.Tamaño - 1019))
                {
                    LeyendoArchivo.Read(TramaEnvioArchivo, 0, 1019);
                    archivoEnviar.Avance += 1019;
                    lock (control)
                    {
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return;
            }
            finally
            {
                LeyendoArchivo.Close();
                FlujoArchivoEnviar.Close();
                OnProcesoEnvio();
            }
            MessageBox.Show("Archivo Enviado");
        }
        private void InicioConstruirArchivo()
        {
            string nombreArchivo;
            long tamarchivo;
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            int LongMensRec = Convert.ToInt16(CabRec);
            nombreArchivo = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
            tamarchivo = long.Parse(ASCIIEncoding.UTF8.GetString(TramaRecibida, LongMensRec + 5, 19));
            try
            {
                //Crea un archivo en el disco
                string filepath = Path.Combine(ConfigurationManager.AppSettings["Path"], nombreArchivo);
                filepath = ChangeFileName(filepath);
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
                EscribiendoArchivo.Write(TramaRecibida, 5, 1019);
                archivoRecibir.Avance += 1019;
            }
            else
            {
                try
                {
                    int tamanito = Convert.ToInt16(archivoRecibir.Tamaño - archivoRecibir.Avance);
                    EscribiendoArchivo.Write(TramaRecibida, 5, tamanito);
                    archivoRecibir.Activo = false;
                    MessageBox.Show("Archivo recibido");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
                    throw;
                }
                finally
                {
                    EscribiendoArchivo.Close();
                    FlujoArchivoRecibir.Close();
                }
            }
        }
    }
}
