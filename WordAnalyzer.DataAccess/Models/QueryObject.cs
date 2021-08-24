using System;

namespace WordAnalyzer.DataAccess
{
    public class QueryObject : IQueryObject
    {
        public QueryObject(string sql, object? parameters = null)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException();
            }

            Sql = sql;
            Params = parameters;
        }

        public string Sql { get; }
        public object? Params { get; }
    }
}
