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
            int StartTime = Environment.TickCount;

            if (args.Count() == 2)
            {
                ///Разархивирует файл 0 в папку 1
                if (System.IO.File.Exists(args[0]))
                {
                    Console.WriteLine(args[1]);
                    System.IO.Directory.CreateDirectory(args[1]);
                    System.Threading.Tasks.Task Methods = new System.Threading.Tasks.Task(() =>
                    {
                        DLA.Archive_reader buff = new DLA.Archive_reader(args[0], args[1]);
                    });
                    Methods.Start();
                    Methods.Wait();

                    StartTime = Environment.TickCount - StartTime;

                    Console.WriteLine("С момента начала разархивации прошло {0}min {1}sec {2}ms", (int)(StartTime / 60000), (int)((StartTime % 60000) / 1000), StartTime % 1000);

                    Console.ReadKey();
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