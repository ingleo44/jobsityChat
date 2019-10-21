using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBotSupervisor
    {
        ICollection<string> GetStocksFromMessage(string message);
        Task<string> GetStockQuote(string stockCode);
    }
}