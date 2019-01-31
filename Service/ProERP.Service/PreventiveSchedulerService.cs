using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ProERP.Service.CL;
using System.Configuration;
using System.Threading;

namespace ProERP.Service
{
    public partial class PreventiveSchedulerService : ServiceBase
    {
        System.Timers.Timer _timer;
        CancellationTokenSource _cancelSource;
        PreventiveScheduler _scheduler;

        public PreventiveSchedulerService()
        {
            InitializeComponent();
            _cancelSource = new CancellationTokenSource();
            _scheduler = new PreventiveScheduler();
            int timerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
            double interval = timerInterval * 60 * 1000;
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += _timer_Elapsed;
        }

        public async Task StartAsync()
        {
            await UpdateAsync(_cancelSource.Token);
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                var startScheduler = Task.Run(() => _scheduler.Start());
                await Task.WhenAll(startScheduler);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            finally
            {
                if (_cancelSource.IsCancellationRequested)
                    await Task.Delay(2000);
            }
        }

        protected override void OnStart(string[] args)
        {
            _timer.Enabled = true;
        }

        private async void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await StartAsync();
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
        }
    }
}
