using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winproySerialPort
{
    class ClassArchivoRecibiendo
    {
        public string Nombre { get; set; }
        public long Tamaño { get; set; }
        public long Avance { get; set; }
        public int Num { get; set; }
        public string Ruta { get; set; }
        public Boolean Activo { get; set; }
        public string TipoArchivo { get; set; }//Para la presentación
        //Mantener a los stream
        public FileStream FlujoArchivoRecibir { get; set; }
        public BinaryWriter EscribiendoArchivo { get; set; }
        
    }
}
