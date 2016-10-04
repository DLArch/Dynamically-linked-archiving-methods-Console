using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dar
{
    class dar
    {
        static void Main(string[] args)
        {
            Console.Write("");
            if (args.Count() == 2)
            {
                if (System.IO.File.Exists(args[0]) && (!(System.IO.File.Exists(args[1]))))
                {
                    System.IO.Directory.CreateDirectory(args[1]);
                }
                else
                {
                    Rules();
                }
            }
            else
            {
                Rules();
            }
        }
        static void Rules()
        {
            char razd = '"';
            Console.WriteLine("Uncorrect: ");
            Console.WriteLine("dar [" + razd + "Path" + razd + "] [" + razd + "Path" + razd + "]");
        }
    }
}