using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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
        public int empty_x;
        public int empty_y;
        public MoMoTreePara(bool w, bool f, int x, int y, int x2, int y2)
        {
            water = w;
            fertilize = f;
            tree_x = x;
            tree_y = y;
            empty_x = x2;
            empty_y = y2;
        }
    }
    internal class MoMoTreeWaterer : GamePlayer
    {
        public bool water, fertilize;
        public int tree_x, tree_y, empty_x, empty_y;
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
            empty_x = para.empty_x;
            empty_y = para.empty_y;
        }

        protected override void GoToGameRegion() { }

        protected override void LeaveGameRegion() { }

        protected override bool PlayGame()
        {
            if (fertilize)
            {
                OpenMoMoTree();
                if (should_stop) { return false; }
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
                Thread.Sleep(150);
            }
            if (water)
            {
                OpenMoMoTree();
                if (should_stop) { return false; }
                MouseInput.MouseClickForMole(window, 411, 397);
                Thread.Sleep(150);
            }
            return true;
        }

        protected override void StartGame() { }

        protected override void StopGame() { }
        private void OpenMoMoTree()
        {
            MouseInput.MouseClickForContent(window, tree_x, tree_y);
            if(!Wait.WaitForMoMoTreeOpen(window, 1000))
            {
                UserInterface.Message.ShowMessageToUser("似乎沒點到毛毛樹，請重新指定", "錯誤");
                should_stop = true;
            }
            MouseInput.MouseClickForContent(window, empty_x, empty_y);
        }
    }
}
