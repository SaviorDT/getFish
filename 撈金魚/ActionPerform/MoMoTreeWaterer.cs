using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform
{
    public struct MoMoTreePara
    {
        public bool water;
        public bool fertilize;
        public int tree_x;
        public int tree_y;
        public MoMoTreePara(bool w, bool f, int x, int y)
        {
            water = w;
            fertilize = f;
            tree_x = x;
            tree_y = y;
        }
    }
    internal class MoMoTreeWaterer : GamePlayer
    {
        public bool water, fertilize;
        public int tree_x, tree_y;
        public MoMoTreeWaterer(WindowPack.WindowSource window, int times, object para) : base(window, times)
        {
            SetProperty((MoMoTreePara)para);
        }

        public void SetProperty(MoMoTreePara para)
        {
            water = para.water;
            fertilize = para.fertilize;
            tree_x = para.tree_x;
            tree_y = para.tree_y;
        }

        protected override void GoToGameRegion() { }

        protected override void LeaveGameRegion() { }

        protected override bool PlayGame()
        {
            if (fertilize)
            {
                MouseInput.MouseClickForMole(window, 510, 398);
                if (!Wait.WaitForNormalYesNoDialog(window, 1000))
                {
                    MouseInput.MouseClickForMole(window, 510, 398);
                    if (!Wait.WaitForNormalYesNoDialog(window, 1000))
                    {
                        return false;
                    }
                }
                MouseInput.MouseClickForMole(window, 428, 348);
            }
            if (water)
            {
                MouseInput.MouseClickForMole(window, 411, 397);
            }
            return true;
        }

        protected override void StartGame()
        {
            MouseInput.MouseClickForContent(window, tree_x, tree_y);
            if (!Wait.WaitForMoMoTreeOpen(window, 1000))
            {
                UserInterface.Message.ShowMessageToUser("似乎沒點到毛毛樹，請重新指定", "錯誤");
                should_stop = true;
            }
        }

        protected override void StopGame() { }
    }
}
