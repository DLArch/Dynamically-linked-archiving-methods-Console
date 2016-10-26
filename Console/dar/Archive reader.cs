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
            System.IO.BinaryReader BinFileReader = new System.IO.BinaryReader(StreamOfAr);

            BufferedFileInfo FileInfo = new BufferedFileInfo();

            this.MakeFileFromArchive(DestinationPath, BinFileReader, FileInfo);

            BinFileReader.Close();
            StreamOfAr.Close();
        }
        public void MakeFileFromArchive(string Path, System.IO.BinaryReader BinFileReader, BufferedFileInfo FileInfo)
        {            
            FileInfo.IsFolder = true;
            
            ++BinFileReader.BaseStream.Position;

            this.Method = BinFileReader.ReadInt16();                                                                                ///Метод
            ++BinFileReader.BaseStream.Position;

            FileInfo.FileAttributes = (System.IO.FileAttributes)BinFileReader.ReadInt32();                                          ///Атрибуты
            ++BinFileReader.BaseStream.Position;

            FileInfo.FileCreationTime = new DateTime(BinFileReader.ReadInt64());                                                    ///Дата создания файла
            ++BinFileReader.BaseStream.Position;

            FileInfo.FileLastAccessTime = new DateTime(BinFileReader.ReadInt64());                                                  ///Дата последнего доступа
            ++BinFileReader.BaseStream.Position;

            FileInfo.FileLastWriteTime = new DateTime(BinFileReader.ReadInt64());                                                   ///Дата последней записи
            ++BinFileReader.BaseStream.Position;

            /// Считаны |meth|attrib|Cdate|Adate|Wdate|
            
            ++BinFileReader.BaseStream.Position;

            FileInfo.FileName = "";
            for (buff = BinFileReader.ReadChar(); buff != FileNameDelim; buff = BinFileReader.ReadChar())
            {
                FileInfo.FileName += buff;
            }

            /// Считаны |meth|attrib|Cdate|Adate|Wdate|-FileName|

            ++BinFileReader.BaseStream.Position;

            FileInfo.FileDirectoryName = "";
            for (buff = BinFileReader.ReadChar(); buff != FileNameDelim; buff = BinFileReader.ReadChar())
            {
                FileInfo.FileDirectoryName += buff;
            }

            /// Считаны |meth|attrib|Cdate|Adate|Wdate|-FileName|-DirName|

            //FileInfo.FileDirectoryName = this.FileNameFix(FileInfo.FileDirectoryName);

            ByteBuff = BinFileReader.ReadByte();
            if (ByteBuff != FileNameDelim)
            {
                --BinFileReader.BaseStream.Position;
                FileInfo.FileLength = BinFileReader.ReadInt64();
                ++BinFileReader.BaseStream.Position;
                FileInfo.IsFolder = false;

                /// Считаны |meth|attrib|Cdate|Adate|Wdate|-FileName|-DirName|12345678|
            }
            else
            {
                FileInfo.FileLength = 0;

                /// Считаны |meth|attrib|Cdate|Adate|Wdate|-FileName|-DirName||
            }
            
            string FileAbsolutePath = FileInfo.WriteFile(Path);

            if (!FileInfo.IsFolder)
            {
                using (System.IO.FileStream FileStream = System.IO.File.Open(FileAbsolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Write))
                {
                    for (Int64 Counter = 0; Counter < FileInfo.FileLength; ++Counter)
                    {
                        ByteBuff = BinFileReader.ReadByte();
                        FileStream.WriteByte(ByteBuff);
                    }
                    FileStream.Close();
                }
            }

            FileInfo.SetAttribs(Path);

            if (BinFileReader.BaseStream.Position < BinFileReader.BaseStream.Length)
            {
                MakeFileFromArchive(Path, BinFileReader, FileInfo);
            }
        }
        public string FileNameFix(string Path)
        {
            Console.WriteLine(System.IO.Path.VolumeSeparatorChar);
            Console.WriteLine(System.IO.Path.DirectorySeparatorChar);
            Path = string.Concat(Path.Except(string.Concat(System.IO.Path.VolumeSeparatorChar)));
            Console.WriteLine("Path: {0}", Path);

            return Path;
        }
        /// <summary>
        /// Символьный буфер
        /// </summary>
        public char buff
        {
            get;
            set;
        }
        /// <summary>
        /// Байтовый буфер. Используется
        /// для снятия данных из архива
        /// </summary>
        public byte ByteBuff
        {
            get;
            set;
        }
        /// <summary>
        /// Номер метода обрабатываемого файла
        /// </summary>
        public Int16 Method
        {
            get;
            set;
        }
        /// <summary>
        /// Разделитель в файловой записи архива
        /// </summary>
        public const char FileNameDelim = '|';
    }
}
