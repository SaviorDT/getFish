using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform
{
    internal class Click
    {
        public static void GoRestaurant(WindowSource window)
        {
            PressMyDomain(window);
            MouseInput.MouseClickForMole(window, 880, 449);
        }
        internal static void PressMyDomain(WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 880, 538, 100);
        }
        //public static void ClickNormalConfirmButton(WindowSource source)
        //{
        //    //DoInput.MouseClickForMole(source, 480, 363);
        //}
        public static void ClickNormalConfirmButton(WindowSource source, int count = 1)
        {
            if(count > 5)
            {
                throw new ArgumentException("To many than checked. This function can only use for less than 5.");
            }
            int origin_x = 460, origin_y = 340;
            for(int fix = count * 20; fix > 0; fix -= 20)
            {
                MouseInput.MouseClickForMole(source, origin_x + fix, origin_y + fix);
                Thread.Sleep(30);
            }
        }

        internal static void ClickNormalYesNoDialog(WindowSource source, bool yes)
        {
            int x = 527, y = 349;
            if (yes)
            {
                x -= 100;
            }
            MouseInput.MouseClickForMole(source, x, y);
        }


        internal static void MuteMole(WindowSource window)
        {
            MouseInput.MouseMoveForMole(window, 38, 506);
            MouseInput.MouseClickForMole(window, 90, 470, 100);
        }

        internal static void GoRanch(WindowSource window)
        {
            PressMyDomain(window);
            MouseInput.MouseClickForMole(window, 880, 308);
        }

        internal static void RanchWater(WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 284, 456);
        }
    }
}
