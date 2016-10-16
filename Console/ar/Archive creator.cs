using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ar
{
    public class Archive_creator
    {
        public Archive_creator(string Spath, string Apath, string Method)
        {
            System.Console.WriteLine(Count_Of_Files(Spath));
            Create_Archive(Spath, Apath);
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
                    if (System.IO.Directory.Exists(e))
                    {
                        Count_Of_Files_ += Count_Of_Files(e);
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
        private void Create_Archive(string Spath, string Apath)
        {
            Apath = string.Concat(Apath.TakeWhile(x => x != '.')) + @".dla";
            System.IO.FileStream CreatedFile = System.IO.File.Create(Apath);

            System.IO.File.SetAttributes(Apath, System.IO.File.GetAttributes(Apath) | System.IO.FileAttributes.Archive | System.IO.FileAttributes.ReparsePoint | System.IO.FileAttributes.Compressed);

            CreatedFile.Close();
            System.IO.StreamWriter OStream = new System.IO.StreamWriter(Apath);
            OStream.Write(Count_Of_Files(Spath) + 1);
            OStream.Close();
        }
    }
}