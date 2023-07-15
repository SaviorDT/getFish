using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 撈金魚.UserInterface
{
    /// <summary>
    /// MouseInputWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MouseInputWindow : Window
    {
        public int LastClickX { get; private set; }
        public int LastClickY { get; private set; }
        public bool GotInput { get; private set; }
        private UserInput.MouseInput input_type;
        public MouseInputWindow(FastBitmap img, UserInput.MouseInput input_type)
        {
            InitializeComponent();

            IntPtr ptr = img.BaseBitmap.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            ImgDisplay.Image.Source = bitmapSource;
            img.Dispose();

            this.input_type = input_type;
            GotInput = false;

            ImgDisplay.Image.MouseUp += MouseUpAction;
        }

        private void MouseUpAction(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(ImgDisplay.Image).Equals(ImgDisplay.MouseDownPoint))
            {
                LastClickX = (int)ImgDisplay.MouseDownPoint.X;
                LastClickY = (int)ImgDisplay.MouseDownPoint.Y;
            }

            if (input_type == UserInput.MouseInput.LeftClick)
            {
                GotInput = true;
            }
        }
    }
}
