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
        internal WindowSource[] Windows = new WindowSource[0];

        public GetProgramWindow(string process_name)
        {
            this.process_name = process_name;
            UpdateRect();
        }
        public void UpdateRect()
        {
            Process[] processes = Process.GetProcessesByName(process_name);
            WindowRect[] rects_of_client;
            processes = Validate_processes(processes);
            rects_of_client = ProgramAttributes.GetContentRect(processes);

            UpdateWindowsInfo(processes, rects_of_client);
        }

        private void UpdateWindowsInfo(Process[] processes, WindowRect[] rects_of_client)
        {
            HashSet<int> actioning_proccesses = new();
            foreach(WindowSource window in Windows)
            {
                if (window.Actioning)
                {
                    actioning_proccesses.Add(window.Process.Id);
                }
            }
            Windows = new WindowSource[processes.Length];
            for(int i=0; i<processes.Length; i++)
            {
                Windows[i] = new WindowSource(processes[i], rects_of_client[i]);
                if (actioning_proccesses.Contains(processes[i].Id))
                {
                    Windows[i].Actioning = true;
                }
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
