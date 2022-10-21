using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 撈金魚
{
    internal class GetProgramWindow
    {
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public MyRect rect = new MyRect();
        public bool rect_enable = false;

        private Process[] processes;

        public GetProgramWindow(string process_name)
        {
            processes = Process.GetProcessesByName(process_name);
            //updateRect();
        }
        public void updateRect()
        {
            refreshWindowRect();
            getContentRect();
            AnalyzeNet.refreshRadius(100 * rect.width / MainWindow.MOLE_W);
        }
        private void getContentRect()
        {
            Size size = new Size(rect.width + rect.left, rect.height + rect.top);
            Bitmap shot = new Bitmap(size.Width, size.Height);
            Graphics gf = Graphics.FromImage(shot);
            gf.CopyFromScreen(rect.left, rect.top, rect.left, rect.top, size);
            FastBitmap fast_shot = new FastBitmap(shot);
            fast_shot.Lock();

            int top = rect.top;
            int bottom = rect.bottom-1;
            int left = rect.left;
            int right = rect.right-1;

            int hmid = left + size.Width / 2;
            int vmid = top + size.Height / 2;

            while (fast_shot.GetPixelInt(right, vmid) == -1 && left < right) right--;
            while (fast_shot.GetPixelInt(hmid, bottom) == -1 && top < bottom) bottom--;
            while (fast_shot.GetPixelInt(left, vmid) == -1 && left < right) left++;
            while ((fast_shot.GetPixelInt(hmid, top) == -1) && (top < bottom)) top++;

            while (!allAreWhite(right + 1, rect.top, right + 1, rect.bottom-1, fast_shot)) right++;
            while (!allAreWhite(left - 1, rect.top, left - 1, rect.bottom-1, fast_shot)) left--;
            while (!allAreWhite(rect.left, bottom + 1, rect.right-1, bottom + 1, fast_shot)) bottom++;
            while (!allAreWhite(rect.left, top - 1, rect.right-1, top - 1, fast_shot)) top--;


            rect.left = Math.Max(left, rect.left);
            rect.top = Math.Max(top, rect.top);
            rect.right = Math.Min(right+1, rect.right);
            rect.bottom = Math.Min(bottom+1, rect.bottom);
            rect_enable = true;

            if ((rect.width & rect.height) == 0)
            {
                rect_enable = false;
                MessageBox.Show("視窗為空白，請先開啟摩爾莊園");
            }
        }
        private bool allAreWhite(int start_x, int start_y, int end_x, int end_y, FastBitmap fast)
        {
            if (start_x < 0 || start_y < 0 || end_x >= fast.Width || end_y >= fast.Height) return true;
            int color = -1;
            for(int x=start_x; x<=end_x; x++)
            {
                for(int y=start_y; y<=end_y; y++)
                {
                    color &= fast.GetPixelInt(x, y);
                }
            }

            return color == -1;
        }
        private void refreshWindowRect()
        {
            rect = new MyRect();
            foreach(Process process in processes) 
            {
                MyRect wrect = new MyRect();
                MyRect crect = new MyRect();
                GetWindowRect(process.MainWindowHandle, ref wrect);
                GetClientRect(process.MainWindowHandle, ref crect);

                if ((crect.right & crect.bottom) == 0) continue;
                SetForegroundWindow(process.MainWindowHandle);
                if (rect.bottom != 0) MessageBox.Show("超過一個flash player");

                if ((wrect.right - wrect.left - crect.right) % 2 != 0) 
                    MessageBox.Show("程式左右邊框寬度不同，將導致出現bug。\n請在不移動/縮放摩爾莊園的情況下，將摩爾莊園移到螢幕最上層，截圖並聯絡作者" +
                        String.Format("\nwrect: {0}, {1}, {2}, {3}", wrect.left, wrect.top, wrect.right, wrect.bottom) +
                        String.Format("\ncrect: {0}, {1}, {2}, {3}", crect.left, crect.top, crect.right, crect.bottom));
                int border = (wrect.right - wrect.left - crect.right) / 2;
                rect.left = wrect.left + border;
                rect.right = wrect.right - border;
                rect.bottom = wrect.bottom - border;

                rect.top = rect.bottom - crect.bottom;
            }
        }

        public struct MyRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int width { get { return right - left; } }

            public int height { get { return bottom - top; } }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref MyRect rect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetClientRect(IntPtr hWnd, ref MyRect rect);
    }
}
