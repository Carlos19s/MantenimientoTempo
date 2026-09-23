namespace MantenimientoTempo
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
            btnMaquinas = new Button();
            btnMantenimientos = new Button();
            btnProduccion = new Button();
            btnUsuarios = new Button();
            lblUsuarioLogueado = new Label();
            panelContenedor = new Panel();
            SuspendLayout();
            // 
            // btnMaquinas
            // 
            btnMaquinas.Location = new Point(469, 30);
            btnMaquinas.Name = "btnMaquinas";
            btnMaquinas.Size = new Size(94, 29);
            btnMaquinas.TabIndex = 0;
            btnMaquinas.Text = "Maquinas";
            btnMaquinas.UseVisualStyleBackColor = true;
            btnMaquinas.Click += btnMaquinas_Click;
            // 
            // btnMantenimientos
            // 
            btnMantenimientos.Location = new Point(621, 30);
            btnMantenimientos.Name = "btnMantenimientos";
            btnMantenimientos.Size = new Size(130, 29);
            btnMantenimientos.TabIndex = 1;
            btnMantenimientos.Text = "Mantenimientos";
            btnMantenimientos.UseVisualStyleBackColor = true;
            btnMantenimientos.Click += btnMantenimientos_Click;
            // 
            // btnProduccion
            // 
            btnProduccion.Location = new Point(788, 30);
            btnProduccion.Name = "btnProduccion";
            btnProduccion.Size = new Size(130, 29);
            btnProduccion.TabIndex = 2;
            btnProduccion.Text = "Produccion";
            btnProduccion.UseVisualStyleBackColor = true;
            btnProduccion.Click += btnProduccion_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Location = new Point(937, 25);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(130, 29);
            btnUsuarios.TabIndex = 3;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // lblUsuarioLogueado
            // 
            lblUsuarioLogueado.AutoSize = true;
            lblUsuarioLogueado.Location = new Point(54, 29);
            lblUsuarioLogueado.Name = "lblUsuarioLogueado";
            lblUsuarioLogueado.Size = new Size(12, 20);
            lblUsuarioLogueado.TabIndex = 4;
            lblUsuarioLogueado.Text = ".";
            lblUsuarioLogueado.Click += lblUsuarioLogueado_Click;
            // 
            // panelContenedor
            // 
            panelContenedor.Location = new Point(144, 82);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(923, 404);
            panelContenedor.TabIndex = 5;
            panelContenedor.Paint += panelContenedor_Paint;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1114, 561);
            Controls.Add(panelContenedor);
            Controls.Add(lblUsuarioLogueado);
            Controls.Add(btnUsuarios);
            Controls.Add(btnProduccion);
            Controls.Add(btnMantenimientos);
            Controls.Add(btnMaquinas);
            Name = "FormPrincipal";
            Text = "FormPrincipal";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnMaquinas;
        private Button btnMantenimientos;
        private Button btnProduccion;
        private Button btnUsuarios;
        private Label lblUsuarioLogueado;
        private Panel panelContenedor;
    }
}