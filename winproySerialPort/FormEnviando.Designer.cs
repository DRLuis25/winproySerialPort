using System;

namespace winproySerialPort
{
    partial class FormEnviando
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.flpEnviando = new System.Windows.Forms.FlowLayoutPanel();
            this.grpArchivoN = new System.Windows.Forms.GroupBox();
            this.prgArchivoN = new System.Windows.Forms.ProgressBar();
            this.lblArchivoN = new System.Windows.Forms.Label();
            this.lbltemp = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.label1.Location = new System.Drawing.Point(257, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "ENVIANDO";
            // 
            // btnSalir
            // 
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.LightGray;
            this.btnSalir.Location = new System.Drawing.Point(7, 7);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(25, 25);
            this.btnSalir.TabIndex = 14;
            this.btnSalir.Text = "X";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Maven Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(45, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(305, 21);
            this.label3.TabIndex = 20;
            this.label3.Text = "Los archivos que envías apareceran aquí";
            // 
            // flpEnviando
            // 
            this.flpEnviando.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpEnviando.AutoScroll = true;
            this.flpEnviando.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.flpEnviando.Location = new System.Drawing.Point(45, 80);
            this.flpEnviando.Margin = new System.Windows.Forms.Padding(1);
            this.flpEnviando.Name = "flpEnviando";
            this.flpEnviando.Size = new System.Drawing.Size(550, 260);
            this.flpEnviando.TabIndex = 19;
            // 
            // grpArchivoN
            // 
            this.grpArchivoN.Location = new System.Drawing.Point(0, 0);
            this.grpArchivoN.Name = "grpArchivoN";
            this.grpArchivoN.Size = new System.Drawing.Size(200, 100);
            this.grpArchivoN.TabIndex = 0;
            this.grpArchivoN.TabStop = false;
            // 
            // prgArchivoN
            // 
            this.prgArchivoN.Location = new System.Drawing.Point(0, 0);
            this.prgArchivoN.Name = "prgArchivoN";
            this.prgArchivoN.Size = new System.Drawing.Size(100, 23);
            this.prgArchivoN.TabIndex = 0;
            // 
            // lblArchivoN
            // 
            this.lblArchivoN.Location = new System.Drawing.Point(0, 0);
            this.lblArchivoN.Name = "lblArchivoN";
            this.lblArchivoN.Size = new System.Drawing.Size(100, 23);
            this.lblArchivoN.TabIndex = 0;
            // 
            // lbltemp
            // 
            this.lbltemp.Location = new System.Drawing.Point(0, 0);
            this.lbltemp.Name = "lbltemp";
            this.lbltemp.Size = new System.Drawing.Size(100, 23);
            this.lbltemp.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // FormEnviando
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(640, 381);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.flpEnviando);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEnviando";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flpEnviando;
        private System.Windows.Forms.GroupBox grpArchivoN;
        private System.Windows.Forms.ProgressBar prgArchivoN;
        private System.Windows.Forms.Label lblArchivoN;
        private System.Windows.Forms.Label lbltemp;
        private System.Windows.Forms.Button button1;
    }
}