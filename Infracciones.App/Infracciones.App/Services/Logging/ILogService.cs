using Infracciones.Models;
using System.Threading.Tasks;

namespace Infracciones.Services.Logging
{
    public interface ILogService
    {
        Task<LogModel> CreateLog();
    }
}
