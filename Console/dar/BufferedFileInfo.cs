using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dar
{
    public class BufferedFileInfo
    {
        public void MakeFile(string Path)
        {
            Path = PathModifier(Path);
            if (IsFolder)
            {
                FolderCreate(Path);
            }
            else
            {
                FileCreate(Path);
            }

            Console.WriteLine(Path);
        }
        public string PathModifier(string Path)
        {
            if (this.FileDirectoryName != "")
            {
                Path += System.IO.Path.DirectorySeparatorChar + this.FileDirectoryName + System.IO.Path.DirectorySeparatorChar + this.FileName;
            }
            else
            {
                Path += System.IO.Path.DirectorySeparatorChar + this.FileName;
            }

            return Path;
        }
        private void FolderCreate(string Path)
        {
            System.IO.Directory.CreateDirectory(System.Environment.CurrentDirectory + Path);
        }
        private void FileCreate(string Path)
        {
            System.IO.File.Create(System.Environment.CurrentDirectory + Path);
        }
        /// <summary>
        /// Атрибуты файла
        /// </summary>
        public System.IO.FileAttributes FileAttributes
        {
            get;
            set;
        }
        /// <summary>
        /// Время создания файла
        /// </summary>
        public DateTime FileCreationTime
        {
            get;
            set;
        }
        /// <summary>
        /// Время последнего доступа к фалу
        /// </summary>
        public DateTime FileLastAccessTime
        {
            get;
            set;
        }
        /// <summary>
        /// Время последней записи в файл
        /// </summary>
        public DateTime FileLastWriteTime
        {
            get;
            set;
        }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// Относительный путь к файлу
        /// </summary>
        public string FileDirectoryName
        {
            get;
            set;
        }
        /// <summary>
        /// Размер файла
        /// </summary>
        public Int64 FileLength
        {
            get;
            set;
        }
        public bool NotReadFile
        {
            get;
            set;
        }
        /// <summary>
        /// Является ли объект папкой true = папка
        /// </summary>
        public bool IsFolder
        {
            get;
            set;
        }
        public Int64 PosBuff
        {
            get;
            set;
        }
        public bool Rep;
        public byte RepI;
    }
}
