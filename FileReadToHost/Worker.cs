using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileReadToHost
{
    public class Worker : BackgroundService
    {
        private FileHandler fh;
        public Worker(IOptions<FileConfiguration> fs)
        {
            fh = new FileHandler(fs);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                fh.syncFiles();
                await Task.Delay(20000, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            EventLog.WriteEntry("FileReadToHost", "Service Started", EventLogEntryType.SuccessAudit);   
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            EventLog.WriteEntry("FileReadToHost", "Service Stopped", EventLogEntryType.SuccessAudit);
            return base.StopAsync(cancellationToken);
        }
    }
}
