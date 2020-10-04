using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace winproySerialPort
{
    public partial class ClassTransRecep
    {
        //Lista Enviando
        private static readonly object contrl = new object();
        private readonly LinkedList<ClassArchivoEnviando> listaEnviando = new LinkedList<ClassArchivoEnviando>();
        private readonly LinkedList<ClassArchivoRecibiendo> listaRecibiendo = new LinkedList<ClassArchivoRecibiendo>();
        private void Enviar()
        {
            bool temp = true;
            while (temp)
            {
                lock (contrl)
                {
                    LinkedListNode<ClassArchivoEnviando> item = listaEnviando.First;
                    while (item!=null)
                    {
                        LinkedListNode<ClassArchivoEnviando> next = item.Next;
                        if (item.Value.Activo)
                        {
                            EnviandoArchivo(item.Value);
                            Thread.Sleep(100);
                        }
                        if (!item.Value.Activo)
                            listaEnviando.Remove(item);
                        item = next;
                    }
                }
                if (listaEnviando.Count == 0)
                    temp = false;
            }
        }
        private void avance(long tam, long avance, int num, bool ED)
        {
            OnProcesoEnvio(tam, avance, num, ED);
        }
        private void EnviandoArchivo(ClassArchivoEnviando archivoEnviar)
        {
            try
            {
                //Envia las tramas de información
                byte[] TramaCabeceraEnvioArchivo = new byte[5];
                byte[] TramaEnvioArchivo = new byte[1019];                                                              //Se deberia esperar la confirmación para enviar el archivo(En la final será, idk)
                TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("I" + archivoEnviar.Num.ToString("D4"));
                if (archivoEnviar.Avance <= (archivoEnviar.Tamaño - 1019))
                {
                    archivoEnviar.LeyendoArchivo.Read(TramaEnvioArchivo, 0, 1019);
                    archivoEnviar.Avance += 1019;
                    Random r = new Random();
                    do
                    {
                        if (!BufferSalidaVacio)
                            Thread.Sleep(r.Next(0, 1000));
                    } while (!BufferSalidaVacio || ENT);
                    puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                    puerto.Write(TramaEnvioArchivo, 0, 1019);
                    Thread a = new Thread(()=> avance(archivoEnviar.Tamaño, archivoEnviar.Avance, archivoEnviar.Num, true));
                    a.Start();
                }
                else
                {
                    int tamanito = Convert.ToInt16(archivoEnviar.Tamaño - archivoEnviar.Avance);
                    archivoEnviar.LeyendoArchivo.Read(TramaEnvioArchivo, 0, tamanito);
                    //Envío de lo que queda del archivo + un relleno
                    Random r = new Random();
                    do
                    {
                        if (!BufferSalidaVacio)
                            Thread.Sleep(r.Next(0, 1000));
                    } while (!BufferSalidaVacio || ENT);
                    puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                    puerto.Write(TramaEnvioArchivo, 0, tamanito);
                    puerto.Write(TramaRelleno, 0, 1019 - tamanito);
                    //Cerrar el flujo
                    archivoEnviar.Avance = archivoEnviar.Tamaño;
                    archivoEnviar.Activo = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return;
            }
            finally
            {
                if (archivoEnviar.Avance == archivoEnviar.Tamaño)
                {
                    archivoEnviar.LeyendoArchivo.Close();
                    archivoEnviar.FlujoArchivoEnviar.Close();
                    Thread asd = new Thread(() => avance(archivoEnviar.Tamaño, archivoEnviar.Tamaño, archivoEnviar.Num, true));
                    asd.Start();
                }
            }
            
        }
    }
}
