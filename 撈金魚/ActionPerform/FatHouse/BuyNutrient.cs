﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using static 撈金魚.structures.WindowPack;
using 撈金魚.ToolToProgram;
using System.Drawing.Text;

namespace 撈金魚.ActionPerform.FatHouse
{
    internal class BuyNutrient
    {
        private static bool buying = false;
        public static void BuyFatNutrient(WindowSource window, int times)
        {
            while (buying)
            {
                Thread.Sleep(50);
            }
            buying = true;
            SimpleProgramAction.SetForegroundWindow(window.Process);
            OpenShopSlot(window);
            for (int i = 0; i < times; i++)
            {
                Buy_99_nutrient(window);
                Thread.Sleep(50);
            }
            buying = false;
        }
        private static void Buy_99_nutrient(WindowSource window)
        {
            InputSimulator input = new();

            MouseInput.MouseClickForMole(window, 583, 276);
            Thread.Sleep(50);
            MouseInput.MouseClickForMole(window, 378, 302);
            Thread.Sleep(50);

            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            //key_input(9,del, 9)

            MouseInput.MouseClickForMole(window, 527, 341);
            Thread.Sleep(100);
            MouseInput.MouseClickForMole(window, 479, 358);
        }

        private static void OpenShopSlot(WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 785, 527);
            Thread.Sleep(1000);
            MouseInput.MouseClickForMole(window, 278, 251);
            Thread.Sleep(50);
            MouseInput.MouseClickForMole(window, 427, 232);
        }
    }
}
