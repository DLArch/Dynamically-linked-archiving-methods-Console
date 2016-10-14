using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dla
{
    class dla
    {
        static void Main(string[] args)
        {
            foreach (var t in args)
            {
                Console.WriteLine(t);
            }
            if (args.Length == 0)
            {
                dlaHelpWanted();
            }
            else
            {
                Commands ResultICommand;
                if (Enum.TryParse(args[0], out ResultICommand))
                {
                    System.Diagnostics.ProcessStartInfo NewProcess = new System.Diagnostics.ProcessStartInfo(ResultICommand.ToString());
                    NewProcess.UseShellExecute = false;
                    NewProcess.Arguments = string.Concat(args.Where(x => x != ResultICommand.ToString()).Select(x => x + ' '));
                    System.Diagnostics.Process.Start(NewProcess);
                }
                else
                {
                    dlaHelpWanted();
                }
            }
        }
        static void dlaHelpWanted()
        {
            Console.WriteLine("Using dla: [Command] [PathBase] [PathDest] {Method}");
            Console.WriteLine("   1:  ar - Zipping");
            Console.WriteLine("   2: dar - Unzipping");
        }
    }
}
