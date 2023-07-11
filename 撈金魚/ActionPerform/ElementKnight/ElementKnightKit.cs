using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform.ElementKnight
{
    internal class ElementKnightKit : ElementKnightPlayer
    {
        public ElementKnightKit(WindowPack.WindowSource window, int times) : base(window, times)
        {
            DoCounterFrameShow = false;
            DoCounterFrameClose = false;
        }

        public override void Start()
        {
            ElementKnightPlayer player = new ElementKnightPlayer(window, 75);
            player.ShowCounterFrame();
            player.DoCounterFrameClose = player.DoCounterFrameShow = false;

            GoInfiniteAbyss();
            //new ElementKnightPlayer(window, 75).Start();
            player.Start(75);
            GoShakesMore();
            //new ElementKnightPlayer(window, 10).Start();
            player.Start(10);
            UseTiliPotion();
            PressShakesMore();
            //new ElementKnightPlayer(window, 25).Start();
            player.Start(24);
            UseTiliPotion();
            PressShakesMore();
            //new ElementKnightPlayer(window, 7).Start();
            player.Start(7);

            GoLastDungeon();
            //new ElementKnightPlayer(window, 19).Start();
            player.Start(18);

            player.CloseCounterFrame();
        }
    }
}
