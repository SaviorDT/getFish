using System.Diagnostics;
using System.Drawing;

namespace 撈金魚.structures
{
    public class WindowPack
    {
        internal struct WindowSource
        {
            public readonly Process Process;
            public readonly WindowRect WindowRect;

            public WindowSource(GetProgramWindow window, int index)
            {
                Process = window.processes[index];
                WindowRect = window.Rects_of_client[index];
            }
        }
    }
    public struct WindowRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public bool is_enable;

        public int Width { get { return right - left; } }
        public int Height { get { return bottom - top; } }
        public Size Size { get { return new Size(Width, Height); } }
        public System.Drawing.Point Content_to_client { get { return new System.Drawing.Point(left, top); } }
        public Rectangle Content_rectangle { get { return new Rectangle(Content_to_client, Size); } }
    }
}
