using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using 撈金魚.structures;
using 撈金魚.UserInterface;

namespace 撈金魚.ActionPerform
{
    internal class CaptureMousePosPlayer : GamePlayer
    {
        public CaptureMousePosPlayer(WindowPack.WindowSource window, int times, string title = "計數器") : base(window, times, title)
        {
            DoCounterFrameShow = false;
        }

        protected override void GoToGameRegion()
        {
        }

        protected override void LeaveGameRegion()
        {
        }

        protected override bool PlayGame()
        {
            FastBitmap shot = ScreenAction.GetContentShot(window);
            UserInput.GetMouseInput(shot, UserInput.MouseInput.LeftClick);
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
