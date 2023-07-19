using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 撈金魚.FileManager
{
    internal class FileActionPerformer
    {
        public static void SaveTextFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
        public static string LoadTextFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            } catch(Exception)
            {
                return "";
            }
        }
    }
}
