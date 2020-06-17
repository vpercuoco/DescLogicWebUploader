using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstASPNETCOREProject
{
    public class FileDeletorService : IFileDeletorService, IHostedService, IDisposable
    {
        private int executionCount = 0;
       
        private Timer _timer;

        private readonly ILogger _logger;
        public FileDeletorService(ILogger<FileDeletorService> logger)
        {
            _logger = logger;
            _logger.LogInformation($"FileDeletorService has initiated at: {DateTime.Now.ToString()}");
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("FileDeletor background worker is operating every 2 minutes");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(2));
            

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation($"Files create 1 hr before {DateTime.Now.ToString()} have been deleted");
            var count = Interlocked.Increment(ref executionCount);
            await Delete();

        }

        public Task StopAsync(CancellationToken stoppingToken)
        { 

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            
        }

        
        public async Task Delete()
        {

            List<string> directories = new List<string>();
            directories.Add(Directory.GetCurrentDirectory() + @"\uploads\Exports\Descriptions\");
            directories.Add(Directory.GetCurrentDirectory() + @"\uploads\Exports\Measurements\");
            directories.Add(Directory.GetCurrentDirectory() + @"\uploads\Imports\Descriptions\");
            directories.Add(Directory.GetCurrentDirectory() + @"\uploads\Imports\Measurements\");
            directories.Add(Directory.GetCurrentDirectory() + @"\uploads\Zips\");

            await Task.Run(() =>
            {
                foreach (var directory in directories)
                {
                    string[] files = Directory.GetFiles(directory);
                    string[] folders = Directory.GetDirectories(directory);

                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.CreationTime < DateTime.Now.AddHours(-1))
                            fi.Delete();
                    }

                    foreach (string folder in folders)
                    {
                        DirectoryInfo di = new DirectoryInfo(folder);
                        if (di.CreationTime < DateTime.Now.AddHours(-1))
                        {
                            di.Delete(true);
                        }
                    }

                }
            });

 
        }
    }
}
