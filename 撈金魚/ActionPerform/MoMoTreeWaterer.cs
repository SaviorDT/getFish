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
    public class MoMoTreePara
    {
        public bool Water { get; set; }
        public bool Fertilize { get; set; }
        public int Tree_x { get; set; }
        public int Tree_y { get; set; }
        public int Empty_x { get; set; }
        public int Empty_y { get; set; }
        public MoMoTreePara(bool w, bool f, int x, int y, int x2, int y2)
        {
            Water = w;
            Fertilize = f;
            Tree_x = x;
            Tree_y = y;
            Empty_x = x2;
            Empty_y = y2;
        }
        public MoMoTreePara() { }
    }
    internal class MoMoTreeWaterer : GamePlayer
    {
        public bool water, fertilize;
        public int tree_x, tree_y, empty_x, empty_y;
        private static bool watering = false;
        public MoMoTreeWaterer(WindowPack.WindowSource window, int times, object para) : base(window, times) 
        {
            SetProperty((MoMoTreePara)para);
        }

        public void SetProperty(MoMoTreePara para)
        {
            water = para.Water;
            fertilize = para.Fertilize;
            tree_x = para.Tree_x;
            tree_y = para.Tree_y;
            empty_x = para.Empty_x;
            empty_y = para.Empty_y;
        }

        protected override void GoToGameRegion() 
        {
            while (watering)
            {
                Thread.Sleep(50);
            }
            watering = true;
        }

        protected override void LeaveGameRegion() 
        {
            watering = false;
        }

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
