using System;
using System.Collections.Generic;
using System.IO;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public class AutosaveService : IDisposable
    {
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Action _saveAction;

        public AutosaveService(Action saveAction, int intervalMs)
        {
            _saveAction = saveAction;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = intervalMs;
            _timer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            _saveAction();
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
