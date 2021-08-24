using System.Threading.Tasks;
using WordAnalyzer.DataAccess;
using WordAnalyzer.Repository.SQL;

namespace WordAnalyzer.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDapperContext _dapperContext;

        public StatisticsRepository(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task AddAsync(DbStatistics dbStatistics)
        {
            await Task.Factory.StartNew(() => _dapperContext.Command(new QueryObject(SqlScripts.AddStatistics, dbStatistics)));
        }
    }
}
