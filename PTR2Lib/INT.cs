using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PTR2Lib
{
    public class INT
    {
        public class SectionInfo
        {
            public long fileCount;
            public long folderID;
            public long infoOffset;
            public long dataOffset;
            public long dataSize;
            private long magicLocation;
            public string folderName;

            public List<INTFile> files = new List<INTFile>();

            public SectionInfo(object[] info, object[][] presetFiles, string name)
            {
                fileCount = (long)info[0];
                folderID = (long)info[1];
                infoOffset = (long)info[2];
                dataOffset = (long)info[3];
                dataSize = (long)info[4];
                magicLocation = (long)info[5];
                folderName = name;
                addFiles(presetFiles);
            }

            public void addFiles(object[][] data)
            {
                foreach (object[] subData in data)
                {
                    files.Add(new INTFile(subData, this));
                }
            }
        }

        public class INTFile
        {
            public SectionInfo section;
            public long dataOffset;
            public long nameOffset;
            public long size;
            public string name;

            public INTFile(object[] info, SectionInfo parent)
            {
                section = parent;
                dataOffset = (long)info[0];
                nameOffset = (long)info[1];
                size = (long)info[2];
                name = (string)info[3];
            }
        }

        public List<SectionInfo> infos = new List<SectionInfo>();
        public List<INTFile> files = new List<INTFile>();
        private Stream intf;
        private byte[] inta;
        public string intp;
        private int BUFF_OFF;
        private readonly byte[] magic = { 0x11, 0x22, 0x33, 0x44 };
        private readonly string[] folders = { null, "Textures", "Sounds", "Stage Props", "Red Hat", "Blue Hat", "Pink Hat", "Yellow Hat" };

        public INT(string path)
        {
            intf = File.Open(path, FileMode.Open);
            intp = path;
            inta = new byte[intf.Length];
            intf.Read(inta, 0, (int)intf.Length); //COPY THE ENTIRETY MY BOYO
            intf.Seek(0, SeekOrigin.Begin);
            getSections();
        }

        public INT(Stream stream)
        {
            intf = stream;
            inta = new byte[intf.Length];
            intf.Read(inta, 0, (int)intf.Length); //COPY THE ENTIRETY MY BOYO
            intf.Seek(0, SeekOrigin.Begin);
            getSections();
        }

        /*protected virtual MemoryStream Decompress(Stream input)
        {
            BinaryReader reader = new BinaryReader(input);

            /// Check LZ77 type.
            if (reader.ReadByte() != 0x10)
                //throw new ArgumentException("Input stream does not contain LZ77-compressed data.", "input");]
                //Console.Write("e");

            // Read the size.
            int size = reader.ReadUInt16() | (reader.ReadByte() << 16);

            // Create output stream.
            MemoryStream output = new MemoryStream(size);

            // Begin decompression.
            while (output.Length < size)
            {
                // Load flags for the next 8 blocks.
                int flagByte = reader.ReadByte();

                // Process the next 8 blocks.
                for (int i = 0; i < 8; i++)
                {
                    // Check if the block is compressed.
                    if ((flagByte & (0x80 >> i)) == 0)
                    {
                        // Uncompressed block; copy single byte.
                        output.WriteByte(reader.ReadByte());
                    }
                    else
                    {
                        // Compressed block; read block.
                        ushort block = reader.ReadUInt16();
                        // Get byte count.
                        int count = ((block >> 4) & 0xF) + 3;
                        // Get displacement.
                        int disp = ((block & 0xF) << 8) | ((block >> 8) & 0xFF);

                        // Save current position and copying position.
                        long outPos = output.Position;
                        long copyPos = output.Position - disp - 1;

                        // Copy all bytes.
                        for (int j = 0; j < count; j++)
                        {
                            // Read byte to be copied.
                            output.Position = copyPos++;
                            byte b = (byte)output.ReadByte();

                            // Write byte to be copied.
                            output.Position = outPos++;
                            output.WriteByte(b);
                        }
                    }

                    // If all data has been decompressed, stop.
                    if (output.Length >= size)
                    {
                        break;
                    }
                }
            }

            output.Position = 0;
            return output;
        }

        protected virtual bool IsFileLocked(FileInfo file)  // https://stackoverflow.com/questions/876473
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }*/

        protected virtual byte[] read(int len, int off)
        {
            intf.Seek(off, SeekOrigin.Begin);
            BUFF_OFF = off + len;
            byte[] buf = new byte[len];
            intf.Read(buf, 0, len);
            foreach (byte x in buf)
            {
                //Console.Write(x + " ");
            }
            //Console.WriteLine();
            return buf;
        }

        protected virtual long getLong(int off)
        {
            return BitConverter.ToInt32(read(4, off), 0);
        }

        protected virtual string getString(int off)
        {
            intf.Seek(off, SeekOrigin.Begin);
            List<string> str = new List<string>();
            List<byte> strB = new List<byte>();
            while (true)
            {
                //ASCIIEncoding ascii = new ASCIIEncoding();
                byte[] strBuff = new byte[1];
                intf.Read(strBuff, 0, 1);
                if (strBuff[0] == 0x00)
                {
                    return Encoding.Default.GetString(strB.ToArray());
                }
                strB.Add(strBuff[0]);
            }
        }

        private void getSections()
        {
            BUFF_OFF = 4;
            int BASE_OFF = 4;
            for (int e = 0; e < 7; e++)
            {
                if (BUFF_OFF == inta.Length - 28)
                {
                    break;
                }

                int INT_SIZE = (int)intf.Length;
                long DATA_OFF = 0;
                object[] sectionInfo = new object[6];
                while (DATA_OFF < INT_SIZE)
                {
                    BASE_OFF = BUFF_OFF;
                    sectionInfo[5] = (long)BUFF_OFF;
                    sectionInfo[0] = getLong(BUFF_OFF);
                    sectionInfo[1] = getLong(BUFF_OFF);
                    if ((long)sectionInfo[1] == 0)
                    {
                        e = 900;
                        sectionInfo = null;
                        break;
                    }
                    sectionInfo[2] = getLong(BUFF_OFF) - 4 + BASE_OFF;
                    sectionInfo[3] = getLong(BUFF_OFF) + (long)sectionInfo[2];
                    sectionInfo[4] = getLong(BUFF_OFF);
                    BUFF_OFF += 8;
                    if ((long)sectionInfo[0] != 0)
                    {
                        long FILES = (long)sectionInfo[0];
                        object[][] fileList = new object[FILES][];
                        long[] A = new long[FILES];
                        long[] B = new long[FILES];
                        long[] C = new long[FILES];
                        string[] D = new string[FILES];
                        object[][] itemData = new object[FILES][];
                        for (int i = 0; i < FILES; i++)
                        {
                            long TOFFSET = getLong(BUFF_OFF);
                            //Console.WriteLine(TOFFSET);
                            A[i] = TOFFSET /*+ DATA_OFF*/;
                        }
                        BUFF_OFF = (int)(long)sectionInfo[2];
                        for (int i = 0; i < FILES; i++)
                        {
                            long NAME_OFF = getLong(BUFF_OFF);
                            long TSIZE = getLong(BUFF_OFF); //temp size?
                            //Console.WriteLine(BUFF_OFF + "     E" + i + " " + TSIZE + " " + NAME_OFF);
                            B[i] = NAME_OFF;
                            C[i] = TSIZE;
                        }
                        long OFFSET = BUFF_OFF;
                        for (int i = 0; i < FILES; i++)
                        {
                            long NAME_OFF = B[i];
                            NAME_OFF += OFFSET; //temp size?
                            string NAME = getString((int)NAME_OFF);
                            D[i] = NAME;
                        }

                        for (int i = 0; i < FILES; i++)
                        {
                            object[] list = new object[4]
                            {
                                A[i],
                                B[i],
                                C[i],
                                D[i]
                            };
                            fileList[i] = list;
                        }
                        SectionInfo section = new SectionInfo(sectionInfo, fileList, folders[(int)(long)sectionInfo[1]]);
                        foreach (INTFile file in section.files)
                        {
                            files.Add(file);
                        }
                        infos.Add(section);
                        e++;
                        BUFF_OFF = (int)section.dataOffset + 4 + (int)section.dataSize;
                    }
                }
            }
        }

        public Dictionary<SectionInfo, List<INTFile>> sectionsToDict()
        {
            Dictionary<SectionInfo, List<INTFile>> dict = new Dictionary<SectionInfo, List<INTFile>>();
            foreach (SectionInfo info in infos)
            {
                dict.Add(info, info.files);
            }
            return dict;
        }

        public void extract(string extFolder, bool timeoutSkip = false)
        {
            int OFFSET = 0;
            int INT_SIZE = (int)intf.Length;
            foreach (SectionInfo info in infos)
            {
                long DATA_OFF = info.dataOffset;
                BUFF_OFF = (int)DATA_OFF;
                long SIZE = getLong(BUFF_OFF);
                long ZSIZE = getLong(BUFF_OFF);
                OFFSET = BUFF_OFF;
                /*hoo boy
                 the clog
                 cloggy boy
                 hmmmmmmmmmmmmmmmm*/
                intf.Seek(DATA_OFF, SeekOrigin.Begin);
                /*byte[] MEMORY_FILE = new byte[ZSIZE];
                intf.Read(MEMORY_FILE, 0, (int)ZSIZE);
                //COPY THE ENTIRETY MY BOYO*/
                //byte[] MEMORY_FILE = inta; //please help
                byte[] MEMORY_FILE = new byte[ZSIZE];
                //byte[] TEMP_FILE = new byte[ZSIZE];
                //intf.Read(TEMP_FILE, 0, (int)ZSIZE);
                intf.Read(MEMORY_FILE, 0, (int)ZSIZE);
                //Stream TEMP_STREAM = new MemoryStream(TEMP_FILE);
                //TEMP_STREAM = Decompress(TEMP_STREAM);
                //TEMP_STREAM.Read(MEMORY_FILE, 0, (int)SIZE);
                foreach (INTFile file in info.files)
                {
                    OFFSET = (int)file.dataOffset;
                    SIZE = file.size;
                    string NAME = file.name;
                    string FNAME = string.Format(@"\{0}\{1}", info.folderName, NAME);
                    //the big EXPORT TIME
                    //Console.WriteLine(extFolder + FNAME);
                    if (!Directory.Exists(extFolder + @"\" + info.folderName))
                    {
                        Directory.CreateDirectory(extFolder + @"\" + info.folderName);
                    }
                    //Console.WriteLine(OFFSET + " " + SIZE);
                    Stream EXPORT;
                    try
                    {
                        EXPORT = File.Open(extFolder + FNAME, FileMode.CreateNew);
                    }
                    catch (IOException)
                    {
                        EXPORT = File.Open(extFolder + FNAME, FileMode.Truncate);
                    };
                    //Console.WriteLine(NAME);
                    byte[] WRITTEN_FILE = new byte[SIZE];
                    EXPORT.Read(WRITTEN_FILE, (int)OFFSET, (int)SIZE);
                    FileInfo fl = new FileInfo(extFolder + FNAME);
                    int r = 0;
                    bool c = false;
                    long osz = 0;
                    if (timeoutSkip)
                    {
                        while (fl.Length != SIZE)
                        {
                            fl.Refresh();
                            r++;
                            if (fl.Length != osz)
                            {
                                r = 0;
                            }

                            if (r > 100000)
                            {
                                //Console.WriteLine(SIZE + " " + OFFSET);
                                c = true;
                                break; //move on, continue to attempt to write in bg
                            }
                        }
                    }
                    if (!c)
                    {
                        EXPORT.Close();
                    }
                }
            }
            ////Console.WriteLine(getLong(skipZeroes()));
        }
    }
}