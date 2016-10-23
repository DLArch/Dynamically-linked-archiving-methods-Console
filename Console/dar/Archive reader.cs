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

            this.MakeFileFromArchive(DestinationPath, BinFileReader);

            BinFileReader.Close();
            StreamOfAr.Close();
        }
        public void MakeFileFromArchive(string Path, System.IO.BinaryReader BinFileReader)
        {
            /*byte[] hh = new byte[BinFileReader.BaseStream.Length];
            long len = BinFileReader.BaseStream.Length - 1;
            BinFileReader.BaseStream.Read(hh, 0, (int)len);
            foreach (var u in hh)
            {
                Console.Write(u.ToString() + '|');
            }*/
            char buff;
            
            buff = BinFileReader.ReadChar();
            this.Method = BinFileReader.ReadInt16();                                                ///Method
            buff = BinFileReader.ReadChar();
            var FileAttributes = (System.IO.FileAttributes)BinFileReader.ReadInt32();               ///Атрибуты
            Console.WriteLine(FileAttributes);
            //Console.WriteLine((System.IO.FileAttributes)BinFileReader.ReadInt32());

            System.IO.DirectoryInfo FileAttrib = new System.IO.DirectoryInfo(Path);

            FileAttrib.Attributes = FileAttributes;

            FileAttrib.Create();

            buff = BinFileReader.ReadChar();
            FileAttrib.CreationTime = new DateTime(BinFileReader.ReadInt64());                      ///Время создания
            buff = BinFileReader.ReadChar();
            FileAttrib.LastAccessTime = new DateTime(BinFileReader.ReadInt64());                    ///Время последнего доступа
            buff = BinFileReader.ReadChar();
            FileAttrib.LastWriteTime = new DateTime(BinFileReader.ReadInt64());                     ///Время последней записи
            buff = BinFileReader.ReadChar();

            /*if ((FileAttributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
            {
                Console.WriteLine("Dir");
                this.DirectoryCreator(System.IO.Directory.CreateDirectory(Path), Path, BinFileReader);
            }
            else
            {
                System.IO.File.Create(Path);
                this.FileCreator(Path, BinFileReader);
            }*/
            

            //BinFileReader.ReadString Считать имя
            buff = BinFileReader.ReadChar();
            //buff = BinFileReader.ReadChar();
            //Считать путь
            //BinFileReader.(FileAttrib.DirectoryName);
            ///Вылетело при попытке чтения символа табуляции(Но это не точно. Пытался распарсить запись о папке)
            //buff = BinFileReader.ReadChar();
            //buff = BinFileReader.ReadChar();
        }
        public void DirectoryCreator(System.IO.DirectoryInfo DirInfo, string Path, System.IO.BinaryReader BinFileReader)
        {
            char buff;
            buff = BinFileReader.ReadChar();
            DirInfo.CreationTime = new DateTime(BinFileReader.ReadInt64());                      ///Время создания
            buff = BinFileReader.ReadChar();
            DirInfo.LastAccessTime = new DateTime(BinFileReader.ReadInt64());                    ///Время последнего доступа
            buff = BinFileReader.ReadChar();
            DirInfo.LastWriteTime = new DateTime(BinFileReader.ReadInt64());                     ///Время последней записи
            buff = BinFileReader.ReadChar();
        }
        public void FileCreator(string Path, System.IO.BinaryReader BinFileReader)
        {
            char buff;
            System.IO.FileInfo CurFileInfo = new System.IO.FileInfo(Path);
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
    }
}
