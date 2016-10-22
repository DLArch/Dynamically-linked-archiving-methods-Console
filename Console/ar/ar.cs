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
            if (args != null && args.Count() > 0)
            {
                int StartTime = Environment.TickCount;

                ///Архивирует файл/папку 1 в файл 2
                if (System.IO.File.Exists(args[0]) || System.IO.Directory.Exists(args[0]))
                {
                    Archive_creator Archive;
                    System.Threading.Tasks.Task Methods = new System.Threading.Tasks.Task(() =>
                    {
                        switch (args.Count())
                        {
                            case 1:
                                Archive = new Archive_creator(args[0]);
                                break;
                            case 2:
                                Archive = new Archive_creator(args[0], args[1]);
                                break;
                            case 3:
                                Archive = new Archive_creator(args[0], args[1], UInt16.Parse(args[2]));
                                break;
                            default:
                                arHelpWanted();
                                break;
                        }
                    });
                    Methods.Start();
                    Methods.Wait();

                    StartTime = Environment.TickCount - StartTime;

                    Console.WriteLine("С момента начала архивации прошло {0}min {1}sec {2}ms", (int)(StartTime / 60000), (int)(StartTime / 1000), StartTime % 1000);
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
            Console.WriteLine(@"[PathBase] [PathDest = %Desktop%\Arch0.dla] [Method:int = 0]");
            Console.WriteLine("Zip Folder/File [PathBase] into archive [PathDest] with method [Method]");
            Console.WriteLine("Упаковывает папку/файл [PathBase] в архив [PathDest] используя метод [Method]");
        }
    }
}