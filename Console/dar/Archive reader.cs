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
            System.IO.StreamReader StreamOfAr = new System.IO.StreamReader(ArchivePath);
            System.IO.BinaryReader BinFileReader = new System.IO.BinaryReader(StreamOfAr.BaseStream);

            MakeFileInArchive(ArchivePath, DestinationPath, BinFileReader);

            BinFileReader.Close();
            StreamOfAr.Close();
        }
        public void MakeFileInArchive(string ArchivePath, string DestinationPath, System.IO.BinaryReader BinFileReader)
        {
            char buff;
            System.IO.FileInfo FileAttrib = new System.IO.FileInfo(DestinationPath);
            buff = BinFileReader.ReadChar();
            this.Method = BinFileReader.ReadInt16();                                            ///Method
            buff = BinFileReader.ReadChar();
            FileAttrib.Attributes = (System.IO.FileAttributes)BinFileReader.ReadInt32();        ///Атрибуты
            buff = BinFileReader.ReadChar();
            try
            {
                FileAttrib.CreationTime = new DateTime(BinFileReader.ReadInt64());                  ///Время создания
                buff = BinFileReader.ReadChar();
                FileAttrib.LastAccessTime = new DateTime(BinFileReader.ReadInt64());                ///Время последнего доступа
                buff = BinFileReader.ReadChar();
                FileAttrib.LastWriteTime = new DateTime(BinFileReader.ReadInt64());                 ///Время последней записи
                buff = BinFileReader.ReadChar();
            }
            catch
            {
                Console.WriteLine("Ошибка чтения дат!");
            }
            //BinFileReader.ReadString Считать имя
            buff = BinFileReader.ReadChar();
            //Считать путь
            //BinFileReader.(FileAttrib.DirectoryName);
            ///Вылетело при попытке чтения символа табуляции(Но это не точно. Пытался распарсить запись о папке)
            //buff = BinFileReader.ReadChar();
            if ((FileAttrib.Attributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
            {
                Console.WriteLine("Dir");
                //BinFileReader.Write(FileAttrib.Length);
                //BinFileReader.Write('|');
            }
        }
        public Int16 Method
        {
            get;
            set;
        }
    }
}
