using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dar
{
    public class BufferedFileInfo
    {
        public BufferedFileInfo()
        {
            this.IsFolder = true;
        }
        /// <summary>
        /// Записывает файл в папку Path, добавляя необходимые подкаталоги
        /// </summary>
        /// <param name="Path"> Папка, в которую необходимо записать файл/папку </param>
        public string WriteFile(string Path)
        {
            if (this.FileDirectoryName == "")
            {
                Path += System.IO.Path.DirectorySeparatorChar + this.FileName;
            }
            else
            {
                Path += System.IO.Path.DirectorySeparatorChar + this.FileDirectoryName + System.IO.Path.DirectorySeparatorChar + this.FileName;
            }
            Console.WriteLine(Path);
            if (IsFolder)
            {
                this.ToFile(Path);
            }
            else
            {
                this.ToDirectory(Path);
            }
            return Path;
        }
        private void ToFile(string Path)
        {
            System.IO.FileInfo FI = new System.IO.FileInfo(Path);
            var CreatedFileStream = FI.Create();
            CreatedFileStream.Close();

            FI.Attributes = this.FileAttributes;
            FI.CreationTime = this.FileCreationTime;
            FI.LastAccessTime = this.FileLastAccessTime;
            FI.LastWriteTime = this.FileLastWriteTime;
            //FI.Name = this.FileName;
            //FI.DirectoryName = this.FileDirectoryName;
            //FI.Length = this.FileLength;
        }
        private void ToDirectory(string Path)
        {
            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Path);
            DI.Create();

            DI.Attributes = this.FileAttributes;
            DI.CreationTime = this.FileCreationTime;
            DI.LastAccessTime = this.FileLastAccessTime;
            DI.LastWriteTime = this.FileLastWriteTime;
            //DI.Name = this.FileName;
            //DI.DirectoryName = this.FileDirectoryName;
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
        /// <summary>
        /// Является ли объект папкой true = файл
        /// </summary>
        public bool IsFolder
        {
            get;
            set;
        }
    }
}
