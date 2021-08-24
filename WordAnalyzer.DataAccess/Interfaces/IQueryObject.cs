namespace WordAnalyzer.DataAccess
{
    public interface IQueryObject
    {
        string Sql { get; }
        object? Params { get; }
    }
}
