namespace WordAnalyzer.DataAccess
{
    public interface IDapperSettings
    {
        string ConnectionString { get; }
        Provider Provider { get; }
    }
}
