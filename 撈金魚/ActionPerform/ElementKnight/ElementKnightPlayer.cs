using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform.ElementKnight
{
    internal class ElementKnightPlayer
    {
        internal static void GoInfiniteAbyss(WindowSource window)
        {
            GoDungeon(window, 2, 1);
            MouseInput.MouseClickForMole(window, 477, 495);
            Thread.Sleep(5000);
        }

        internal static void GoShakesMore(WindowSource window)
        {
            GoDungeon(window, 2, 2);
            PressShakesMore(window);
            Thread.Sleep(4000);
        }

        internal static void GoLastDungeon(WindowSource window)
        {
            GoDungeon(window, 4, 1);
            MouseInput.MouseClickForMole(window, 236, 208);
            Thread.Sleep(200);
            MouseInput.MouseClickForMole(window, 473, 378);
            Thread.Sleep(10000);
        }

        internal static void UseTiliPotion(WindowSource window)
        {
            OpenElementKnight(window);
            Thread.Sleep(1000);
            PressToolSlot(window);
            Thread.Sleep(500);
            UseTool(window, 0);
            Thread.Sleep(100);
            CloseElementKnight(window);
            Thread.Sleep(100);
        }

        private static void PressToolSlot(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 717, 334);
        }

        public static void UseTool(WindowSource source, int slot)
        {
            int[] x = { 518, 581, 644 }, y = { 188, 251, 315, 384 };
            MouseInput.MouseClickForMole(source, x[slot % 3], y[slot / 3]);
            Thread.Sleep(50);
            Common.ClickNormalYesNoDialog(source, true);
            Thread.Sleep(100);
            Common.ClickNormalConfirmButton(source);
            Thread.Sleep(50);
        }

        private static void CloseElementKnight(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 445, 150);
        }

        internal static void PressShakesMore(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 526, 381);
            Thread.Sleep(500);
        }
        private static void OpenElementKnight(WindowSource source)
        {
            Common.PressMyDomain(source);
            MouseInput.MouseClickForMole(source, 875, 275);
        }
        private static void ClickActivyty(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 635, 124);
        }
        private static void GoDungeon(WindowSource source, int pages, int slot)
        {
            int[] x = { 410, 665 }, y = { 200, 408 };
            OpenElementKnight(source);
            Thread.Sleep(1000);
            ClickActivyty(source);
            Thread.Sleep(50);
            MouseInput.MouseClickForMole(source, 578, 246);
            Thread.Sleep(1000);
            for (int i = 1; i < pages; i++)
            {
                MouseInput.MouseClickForMole(source, 512, 514);
                Thread.Sleep(50);
            }
            MouseInput.MouseClickForMole(source, x[slot % 2], y[slot / 2]);
            Thread.Sleep(5000);
        }
    }
}
