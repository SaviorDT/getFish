using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform.Common
{
    internal abstract class OldAngelPlayer(WindowPack.WindowSource window, int times, string title = "舊天使戰鬥") : GamePlayer(window, times, title)
    {
        protected override bool PlayGame()
        {
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
