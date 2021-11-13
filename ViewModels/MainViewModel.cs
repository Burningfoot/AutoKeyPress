using AutoKeyPress.Models.Tools;
using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Controls;

namespace AutoKeyPress.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        public RelayCommand<Button> BtnStart { get; set; }
        public RelayCommand<Button> BtnStop { get; set; }
        private String _textForDialog;
        public String TextForDialog
        {
            get
            {
                if (_textForDialog == null)
                    _textForDialog = "";
                return _textForDialog;
            }
            set { _textForDialog = value; OnPropertyChange("TextForDialog");}
        }

        public Timer TimerKeyPress { get; set; } = new Timer();
        public int Counter { get; set; } = 0;

        private int _intervalKeyPresses;
        public int IntervalKeyPresses
        {
            get { return _intervalKeyPresses; }
            set
            {
                _intervalKeyPresses = value;
                TimerKeyPress.Interval = 1000 * 1 / _intervalKeyPresses;
                TimerKeyPress.Stop();
                TimerKeyPress.Start();
                OnPropertyChange("_intervalKeyPresses");
            }
        }
        public MainViewModel()
        {
            InitiateVars();
            RelayCommandCollection();
            TextForDialog = "Click Start!";
        }
        private void InitiateVars()
        {
            TimerKeyPress.Interval = 1000 * 1 / 5;
            TimerKeyPress.Elapsed += TimerKeyPressTick;
            TimerKeyPress.Enabled = false;
        }
        private void TimerKeyPressTick(Object source, ElapsedEventArgs e)
        {
            Counter++;
            TextForDialog = Counter.ToString();
            keybd_event((byte)0x20, 0, 0x0001 | 0, 0);
        }

        private void RelayCommandCollection()
        {
            BtnStart = new RelayCommand<Button>((o) =>
            {
                StartPressingBtn();
            });

            BtnStop = new RelayCommand<Button>((o) =>
            {
                StopPressingBtn();
            });
        }

        private void StopPressingBtn()
        {
            TimerKeyPress.Stop();
        }

        private void StartPressingBtn()
        {
            TimerKeyPress.Start();
        }

        //async private void PressingKeysAsync()
        //{
        //    await Task.Run(() => 
        //    {
        //        Task.Delay(100).Wait();
        //        TextForDialog += "Going! \n";
        //    });
        //}

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
    }
}
