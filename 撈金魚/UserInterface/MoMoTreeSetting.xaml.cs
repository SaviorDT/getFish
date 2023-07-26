using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 撈金魚.ActionPerform;
using 撈金魚.FileManager;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.UserInterface
{
    public partial class MoMoTreeSetting : Window
    {
        private readonly GetProgramWindow windows;
        private FileManager.MoMoTreeSettings settings;
        public bool DoWater
        {
            get { return water.IsChecked ?? false; }
            set => water.IsChecked = value;
        }
        public bool DoFertilize
        {
            get { return fertilize.IsChecked ?? false; }
            set => fertilize.IsChecked = value;
        }
        public int MoMoX { get; private set; }
        public int MoMoY { get; private set; }
        public int EmptyX { get; private set; }
        public int EmptyY { get; private set; }

        public MoMoTreeSetting(GetProgramWindow windows, MoMoTreeSettings settings)
        {
            InitializeComponent();
            this.windows = windows;
            this.settings = settings;
            MoMoX = MoMoY = EmptyX = EmptyY = 0;
            if (settings.ReadFile)
            {
                SetPara();
            }
        }
        private void SetPara()
        {
            use_save.IsChecked = settings.ReadFile;
            DoWater = settings.Para.Water;
            DoFertilize = settings.Para.Fertilize;
            MoMoX = settings.Para.Tree_x;
            MoMoY = settings.Para.Tree_y;
            EmptyX = settings.Para.Empty_x;
            EmptyY = settings.Para.Empty_y;
        }
        public MoMoTreePara GetPara()
        {
            return new MoMoTreePara(DoWater, DoFertilize, MoMoX, MoMoY, EmptyX, EmptyY);
        }
        void ClosingAction(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void SetMomoTreePosClick(object sender, RoutedEventArgs e)
        {
            windows.UpdateRect();
            WindowSource random_window = null;
            foreach (WindowSource tmp in  windows.Windows.Values)
            {
                if(!tmp.Actioning && tmp.IsEnable)
                {
                    random_window = tmp;
                    break;
                }
            }
            if(random_window == null)
            {
                Message.ShowMessageToUser("沒有空閒中的視窗", "錯誤");
                return;
            }
            new Thread(() =>
            {
                do
                {
                    FastBitmap random_img = ScreenAction.GetContentShot(random_window);
                    Message.ShowMessageToUser("請點擊圖片中的毛毛樹\n" +
                        "如果你發現程式本來好好的\n" +
                        "點著點著就點不到毛毛樹了\n" +
                        "就代表那個位置會被施肥的鏟子擋住\n" +
                        "需要重新指定一個更好的位置");
                    (MoMoX, MoMoY) = UserInput.GetMouseInput(random_img, UserInput.MouseInput.LeftClick);// disposed in MouseInputWindow
                    MouseInput.MouseClickForContent(random_window, MoMoX, MoMoY);
                } while (!Message.ShowYesNoToUser("請確認毛毛樹是否被點開"));
                MouseInput.MouseClickForMole(random_window, 569, 115);
                do
                {
                    FastBitmap random_img = ScreenAction.GetContentShot(random_window);
                    Message.ShowMessageToUser("請點擊一個遠離毛毛樹\n" +
                        "並且可以走過去的位置");
                    (EmptyX, EmptyY) = UserInput.GetMouseInput(random_img, UserInput.MouseInput.LeftClick);// disposed in MouseInputWindow
                    MouseInput.MouseClickForContent(random_window, EmptyX, EmptyY);
                } while (!Message.ShowYesNoToUser("請確認摩爾是否移動"));
                Message.ShowMessageToUser("設定成功");
            }).Start();
        }

        private void Save_settings_Click(object sender, RoutedEventArgs e)
        {
            settings.ReadFile = use_save.IsChecked ?? false;
            settings.Para = GetPara();
            Hide();
        }
    }
}
