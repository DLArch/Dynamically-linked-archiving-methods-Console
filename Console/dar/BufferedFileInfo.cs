using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dar
{
    public class BufferedFileInfo
    {
        public System.IO.FileInfo ToFileInfo(string Path)
        {
            System.IO.FileInfo FI = new System.IO.FileInfo(Path);

            FI.Attributes = this.FileAttributes;
            FI.CreationTime = this.FileCreationTime;
            FI.LastAccessTime = this.FileLastAccessTime;
            FI.LastWriteTime = this.FileLastWriteTime;
            //FI.Name = this.FileName;
            //FI.DirectoryName = this.FileDirectoryName;
            //FI.Length = this.FileLength;

            return FI;
        }
        public System.IO.DirectoryInfo ToDirectoryInfo(string Path)
        {
            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Path);

            DI.Attributes = this.FileAttributes;
            DI.CreationTime = this.FileCreationTime;
            DI.LastAccessTime = this.FileLastAccessTime;
            DI.LastWriteTime = this.FileLastWriteTime;
            //DI.Name = this.FileName;
            //DI.DirectoryName = this.FileDirectoryName;

            return DI;
        }
        public System.IO.FileAttributes FileAttributes
        {
            get;
            set;    
        }
        public DateTime FileCreationTime
        {
            get;
            set;
        }
        public DateTime FileLastAccessTime
        {
            get;
            set;
        }
        public DateTime FileLastWriteTime
        {
            get;
            set;
        }
        public string FileName
        {
            get;
            set;
        }
        public string FileDirectoryName
        {
            get;
            set;
        }
        public Int64 FileLength
        {
            get;
            set;
        }
    }
}
