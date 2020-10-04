using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winproySerialPort
{
    public partial class FormRecibiendo : Form
    {
        public FormRecibiendo()
        {
            InitializeComponent();
            delegadoEnvio = new MostrarEnvio(MostrandoProceso);
            delegadoInicioEnvio = new MostrarInicioEnvio(MostrandoInicioProceso);
            y = 3;
        }
        private int y;
        private static readonly object control = new object();
        //Delegado proceso Envío
        delegate void MostrarEnvio(long tam, long avance, int num, bool ED);
        MostrarEnvio delegadoEnvio;
        delegate void MostrarInicioEnvio(int num, string nombreArchivo, bool ED);
        MostrarInicioEnvio delegadoInicioEnvio;
        private GroupBox ArchivoNuevo(int num, string nombreArchivo, string x)
        {
            //FALTA CAMBIAR LOS VALORES PARA EL DISEÑO
            GroupBox grpArchivoN = new GroupBox();
            Button btnCerrarArchivoN = new Button();
            ProgressBar prgArchivoN = new ProgressBar();
            Label lblArchivoN = new Label();
            Label lbltemp = new Label();
            // 
            // lblArchivoN
            // 
            lblArchivoN.AutoSize = true;
            lblArchivoN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblArchivoN.Location = new System.Drawing.Point(30, 30);
            lblArchivoN.Name = "lblArchivoN" + num.ToString("D4");
            lblArchivoN.Size = new System.Drawing.Size(0, 20);
            lblArchivoN.TabIndex = 0;
            lblArchivoN.Text = nombreArchivo.Length > 15 ? nombreArchivo.Substring(0, 12) + "..." : nombreArchivo;
            // 
            // lbltemp
            // 
            lbltemp.BackColor = System.Drawing.Color.Transparent;
            lbltemp.Location = new System.Drawing.Point(6, 25);
            lbltemp.Name = "lblTemp" + num.ToString("D4");
            lbltemp.Size = new System.Drawing.Size(100, 30);
            lbltemp.TabIndex = 3;
            lbltemp.Text = x;
            lbltemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //lbltemp.Visible = false;
            // 
            // prgArchivoN
            // 
            prgArchivoN.Location = new System.Drawing.Point(138, 30);
            prgArchivoN.Name = "prgArchivoN" + num.ToString("D4");
            prgArchivoN.Size = new System.Drawing.Size(300, 30);
            prgArchivoN.TabIndex = 1;
            // 
            // btnCerrarArchivoN
            // 
            btnCerrarArchivoN.BackColor = System.Drawing.Color.Transparent;
            btnCerrarArchivoN.FlatAppearance.BorderSize = 0;
            btnCerrarArchivoN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            btnCerrarArchivoN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCerrarArchivoN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnCerrarArchivoN.ForeColor = System.Drawing.Color.Black;
            btnCerrarArchivoN.Location = new System.Drawing.Point(484, 25);
            btnCerrarArchivoN.Name = "btnCerrarArchivoN" + num.ToString("D4");
            btnCerrarArchivoN.Size = new System.Drawing.Size(30, 30);
            btnCerrarArchivoN.TabIndex = 2;
            btnCerrarArchivoN.Text = "X";
            btnCerrarArchivoN.UseVisualStyleBackColor = false;
            btnCerrarArchivoN.Click += new System.EventHandler(Borrar);
            // 
            // grpArchivoN
            // 
            grpArchivoN.Controls.Add(btnCerrarArchivoN);
            grpArchivoN.Controls.Add(prgArchivoN);
            grpArchivoN.Controls.Add(lblArchivoN);
            grpArchivoN.Controls.Add(lbltemp);
            grpArchivoN.Location = new System.Drawing.Point(y, 3);
            y += 86;
            grpArchivoN.Name = "grpArchivoN" + num.ToString("D4");
            grpArchivoN.Size = new System.Drawing.Size(540, 80);
            grpArchivoN.TabIndex = 0;
            grpArchivoN.TabStop = false;
            return grpArchivoN;
        }
        private void Borrar(object sender, EventArgs e)
        {
            GroupBox group;
            string num;
            Button button = sender as Button;
            num = button.Name.Substring(17, 4);
            group = flpDescargando.Controls.OfType<GroupBox>().FirstOrDefault(b => b.Name.Equals("grpArchivoN" + num));
            Label etiqueta = group.Controls.OfType<Label>().FirstOrDefault(b => b.Name.Equals("lblTemp" + num));
            string x = etiqueta.Text;
            ProgressBar proceso = group.Controls.OfType<ProgressBar>().FirstOrDefault(b => b.Name.Equals("prgArchivoN" + num));
            if (proceso.Value == proceso.Maximum)
            {
                if (x != "E")
                    flpDescargando.Controls.Remove(flpDescargando.Controls.Find("grpArchivoN" + num, true)[0]);

            }
        }
        private void MostrandoInicioProceso(int num, string nombreArchivo, bool ED)
        {
            lock (control)
            {
                if (!ED)
                {
                    GroupBox insertar = ArchivoNuevo(num, nombreArchivo, "D");
                    flpDescargando.Controls.Add(insertar);
                }
            }
        }
        private void MostrandoProceso(long tam, long avance, int num, bool ED)
        {
            lock (control)
            {
                if (!ED)
                {
                    GroupBox group = flpDescargando.Controls.OfType<GroupBox>().FirstOrDefault(b => b.Name.Equals("grpArchivoN" + num.ToString("D4")));
                    ProgressBar proceso = group.Controls.OfType<ProgressBar>().FirstOrDefault(b => b.Name.Equals("prgArchivoN" + num.ToString("D4")));
                    proceso.Maximum = (int)tam;
                    proceso.Value = (int)avance;
                }
            }
        }
        internal void ObjTrRX_InicioProceso(int num, string nombreArchivo, bool ED)
        {
            Invoke(delegadoInicioEnvio, num, nombreArchivo, ED);
        }
        internal void ObjTrRX_proceso(long tam, long avance, int num, bool ED)
        {
            try
            {
                Invoke(delegadoEnvio, tam, avance, num, ED);
            }
            catch (Exception)
            {
                //No se enviará el archivo
                Environment.Exit(0);
            }

        }
    }
}
