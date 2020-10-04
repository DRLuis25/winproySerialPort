using System;

namespace winproySerialPort
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.panelSideMenu = new System.Windows.Forms.Panel();
            this.btnAyuda = new System.Windows.Forms.Button();
            this.btnAjustes = new System.Windows.Forms.Button();
            this.panelTransferenciasSubMenu = new System.Windows.Forms.Panel();
            this.btnRecibiendo = new System.Windows.Forms.Button();
            this.btnEnviando = new System.Windows.Forms.Button();
            this.btnTransferencias = new System.Windows.Forms.Button();
            this.btnChat = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelChildForm = new System.Windows.Forms.Panel();
            this.btnCerrarPuerto = new System.Windows.Forms.Button();
            this.grpParametros = new System.Windows.Forms.GroupBox();
            this.cbmPuerto = new System.Windows.Forms.ComboBox();
            this.nudBaudRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelPlayer = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelSideMenu.SuspendLayout();
            this.panelTransferenciasSubMenu.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelChildForm.SuspendLayout();
            this.grpParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).BeginInit();
            this.panelPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSideMenu
            // 
            this.panelSideMenu.AutoScroll = true;
            this.panelSideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(35)))));
            this.panelSideMenu.Controls.Add(this.btnAyuda);
            this.panelSideMenu.Controls.Add(this.btnAjustes);
            this.panelSideMenu.Controls.Add(this.panelTransferenciasSubMenu);
            this.panelSideMenu.Controls.Add(this.btnTransferencias);
            this.panelSideMenu.Controls.Add(this.btnChat);
            this.panelSideMenu.Controls.Add(this.btnSalir);
            this.panelSideMenu.Controls.Add(this.panelLogo);
            this.panelSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSideMenu.Location = new System.Drawing.Point(0, 0);
            this.panelSideMenu.Name = "panelSideMenu";
            this.panelSideMenu.Size = new System.Drawing.Size(194, 461);
            this.panelSideMenu.TabIndex = 1;
            // 
            // btnAyuda
            // 
            this.btnAyuda.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAyuda.FlatAppearance.BorderSize = 0;
            this.btnAyuda.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.btnAyuda.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(22)))), ((int)(((byte)(34)))));
            this.btnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAyuda.ForeColor = System.Drawing.Color.Silver;
            this.btnAyuda.Image = ((System.Drawing.Image)(resources.GetObject("btnAyuda.Image")));
            this.btnAyuda.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAyuda.Location = new System.Drawing.Point(0, 320);
            this.btnAyuda.Name = "btnAyuda";
            this.btnAyuda.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnAyuda.Size = new System.Drawing.Size(194, 45);
            this.btnAyuda.TabIndex = 14;
            this.btnAyuda.Text = "  Ayuda";
            this.btnAyuda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAyuda.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAyuda.UseVisualStyleBackColor = true;
            // 
            // btnAjustes
            // 
            this.btnAjustes.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAjustes.FlatAppearance.BorderSize = 0;
            this.btnAjustes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.btnAjustes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(22)))), ((int)(((byte)(34)))));
            this.btnAjustes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjustes.ForeColor = System.Drawing.Color.Silver;
            this.btnAjustes.Image = ((System.Drawing.Image)(resources.GetObject("btnAjustes.Image")));
            this.btnAjustes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAjustes.Location = new System.Drawing.Point(0, 275);
            this.btnAjustes.Name = "btnAjustes";
            this.btnAjustes.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnAjustes.Size = new System.Drawing.Size(194, 45);
            this.btnAjustes.TabIndex = 13;
            this.btnAjustes.Text = "  Ajustes";
            this.btnAjustes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAjustes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAjustes.UseVisualStyleBackColor = true;
            this.btnAjustes.Click += new System.EventHandler(this.btnAjustes_Click);
            // 
            // panelTransferenciasSubMenu
            // 
            this.panelTransferenciasSubMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(19)))), ((int)(((byte)(23)))));
            this.panelTransferenciasSubMenu.Controls.Add(this.btnRecibiendo);
            this.panelTransferenciasSubMenu.Controls.Add(this.btnEnviando);
            this.panelTransferenciasSubMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTransferenciasSubMenu.Location = new System.Drawing.Point(0, 182);
            this.panelTransferenciasSubMenu.Name = "panelTransferenciasSubMenu";
            this.panelTransferenciasSubMenu.Size = new System.Drawing.Size(194, 93);
            this.panelTransferenciasSubMenu.TabIndex = 12;
            // 
            // btnRecibiendo
            // 
            this.btnRecibiendo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRecibiendo.FlatAppearance.BorderSize = 0;
            this.btnRecibiendo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.btnRecibiendo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.btnRecibiendo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecibiendo.ForeColor = System.Drawing.Color.Silver;
            this.btnRecibiendo.Location = new System.Drawing.Point(0, 40);
            this.btnRecibiendo.Name = "btnRecibiendo";
            this.btnRecibiendo.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnRecibiendo.Size = new System.Drawing.Size(194, 40);
            this.btnRecibiendo.TabIndex = 1;
            this.btnRecibiendo.Text = "Recibiendo";
            this.btnRecibiendo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecibiendo.UseVisualStyleBackColor = true;
            this.btnRecibiendo.Click += new System.EventHandler(this.btnRecibiendo_Click);
            // 
            // btnEnviando
            // 
            this.btnEnviando.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEnviando.FlatAppearance.BorderSize = 0;
            this.btnEnviando.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.btnEnviando.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.btnEnviando.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviando.ForeColor = System.Drawing.Color.Silver;
            this.btnEnviando.Location = new System.Drawing.Point(0, 0);
            this.btnEnviando.Name = "btnEnviando";
            this.btnEnviando.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnEnviando.Size = new System.Drawing.Size(194, 40);
            this.btnEnviando.TabIndex = 0;
            this.btnEnviando.Text = "Enviando";
            this.btnEnviando.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnviando.UseVisualStyleBackColor = true;
            this.btnEnviando.Click += new System.EventHandler(this.btnEnviando_Click);
            // 
            // btnTransferencias
            // 
            this.btnTransferencias.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTransferencias.FlatAppearance.BorderSize = 0;
            this.btnTransferencias.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.btnTransferencias.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(22)))), ((int)(((byte)(34)))));
            this.btnTransferencias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferencias.ForeColor = System.Drawing.Color.Silver;
            this.btnTransferencias.Image = ((System.Drawing.Image)(resources.GetObject("btnTransferencias.Image")));
            this.btnTransferencias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransferencias.Location = new System.Drawing.Point(0, 137);
            this.btnTransferencias.Name = "btnTransferencias";
            this.btnTransferencias.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnTransferencias.Size = new System.Drawing.Size(194, 45);
            this.btnTransferencias.TabIndex = 11;
            this.btnTransferencias.Text = "Transferencias";
            this.btnTransferencias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransferencias.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTransferencias.UseVisualStyleBackColor = true;
            this.btnTransferencias.Click += new System.EventHandler(this.btnTransferencias_Click);
            // 
            // btnChat
            // 
            this.btnChat.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChat.FlatAppearance.BorderSize = 0;
            this.btnChat.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.btnChat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(22)))), ((int)(((byte)(34)))));
            this.btnChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChat.ForeColor = System.Drawing.Color.Silver;
            this.btnChat.Image = ((System.Drawing.Image)(resources.GetObject("btnChat.Image")));
            this.btnChat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChat.Location = new System.Drawing.Point(0, 92);
            this.btnChat.Name = "btnChat";
            this.btnChat.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnChat.Size = new System.Drawing.Size(194, 45);
            this.btnChat.TabIndex = 10;
            this.btnChat.Text = "Chat";
            this.btnChat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.btnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(22)))), ((int)(((byte)(34)))));
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ForeColor = System.Drawing.Color.Silver;
            this.btnSalir.Image = ((System.Drawing.Image)(resources.GetObject("btnSalir.Image")));
            this.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.Location = new System.Drawing.Point(0, 416);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSalir.Size = new System.Drawing.Size(194, 45);
            this.btnSalir.TabIndex = 9;
            this.btnSalir.Text = "  Exit";
            this.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(35)))));
            this.panelLogo.Controls.Add(this.pictureBox1);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(194, 92);
            this.panelLogo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(159, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelChildForm
            // 
            this.panelChildForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            this.panelChildForm.Controls.Add(this.btnCerrarPuerto);
            this.panelChildForm.Controls.Add(this.grpParametros);
            this.panelChildForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChildForm.ForeColor = System.Drawing.Color.Transparent;
            this.panelChildForm.Location = new System.Drawing.Point(194, 80);
            this.panelChildForm.Name = "panelChildForm";
            this.panelChildForm.Size = new System.Drawing.Size(640, 381);
            this.panelChildForm.TabIndex = 4;
            // 
            // btnCerrarPuerto
            // 
            this.btnCerrarPuerto.FlatAppearance.BorderSize = 0;
            this.btnCerrarPuerto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.btnCerrarPuerto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.btnCerrarPuerto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarPuerto.ForeColor = System.Drawing.Color.Silver;
            this.btnCerrarPuerto.Location = new System.Drawing.Point(209, 241);
            this.btnCerrarPuerto.Name = "btnCerrarPuerto";
            this.btnCerrarPuerto.Size = new System.Drawing.Size(233, 40);
            this.btnCerrarPuerto.TabIndex = 14;
            this.btnCerrarPuerto.Text = "Cerrar Puerto";
            this.btnCerrarPuerto.UseCompatibleTextRendering = true;
            this.btnCerrarPuerto.UseVisualStyleBackColor = true;
            this.btnCerrarPuerto.Click += new System.EventHandler(this.btnCerrarPuerto_Click);
            // 
            // grpParametros
            // 
            this.grpParametros.BackColor = System.Drawing.Color.Transparent;
            this.grpParametros.Controls.Add(this.cbmPuerto);
            this.grpParametros.Controls.Add(this.nudBaudRate);
            this.grpParametros.Controls.Add(this.label1);
            this.grpParametros.Controls.Add(this.label4);
            this.grpParametros.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpParametros.ForeColor = System.Drawing.Color.Silver;
            this.grpParametros.Location = new System.Drawing.Point(149, 92);
            this.grpParametros.Name = "grpParametros";
            this.grpParametros.Size = new System.Drawing.Size(347, 129);
            this.grpParametros.TabIndex = 13;
            this.grpParametros.TabStop = false;
            this.grpParametros.Text = "Parametros";
            // 
            // cbmPuerto
            // 
            this.cbmPuerto.BackColor = System.Drawing.Color.Silver;
            this.cbmPuerto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmPuerto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbmPuerto.ForeColor = System.Drawing.Color.Transparent;
            this.cbmPuerto.FormattingEnabled = true;
            this.cbmPuerto.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7"});
            this.cbmPuerto.Location = new System.Drawing.Point(199, 82);
            this.cbmPuerto.Name = "cbmPuerto";
            this.cbmPuerto.Size = new System.Drawing.Size(133, 28);
            this.cbmPuerto.TabIndex = 5;
            this.cbmPuerto.SelectedIndexChanged += new System.EventHandler(this.cbmPuerto_SelectedIndexChanged_1);
            // 
            // nudBaudRate
            // 
            this.nudBaudRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.nudBaudRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudBaudRate.ForeColor = System.Drawing.Color.Silver;
            this.nudBaudRate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudBaudRate.Location = new System.Drawing.Point(199, 40);
            this.nudBaudRate.Maximum = new decimal(new int[] {
            115700,
            0,
            0,
            0});
            this.nudBaudRate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudBaudRate.Name = "nudBaudRate";
            this.nudBaudRate.Size = new System.Drawing.Size(133, 26);
            this.nudBaudRate.TabIndex = 11;
            this.nudBaudRate.Value = new decimal(new int[] {
            115700,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(6, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Puerto Comunicaciones";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Baud rate";
            // 
            // panelPlayer
            // 
            this.panelPlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(61)))));
            this.panelPlayer.Controls.Add(this.label2);
            this.panelPlayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPlayer.Location = new System.Drawing.Point(194, 0);
            this.panelPlayer.Name = "panelPlayer";
            this.panelPlayer.Size = new System.Drawing.Size(640, 80);
            this.panelPlayer.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(26, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(393, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "Proyecto Transmisión y Recepción Serial Port";
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.panelChildForm);
            this.Controls.Add(this.panelPlayer);
            this.Controls.Add(this.panelSideMenu);
            this.Name = "FormPrincipal";
            this.Text = "Delgado Rodriguez Luis Guillermo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.panelSideMenu.ResumeLayout(false);
            this.panelTransferenciasSubMenu.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelChildForm.ResumeLayout(false);
            this.grpParametros.ResumeLayout(false);
            this.grpParametros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).EndInit();
            this.panelPlayer.ResumeLayout(false);
            this.panelPlayer.PerformLayout();
            this.ResumeLayout(false);

        }
        

        #endregion

        private System.Windows.Forms.Panel panelSideMenu;
        private System.Windows.Forms.Button btnChat;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelChildForm;
        private System.Windows.Forms.Panel panelPlayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpParametros;
        private System.Windows.Forms.ComboBox cbmPuerto;
        private System.Windows.Forms.NumericUpDown nudBaudRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCerrarPuerto;
        private System.Windows.Forms.Panel panelTransferenciasSubMenu;
        private System.Windows.Forms.Button btnRecibiendo;
        private System.Windows.Forms.Button btnEnviando;
        private System.Windows.Forms.Button btnTransferencias;
        private System.Windows.Forms.Button btnAjustes;
        private System.Windows.Forms.Button btnAyuda;
    }
}