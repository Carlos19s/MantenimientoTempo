using Libsql.Client;

namespace MantenimientoTempoApi.Data
{
    public class TursoDatabaseContext
    {
        private readonly IConfiguration _config;
        private readonly string _databaseUrl;
        private readonly string _authToken;

        public TursoDatabaseContext(IConfiguration config)
        {
            _config = config;
            _databaseUrl = _config["Turso:DatabaseUrl"] ?? throw new ArgumentNullException("Turso:DatabaseUrl no está configurado en appsettings.json");
            _authToken = _config["Turso:AuthToken"] ?? throw new ArgumentNullException("Turso:AuthToken no está configurado en appsettings.json");
        }

        public async Task<IDatabaseClient> GetClientAsync()
        {
            return await DatabaseClient.Create(options =>
            {
                options.Url = _databaseUrl;
                options.AuthToken = _authToken;
            });
        }
    }
}
