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
                Console.WriteLine("Using dla: [Command] [PathBase] [PathDest]");
                Console.WriteLine("   1:  ar - Zipping");
                Console.WriteLine("   2: dar - Unzipping");
            }
        }
        void dlaHelpWanted()
        {
            Console.WriteLine("Using dla: [Command] [PathBase] [PathDest]");
        }
    }
}
