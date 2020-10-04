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
        public delegate void HandlerProceso(long tam, long avance, int num, bool ED);
        public event HandlerProceso Proceso;
        //Delegado Crear progressbar
        public delegate void HandlerInicioProceso(int num, string nombreArchivo, bool ED);
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
                t.Start();
                Thread a = new Thread(() => inicio(archivoEnvia.Num, archivoEnvia.Nombre, true));
                a.Start();
                listaEnviando.AddLast(archivoEnvia);
                //If(Llegó la cabecera){
                //Agregar a la linkedlist el archivo a enviar
                //} (Falta x'D)
            }
            catch (Exception e)
            {
                MessageBox.Show("Excepción Inicia envío archivo" + e.Message);
                throw e;
            }
        }
        private void inicio(int Num, string Nombre, bool ED)
        {
            OnInicioProceso(Num, Nombre, ED);
        }
        private void EnviarNameyTam(ClassArchivoEnviando archivoEnviar)
        {
            ENT = true;
            //Envia una trama mensaje que contiene el nombre del archivo a enviar y su tamaño
            byte[] TramaEnvioNameyTam;
            byte[] TramaCabeceraEnvioArchivo;
            string cabeceraArchivo = "C" + archivoEnviar.Nombre.Length.ToString("D4");
            TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes(cabeceraArchivo);
            TramaEnvioNameyTam = ASCIIEncoding.UTF8.GetBytes(archivoEnviar.Nombre + archivoEnviar.Tamaño.ToString("D19")+archivoEnviar.Num.ToString("D4"));
            Random r = new Random();
            int n = 2;
            do
            {
                if (!BufferSalidaVacio)
                    Thread.Sleep(r.Next(0, n));
                n *= 2;
            } while (!BufferSalidaVacio);
            puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
            puerto.Write(TramaEnvioNameyTam, 0, TramaEnvioNameyTam.Length);
            puerto.Write(TramaRelleno, 0, 1019 - TramaEnvioNameyTam.Length);
            if (procesoEnvioArchivo ==null || procesoEnvioArchivo.ThreadState != ThreadState.Running)
            {
                procesoEnvioArchivo = new Thread(Enviar);
                procesoEnvioArchivo.Start();
            }
            ENT = false;
        }
        private void Mostrar(string temp) {
            MessageBox.Show(temp);
        }
        private void InicioConstruirArchivo()
        {
            lock (control2)
            {
                //archivoRecibir
                ClassArchivoRecibiendo archivoRecibir = new ClassArchivoRecibiendo();
            string nombreArchivo="";
            long tamarchivo=0;
            string num;
            string CabRec = ASCIIEncoding.UTF8.GetString(TramaRecibida, 1, 4);
            int LongMensRec = Convert.ToInt16(CabRec);
            bool temp2 = true;
            do
            {
                try
                {
                    nombreArchivo = ASCIIEncoding.UTF8.GetString(TramaRecibida, 5, LongMensRec);
                    string temp = ASCIIEncoding.UTF8.GetString(TramaRecibida, LongMensRec + 5, 19);
                    tamarchivo = long.Parse(temp);
                }
                catch (Exception)
                {
                        Console.WriteLine("Error");
                    temp2 = false;
                }
            } while (!temp2);
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
                
                        listaRecibiendo.AddLast(archivoRecibir);
                        OnInicioProceso(archivoRecibir.Num, archivoRecibir.Nombre, false);
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERROR!: No se ha podido crear el archivo: " + e.Message);
                    rarchivo = false;
                }
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
                    if (item.Num == num )
                    {
                        test = true;
                        if (item.Avance <= item.Tamaño - 1019)//ERROR
                        {
                            item.EscribiendoArchivo.Write(TramaRecibida, 5, 1019);
                            item.Avance += 1019;
                            //listaRecibiendo.Find(Buscar(num)).Value = item;
                            Thread a = new Thread(() => avance(item.Tamaño, item.Avance, item.Num, false));
                            a.Start();
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
                                    Thread a = new Thread(() => avance(item.Tamaño, item.Tamaño, item.Num, false));
                                    a.Start();
                                }
                            }
                        }
                    }
                }
                if (!test)
                {
                    Thread envio2 = new Thread(() => Mostrar("No se encontró: " + num.ToString()));
                    envio2.Start();
                }
            }
                
            
        }
    }
}
