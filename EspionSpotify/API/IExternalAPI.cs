using System.Threading.Tasks;
using EspionSpotify.Enums;
using EspionSpotify.Models;

namespace EspionSpotify.API
{
    public interface IExternalAPI
    {
        ExternalAPIType ApiType { get; }
        bool IsAuthenticated { get; }
        Task Authenticate();
        void Reset();

        Task<bool> UpdateTrack(Track track);
    }
}