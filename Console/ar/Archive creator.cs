using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ar
{
    public class Archive_creator
    {
        /// <summary>
        /// Создает новый архив
        /// </summary>
        /// <param name="Spath"></param>
        /// <param name="Apath"></param>
        /// <param name="Method"></param>
        public Archive_creator(string Spath, string Apath = @"789987", UInt16 Method = 0)
        {
            Console.WriteLine(Apath);
            init(Apath, Method);
            Create_Archive(Spath);
        }
        public Archive_creator(string Spath, System.IO.BinaryWriter Wr, string Apath = "789987", UInt16 Method = 0)
        {
            this.ArchPath = Apath;
            this.MethodIndex = Method;
            foreach (var z in System.IO.Directory.EnumerateFileSystemEntries(Spath))
            {
                Compress(z, Wr);
            }
        }
        /// <summary>
        /// Инициализирует поля класса
        /// </summary>
        /// <param name="path"></param>
        private void init(string path, UInt16 method)
        {
            if (path == "789987")
            {
                path = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + System.IO.Path.DirectorySeparatorChar + @"Arch0.dla";
                Console.WriteLine(path);
            }
            this.ArchPath = string.Concat(path.Take(path.Length - (string.Concat(path.Reverse().TakeWhile(x => x != '.')) + '.').Length)) + Archive_creator.Extension;
            Console.WriteLine(this.ArchPath);
            Console.ReadKey();
            this.MethodIndex = method;
        }
        /// <summary>
        /// Подсчитывает количиство каталогов в указанной папке
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int Count_Of_Files(string path)
        {
            int Count_Of_Files_ = 0;
            if (System.IO.Directory.Exists(path))
            {
                foreach (var e in System.IO.Directory.EnumerateFileSystemEntries(path))
                {
                    try
                    {
                        if (System.IO.Directory.Exists(e))
                        {
                            Count_Of_Files_ += Count_Of_Files(e);
                        }
                    }
                    catch
                    {

                    }
                    ++Count_Of_Files_;
                }
            }
            return Count_Of_Files_;
        }
        /// <summary>
        /// Создает и заполняет архивный файл
        /// </summary>
        /// <param name="Spath"></param>
        /// <param name="Apath"></param>
        private void Create_Archive(string Spath)
        {
            System.IO.FileStream CreatedFile = System.IO.File.Create(this.ArchPath);

            System.IO.File.SetAttributes(this.ArchPath, System.IO.File.GetAttributes(this.ArchPath) | System.IO.FileAttributes.Archive | System.IO.FileAttributes.ReparsePoint | System.IO.FileAttributes.Compressed);

            CreatedFile.Close();
            System.IO.StreamWriter OStream = new System.IO.StreamWriter(this.ArchPath);
            /// Запись количества файлов в архив
            OStream.Write(Count_Of_Files(Spath));
            OStream.Close();

            System.IO.FileStream StreamOfCreatedFile = new System.IO.FileStream(this.ArchPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
            System.IO.BinaryWriter BinFileWriter = new System.IO.BinaryWriter(StreamOfCreatedFile);

            //BinFileWriter.Write(this.MethodIndex);

            Compress(Spath, BinFileWriter);
        }
        private void Compress(string path, System.IO.BinaryWriter BinFileWriter)
        {

            try
            {
                System.IO.FileStream StreamOfLogFile = new System.IO.FileStream(Archive_creator.Log, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                var BuffStream = new System.IO.BinaryWriter(StreamOfLogFile);
                BuffStream.Write(path.ToCharArray());
                BuffStream.Write(Environment.NewLine);
                BuffStream.Close();
                StreamOfLogFile.Close();
            }
            catch
            {
                ;
            }


            if (System.IO.File.Exists(path))
            {
                byte[] buff;

                System.IO.FileStream StreamOfBaseFile;

                if (path != this.ArchPath)
                {
                    try
                    {
                        StreamOfBaseFile = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    }
                    catch
                    {
                        try
                        {
                            System.IO.FileStream StreamOfLogFile = new System.IO.FileStream(Archive_creator.Log, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                            var BuffStream = new System.IO.BinaryWriter(StreamOfLogFile);
                            BuffStream.Write(" |!!!Except of access to file: " + path + "!!!|");
                            BuffStream.Write(Environment.NewLine);
                            BuffStream.Close();
                            StreamOfLogFile.Close();
                        }
                        catch
                        {
                            Console.WriteLine("Невозможно записать событие");
                        }
                        Console.WriteLine("Невозможно получить доступ к файлу: " + path);
                        Console.WriteLine("Продолжить работы с ошибкой [y/n]");
                        if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                        {
                            return;
                        }
                        else
                        {
                            throw new Exception("Программа остановлена по запросу пользователя");
                        }
                    }
                }
                else
                {
                    return;
                }
                
                System.IO.FileInfo FileAttrib = new System.IO.FileInfo(path);
                BinFileWriter.Write(this.MethodIndex);
                BinFileWriter.Write('|' + FileAttrib.DirectoryName + '|');
                BinFileWriter.Write(FileAttrib.Name + '|');
                BinFileWriter.Write(FileAttrib.Attributes.ToString() + '|');
                BinFileWriter.Write(FileAttrib.LastAccessTimeUtc.ToString() + '|');
                BinFileWriter.Write(FileAttrib.LastWriteTimeUtc.ToString() + '|');
                BinFileWriter.Write(FileAttrib.Length.ToString() + '|');

                buff = new byte[StreamOfBaseFile.Length];

                List<Task> Tasks;

                Tasks = new List<Task>();
                /// Не хочет работать :(
                /*Tasks.Add(new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    StreamOfBaseFile.ReadAsync(buff, 0, (int)StreamOfBaseFile.Length);
                    BinFileWriter.BaseStream.WriteAsync(buff, 0, buff.Length);
                }));*/

                Console.WriteLine((byte)new System.IO.FileInfo(path).Attributes);
                Console.WriteLine("-------------");
                foreach (var z in System.IO.File.ReadAllLines(path))
                {
                    Console.WriteLine(z);
                }
                Console.WriteLine("-------------");

                // -- Task.WaitAll(Tasks.ToArray());

                StreamOfBaseFile.ReadAsync(buff, 0, (int)StreamOfBaseFile.Length);
                BinFileWriter.BaseStream.WriteAsync(buff, 0, buff.Length);

                /*var ForTests = true;
                var OneHex = 0;*/
                foreach (var z in buff)
                {
                    Console.Write(z.ToString() + '|');
                    /*if (ForTests)
                    {
                        Console.Write(' ');
                    }
                    Console.Write(z);
                    ForTests = !ForTests;
                    if (OneHex >= (16 -1))
                    {
                        OneHex = 0;
                        Console.Write("||");
                        ForTests = true;
                    }
                    else
                    {
                        ++OneHex;
                    }*/
                }
            }
            else
            {
                new Archive_creator(path, BinFileWriter, this.ArchPath, this.MethodIndex);
                //foreach (var x in System.IO.Directory.EnumerateFileSystemEntries(path))
                //{
                //    Compress(x, BinFileWriter);
                //}
            }
        }
        /// <summary>
        /// Расширение для архива
        /// </summary>
        public const string Extension = @".dla";
        public const string Log = @"Log.dla";
        /// <summary>
        /// Путь к архиву
        /// </summary>
        public string ArchPath
        {
            get;
            set;
        }
        public UInt16 MethodIndex
        {
            get;
            set;
        }
    }
}