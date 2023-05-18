using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace 撈金魚.UserInterface
{
    internal class Message
    {
        internal static void ShowMessageToUser(string message, string caption = "提示")
        {
            //MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBox.Show(message, caption);
        }
        internal static bool ShowYesNoToUser(string message, string caption = "提示")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
