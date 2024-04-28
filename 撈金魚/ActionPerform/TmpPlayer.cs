using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform
{
    internal class TmpPlayer : GamePlayer
    {
        public TmpPlayer(WindowPack.WindowSource window, int times) : base(window, times)
        {
        }

        protected override void GoToGameRegion()
        { }

        protected override void LeaveGameRegion()
        { }

        protected override bool PlayGame()
        {
            MouseInput.MouseClickForMole(window, 480, 280);
            Wait.WaitForNormalConfirmDialog(window);
            Click.ClickNormalConfirmButton(window);
            return true;
        }

        protected override void StartGame()
        { }

        protected override void StopGame()
        { }
    }
}
