namespace MantenimientoTempo
{
    partial class CardMaquinaControl
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
            picMaquina = new PictureBox();
            lblCodigo = new Label();
            lblEstadoSemaforo = new Label();
            lblModelo = new Label();
            pbUso = new ProgressBar();
            lblKilos = new Label();
            ((System.ComponentModel.ISupportInitialize)picMaquina).BeginInit();
            SuspendLayout();
            // 
            // picMaquina
            // 
            picMaquina.Location = new Point(21, 12);
            picMaquina.Name = "picMaquina";
            picMaquina.Size = new Size(307, 122);
            picMaquina.TabIndex = 0;
            picMaquina.TabStop = false;
            // 
            // lblCodigo
            // 
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(21, 171);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(56, 20);
            lblCodigo.TabIndex = 1;
            lblCodigo.Text = "código";
            // 
            // lblEstadoSemaforo
            // 
            lblEstadoSemaforo.AutoSize = true;
            lblEstadoSemaforo.Location = new Point(21, 204);
            lblEstadoSemaforo.Name = "lblEstadoSemaforo";
            lblEstadoSemaforo.Size = new Size(74, 20);
            lblEstadoSemaforo.TabIndex = 2;
            lblEstadoSemaforo.Text = "Semaforo";
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Location = new Point(21, 240);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(117, 20);
            lblModelo.TabIndex = 3;
            lblModelo.Text = "marca y modelo";
            // 
            // pbUso
            // 
            pbUso.Location = new Point(21, 272);
            pbUso.Name = "pbUso";
            pbUso.Size = new Size(307, 29);
            pbUso.TabIndex = 4;
            // 
            // lblKilos
            // 
            lblKilos.AutoSize = true;
            lblKilos.Location = new Point(21, 318);
            lblKilos.Name = "lblKilos";
            lblKilos.Size = new Size(41, 20);
            lblKilos.TabIndex = 5;
            lblKilos.Text = "Kilos";
            // 
            // CardMaquinaControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(362, 392);
            Controls.Add(lblKilos);
            Controls.Add(pbUso);
            Controls.Add(lblModelo);
            Controls.Add(lblEstadoSemaforo);
            Controls.Add(lblCodigo);
            Controls.Add(picMaquina);
            Name = "CardMaquinaControl";
            Text = "CardMaquinaControl";
            ((System.ComponentModel.ISupportInitialize)picMaquina).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picMaquina;
        private Label lblCodigo;
        private Label lblEstadoSemaforo;
        private Label lblModelo;
        private ProgressBar pbUso;
        private Label lblKilos;
    }
}