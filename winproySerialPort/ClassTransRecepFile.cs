using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.CodeDom;

namespace winproySerialPort
{
    public partial class ClassTransRecep
    {
        //temporal
        private int num;
        
        //Delegado para envío archivo
        public delegate void HandlerProceso(long tam, long avance, int num);
        public event HandlerProceso Proceso;
        //Delegado Crear progressbar
        public delegate void HandlerInicioProceso(int num, string nombreArchivo);
        public event HandlerInicioProceso InicioProceso;
        //Hilos envío Archivo
        Thread procesoEnvioArchivo;
        Thread procesoConstruyeArchivo;
        private bool rarchivo;
        public void IniciaEnvioArchivo(string path) //Para solo 1 file
        {
            //archivoEnviar
            ClassArchivoEnviando archivoEnvia = new ClassArchivoEnviando();
            try
            {
                archivoEnvia.FlujoArchivoEnviar = new FileStream(path, FileMode.Open, FileAccess.Read);
                archivoEnvia.LeyendoArchivo = new BinaryReader(archivoEnvia.FlujoArchivoEnviar);
                archivoEnvia.Nombre = Path.GetFileName(path);
                archivoEnvia.Tamaño = archivoEnvia.FlujoArchivoEnviar.Length;
                archivoEnvia.Avance = 0;
                archivoEnvia.Num = num;
                num++;
                archivoEnvia.Activo = true;
                Thread t = new Thread(()=>EnviarNameyTam(archivoEnvia));
                //EnviarNameyTam(archivoEnvia);         
                t.Start();
                lock (contrl)
                {
                    Thread.Sleep(1000);
                    OnInicioProceso(archivoEnvia.Num, archivoEnvia.Nombre);
                    listaEnviando.AddLast(archivoEnvia);
                    
                }
                //If(Llegó la cabecera){
                //Agregar a la linkedlist el archivo a enviar
                //} (Falta x'D)
                /*
                procesoEnvioArchivo iria en otro hilo que estaría enviando siempre
                procesoEnvioArchivo = new Thread(EnviandoArchivo(archivoEnvia));
                procesoEnvioArchivo.Start();
                */
            }
            catch (Exception e)
            {
                MessageBox.Show("Excepción Inicia envío archivo" + e.Message);
                throw e;
            }
        }
        private void EnviarNameyTam(ClassArchivoEnviando archivoEnviar)
        {
            //Envia una trama mensaje que contiene el nombre del archivo a enviar y su tamaño
            byte[] TramaEnvioNameyTam;
            byte[] TramaCabeceraEnvioArchivo;
            string cabeceraArchivo = "C" + archivoEnviar.Nombre.Length.ToString("D4");
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(cabeceraArchivo);
            TramaEnvioNameyTam = ASCIIEncoding.UTF8.GetBytes(archivoEnviar.Nombre + archivoEnviar.Tamaño.ToString("D19")+archivoEnviar.Num.ToString("D4"));
            lock (control)
            {
                puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                puerto.Write(TramaEnvioNameyTam, 0, TramaEnvioNameyTam.Length);
                puerto.Write(TramaRelleno, 0, 1019 - TramaEnvioNameyTam.Length);
            }
            //MessageBox.Show("Name y tam enviado");
        }
        
        private void InicioConstruirArchivo()
        {
            //archivoRecibir
            ClassArchivoRecibiendo archivoRecibir = new ClassArchivoRecibiendo();
            string nombreArchivo;
            long tamarchivo;
            string num;
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            int LongMensRec = Convert.ToInt16(CabRec);
            nombreArchivo = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
            tamarchivo = long.Parse(ASCIIEncoding.UTF8.GetString(TramaRecibida, LongMensRec + 5, 19));
            num = ASCIIEncoding.UTF8.GetString(TramaRecibida, LongMensRec + 24, 4);
            try
            {
                //Crea un archivo en el disco
                string filepath = Path.Combine(ConfigurationManager.AppSettings["Path"], nombreArchivo);
                filepath = ChangeFileName(filepath);
                archivoRecibir.FlujoArchivoRecibir = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);//Manejar excepcion
                archivoRecibir.EscribiendoArchivo = new BinaryWriter(archivoRecibir.FlujoArchivoRecibir);
                archivoRecibir.Nombre = nombreArchivo;
                archivoRecibir.Num = Convert.ToInt16(num);
                //MessageBox.Show("Num recibido: " + archivoRecibir.Num);
                archivoRecibir.Tamaño = tamarchivo;//Obviamente obtener tamaño
                archivoRecibir.Avance = 0;
                archivoRecibir.Activo = true;
                rarchivo = true;
                //Añadir a la lista
                lock (control2)
                {
                    listaRecibiendo.AddLast(archivoRecibir);
                    
                }
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
            lock (control2)
            {
                bool test = false;
                //Se lee de la lista según el nro del archivo recibida
                string nro = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
                int num = Convert.ToInt16(nro);
                //ClassArchivoRecibiendo temp = Buscar(num);
                foreach (ClassArchivoRecibiendo item in listaRecibiendo)
                {
                    if (item.Num == num)
                    {
                        test = true;
                        if (item.Avance <= item.Tamaño - 1019)//ERROR
                        {
                            item.EscribiendoArchivo.Write(TramaRecibida, 5, 1019);
                            item.Avance += 1019;
                            //listaRecibiendo.Find(Buscar(num)).Value = item;
                        }
                        else
                        {
                            try
                            {
                                int tamanito = Convert.ToInt16(item.Tamaño - item.Avance);
                                item.EscribiendoArchivo.Write(TramaRecibida, 5, tamanito);
                                item.Activo = false;
                                //MessageBox.Show("Archivo recibido");
                                item.Avance = item.Tamaño;
                                //listaRecibiendo.Remove(listaRecibiendo.Find(Buscar(num)).Value);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Error contruir archivo: " + e.Message);
                                //throw e;
                            }
                            finally
                            {
                                if (item.Avance == item.Tamaño)
                                {
                                    //MessageBox.Show("Entrooo finalizado");
                                    item.EscribiendoArchivo.Close();
                                    item.FlujoArchivoRecibir.Close();
                                }
                            }
                        }
                    }
                }
                if (!test)
                    MessageBox.Show("No se encontró: " + num.ToString());
            }
        }
    }
}
