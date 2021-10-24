using System.Threading.Tasks;
using SearchEngineHitCounter.Models;

namespace SearchEngineHitCounter.Contracts
{
    public interface IBearerTokenService
    {
        Task<string> GetBearerToken(BearerTokenAppSetting bearerTokenAppSetting);
    }
}