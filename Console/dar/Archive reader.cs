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
            
            System.Environment.CurrentDirectory += System.IO.Path.DirectorySeparatorChar + DestinationPath;

            BufferedFileInfo FileInfo = new BufferedFileInfo();

            this.MakeFileFromArchive("", BinFileReader, FileInfo);

            BinFileReader.Close();
            StreamOfAr.Close();
        }
        public void MakeFileFromArchive(string Path, System.IO.BinaryReader BinFileReader, BufferedFileInfo FileInfo)
        {
            FileInfo.IsFolder = false;

            ++BinFileReader.BaseStream.Position;                                                                        ///|
            this.Method = BinFileReader.ReadInt16();                                                                    ///|Method
            ++BinFileReader.BaseStream.Position;                                                                        ///|Method|
            FileInfo.FileAttributes = (System.IO.FileAttributes)BinFileReader.ReadInt32();                              ///|Method|Атрибуты
            ++BinFileReader.BaseStream.Position;                                                                        ///|Method|Атрибуты|
            FileInfo.FileCreationTime = new DateTime(BinFileReader.ReadInt64());                                        ///|Method|Атрибуты|Д
            ++BinFileReader.BaseStream.Position;                                                                        ///|Method|Атрибуты|Д|
            FileInfo.FileLastAccessTime = new DateTime(BinFileReader.ReadInt64());                                      ///|Method|Атрибуты|Д|Д
            ++BinFileReader.BaseStream.Position;                                                                        ///|Method|Атрибуты|Д|Д|
            FileInfo.FileLastWriteTime = new DateTime(BinFileReader.ReadInt64());                                       ///|Method|Атрибуты|Д|Д|Д
            ++BinFileReader.BaseStream.Position;                                                                        ///|Method|Атрибуты|Д|Д|Д|

            ++BinFileReader.BaseStream.Position;
            FileInfo.FileName = "";
            for (this.buff = BinFileReader.ReadChar(); this.buff != Archive_reader.FileNameDelim; this.buff = BinFileReader.ReadChar())
            {
                FileInfo.FileName += this.buff;
            }                                                                                                           ///|Method|Атрибуты|Д|Д|Д|-Name|

            ++BinFileReader.BaseStream.Position;
            FileInfo.FileDirectoryName = "";
            for (this.buff = BinFileReader.ReadChar(); this.buff != Archive_reader.FileNameDelim; this.buff = BinFileReader.ReadChar())
            {
                FileInfo.FileDirectoryName += this.buff;
            }                                                                                                           ///|Method|Атрибуты|Д|Д|Д|-Name|-Path|

            this.buff = BinFileReader.ReadChar();
            if (this.buff == Archive_reader.FileNameDelim)
            {
                FileInfo.IsFolder = true;                                                                               ///|Method|Атрибуты|Д|Д|Д|-Name|-Path|-|
            }
            else
            {
                --BinFileReader.BaseStream.Position;
                FileInfo.FileLength = BinFileReader.ReadInt64();                                                        ///|Method|Атрибуты|Д|Д|Д|-Name|-Path|12345
                ++BinFileReader.BaseStream.Position;                                                                    ///|Method|Атрибуты|Д|Д|Д|-Name|-Path|12345|
            }
            
            FileInfo.MakeFile(Path);

            if (!FileInfo.IsFolder)
            {
                try
                {
                    using (System.IO.FileStream bFS = new System.IO.FileStream(System.Environment.CurrentDirectory + FileInfo.PathModifier(Path), System.IO.FileMode.Open, System.IO.FileAccess.Write))
                    {
                        for (FileInfo.PosBuff = 0; FileInfo.PosBuff < FileInfo.FileLength; ++FileInfo.PosBuff)
                        {
                            this.ByteBuff = BinFileReader.ReadByte();
                            bFS.WriteByte(this.ByteBuff);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Программа не может получить доступ к {0}", System.Environment.CurrentDirectory + FileInfo.PathModifier(Path));
                    BinFileReader.BaseStream.Position += FileInfo.FileLength;
                }
            }
            if (BinFileReader.BaseStream.Position < BinFileReader.BaseStream.Length)
            {
                MakeFileFromArchive(Path, BinFileReader, FileInfo);
            }
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
