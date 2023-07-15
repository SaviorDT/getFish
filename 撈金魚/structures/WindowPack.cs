using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using 撈金魚.ActionPerform;
using 撈金魚.ToolToProgram;

namespace 撈金魚.structures
{
    public class WindowPack
    {
        internal class WindowSource
        {
            internal Process Process { get; private set; }
            private WindowRect window_rect;
            internal WindowRect WindowRect { get => window_rect; private set => window_rect = value; }
            //internal bool IsEnable;
            internal bool IsEnable { get => window_rect.is_enable; private set => window_rect.is_enable = value; }
            public bool Actioning;

            internal WindowSource(Process process)
            {
                Process = process;
            }
            internal void UpdateRect(bool checkWhiteArea = true)
            {
                WindowRect = ProgramAttributes.GetContentRect(Process, checkWhiteArea);
            }
            internal void ReOpen()
            {
                Process.CloseMainWindow();
                Process.Dispose();
                Process = MoleProgram.ReOpenMole(Process);
                Login.WaitForProgramDraw();
                UpdateRect(false);
                MoleProgram.Login(this);
            }
        }
    }
    internal struct WindowRect
    {
        internal int left;
        internal int top;
        internal int right;
        internal int bottom;
        internal bool is_enable;

        internal int Width { get { return right - left; } }
        internal int Height { get { return bottom - top; } }
        internal Size Size { get { return new Size(Width, Height); } }
        internal System.Drawing.Point Content_to_client { get { return new System.Drawing.Point(left, top); } }
        internal Rectangle Content_rectangle { get { return new Rectangle(Content_to_client, Size); } }
    }
}
