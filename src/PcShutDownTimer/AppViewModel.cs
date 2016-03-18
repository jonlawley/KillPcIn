using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;

namespace PcShutdownTimer
{
    public class AppViewModel : ViewModelBase
    {
        private ICommand _closing;
        public ICommand ClosingCommand
        {
            get
            {
                if (_closing == null)
                {
                    _closing = new ActionCommand(Closing);
                }
                return _closing;
            }
        }

        private ICommand _loadedCommand;

        private Timer _timer;

        private DateTime _appEndTime;

        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new ActionCommand(Loaded);
                }
                return _loadedCommand;
            }
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new ActionCommand(Close);
                }
                return _closeCommand;
            }
        }

        private void Close()
        {
            Application.Current.Shutdown(0);
        }

        private void Loaded()
        {
            const int defaultAppRunTimeInMinutes = 2;

            var appRunTime = GetAppRunTime();

            const double minAppRunMinutes = 0.0001;

            if (Math.Abs(appRunTime) < minAppRunMinutes)
            {
                appRunTime = defaultAppRunTimeInMinutes;
            }

            const int secondsPerMinute = 60;

            _appEndTime = DateTime.Now + new TimeSpan(0, 0, 0, (int)( appRunTime * secondsPerMinute));

            const int updateUiInterval = 100;

            _timer = new Timer
            {
                Interval = updateUiInterval
            };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private double GetAppRunTime()
        {
            var args = Environment.GetCommandLineArgs();
            double appRuntime = 0;
            if (args.Length > 1)
            {
                double.TryParse(args[1], out appRuntime);
            }
            return appRuntime;
        }

        private void Closing()
        {
            _timer.Stop();
            _timer.Elapsed -= TimerElapsed;
            _timer = null;
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Remaining = _appEndTime - DateTime.Now ;
            if (Remaining.TotalSeconds < 0)
            {
                Process.Start("shutdown", "/s /t 1");
                Application.Current.Shutdown(0);
            }
        }

        private TimeSpan _remaining;
        public TimeSpan Remaining
        {
            get { return _remaining; }
            set { SetProperty(ref _remaining, value); }
        }
    }
}