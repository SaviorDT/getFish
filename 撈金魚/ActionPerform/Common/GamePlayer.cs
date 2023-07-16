using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using 撈金魚.structures;
using 撈金魚.UserInterface;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform.Common
{
    internal abstract class GamePlayer
    {
        protected WindowSource window;
        protected int total_times;
        protected CounterFrame counter;
        internal bool DoCounterFrameShow = true, DoCounterFrameClose = true;
        protected bool should_stop = false;
        public GamePlayer(WindowSource window, int times)
        {
            this.window = window;
            total_times = times;

            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                counter = new CounterFrame(ScreenAction.GetContentShot(window), total_times);
            });
            //counter = new CounterFrame(ScreenAction.GetContentShot(window), total_times);
        }
        protected abstract void StartGame();
        protected abstract bool PlayGame();
        protected abstract void StopGame();
        protected abstract void GoToGameRegion();
        protected abstract void LeaveGameRegion();
        public virtual void Start()
        {
            if (DoCounterFrameShow)
            {
                //counter.FinishCount = 0;
                Application.Current.Dispatcher.Invoke((ThreadStart)delegate
                {
                    counter.Show();
                });
            }

            GoToGameRegion();
            int play_times = 0;
            while(play_times < total_times && !should_stop)
            {
                StartGame();
                if (PlayGame())
                {
                    Application.Current.Dispatcher.Invoke((ThreadStart)delegate
                    {
                        counter.FinishCount = ++play_times;
                    });
                }
                StopGame();
            }
            LeaveGameRegion();

            if (DoCounterFrameClose)
            {
                Application.Current.Dispatcher.Invoke((ThreadStart)delegate
                {
                    counter.Close();
                });
            }
        }
        public virtual void Start(int times)
        {
            Reset(times);
            Start();
        }
        public virtual void ShowCounterFrame()
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                counter.Show();
            });
        }
        public virtual void CloseCounterFrame()
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                counter.Close();
            });
        }
        internal void Reset(int total_count)
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                counter.FinishCount = 0;
                counter.TotalCount = total_times = total_count;
            });
        }
    }
}
