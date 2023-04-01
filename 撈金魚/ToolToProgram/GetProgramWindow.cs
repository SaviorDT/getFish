using System.Diagnostics;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;

namespace 撈金魚
{
    internal class GetProgramWindow
    {

        public readonly Process[] processes;
        public WindowRect[] Rects_of_client { get; private set; }

        public GetProgramWindow(string process_name)
        {
            processes = Process.GetProcessesByName(process_name);
            processes = Validate_processes(processes);
            UpdateRect();
        }
        public void UpdateRect()
        {
            Rects_of_client = ProgramAttributes.GetContentRect(processes);
        }
        public void UpdateRect(int index)
        {
            Rects_of_client[index] = ProgramAttributes.GetContentRect(processes[index]);
            //AnalyzeNet.refreshRadius(100 * rect.width / MainWindow.MOLE_W);
        }

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
