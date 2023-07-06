using Gma.System.MouseKeyHook;
using Hazdryx.Drawing;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using 撈金魚.ActionPerform;
using 撈金魚.ActionPerform.Common;
using 撈金魚.ActionPerform.ElementKnight;
using 撈金魚.Analyzer;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;
using 撈金魚.UserInterface;
using static 撈金魚.ActionPerform.ButtonPerformer;
using Application = System.Windows.Application;
using Point = System.Drawing.Point;

namespace 撈金魚
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly GetProgramWindow window = new("flashplayer_32_sa");

        private void PlayFishButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window, GetLoopTimes(), ActionKit.fish);
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeGlobalHook();

            //debugger.Text = Convert.ToString(window.processes.Length);
        }

        private void ClosingAction(object sender, CancelEventArgs e)
        {
            //the default closing action may not actually stop the program.
            //It only close the window and work in background until your threads are done.
            Environment.Exit(0);
        }

        private void InitializeGlobalHook()
        {
            IKeyboardMouseEvents hook = Hook.GlobalEvents();
            //hook.MouseUpExt += mouseUp;
            hook.KeyUp += Exit;
        }

        private void Exit(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Back) && e.Modifiers == Keys.Control)
                Environment.Exit(0);
        }

        private void BuyFatNutrientButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window, GetLoopTimes(), ActionKit.buy_nutrient);
        }

        private void ElementKnightButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window, GetLoopTimes(), ActionKit.play_element_knight);
        }


        //private void RemoveOneTime()
        //{
        //    Application.Current.Dispatcher.Invoke((ThreadStart)delegate
        //    {
        //        input.Text = Convert.ToString(Convert.ToInt32(input.Text) - 1);
        //    });
        //}

        private int GetLoopTimes()
        {
            if(input.Text.Length == 0) return 0;
            return Convert.ToInt32(input.Text);
        }

        private void ElementKnightKitButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window, GetLoopTimes(), ActionKit.element_knight_kit);
        }

        private void DragonButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window, GetLoopTimes(), ActionKit.dragon);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            testFun(new structures.WindowPack.WindowSource(window, 0));
            //new structures.WindowPack.WindowSource(window, 0).ReOpen();
        }

        private void testFun(WindowPack.WindowSource window)
        {
            //Thread.Sleep(1000);
            //window.ReOpen();
            CounterFrame f = new CounterFrame(ScreenAction.GetContentShot(window), 50);
            f.Show();
            f.FinishCount++;
        }

        //public void addText(string s)
        //{
        //    Application.Current.Dispatcher.Invoke((ThreadStart)delegate
        //    {
        //        input.Text += s + "\n";
        //    }
        //    );
        //}

        //public void AddNum(int num)
        //{
        //    Application.Current.Dispatcher.Invoke((ThreadStart)delegate
        //    {
        //        input.Text = Convert.ToString(Convert.ToInt32(input.Text) + num);
        //    }
        //    );
        //}

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
