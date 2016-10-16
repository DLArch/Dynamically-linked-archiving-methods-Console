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
        public Archive_creator(string Spath, string Apath = @"%Desctop%\Arch0.dla", int Method = 0)
        {
            init(Apath);
            Create_Archive(Spath);
        }
        /// <summary>
        /// Инициализирует поля класса
        /// </summary>
        /// <param name="path"></param>
        private void init(string path)
        {
            this.ArchPath = string.Concat(path.Except(path.Split('.')[path.Split('.').Length - 1])) + Archive_creator.Extension;
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
            OStream.Write(Count_Of_Files(Spath) + 1);
            OStream.Close();
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
    }
}