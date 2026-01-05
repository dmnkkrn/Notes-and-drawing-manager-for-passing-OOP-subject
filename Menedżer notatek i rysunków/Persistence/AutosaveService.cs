using System;
using System.Collections.Generic;
using System.IO;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public class AutosaveService<T> : IDisposable
    {
        private readonly System.Windows.Forms.Timer _timer;
        private readonly string _mainPath;
        private readonly string _restorePath;
        private readonly string _workBackup;
        private readonly Func<IEnumerable<T>> _getData;
        private readonly Action<string, IEnumerable<T>> _save;

        public AutosaveService(
            string mainPath,
            string restorePath,
            string workBackup,
            Func<IEnumerable<T>> getData,
            Action<string, IEnumerable<T>> saveAction,
            int intervalMs)
        {
            _mainPath = mainPath;
            _restorePath = restorePath;
            _getData = getData;
            _save = saveAction;
            _workBackup = workBackup;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = intervalMs;
            _timer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            _save(_restorePath, _getData());
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public void SaveUser()
        {
            var data = _getData();
            _save(_mainPath, data);
            _save(_restorePath, data);
        }

        public bool HasUnsavedChanges()
        {
            if (!File.Exists(_mainPath) || !File.Exists(_restorePath))
                return false;

            var a = File.ReadAllBytes(_mainPath);
            var b = File.ReadAllBytes(_restorePath);

            if (a.Length != b.Length)
                return true;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return true;

            return false;
        }

        public void DiscardRestore()
        {
            if (File.Exists(_restorePath))
                File.Delete(_restorePath);
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
