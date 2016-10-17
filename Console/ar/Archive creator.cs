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
        public Archive_creator(string Spath, string Apath = @"%Desctop%\Arch0.dla", UInt16 Method = 0)
        {
            init(Apath, Method);
            Create_Archive(Spath);
        }
        /// <summary>
        /// Инициализирует поля класса
        /// </summary>
        /// <param name="path"></param>
        private void init(string path, UInt16 method)
        {
            this.ArchPath = string.Concat(path.Except(path.Split('.')[path.Split('.').Length - 1])) + Archive_creator.Extension;
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
            OStream.Write(Count_Of_Files(Spath) + 1);
            OStream.Close();

            System.IO.FileStream StreamOfCreatedFile = new System.IO.FileStream(this.ArchPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
            System.IO.BinaryWriter BinFileWriter = new System.IO.BinaryWriter(StreamOfCreatedFile);

            BinFileWriter.Write(this.MethodIndex);

            Compress(Spath, BinFileWriter);
        }
        private void Compress(string path, System.IO.BinaryWriter BinFileWriter)
        {
            if (System.IO.File.Exists(path))
            {
                byte[] buff;

                System.IO.FileStream StreamOfBaseFile;

                try
                {
                    StreamOfBaseFile = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                }
                catch
                {
                    return;
                }

                buff = new byte[StreamOfBaseFile.Length];
                StreamOfBaseFile.ReadAsync(buff, 0, (int)StreamOfBaseFile.Length);

                BinFileWriter.BaseStream.WriteAsync(buff, 0, buff.Length);

                Console.WriteLine((byte)new System.IO.FileInfo(path).Attributes);
                Console.WriteLine("-------------");
                foreach (var z in System.IO.File.ReadAllLines(path))
                {
                    Console.WriteLine(z);
                }
                Console.WriteLine("-------------");

                var ForTests = true;
                var OneHex = 0;
                foreach (var z in buff)
                {
                    if (ForTests)
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
                    }
                }
            }
            else
            {
                foreach (var x in System.IO.Directory.EnumerateFileSystemEntries(path))
                {
                    Compress(x, BinFileWriter);
                }
            }
        }
        /// <summary>
        /// Расширение для архива
        /// </summary>
        public const string Extension = @".dla";
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