using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform;
using 撈金魚.structures;
using 撈金魚.UserInterface;

namespace 撈金魚.ToolToProgram
{
    internal class MoleProgram
    {
        private const string MOLE_WEBSITE = "http://mole.61.com.tw/Client.swf";
        private static string flashplayer = "flashplayer_32_sa.exe";

        internal static void Login(WindowPack.WindowSource window)
        {
            ActionPerform.Login.WaitForLogin(window);
            ActionPerform.Login.ClickLoginButton(window);
            ActionPerform.Login.WaitForSelectAccount(window);
            ActionPerform.Login.LoginAccount(window);
            ActionPerform.Login.WaitForSelectServer(window);
            ActionPerform.Login.SelectServer(window);
            Wait.WaitForMainWindow(window);
        }

        internal static Process ReOpenMole()
        {
            Process mole = null;
            try
            {
                mole = Process.Start(flashplayer, MOLE_WEBSITE);
            }
            catch (Exception e) when(e is InvalidOperationException || e is Win32Exception)
            {
                ReOpenMoleExceptionHandler(ref mole);
            }

            return mole;
        }

        private static void ReOpenMoleExceptionHandler(ref Process mole)
        {
            Message.ShowMessageToUser("未發現flashplayer，請指定位置");
            flashplayer = UserInput.SelectFile();

            if (flashplayer != "")
            {
                try
                {
                    mole = Process.Start(flashplayer, MOLE_WEBSITE);
                }
                catch (Exception ex)
                {
                    Message.ShowMessageToUser("啟動檔案失敗，如果你沒有在亂搞，請聯絡作者\n\n" + ex.ToString());
                }
            }
        }
    }
}
