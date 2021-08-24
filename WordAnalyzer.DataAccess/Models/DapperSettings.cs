namespace WordAnalyzer.DataAccess
{
    public class DapperSettings : IDapperSettings
    {
        public DapperSettings(string? connectionString, Provider provider)
        {
            ConnectionString = connectionString ?? string.Empty;
            Provider = provider;
        }

        public string ConnectionString { get; }
        public Provider Provider { get; }
    }
}
