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
            this.NotReadFile = false;
        }
        /// <summary>
        /// Записывает файл в папку Path, добавляя необходимые подкаталоги
        /// </summary>
        /// <param name="Path"> Папка, в которую необходимо записать файл/папку </param>
        public string WriteFile(string Path)
        {
            Path = this.PathChanger(Path);
            if (IsFolder)
            {
                this.ToDirectory(Path);
            }
            else
            {
                this.ToFile(Path);
            }
            return Path;
        }
        private void ToFile(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                Console.WriteLine("Except: Файл по пути: {0} уже существует", Path);
                return;
            }
            
            System.IO.FileInfo FI = new System.IO.FileInfo(Path);

            try
            {
                FI.Create().Close();
            }
            catch
            {
                Console.WriteLine("Except: Файл по пути: {0} невозможно создать", Path);
                NotReadFile = true;
                return;
            }

            FI.CreationTime = this.FileCreationTime;
            FI.LastAccessTime = this.FileLastAccessTime;
            FI.LastWriteTime = this.FileLastWriteTime;

            FI.Attributes = System.IO.FileAttributes.Normal;

            //FI.Name = this.FileName;
            //FI.DirectoryName = this.FileDirectoryName;
            //FI.Length = this.FileLength;
        }
        private void ToDirectory(string Path)
        {
            var a = string.Concat(System.IO.Path.GetInvalidFileNameChars());
            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Path);
            DI.Create();

            DI.CreationTime = this.FileCreationTime;
            DI.LastAccessTime = this.FileLastAccessTime;
            DI.LastWriteTime = this.FileLastWriteTime;

            DI.Attributes = System.IO.FileAttributes.Directory;

            //DI.Name = this.FileName;
            //DI.DirectoryName = this.FileDirectoryName;
        }
        public void SetAttribs(string Path)
        {
            Path = this.PathChanger(Path);
            if (this.IsFolder)
            {
                System.IO.DirectoryInfo FI = new System.IO.DirectoryInfo(Path);
                FI.Create();
                FI.Attributes = this.FileAttributes;
            }
            else
            {
                if (!System.IO.File.Exists(Path))
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                    using (System.IO.FileStream FS = FI.Create())
                    {
                        if (FS != null)
                        {
                            FS.Close();
                        }
                    }
                    FI.Attributes = this.FileAttributes;
                }
            }
        }
        private string PathChanger(string Path)
        {
            if (this.FileDirectoryName == "")
            {
                Path += System.IO.Path.DirectorySeparatorChar + this.FileName;
            }
            else
            {
                Path += System.IO.Path.DirectorySeparatorChar + this.FileDirectoryName + System.IO.Path.DirectorySeparatorChar + this.FileName;
            }
            Path = string.Concat(Path.Where(x => x != System.IO.Path.VolumeSeparatorChar));
            Console.WriteLine(Path);
            return Path;
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
        public bool Rep;
        public byte RepI;
    }
}
