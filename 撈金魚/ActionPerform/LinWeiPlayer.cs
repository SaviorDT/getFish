
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 撈金魚.ActionPerform.Common;
using 撈金魚.FileManager;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;

namespace 撈金魚.ActionPerform
{
    internal class LinWeiPlayer : GamePlayer
    {
        protected static int para_index = -1;
        protected readonly string account = "";
        protected readonly string password = "";
        public LinWeiPlayer(WindowPack.WindowSource window, object para_o) : base(window, 0, "這個視窗不應該出現")
        {
            List<AccountDatum> para = (List<AccountDatum>)para_o;
            DoCounterFrameClose = DoCounterFrameShow = false;
            int i = Interlocked.Increment(ref para_index);
            if (i < para.Count)
            {
                account = para[i].Account;
                password = para[i].Password;
            }
        }

        protected override void GoToGameRegion()
        {
            MoleProgram.LoginAccount(window, account, password);
            Click.MuteMole(window);
            Wait.WaitForTimeout(window, 100);
            Click.GoRanch(window);
            Wait.WaitForMainWindow(window, 5000);
            Click.RanchWater(window);
        }

        protected override void LeaveGameRegion()
        {
        }

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
