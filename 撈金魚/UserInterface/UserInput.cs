using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 撈金魚.UserInterface
{
    public class UserInput
    {
        public enum MouseInput
        {
            LeftClick
        }
        internal static string SelectFile()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.CheckFileExists = true;
            file.ShowDialog();

            return file.FileName;
        }

        internal static (int MoMoX, int MoMoY) GetMouseInput(FastBitmap img, MouseInput input_type)
        {
            MouseInputWindow input = null;
            System.Windows.Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                input = new MouseInputWindow(img, input_type);
                input.Show();
            });
            while (!input.GotInput)
            {
                Thread.Sleep(100);
            }
            System.Windows.Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                input.Close();
            });
            return (input.LastClickX, input.LastClickY);
        }
    }
}
