using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 撈金魚.ActionPerform.Common;
using 撈金魚.Analyzer;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform.ElementKnight
{
    internal class ElementKnightPlayer : GamePlayer
    {
        public ElementKnightPlayer(WindowSource window, int times) : base(window, times)
        {
        }
        protected override void StartGame()
        {
            //Console.WriteLine(i);
            StartElementKnightBattle();

            //move mouse leave start button, or the button will change color
            //and ImageDetermine.ElementKnightBattleEnd may not work.
            MouseInput.MouseClickForContent(window, 0, 0);
        }

        protected override bool PlayGame()
        {
            Thread.Sleep(1000);
            while (!ImageDetermine.ElementKnightBattleEnd(window))
            {
                Thread.Sleep(50);
            }
            return true;
        }

        protected override void StopGame()
        {
            Thread.Sleep(100);
            Click.ClickNormalConfirmButton(window, 2);
            //Slept in ClickNormalConfirmButton
            ClickElementKnightGetCardButton();
            Thread.Sleep(30);
        }

        protected override void GoToGameRegion() { }

        protected override void LeaveGameRegion() 
        {
            CloseElementKnight();
        }

        protected void GoInfiniteAbyss()
        {
            GoDungeon(2, 1);
            MouseInput.MouseClickForMole(window, 477, 495);
            Thread.Sleep(5000);
        }

        protected void GoShakesMore()
        {
            GoDungeon(2, 2);
            PressShakesMore();
            Thread.Sleep(4000);
        }

        protected void GoLastDungeon()
        {
            GoDungeon(4, 1);
            MouseInput.MouseClickForMole(window, 236, 208);
            Thread.Sleep(200);
            MouseInput.MouseClickForMole(window, 473, 378);
            Thread.Sleep(10000);
        }

        protected void UseTiliPotion()
        {
            OpenElementKnight();
            Thread.Sleep(1000);
            PressToolSlot();
            Thread.Sleep(500);
            UseTool(0);
            Thread.Sleep(100);
            CloseElementKnightBackpack();
            Thread.Sleep(100);
        }

        protected void PressToolSlot()
        {
            MouseInput.MouseClickForMole(window, 717, 334);
        }

        protected void UseTool(int slot)
        {
            int[] x = { 518, 581, 644 }, y = { 188, 251, 315, 384 };
            MouseInput.MouseClickForMole(window, x[slot % 3], y[slot / 3]);
            Thread.Sleep(50);
            Click.ClickNormalYesNoDialog(window, true);
            Thread.Sleep(100);
            Click.ClickNormalConfirmButton(window);
            Thread.Sleep(50);
        }

        protected void CloseElementKnightBackpack()
        {
            MouseInput.MouseClickForMole(window, 445, 150);
        }

        protected void PressShakesMore()
        {
            MouseInput.MouseClickForMole(window, 526, 381);
            Thread.Sleep(500);
        }
        protected void OpenElementKnight()
        {
            Click.PressMyDomain(window);
            MouseInput.MouseClickForMole(window, 875, 275);
        }
        protected void ClickActivyty()
        {
            MouseInput.MouseClickForMole(window, 635, 124);
        }
        protected void GoDungeon(int pages, int slot)
        {
            int[] x = { 410, 665 }, y = { 200, 408 };
            OpenElementKnight();
            Thread.Sleep(1000);
            ClickActivyty();
            Thread.Sleep(50);
            MouseInput.MouseClickForMole(window, 578, 246);
            Thread.Sleep(1000);
            for (int i = 1; i < pages; i++)
            {
                MouseInput.MouseClickForMole(window, 512, 514);
                Thread.Sleep(50);
            }
            MouseInput.MouseClickForMole(window, x[slot % 2], y[slot / 2]);
            Thread.Sleep(5000);
        }

        protected void ClickElementKnightGetCardButton()
        {
            MouseInput.MouseClickForMole(window, 480, 410);
        }

        protected void CloseElementKnight()
        {
            MouseInput.MouseClickForMole(window, 716, 90);
        }

        protected void StartElementKnightBattle()
        {
            MouseInput.MouseClickForMole(window, 480, 480);
        }
    }
}
