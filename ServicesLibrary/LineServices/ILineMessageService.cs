using ServicesLibrary.Models.Line;
using System.Threading.Tasks;

namespace ServicesLibrary.LineServices
{
    public interface ILineMessageService
    {
        Task<object> GetProfile(string lineChannelAccessToken, string userid);

        Task SendTextMessage(LineMessageRequestModel lineMessageRequest);

        Task SendTextMessageNoWait(LineMessageRequestModel lineMessageRequest);
    }
}