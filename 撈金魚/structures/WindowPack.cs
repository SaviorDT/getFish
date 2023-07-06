﻿using System;
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
            internal WindowRect WindowRect { get; private set; }
            internal bool IsEnable;

            internal WindowSource(GetProgramWindow window, int index)
            {
                Process = window.Processes[index];
                WindowRect = window.Rects_of_client[index];
                IsEnable = WindowRect.is_enable;
            }
            internal void UpdateRect(bool checkWhiteArea = true)
            {
                WindowRect = ProgramAttributes.GetContentRect(Process, checkWhiteArea);
                IsEnable = WindowRect.is_enable;
            }
            internal void ReOpen()
            {
                Process.CloseMainWindow();
                Process.Dispose();
                Process = MoleProgram.ReOpenMole(Process);
                Login.WaitMainWindow();
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
