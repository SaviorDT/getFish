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
        private readonly MoMoTreeSetting momo_tree;

        private void PlayFishButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(), ActionKit.fish, false);
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeGlobalHook();

            momo_tree = new MoMoTreeSetting(window);
        }

        private void ClosingAction(object sender, CancelEventArgs _)
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

        private void BuyFatNutrientButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(), ActionKit.buy_nutrient);
        }

        private void ElementKnightButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(), ActionKit.play_element_knight);
        }

        private int GetLoopTimes()
        {
            if(input.Text.Length == 0) return 0;
            try
            {
                return Convert.ToInt32(input.Text);
            } catch(OverflowException)
            {
                UserInterface.Message.ShowMessageToUser("數字太大了哈囉", "錯誤");
            } catch (FormatException)
            {
                UserInterface.Message.ShowMessageToUser("這不是數字哈囉", "錯誤");
            }
            //this means invalid number
            return -2;
        }

        private void ElementKnightKitButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, 0, ActionKit.element_knight_kit);
        }

        private void DragonButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(), ActionKit.dragon);
        }

        private void Button_Click(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            testFun(window.Windows[0]);
        }

        private void testFun(WindowPack.WindowSource window)
        {
            FastBitmap shot = ScreenAction.GetContentShot(window);
            shot.Save("test.png");
        }

        private void MoMoTreeButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(), ActionKit.momo_tree, false, momo_tree.GetPara());
        }

        private void MoMoTreeSettingButton(object sender, RoutedEventArgs e)
        {
            momo_tree.Show();
        }
    }
}
