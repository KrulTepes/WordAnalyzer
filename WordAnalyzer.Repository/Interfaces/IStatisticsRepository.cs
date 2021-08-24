using System.Threading.Tasks;

namespace WordAnalyzer.Repository
{
    public interface IStatisticsRepository
    {
        Task AddAsync(DbStatistics dbStatistics);
    }
}
