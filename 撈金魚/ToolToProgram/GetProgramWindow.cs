using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Markup;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚
{
    internal class GetProgramWindow
    {

        private readonly string process_name;
        internal Dictionary<int, WindowSource> Windows = new();

        public GetProgramWindow(string process_name)
        {
            this.process_name = process_name;
            UpdateRect();
        }
        public void UpdateRect()
        {
            Process[] processes = Process.GetProcessesByName(process_name);
            //WindowRect[] rects_of_client;
            processes = Validate_processes(processes);
            //rects_of_client = ProgramAttributes.GetContentRect(processes);

            UpdateWindowsInfo(processes);
        }

        private void UpdateWindowsInfo(Process[] processes)
        {
            HashSet<int> new_ids = new();
            HashSet<int> delete_ids = new();
            foreach(Process p in processes)
            {
                new_ids.Add(p.Id);
                if (!Windows.ContainsKey(p.Id))
                {
                    Windows.Add(p.Id, new WindowSource(p));
                }
            }
            foreach(KeyValuePair<int,  WindowSource> w in Windows)
            {
                if(new_ids.Contains(w.Key))
                {
                    w.Value.UpdateRect();
                }
                else
                {
                    delete_ids.Add(w.Key);
                }
            }
            foreach(int id in delete_ids)
            {
                Windows.Remove(id);
            }
        }

        //public void UpdateRect(int index)
        //{
        //    Rects_of_client[index] = ProgramAttributes.GetContentRect(Processes[index]);
        //    //AnalyzeNet.refreshRadius(100 * rect.width / MainWindow.MOLE_W);
        //}

        private Process[] Validate_processes(Process[] processes)
        {
            int valid_count = 0;
            foreach (Process process in processes)
            {
                if (ProgramAttributes.IsValid(process))
                {
                    valid_count++;
                }
            }

            Process[] valid_process = new Process[valid_count];
            int copied_count = 0;
            foreach (Process process in processes)
            {
                if (ProgramAttributes.IsValid(process))
                {
                    valid_process[copied_count++] = process;
                }
            }

            return valid_process;
        }
    }
}
