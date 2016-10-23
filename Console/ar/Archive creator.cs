using System;
using System.Linq;

namespace ar
{
    public class Archive_creator
    {
        /// <summary>
        /// Создает новый архив
        /// </summary>
        /// <param name="Spath"> Путь к файлам для архивации </param>
        /// <param name="Apath"> Путь к архиву </param>
        /// <param name="Method"> Номер метода для файлов </param>
        public Archive_creator(string Spath, string Apath = @"789987", UInt16 Method = 0)
        {
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
        /// <param name="path"> Путь для инициализации класса </param>
        /// <param name="method"> Номер метода для файлов </param>
        private void init(string path, UInt16 method)
        {
            if (path == "789987")
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + System.IO.Path.DirectorySeparatorChar + Archive_creator.DefaultArchiveName;
            }
            if (path.Where(x => x == DefaultExtensionDelimiter).Count() != 0)
            {
                this.ArchPath = string.Concat(path.Take(path.Length - (string.Concat(path.Reverse().TakeWhile(x => x != DefaultExtensionDelimiter)) + DefaultExtensionDelimiter).Length)) + Archive_creator.Extension;
            }
            else
            {
                this.ArchPath += Archive_creator.Extension;
            }
            this.MethodIndex = method;
        }
        /// <summary>
        /// Подсчитывает количиство файлов в указанной папке
        /// </summary>
        /// <param name="path"> Путь к папке/файлам для подсчета </param>
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
        /// <param name="Spath"> Путь к папке/файлам для архивации </param>
        private void Create_Archive(string Spath)
        {
            System.IO.FileStream CreatedFile = System.IO.File.Create(this.ArchPath);

            System.IO.File.SetAttributes(this.ArchPath, System.IO.File.GetAttributes(this.ArchPath) | System.IO.FileAttributes.Archive | System.IO.FileAttributes.ReparsePoint | System.IO.FileAttributes.Compressed);

            CreatedFile.Close();

            System.IO.FileStream StreamOfCreatedFile = new System.IO.FileStream(this.ArchPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
            System.IO.BinaryWriter BinFileWriter = new System.IO.BinaryWriter(StreamOfCreatedFile);

            Compress(Spath, BinFileWriter);

            BinFileWriter.Close();
            StreamOfCreatedFile.Close();
        }
        /// <summary>
        /// Сжимает папку/файл
        /// </summary>
        /// <param name="path"> Путь к файлу/папке, который[ую] необходимо сжать </param>
        /// <param name="BinFileWriter"> Поток записи в архив </param>
        private void Compress(string path, System.IO.BinaryWriter BinFileWriter)
        {

            try
            {
                System.IO.FileStream StreamOfLogFile = new System.IO.FileStream(Archive_creator.LogFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write);
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

            if (path == this.ArchPath || path == (AppDomain.CurrentDomain.BaseDirectory + this.ArchPath))
            {
                return;
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream StreamOfBaseFile;

                try
                {
                    StreamOfBaseFile = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                }
                catch
                {
                    try
                    {
                        System.IO.FileStream StreamOfLogFile = new System.IO.FileStream(Archive_creator.LogFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write);
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
                    var KeyPressed = Console.ReadKey().KeyChar;
                    if (KeyPressed == 'y' || KeyPressed == 'Y')
                    {
                        return;
                    }
                    else
                    {
                        throw new Exception("Программа остановлена по запросу пользователя");
                    }
                }
                
                MakeFileInArchive(path, BinFileWriter);

                /// <summary>
                /// Чтение файла из потока StreamOfBaseFile
                /// TODO: Ускорить чтение, путем чтения не одного байта, а набора байтов сразу
                /// Количество выделяемых байт под буффер должно определяться автоматически,
                /// в зависимости от количества доступной оперативной памяти
                /// </summary>
                byte Byte_Buff;

                for (Byte_Buff = 0; StreamOfBaseFile.Position < StreamOfBaseFile.Length;)
                {
                    Byte_Buff = (byte)StreamOfBaseFile.ReadByte();
                    BinFileWriter.Write(Byte_Buff);
                }
            }
            else
            {
                MakeFileInArchive(path, BinFileWriter);
                new Archive_creator(path, BinFileWriter, this.ArchPath, this.MethodIndex);
            }
        }
        /// <summary>
        /// Записывает файловую запись в архив
        /// </summary>
        /// <param name="path"> Путь к файлу/папке, свединия о котором необходимо занести в архив </param>
        /// <param name="BinFileWriter"> Поток записи в архив </param>
        public void MakeFileInArchive(string path, System.IO.BinaryWriter BinFileWriter)
        {
            System.IO.FileInfo FileAttrib = new System.IO.FileInfo(path);
            BinFileWriter.Write('|');
            BinFileWriter.Write(this.MethodIndex);
            BinFileWriter.Write('|');
            BinFileWriter.Write((Int32)FileAttrib.Attributes);
            BinFileWriter.Write('|');
            BinFileWriter.Write(FileAttrib.CreationTime.Ticks);
            BinFileWriter.Write('|');
            BinFileWriter.Write(FileAttrib.LastAccessTime.Ticks);
            BinFileWriter.Write('|');
            BinFileWriter.Write(FileAttrib.LastWriteTime.Ticks);
            BinFileWriter.Write('|');
            BinFileWriter.Write(FileAttrib.Name);
            BinFileWriter.Write('|');
            BinFileWriter.Write(FileAttrib.DirectoryName);
            BinFileWriter.Write('|');
            if (System.IO.File.Exists(path))
            {
                BinFileWriter.Write(FileAttrib.Length);
                BinFileWriter.Write('|');
            }
        }
        /// <summary>
        /// Разделитель расширения
        /// </summary>
        public const char DefaultExtensionDelimiter = '.';
        /// <summary>
        /// Расширение архива
        /// </summary>
        public const string Extension = @".dla";
        /// <summary>
        /// Путь и расширение log - файла
        /// </summary>
        public const string LogFileName = @"Log.dla";
        /// <summary>
        /// Стандартное название архива
        /// </summary>
        public const string DefaultArchiveName = @"Arch0.dla";
        /// <summary>
        /// Путь к архиву
        /// </summary>
        public string ArchPath
        {
            get;
            set;
        }
        /// <summary>
        /// Номер метода для сжатия. Если 0, система
        /// автоматически определяет наиболее подходящий
        /// </summary>
        public UInt16 MethodIndex
        {
            get;
            set;
        }
    }
}