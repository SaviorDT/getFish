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

        public MoMoTreeSetting(GetProgramWindow windows)
        {
            InitializeComponent();
            this.windows = windows;
            MoMoX = MoMoY = -1;
        }
        public MoMoTreePara GetPara()
        {
            return new MoMoTreePara(DoWater, DoFertilize, MoMoX, MoMoY);
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
            FastBitmap random_img = ScreenAction.GetContentShot(windows.Windows.First().Value);
            new Thread(() =>
            {
                (MoMoX, MoMoY) = UserInput.GetMouseInput(random_img, UserInput.MouseInput.LeftClick);// disposed in MouseInputWindow
            }).Start();
        }
    }
}
