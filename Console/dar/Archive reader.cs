using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dar
{
    public class Archive_reader
    {
        public Archive_reader(string ArchivePath, string DestinationPath)
        {
            System.IO.FileStream StreamOfAr = new System.IO.FileStream(ArchivePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //System.IO.StreamReader StreamOfAr = new System.IO.StreamReader(ArchivePath);
            System.IO.BinaryReader BinFileReader = new System.IO.BinaryReader(StreamOfAr);

            Environment.CurrentDirectory = DestinationPath;

            BufferedFileInfo FileInfo = new BufferedFileInfo();

            this.MakeFileFromArchive(DestinationPath, BinFileReader, FileInfo);

            BinFileReader.Close();
            StreamOfAr.Close();
        }
        public void MakeFileFromArchive(string Path, System.IO.BinaryReader BinFileReader, BufferedFileInfo FileInfo)
        {
            Console.WriteLine(@"-------------//-----------||-----------\\--------------");
            char buff;
            
            FileInfo.IsFolder = true;

            buff = BinFileReader.ReadChar();
            this.Method = BinFileReader.ReadInt16();                                                                                ///Метод

            buff = BinFileReader.ReadChar();
            Console.WriteLine(buff);
            FileInfo.FileAttributes = (System.IO.FileAttributes)BinFileReader.ReadInt32();                                          ///Атрибуты

            buff = BinFileReader.ReadChar();
            Console.WriteLine(buff);
            FileInfo.FileCreationTime = new DateTime(BinFileReader.ReadInt64());                                                    ///Дата создания файла
            buff = BinFileReader.ReadChar();
            Console.WriteLine(buff);
            FileInfo.FileLastAccessTime = new DateTime(BinFileReader.ReadInt64());                                                  ///Дата последнего доступа
            buff = BinFileReader.ReadChar();
            Console.WriteLine(buff);
            FileInfo.FileLastWriteTime = new DateTime(BinFileReader.ReadInt64());                                                   ///Дата последней записи
            buff = BinFileReader.ReadChar();
            Console.WriteLine(buff);

            /// <summary>
            /// Понять почему при прочтении первый символ из строки с именем странен.
            /// </summary>
            buff = BinFileReader.ReadChar();
            FileInfo.FileName = "";
            for (buff = BinFileReader.ReadChar(); buff != FileNameDelim; buff = BinFileReader.ReadChar())
            {
                FileInfo.FileName += buff;
            }
            Console.WriteLine("Name: {0}; Last char: {1}", FileInfo.FileName, buff);

            buff = BinFileReader.ReadChar();
            FileInfo.FileDirectoryName = "";
            for (buff = BinFileReader.ReadChar(); buff != FileNameDelim; buff = BinFileReader.ReadChar())
            {
                FileInfo.FileDirectoryName += buff;
            }
            Console.WriteLine("DirectoryName: {0}; Last char: {1}", FileInfo.FileDirectoryName, buff);

            buff = BinFileReader.ReadChar();
            Console.WriteLine(buff);
            if (buff != FileNameDelim)
            {
                --BinFileReader.BaseStream.Position;
                FileInfo.FileLength = BinFileReader.ReadInt64();
            }
            else
            {
                FileInfo.IsFolder = false;
                FileInfo.FileLength = 0;
            }

            string FileAbsolutePath = FileInfo.WriteFile(Path);

            if (FileInfo.IsFolder)
            {
                buff = BinFileReader.ReadChar();

                System.IO.FileStream FileStream = System.IO.File.Open(FileAbsolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Write);

                byte ByteBuff;

                for (Int64 Counter = 0; Counter < FileInfo.FileLength; ++Counter)
                {
                    ByteBuff = BinFileReader.ReadByte();
                    FileStream.WriteByte(ByteBuff);
                }

                FileStream.Close();
            }

            if (BinFileReader.BaseStream.Length > 0)
            {
                MakeFileFromArchive(Path, BinFileReader, FileInfo);
            }
        }
        public void DirectoryCreator(string Path, System.IO.BinaryReader BinFileReader, System.IO.FileAttributes Attribs)
        {
            char buff;

            System.IO.DirectoryInfo DirInfo = new System.IO.DirectoryInfo(Path);

            DirInfo.Attributes = Attribs;
            
            buff = BinFileReader.ReadChar();
            DirInfo.CreationTime = new DateTime(BinFileReader.ReadInt64());                      ///Время создания
            buff = BinFileReader.ReadChar();
            DirInfo.LastAccessTime = new DateTime(BinFileReader.ReadInt64());                    ///Время последнего доступа
            buff = BinFileReader.ReadChar();
            DirInfo.LastWriteTime = new DateTime(BinFileReader.ReadInt64());                     ///Время последней записи
            buff = BinFileReader.ReadChar();
        }
        public void FileCreator(string Path, System.IO.BinaryReader BinFileReader, System.IO.FileAttributes Attribs)
        {
            char buff;
            System.IO.FileInfo CurFileInfo = new System.IO.FileInfo(Path);

            CurFileInfo.Attributes = Attribs;

            buff = BinFileReader.ReadChar();
            CurFileInfo.CreationTime = new DateTime(BinFileReader.ReadInt64());                      ///Время создания
            buff = BinFileReader.ReadChar();
            CurFileInfo.LastAccessTime = new DateTime(BinFileReader.ReadInt64());                    ///Время последнего доступа
            buff = BinFileReader.ReadChar();
            CurFileInfo.LastWriteTime = new DateTime(BinFileReader.ReadInt64());                     ///Время последней записи
            buff = BinFileReader.ReadChar();
        }
        public Int16 Method
        {
            get;
            set;
        }
        public const char FileNameDelim = '|';
    }
}
