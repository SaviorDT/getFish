using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.structures;
using static 撈金魚.GetProgramWindow;

namespace 撈金魚.Analyzer
{
    internal class AnalyzeContentRect
    {
        public static WindowRect Remove_white_area(WindowRect rect, FastBitmap shot, bool check_white_edge)
        {
            if ((rect.left | rect.top) != 0)
            {
                throw new ArgumentException("left and top of rect must be 0");
            }

            int top = rect.top;
            int bottom = rect.bottom - 1;
            int left = rect.left;
            int right = rect.right - 1;

            int hmid = left + rect.Width / 2;
            int vmid = top + rect.Height / 2;

            while (ColorDetermine.IsTransparent(shot, right, vmid) && left < right) right--;
            while (ColorDetermine.IsTransparent(shot, hmid, bottom) && top < bottom) bottom--;
            while (ColorDetermine.IsTransparent(shot, left, vmid) && left < right) left++;
            while (ColorDetermine.IsTransparent(shot, hmid, top) && (top < bottom)) top++;

            if (check_white_edge)
            {
                while (ColorDetermine.IsWhite(shot, right, vmid) && left < right) right--;
                while (ColorDetermine.IsWhite(shot, hmid, bottom) && top < bottom) bottom--;
                while (ColorDetermine.IsWhite(shot, left, vmid) && left < right) left++;
                while (ColorDetermine.IsWhite(shot, hmid, top) && (top < bottom)) top++;

                while (!ImageDetermine.AllLineWhite(right + 1, rect.top, right + 1, rect.bottom - 1, shot)) right++;
                while (!ImageDetermine.AllLineWhite(left - 1, rect.top, left - 1, rect.bottom - 1, shot)) left--;
                while (!ImageDetermine.AllLineWhite(rect.left, bottom + 1, rect.right - 1, bottom + 1, shot)) bottom++;
                while (!ImageDetermine.AllLineWhite(rect.left, top - 1, rect.right - 1, top - 1, shot)) top--;
            }

            WindowRect toReturn = new()
            {
                left = Math.Max(left, rect.left),
                top = Math.Max(top, rect.top),
                right = Math.Min(right + 1, rect.right),
                bottom = Math.Min(bottom + 1, rect.bottom)
            };

            return toReturn;
        }
    }
}
