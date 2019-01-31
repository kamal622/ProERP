using ProERP.Service.CL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProERP.Service.Test
{
    public partial class frmMain : Form
    {
        CancellationTokenSource _cancelSource;
        PreventiveScheduler _scheduler;
        public frmMain()
        {
            InitializeComponent();
            _cancelSource = new CancellationTokenSource();
            _scheduler = new PreventiveScheduler();
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

        private async void btnTest_Click(object sender, EventArgs e)
        {
            await StartAsync();
            MessageBox.Show(this, "Success");
        }
    }
}
