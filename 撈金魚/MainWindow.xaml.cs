using FastBitmapLib;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsInput;
using WindowsInput.Native;
using static 撈金魚.AnalyzeNet;
using Application = System.Windows.Application;
using Color = System.Drawing.Color;
using Cursor = System.Windows.Forms.Cursor;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace 撈金魚
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        GetProgramWindow window = new GetProgramWindow("flashplayer_32_sa");
        public const int MOLE_W = 960, MOLE_H = 560, BUCKET_NET_X = 150, BUCKET_NET_Y = -10, BUCKET_WATER_X = 235, BUCKET_WATER_Y = 415, NET_FIX_X = 50, NET_FIX_Y = 45;

        private void playFishButton(object sender, RoutedEventArgs e)
        {
            window.updateRect();
            new Thread(playFish).Start();
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeGlobalHook();
        }

        private void InitializeGlobalHook()
        {
            IKeyboardMouseEvents hook = Hook.GlobalEvents();
            //hook.MouseUpExt += mouseUp;
            hook.KeyUp += exit;
        }

        private void exit(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Escape))
                Environment.Exit(0);
        }

        private void playFish()
        {
            int count = 0;
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                count = Convert.ToInt32(input.Text);
            }
            );
            for (int i=0; i<count; i++)
            {
                removeOneTime();
                Thread.Sleep(10);
                if (i == 0) 
                    startFish(2500);
                else
                    startFish(300);
                Thread.Sleep(50);
                getFish();
                Thread.Sleep(50);
                endFish();
            }
            goRestaurant();
        }

        private void startFish(int delay)
        {
            DoInput.mouseClickForMole(window.rect, 838, 32);
            Thread.Sleep(delay);
            DoInput.mouseClickForMole(window.rect, 788, 443);
            Thread.Sleep(delay);
            DoInput.mouseClickForMole(window.rect, 474, 485);
        }

        private void buyFatNutrientButton(object sender, RoutedEventArgs e)
        {
            window.updateRect();
            new Thread(buyFatButrient).Start();
        }

        private void buyFatButrient()
        {
            int count = 0;
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                count = Convert.ToInt32(input.Text);
            }
            );
            openShopSlot();
            for (int i = 0; i < count; i++)
            {
                removeOneTime();
                buy_99_nutrient();
            }
        }

        private void buy_99_nutrient()
        {
            InputSimulator input = new InputSimulator();

            DoInput.mouseClickForMole(window.rect, 583, 276);
            Thread.Sleep(50);
            DoInput.mouseClickForMole(window.rect, 378, 302);
            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(20);
            input.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            Thread.Sleep(20);
            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(20);
            //key_input(9,del, 9)
            DoInput.mouseClickForMole(window.rect, 527, 341);
            Thread.Sleep(20);
            DoInput.mouseClickForMole(window.rect, 479, 358);
        }

        private void openShopSlot()
        {
            DoInput.mouseClickForMole(window.rect, 785, 527);
            Thread.Sleep(1000);
            DoInput.mouseClickForMole(window.rect, 278, 251);
            Thread.Sleep(50);
            DoInput.mouseClickForMole(window.rect, 427, 232);
        }

        private void endFish()
        {
            DoInput.mouseClickForMole(window.rect, 480, 358);
        }

        private void goRestaurant()
        {
            DoInput.mouseClickForMole(window.rect, 880, 538);
            DoInput.mouseClickForMole(window.rect, 880, 449);
        }

        private void getFish()
        {
            for(int i=0; i<25; i++)
            {
                Thread.Sleep(500);
                Point net_center;
                do
                {
                    Point[] fish = findDiferences();
                    if (fish.Length == 7)
                        return;
                    net_center = AnalyzeNet.CalculateBestPoint(fish, window.rect, this);
                } while (net_center.X == -1);
                DoInput.fishClickKit(window.rect, net_center.X + NET_FIX_X*MOLE_W/window.rect.width, net_center.Y + NET_FIX_Y*MOLE_H / window.rect.height);
            }
        }

        private Point[] findDiferences()
        {
            Point[] fish = ScreenAction.GetFish(window);
            if (fish.Length < 3)
            {
                DateTime start = DateTime.Now;
                while (fish.Length < 3)
                {
                    if (DateTime.Now - start >= TimeSpan.FromSeconds(5))
                    {
                        return new Point[7];
                    }
                    fish = ScreenAction.GetFish(window);
                }
            }

            return fish;
        }

        private void removeOneTime()
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                input.Text = Convert.ToString(Convert.ToInt32(input.Text)-1);
            }
            );
        }

        public void addText(string s)
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                input.Text += s + "\n";
            }
            );
        }

        public void addNum(int num)
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate
            {
                input.Text = Convert.ToString(Convert.ToInt32(input.Text) + num);
            }
            );
        }

        //public void addText(string s)
        //{
        //Application.Current.Dispatcher.Invoke((ThreadStart)delegate
        //    {
        //        text_block.Text += s + "\n";
        //    }
        //    );
        //}
    }
}
