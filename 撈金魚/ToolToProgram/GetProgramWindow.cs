using System.Diagnostics;
using System.Windows.Markup;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;

namespace 撈金魚
{
    internal class GetProgramWindow
    {

        private readonly string process_name;
        internal Process[] Processes { get; private set; }
        internal WindowRect[] Rects_of_client { get; private set; }

        public GetProgramWindow(string process_name)
        {
            this.process_name = process_name;
            UpdateRect();
        }
        public void UpdateRect()
        {
            Processes = Process.GetProcessesByName(process_name);
            Processes = Validate_processes(Processes);
            Rects_of_client = ProgramAttributes.GetContentRect(Processes);
        }
        public void UpdateRect(int index)
        {
            Rects_of_client[index] = ProgramAttributes.GetContentRect(Processes[index]);
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
