namespace MantenimientoTempo
{
    partial class FormDashboard
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
            panelKpis = new Panel();
            panelFiltros = new Panel();
            button1 = new Button();
            textBox1 = new TextBox();
            flowTarjetas = new FlowLayoutPanel();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label2 = new Label();
            panelKpis.SuspendLayout();
            panelFiltros.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelKpis
            // 
            panelKpis.Controls.Add(flowLayoutPanel1);
            panelKpis.Controls.Add(label1);
            panelKpis.Dock = DockStyle.Top;
            panelKpis.Location = new Point(0, 0);
            panelKpis.Name = "panelKpis";
            panelKpis.Size = new Size(1046, 140);
            panelKpis.TabIndex = 0;
            // 
            // panelFiltros
            // 
            panelFiltros.Controls.Add(button1);
            panelFiltros.Controls.Add(textBox1);
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Location = new Point(0, 140);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.Size = new Size(1046, 55);
            panelFiltros.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(357, 15);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(215, 15);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 0;
            // 
            // flowTarjetas
            // 
            flowTarjetas.AutoScroll = true;
            flowTarjetas.BackColor = Color.FromArgb(248, 249, 250);
            flowTarjetas.Dock = DockStyle.Fill;
            flowTarjetas.Location = new Point(0, 195);
            flowTarjetas.Name = "flowTarjetas";
            flowTarjetas.Size = new Size(1046, 328);
            flowTarjetas.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(226, 20);
            label1.TabIndex = 1;
            label1.Text = "Sistema de Mantenimiento Textil";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(235, 245, 255);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Location = new Point(273, 23);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(198, 89);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(113, 20);
            label2.TabIndex = 0;
            label2.Text = "Total Maquinas:";
            label2.Click += label2_Click;
            // 
            // FormDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1046, 523);
            Controls.Add(flowTarjetas);
            Controls.Add(panelFiltros);
            Controls.Add(panelKpis);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormDashboard";
            Text = "FormDashboard";
            panelKpis.ResumeLayout(false);
            panelKpis.PerformLayout();
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelKpis;
        private Panel panelFiltros;
        private Button button1;
        private TextBox textBox1;
        private FlowLayoutPanel flowTarjetas;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label2;
    }
}