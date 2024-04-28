using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform
{
    internal class ReopenPlayer : GamePlayer
    {
        public ReopenPlayer(WindowPack.WindowSource window, int times, string title = "計數器") : base(window, times, title)
        {
        }

        protected override void GoToGameRegion()
        {
        }

        protected override void LeaveGameRegion()
        {
        }

        protected override bool PlayGame()
        {
            Thread.Sleep(30*60*1000);
            window.ReOpen();
            Click.GoRestaurant(window);
            return true;
        }

        protected override void StartGame()
        {
        }

        protected override void StopGame()
        {
        }
    }
}
