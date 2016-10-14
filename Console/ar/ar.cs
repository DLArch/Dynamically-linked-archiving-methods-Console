using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ar
{
    class ar
    {
        static void Main(string[] args)
        {
            if (args.Count() == 2)
            {
                ///Архивирует файл/папку 1 в файл 2
                if (System.IO.File.Exists(args[0]) || System.IO.Directory.Exists(args[0]))
                {
                    System.IO.File.Create(args[1]);
                }
                else
                {
                    arHelpWanted();
                }
            }
            else
            {
                arHelpWanted();
            }
        }
        static void arHelpWanted()
        {
            Console.WriteLine("Zipping - ar:");
            Console.WriteLine("[PathBase] [PathDest]");
            Console.WriteLine("Zip Folder/File [PathBase] into archive [PathDest]");
            Console.WriteLine("Упаковывает папку/файл [PathBase] в архив [PathDest]");
        }
    }
}