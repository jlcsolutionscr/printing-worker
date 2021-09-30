using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JLCSolutionsCR
{
    public class WindowsBackgroundService : BackgroundService
    {
        private readonly DomainService _queueService;
        private readonly CommandLineOptions _options;

        public WindowsBackgroundService(DomainService queueService, CommandLineOptions options)
        {
            _queueService = queueService;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _queueService.ExecutePendingTickets(_options.CompanyId, _options.BranchId, _options.ChartCount, _options.Filter, _options.PrinterName);
                await Task.Delay(TimeSpan.FromSeconds(_options.DelaySeconds), stoppingToken);
            }
        }
    }
}
