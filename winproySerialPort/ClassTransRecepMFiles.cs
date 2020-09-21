﻿using System;
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
            while (puerto.IsOpen)
            {
                lock (contrl)
                {
                    if(listaEnviando.Count>0)
                    {
                        foreach (ClassArchivoEnviando item in listaEnviando)
                        {
                            if (item.Activo)
                            {
                                EnviandoArchivo(item);
                                Thread.Sleep(100);
                            }
                                
                            //if (!item.Activo)
                            //    listaEnviando.Remove(item);
                        }
                    }
                }  
            }
        }
        private void avance(long tam, long avance, int num)
        {
            OnProcesoEnvio(tam, avance, num);
        }
        private void EnviandoArchivo(ClassArchivoEnviando archivoEnviar)
        {
            try
            {
                //Envia las tramas de información
                byte[] TramaEnvioArchivo = new byte[1019];
                byte[] TramaCabeceraEnvioArchivo = new byte[5];
                //Se deberia esperar la confirmación para enviar el archivo(En la final será, idk)
                TramaCabeceraEnvioArchivo = ASCIIEncoding.UTF8.GetBytes("I" + archivoEnviar.Num.ToString("D4"));
                //MessageBox.Show("TramaCabeceraEnvioArchivo: " + "I" + archivoEnviar.Num.ToString("D4"));
                
                if (archivoEnviar.Avance <= (archivoEnviar.Tamaño - 1019))
                {
                    archivoEnviar.LeyendoArchivo.Read(TramaEnvioArchivo, 0, 1019);
                    archivoEnviar.Avance += 1019;
                    lock (control)
                    {
                        puerto.Write(TramaCabeceraEnvioArchivo, 0, 5);
                        puerto.Write(TramaEnvioArchivo, 0, 1019);
                        Thread a = new Thread(()=> avance(archivoEnviar.Tamaño, archivoEnviar.Avance, archivoEnviar.Num));
                        a.Start();
                    }
                }
                else
                {
                    int tamanito = Convert.ToInt16(archivoEnviar.Tamaño - archivoEnviar.Avance);
                    archivoEnviar.LeyendoArchivo.Read(TramaEnvioArchivo, 0, tamanito);
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Errorwsdfs: " + e.Message);
                return;
            }
            finally
            {
                if (archivoEnviar.Avance == archivoEnviar.Tamaño)
                {
                    archivoEnviar.LeyendoArchivo.Close();
                    archivoEnviar.FlujoArchivoEnviar.Close();
                    lock (control)
                    {
                        OnProcesoEnvio(archivoEnviar.Tamaño, archivoEnviar.Tamaño, archivoEnviar.Num);
                    }
                    Thread x = new Thread(a);
                    x.Start();
                }
            }
            
        }
        private void a()
        {
            MessageBox.Show("Archivo Enviado xD");
        }
    }
}