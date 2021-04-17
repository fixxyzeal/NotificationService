using Line.Messaging;
using ServicesLibrary.Models.Line;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ServicesLibrary.CacheServices;
using ServicesLibrary.Models;
using System.Linq;

namespace ServicesLibrary.LineServices
{
    public class LineMessageService : ILineMessageService
    {
        private LineMessagingClient _lineMessagingClient;
        private readonly ICacheService _cacheService;

        public LineMessageService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task SendTextMessage(LineMessageRequestModel lineMessageRequest)
        {
            _lineMessagingClient = new LineMessagingClient(lineMessageRequest.LineChannelAccessToken);

            await _lineMessagingClient.PushMessageAsync(lineMessageRequest.To, lineMessageRequest.Messages).ConfigureAwait(false);
        }

        public async Task<object> GetProfile(string lineChannelAccessToken, string userid)
        {
            _lineMessagingClient = new LineMessagingClient(lineChannelAccessToken);

            return await _lineMessagingClient.GetUserProfileAsync(userid).ConfigureAwait(false);
        }

        public async Task SendTextMessageNoWait(LineMessageRequestModel lineMessageRequest)
        {
            IList<HostedNotiJob> remainJob = await _cacheService.Get<IList<HostedNotiJob>>(HostedNotiJobKey.HostedJob).ConfigureAwait(false) ?? new List<HostedNotiJob>();
            HostedNotiJob hostedNotiJob = new HostedNotiJob() { Type = (int)HostedNotiJobType.LineNotification, LineMessageRequestModel = lineMessageRequest };
            remainJob.Add(hostedNotiJob);

            DateTime expire = DateTime.UtcNow.AddMonths(1);

            await _cacheService.Set<IList<HostedNotiJob>>(HostedNotiJobKey.HostedJob, remainJob, expire).ConfigureAwait(false);
        }
    }
}