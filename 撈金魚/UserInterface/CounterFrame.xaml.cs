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
            ImgDisplay.Image.Source = bitmapSource;
            img.Dispose();

            this.TotalCount = totalCount;
            counter.Text = "0/" + totalCount;
        }
    }
}
