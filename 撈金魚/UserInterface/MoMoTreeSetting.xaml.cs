﻿using Hazdryx.Drawing;
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
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.UserInterface
{
    public partial class MoMoTreeSetting : Window
    {
        private readonly GetProgramWindow windows;
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

        public MoMoTreeSetting(GetProgramWindow windows)
        {
            InitializeComponent();
            this.windows = windows;
            MoMoX = MoMoY = -1;
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
                    Message.ShowMessageToUser("請點擊圖片中的毛毛樹" +
                        "\n需要點擊不會被施肥的鏟子擋到的地方" +
                        "\n多試幾次就能抓到訣竅了");
                    (MoMoX, MoMoY) = UserInput.GetMouseInput(random_img, UserInput.MouseInput.LeftClick);// disposed in MouseInputWindow
                    MouseInput.MouseClickForContent(random_window, MoMoX, MoMoY);
                } while (!Message.ShowYesNoToUser("請確認毛毛樹是否被點開"));
                MouseInput.MouseClickForMole(random_window, 569, 115);
                do
                {
                    FastBitmap random_img = ScreenAction.GetContentShot(random_window);
                    Message.ShowMessageToUser("請點擊一個可以走過去的位置");
                    (EmptyX, EmptyY) = UserInput.GetMouseInput(random_img, UserInput.MouseInput.LeftClick);// disposed in MouseInputWindow
                    MouseInput.MouseClickForContent(random_window, EmptyX, EmptyY);
                } while (!Message.ShowYesNoToUser("請確認摩爾是否移動"));
                Message.ShowMessageToUser("設定成功");
            }).Start();
        }
    }
}
