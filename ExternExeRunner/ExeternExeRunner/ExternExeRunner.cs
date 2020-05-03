using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ExeRunner
{
    public class ExternExeRunner
    {
        public static void Main(string[] args)
        { }

        // run an external exe designated by filename 
        // which takes args as arguments
        // return the exe's output
        public static string[] Run(string filename, string[] args)
        {
            // create an external process
            Process ExternExe = CreateNewProcess(filename);

            // run the process using args
            RunProcess(ExternExe, args);

            // get output
            string[] outputs = GetOutput(ExternExe);

            // exit the process
            ExitProcess(ExternExe);

            // return outputs
            return outputs.ToArray();
        }

        private static Process CreateNewProcess(string filename)
        {
            Process NewProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @filename,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };

            return NewProcess;
        }
        
        private static void RunProcess(Process proc, string[] args)
        {
            // run the extern process
            proc.Start();
            // input
            foreach (string arg in args)
            {
                proc.StandardInput.WriteLine(arg);
            }
        }
        
        private static string[] GetOutput(Process proc)
        {
            // get output
            List<string> outputs = new List<string>();
            while (!proc.StandardOutput.EndOfStream)
            {
                outputs.Add(proc.StandardOutput.ReadLine());
            }
            return outputs.ToArray();
        }
        
        private static void ExitProcess(Process proc)
        {
            proc.WaitForExit();
        }
    }
}
