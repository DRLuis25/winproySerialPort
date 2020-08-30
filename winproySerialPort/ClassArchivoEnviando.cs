using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winproySerialPort
{
    class ClassArchivoEnviando
    {
        public string Nombre { get; set; }
        public long Tamaño { get; set; }
        public int Avance { get; set; }
        public int Num { get; set; }
        public string Ruta { get; set; }
        public Boolean Activo { get; set; }
        public string TipoArchivo { get; set; }//Para la presentación
        //Mantener a los stream
    }
}
