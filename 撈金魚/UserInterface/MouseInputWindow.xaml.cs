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
        private Point mouse_down_point;
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

            ImgDisplay.Image.MouseDown += MouseDownAction;
            ImgDisplay.Image.MouseUp += MouseUpAction;
        }

        private void MouseDownAction(object sender, MouseButtonEventArgs e)
        {
            mouse_down_point = e.GetPosition(this);
        }

        private void MouseUpAction(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).Equals(mouse_down_point))
            {
                LastClickX = (int)(ImgDisplay.MouseDownPoint.X * ImgDisplay.Image.Source.Width / ImgDisplay.ActualWidth);
                LastClickY = (int)(ImgDisplay.MouseDownPoint.Y * ImgDisplay.Image.Source.Height / ImgDisplay.ActualHeight);

                if (input_type == UserInput.MouseInput.LeftClick)
                {
                    GotInput = true;
                }
            }
        }
    }
}
