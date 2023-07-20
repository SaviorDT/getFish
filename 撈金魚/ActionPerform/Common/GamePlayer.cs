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
        protected virtual void InterruptAction() 
        { 
            CloseCounterFrame();
        }
        public virtual void Start()
        {
            ShowCounterFrame();

            GoToGameRegion();
            int play_times = 0;
            while(play_times < total_times)
            {
                if(should_stop) { InterruptAction(); return; }
                StartGame();
                if (should_stop) { InterruptAction(); return; }
                if (PlayGame())
                {
                    Application.Current.Dispatcher.Invoke((ThreadStart)delegate
                    {
                        counter.FinishCount = ++play_times;
                    });
                }
                if (should_stop) { InterruptAction(); return; }
                StopGame();
            }
            if (should_stop) { InterruptAction(); return; }
            LeaveGameRegion();

            CloseCounterFrame();
        }
        public virtual void ShowCounterFrame()
        {
            if (DoCounterFrameShow)
            {
                ForceShowCounterFrame();
            }
        }
        public virtual void CloseCounterFrame()
        {
            if (DoCounterFrameClose)
            {
                ForceCloseCounterFrame();
            }
        }
        public virtual void Start(int times)
        {
            Reset(times);
            Start();
        }
        public virtual void ForceShowCounterFrame()
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                counter.Show();
            });
        }
        public virtual void ForceCloseCounterFrame()
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
