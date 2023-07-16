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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 撈金魚.UserInterface
{
    /// <summary>
    /// IImageMonitor.xaml 的互動邏輯
    /// </summary>
    public partial class ImageMonitor : UserControl
    {
        private Point mouse_down_point = new(0, 0);
        public Point MouseDownPoint
        {
            get { return mouse_down_point; }
            private set { mouse_down_point = value; }
        }
        public ImageMonitor()
        {
            InitializeComponent();

            Image.MouseDown += ImgMouseDown;
            Image.MouseMove += ImgMouseMove;
            Image.MouseWheel += ImgMouseWheel;
        }
        private void ImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownPoint = e.GetPosition(Image);
        }
        private void ImgMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mouse_pos = e.GetPosition(Image);

                TransformGroup new_transform = (TransformGroup)Image.RenderTransform.CloneCurrentValue();
                TranslateTransform translate = new_transform.Children[0] as TranslateTransform;

                translate.X += mouse_pos.X - MouseDownPoint.X;
                translate.Y += mouse_pos.Y - MouseDownPoint.Y;

                Image.RenderTransform = new_transform;
            }
        }
        private void ImgMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double delta_scale = e.Delta / 1000.0;
            Point mouse_pos = e.GetPosition(Image);
            TransformGroup new_transform = (TransformGroup)Image.RenderTransform.CloneCurrentValue();
            TranslateTransform translate = new_transform.Children[0] as TranslateTransform;
            ScaleTransform scale = new_transform.Children[1] as ScaleTransform;

            //Let the mouse still on the same point
            translate.X = (mouse_pos.X + translate.X) / (1 + delta_scale) - mouse_pos.X;
            translate.Y = (mouse_pos.Y + translate.Y) / (1 + delta_scale) - mouse_pos.Y;

            // change scale
            scale.ScaleX *= (1 + delta_scale);
            scale.ScaleY *= (1 + delta_scale);

            Image.RenderTransform = new_transform;
        }
    }
}
