using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
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

namespace 撈金魚.UserInterface
{
    /// <summary>
    /// CounterFrame.xaml 的互動邏輯
    /// </summary>
    public partial class CounterFrame : Window
    {
        private System.Windows.Point MouseDownPoint = new(0, 0);
        private int totalCount = 0;
        private int finishCount = 0;
        internal int FinishCount
        {
            get => finishCount;
            set
            {
                finishCount = value;
                counter.Text = finishCount + "/" + totalCount;
            }
        }
        internal int TotalCount
        {
            get => totalCount;
            set
            {
                totalCount = value;
                counter.Text = finishCount + "/" + totalCount;
            }
        }
        public CounterFrame(FastBitmap img, int totalCount)
        {
            InitializeComponent();

            IntPtr ptr = img.BaseBitmap.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            ImgDisplay.Source = bitmapSource;
            img.Dispose();

            ImgDisplay.MouseDown += ImgMouseDown;
            ImgDisplay.MouseMove += ImgMouseMove;
            ImgDisplay.MouseWheel += ImgMouseWheel;

            this.TotalCount = totalCount;
            counter.Text = "0/" + totalCount;
        }
        private void ImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownPoint = e.GetPosition(ImgDisplay);
        }
        private void ImgMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point mouse_pos = e.GetPosition(ImgDisplay);

                TransformGroup new_transform = (TransformGroup)ImgDisplay.RenderTransform.CloneCurrentValue();
                TranslateTransform translate = new_transform.Children[0] as TranslateTransform;

                translate.X += mouse_pos.X - MouseDownPoint.X;
                translate.Y += mouse_pos.Y - MouseDownPoint.Y;

                ImgDisplay.RenderTransform = new_transform;
            }
        }
        private void ImgMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double delta_scale = e.Delta / 1000.0;
            System.Windows.Point mouse_pos = e.GetPosition(ImgDisplay);
            TransformGroup new_transform = (TransformGroup)ImgDisplay.RenderTransform.CloneCurrentValue();
            TranslateTransform translate = new_transform.Children[0] as TranslateTransform;
            ScaleTransform scale = new_transform.Children[1] as ScaleTransform;

            //Let the mouse still on the same point
            translate.X = (mouse_pos.X + translate.X) / (1 + delta_scale) - mouse_pos.X;
            translate.Y = (mouse_pos.Y + translate.Y) / (1 + delta_scale) - mouse_pos.Y;

            // change scale
            scale.ScaleX *= (1 + delta_scale);
            scale.ScaleY *= (1 + delta_scale);

            ImgDisplay.RenderTransform = new_transform;
        }
    }
}
