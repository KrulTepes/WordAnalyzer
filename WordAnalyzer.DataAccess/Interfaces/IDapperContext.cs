using System.Threading.Tasks;

namespace WordAnalyzer.DataAccess
{
    public interface IDapperContext
    {
        Task Command(IQueryObject queryObject);
    }
}
