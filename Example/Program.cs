using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sontx.Utils.Executor.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ProcessExecutor executor = new ProcessExecutor("notepad.exe");
            executor.ExecuteAndWaitAsync().Wait();
        }
    }
}