using MantenimientoTempoForms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MantenimientoTempo
{
    public partial class FormPrincipal : Form
    {
        private Form? formularioActivo = null;
        public FormPrincipal()
        {
            InitializeComponent();
            this.Text = $"Tempo Mantenimiento Industrial - Usuario: {ApiClient.NombreCompleto} [{ApiClient.Rol}]";

            // 2. Asignar los datos al Label de forma inmediata
            if (lblUsuarioLogueado != null)
            {
                lblUsuarioLogueado.Text = $"{ApiClient.NombreCompleto}\n({ApiClient.Rol})";
            }

            // 3. Aplicar los permisos correspondientes
            AplicarPermisosPorRol();
            AbrirFormularioHijo(new FormDashboard());
        }
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            // Mostrar los datos del usuario conectado en la barra de título
            this.Text = $"Tempo Mantenimiento Industrial - Usuario: {ApiClient.NombreCompleto} [{ApiClient.Rol}]";

            // Si tienes el label en el menú lateral:
            if (lblUsuarioLogueado != null)
            {
                lblUsuarioLogueado.Text = $"{ApiClient.NombreCompleto}\n({ApiClient.Rol})";
            }

            // Aplicar restricciones según el Rol y Permisos
            AplicarPermisosPorRol();
        }
        private void AplicarPermisosPorRol()
        {
            // 1. Si es Administrador, tiene acceso total a todos los botones
            if (ApiClient.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // 2. Módulo de Usuarios: Solo visible para Administradores
            btnUsuarios.Visible = false;

            // 3. Mantenimientos: Solo si tiene permiso de ver/gestionar mantenimientos
            btnMantenimientos.Enabled = ApiClient.TienePermiso("mantenimiento.ver") || ApiClient.TienePermiso("mantenimiento.completar");

            // 4. Producción: Solo si tiene permiso de registrar/importar producción
            btnProduccion.Enabled = ApiClient.TienePermiso("produccion.importar");

            // 5. Máquinas: Generalmente visible para técnicos y operadores, pero validamos
            btnMaquinas.Enabled = ApiClient.TienePermiso("maquinas.ver");
        }
        private void AbrirFormularioHijo(Form formHijo)
        {
            // Si ya hay un formulario abierto, lo cerramos para liberar memoria
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }

            formularioActivo = formHijo;

            // Configurarlo para que se comporte como un control embebido
            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            panelContenedor.Controls.Clear();
            panelContenedor.Controls.Add(formHijo);
            panelContenedor.Tag = formHijo;

            formHijo.BringToFront();
            formHijo.Show();
        }

        private void btnMaquinas_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Máquinas (Próximo paso)", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMantenimientos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Mantenimientos", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnProduccion_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Carga de Producción", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Gestión de Usuarios", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lblUsuarioLogueado_Click(object sender, EventArgs e)
        {

        }

        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
