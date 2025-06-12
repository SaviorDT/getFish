using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
using 撈金魚.FileManager;

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
        private System.Windows.Point mouse_down_point;

        private static byte[] img_data;
        private static int img_w;
        private static int img_h;
        private static int bytes_per_pixel;
        private static int stride;
        public MouseInputWindow(FastBitmap img, UserInput.MouseInput input_type)
        {
            InitializeComponent();

            IntPtr ptr = img.BaseBitmap.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            ImgDisplay.Image.Source = bitmapSource;

            img_w = bitmapSource.PixelWidth;
            img_h = bitmapSource.PixelHeight;
            bytes_per_pixel = (bitmapSource.Format.BitsPerPixel + 7) / 8;
            stride = img_w * bytes_per_pixel;

            img_data = new byte[stride * img_h];
            bitmapSource.CopyPixels(img_data, stride, 0);

            img.Dispose();

            this.input_type = input_type;
            GotInput = false;

            ImgDisplay.Image.MouseMove += MouseMoveAction;
            ImgDisplay.Image.MouseDown += MouseDownAction;
            ImgDisplay.Image.MouseUp += MouseUpAction;
        }
        public System.Windows.Media.Color GetPixelColor(int x, int y)
        {
            if (img_data == null)
                return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            if (x < 0 || x >= img_w || y < 0 || y >= img_h)
                return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            int index = (y * stride) + (x * bytes_per_pixel);

            byte b = img_data[index];
            byte g = img_data[index + 1];
            byte r = img_data[index + 2];
            byte a = img_data[index + 3];

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        private void MouseMoveAction(object sender, MouseEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(ImgDisplay.Image);
            int x = (int)(point.X * ImgDisplay.Image.Source.Width / ImgDisplay.ActualWidth);
            int y = (int)(point.Y * ImgDisplay.Image.Source.Height / ImgDisplay.ActualHeight);

            System.Windows.Media.Color color = GetPixelColor(x, y);

            string info_str = $"X: {x:D3}, Y: {y:D3}, R: {color.R:D3}, G: {color.G:D3}, B: {color.B:D3}, code: {color}";

            InfoText.Text = info_str;
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

        private void ClosingAction(object sender, CancelEventArgs _)
        {
            //Not to block the waiting thread
            GotInput = true;
            LastClickX = -1;
            LastClickY = -1;
        }
    }
}
