using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform
{
    internal class DefenceMoonCakePlayer(structures.WindowPack.WindowSource window, int times) : GamePlayer(window, times, "保衛月餅島")
    {
        protected override void StartGame()
        {
        }

        protected override bool PlayGame()
        {
            return true;
        }

        protected override void StopGame()
        {
        }

        protected override void GoToGameRegion()
        {
        }

        protected override void LeaveGameRegion()
        {
        }
    }
}
