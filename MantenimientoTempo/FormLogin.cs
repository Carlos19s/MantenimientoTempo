using MantenimientoTempoForms.Services;
using MantenimientoTempoModels.DTOs.Auth;
using System.Net.Http.Json;

namespace MantenimientoTempo
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsername.Text.Trim();
            string clave = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                lblMensaje.Text = "Por favor ingrese usuario y contraseña.";
                return;
            }

            btnLogin.Enabled = false;
            lblMensaje.Text = "Conectando con el servidor...";

            try
            {
                var credenciales = new LoginRequestDto
                {
                    Username = usuario,
                    Password = clave
                };

                var respuesta = await ApiClient.Http.PostAsJsonAsync("api/Auth/login", credenciales);
                var datos = await respuesta.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (respuesta.IsSuccessStatusCode && datos != null && datos.Exito)
                {
                    // Guarda el token, rol y permisos en ApiClient
                    ApiClient.SetSession(datos);

                    // Ocultar formulario de login
                    this.Hide();

                    // Crear y mostrar el Menú Principal
                    FormPrincipal principal = new FormPrincipal();

                    // Si cierran la ventana principal, se detiene la aplicación
                    principal.FormClosed += (s, args) => this.Close();

                    principal.Show();
                }
                else
                {
                    lblMensaje.Text = datos?.Mensaje ?? "Credenciales incorrectas.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al conectar con la API.";
                MessageBox.Show($"No se pudo conectar con el servidor:\n{ex.Message}",
                                "Error de red",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        // Métodos que el diseñador dejó vinculados en FormLogin.Designer.cs
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }
        private void lblusername_Click(object sender, EventArgs e)
        {
        }

        private void lblpassword_Click(object sender, EventArgs e)
        {
        }
    }
}
