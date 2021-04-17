using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServicesLibrary.CacheServices;
using ServicesLibrary.LineServices;
using ServicesLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.HostedServices
{
    public class NotificationHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer;
        private readonly ICacheService _cacheService;
        private readonly ILineMessageService _lineMessageService;
        private readonly ILogger<NotificationHostedService> _logger;

        public NotificationHostedService(
            ILogger<NotificationHostedService> logger,
            ICacheService cacheService,
            ILineMessageService lineMessageService)
        {
            _logger = logger;
            _cacheService = cacheService;
            _lineMessageService = lineMessageService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Hosted Service running.");

            _timer = new Timer(SendNotification, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void SendNotification(object state)
        {
            //Get Joblist Form cache
            var jobList = _cacheService.Get<IList<HostedNotiJob>>(HostedNotiJobKey.HostedJob).GetAwaiter().GetResult();

            if (jobList != null)
            {
                executionCount = jobList.Count;

                foreach (var job in jobList)
                {
                    //Send LineNotification
                    if (job.Type == (int)HostedNotiJobType.LineNotification)
                    {
                        _logger.LogInformation(
                             "Processing LineNotification Service is working. Count: {Count}", executionCount);
                        _lineMessageService.SendTextMessage(job.LineMessageRequestModel).GetAwaiter().GetResult();

                        executionCount--;

                        _logger.LogInformation(
                        "Processing Notification Service is Remaining. Count: {Count}", executionCount);
                    }
                }

                //Delete all job
                _cacheService.Delete(HostedNotiJobKey.HostedJob).GetAwaiter().GetResult();
            }
            //No job found
            _logger.LogInformation("No Job in Queued.");
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}