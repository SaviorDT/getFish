using Gma.System.MouseKeyHook;
using Hazdryx.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using 撈金魚.ActionPerform;
using 撈金魚.ActionPerform.Common;
using 撈金魚.ActionPerform.ElementKnight;
using 撈金魚.ActionPerform.FatHouse;
using 撈金魚.Analyzer;
using 撈金魚.FileManager;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;
using 撈金魚.UserInterface;
using static 撈金魚.ActionPerform.ButtonPerformer;
using static 撈金魚.ActionPerform.Common.MapMove;
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
        private AllSettings user_settings = UserSettings.Load();
        private readonly MoMoTreeSetting momo_tree;

        private void PlayFishButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(ActionKit.fish), ActionKit.fish, false);
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeGlobalHook();

            momo_tree = new MoMoTreeSetting(window, user_settings.Momo);
        }

        private void ClosingAction(object sender, CancelEventArgs _)
        {
            //the default closing action may not actually stop the program
            //Which only close the window and work in background until your threads are done.
            UserSettings.Save(user_settings);
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
                this.Close();
        }

        private void BuyFatNutrientButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(ActionKit.buy_nutrient), ActionKit.buy_nutrient);
        }

        private void ElementKnightButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(ActionKit.play_element_knight), ActionKit.play_element_knight);
        }

        private void ElementKnightKitButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, 0, ActionKit.element_knight_kit);
        }

        private void DragonButton(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(ActionKit.dragon), ActionKit.dragon);
        }
        private void MoMoTreeButton(object sender, RoutedEventArgs e)
        {
            window.UpdateRect();
            ButtonPerformer.PerformButton(window.Windows, GetLoopTimes(ActionKit.momo_tree), ActionKit.momo_tree, true, momo_tree.GetPara());
        }

        private void MoMoTreeSettingButton(object sender, RoutedEventArgs e)
        {
            momo_tree.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs _)
        {
            window.UpdateRect();
            testFun(window.Windows.Values.ToArray()[0]);
            
        }

        private void testFun(WindowPack.WindowSource window)
        {
            window.ReOpen();
            //MapMove.MapMoveTo(new MapPlace(MapType.Black_Forest, (int)PlaceInBlackForest.VineForest), window);

            Click.GoRestaurant(window);
            Wait.WaitForPlaceChange(window, 5000);
            MouseInput.MouseClickForMole(window, 39, 515);
            //ScreenAction.GetContentShot(window).Save("test/shot.png");
        }
        private int GetLoopTimes(ActionKit action)
        {
            int times = GetInputTimes();
            if (times == -2)
                return times;
            else if (times == -1)
            {
                return LoadCount(action);
            }
            else
            {
                return SaveCount(action, times);
            }
        }
        
        private int LoadCount(ActionKit action)
        {
            int times = action switch
            {
                ActionKit.fish => user_settings.Counts.GetFish,
                ActionKit.play_element_knight => user_settings.Counts.ElementKnight,
                ActionKit.buy_nutrient => user_settings.Counts.BuyFatNutrient,
                ActionKit.dragon => user_settings.Counts.Dragon,
                ActionKit.momo_tree => user_settings.Counts.MoMoTree,
                _ => 0,
            };
            input.Text = Convert.ToString(times);
            return times;
        }

        private int SaveCount(ActionKit action, int times)
        {
            switch (action)
            {
                case ActionKit.fish:
                    user_settings.Counts.GetFish = times;
                    break;
                case ActionKit.play_element_knight:
                    user_settings.Counts.ElementKnight = times;
                    break;
                case ActionKit.buy_nutrient:
                    user_settings.Counts.BuyFatNutrient = times;
                    break;
                case ActionKit.dragon:
                    user_settings.Counts.Dragon = times;
                    break;
                case ActionKit.momo_tree:
                    user_settings.Counts.MoMoTree = times;
                    break;
            }
            return times;
        }

        private int GetInputTimes()
        {
            if (input.Text.Length == 0)
            {
                return -1;
            }
            try
            {
                return Convert.ToInt32(input.Text);
            }
            catch (OverflowException)
            {
                UserInterface.Message.ShowMessageToUser("數字太大了哈囉", "錯誤");
            }
            catch (FormatException)
            {
                UserInterface.Message.ShowMessageToUser("這不是數字哈囉", "錯誤");
            }
            //this means invalid number
            return -2;
        }
    }
}
