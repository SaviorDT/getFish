using Hazdryx.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using 撈金魚.Analyzer;
using 撈金魚.structures;

namespace 撈金魚.ToolToProgram
{
    internal class ProgramAttributes
    {
        public static WindowRect[] GetContentRect(Process[] ps)
        {
            WindowRect[] toReturn = new WindowRect[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                toReturn[i] = GetContentRect(ps[i]);
                //WindowRect tmp_rect = new();
                //Process process = ps[i];
                //GetClientRect(process.MainWindowHandle, ref tmp_rect);
                //Bitmap test = ScreenAction.GetClientImage(process.MainWindowHandle, tmp_rect);
                //FastBitmap client_shot = new(test);

                //tmp_rect = AnalyzeContentRect.Remove_white_area(tmp_rect, client_shot);
                //tmp_rect.is_enable = (tmp_rect.Width & tmp_rect.Height) != 0;
                //toReturn[i] = tmp_rect;

                //client_shot.Dispose();
            }

            return toReturn;
        }
        public static WindowRect GetContentRect(Process process, bool check_white_edge = true)
        {
            WindowRect toReturn = new();
            if(process == null)
            {
                toReturn.is_enable = false;
                toReturn.top = toReturn.bottom  = toReturn.left = toReturn.right = 0;
                return toReturn;
            }
            GetClientRect(process.MainWindowHandle, ref toReturn);
            FastBitmap client_shot = new(ScreenAction.GetClientImage(process.MainWindowHandle, toReturn));
            toReturn = AnalyzeContentRect.Remove_white_area(toReturn, client_shot, check_white_edge);
            toReturn.is_enable = (toReturn.Width & toReturn.Height) != 0;
            client_shot.Dispose();

            return toReturn;
        }

        public static bool IsValid(Process process)
        {
            return !IsIconic(process.MainWindowHandle);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref WindowRect rect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetClientRect(IntPtr hWnd, ref WindowRect rect);
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
    }
}
