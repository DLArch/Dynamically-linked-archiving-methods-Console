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
            if (args.Count() == 2)
            {
                ///Разархивирует файл 1 в папку 2
                if (System.IO.File.Exists(args[0]))
                {
                    Console.WriteLine(args[1]);
                    System.IO.Directory.CreateDirectory(args[1]);
                    Archive_reader buff = new Archive_reader(args[0], args[1]);
                }
                else
                {
                    darHelpWanted();
                }
            }
            else
            {
                darHelpWanted();
            }
            Console.ReadKey();
        }
        static void darHelpWanted()
        {
            Console.WriteLine("Unzipping - dar:");
            Console.WriteLine("[FileBase] [PathDest]");
            Console.WriteLine("Unzip file [FileBase] into directory [PathDest]");
            Console.WriteLine("Разархивирует содержимое файла [FileBase] в папку [PathDest]");
        }
    }
}