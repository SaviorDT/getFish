using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 撈金魚.UserInterface
{
    internal class UserInput
    {
        internal static string SelectFile()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.CheckFileExists = true;
            file.ShowDialog();

            return file.FileName;
        }
    }
}
