﻿using System;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace winproySerialPort
{
    public partial class ClassTransRecep
    {
        public delegate void HandlerTxRx(object oo, string mensRec);
        public event HandlerTxRx LlegoMensaje;
        //Hilos envío Mensaje
        Thread procesoEnvio;
        Thread procesoVerificaSalida;
        Thread procesoRecibirMensaje;

        //Mensaje
        private string mensajeEnviar;
        private string mensRecibido;

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
                Proceso(archivoEnviar.Tamaño, archivoEnviar.Avance);//No está aquí F
            }
        }
        public void Enviar(string mens)
        {
            mensajeEnviar = mens;
            int l = mensajeEnviar.Length;
            //Añadir ceros a la izq
            string LongitudMensaje = "M" + l.ToString("D4");
            TramaEnvio = ASCIIEncoding.UTF8.GetBytes(mensajeEnviar);
            TramaCabeceraEnvio = ASCIIEncoding.UTF8.GetBytes(LongitudMensaje);
            procesoEnvio = new Thread(Enviando);
            procesoEnvio.Start();
        }
        private void Enviando()
        {
            lock (control)
            {
                try
                {
                    puerto.Write(TramaCabeceraEnvio, 0, 5);
                    puerto.Write(TramaEnvio, 0, TramaEnvio.Length);
                    puerto.Write(TramaRelleno, 0, 1019 - TramaEnvio.Length);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }
    }
}
