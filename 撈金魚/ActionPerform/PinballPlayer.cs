using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform
{
    internal class PinballPlayer : GamePlayer
    {
        public PinballPlayer(WindowPack.WindowSource window, int times) : base(window, times)
        {
        }

        protected override void GoToGameRegion() { }

        protected override void LeaveGameRegion() { }

        protected override bool PlayGame()
        {
            throw new NotImplementedException();
        }

        protected override void StartGame()
        {
            MouseInput.MouseClickForMole(window, 350, 455);
            //wait
            MouseInput.MouseClickForMole(window, 846, 423);
            //wait
            MouseInput.MouseClickForMole(window, 333, 362);
            //460, 348
            //598, 356
            //wait
        }

        protected override void StopGame()
        {
            MouseInput.MouseClickForMole(window, 467, 369);
            Wait.WaitForMainWindow(window);
        }
    }
}
