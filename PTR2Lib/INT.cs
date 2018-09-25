using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTR2Lib
{
    public class INT
    {
        private Stream intf;
        public string intp;
        private readonly byte[] magic = { 0x11, 0x22, 0x33, 0x44 };
        private readonly string[] folders = { "TEXTURES", "SOUNDS", "TEXTURES", "HAT_RED", "HAT_BLUE", "HAT_PINK", "HAT_YELL" };

        public INT(string path)
        {
            intf = File.Open(path, FileMode.Open);
            intp = path;
        }

        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern) // https://stackoverflow.com/questions/283456
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }

        public void extract(string extFolder)
        {
            int BASE_OFF = 0;
            int BUFF_OFF = 0;
            for (int e = 0; e < folders.Length; e++)
            {
                byte[] read(int len, int off)
                {
                    intf.Seek(off, SeekOrigin.Begin);
                    BUFF_OFF = off + len;
                    byte[] buf = new byte[len];
                    intf.Read(buf, 0, len);
                    foreach (byte x in buf)
                    {
                        Console.Write(x + " ");
                    }
                    Console.WriteLine();
                    return buf;
                }
                long getLong(int off)
                {
                    return BitConverter.ToInt32(read(4, off), 0);
                }
                string getString(int off)
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
                int skipZeroes() //actually might be unused ffffufvg bgfstresf
                {
                    //probably the least time efficient method youve seen in your life, unless this goes fast
                    int zeroOff = BUFF_OFF;
                    int loops = 0;
                    intf.Seek(zeroOff, SeekOrigin.Begin);
                    while (true)
                    {
                        byte[] zeroBuff = new byte[1];
                        if (intf.Read(zeroBuff, 0, 1) != 0)
                        {
                            Console.WriteLine(zeroOff + " " + loops);
                            return zeroOff;
                        }
                        zeroOff += 1;
                        loops += 1;
                    }
                }
                //hoo boy here we go
                //skipping lzss line for now, have no idea how to get it working
                //will export in lzss compressed for now, i have no idea how to get lzss uncompressio n

                //based off bms script

                int INT_SIZE = (int)intf.Length;
                int FOLDER = 0;
                long DATA_OFF = 0;
                while (DATA_OFF < INT_SIZE)
                {
                    //id string gonna figure out soon
                    BASE_OFF = BUFF_OFF;
                    long FILES = getLong(BUFF_OFF);
                    long ID = getLong(BUFF_OFF);
                    long INFO_OFF = getLong(BUFF_OFF) - 4;
                    DATA_OFF = getLong(BUFF_OFF);
                    long DATA_SIZE = getLong(BUFF_OFF);
                    /*long ZERO = getLong(BUFF_OFF);
                    ZERO = getLong(BUFF_OFF);

                    skip them ZEROSSSssssssSsSsSs*/
                    BUFF_OFF += 8;
                    INFO_OFF += BASE_OFF;
                    DATA_OFF += INFO_OFF;
                    Console.WriteLine(FILES + " f");
                    if (FILES != 0)
                    {
                        long[] A = new long[FILES];
                        long[] B = new long[FILES];
                        long[] C = new long[FILES];
                        string[] D = new string[FILES];
                        Console.WriteLine(INFO_OFF);
                        for (int i = 0; i < FILES; i++)
                        {
                            long TOFFSET = getLong(BUFF_OFF);
                            Console.WriteLine(TOFFSET);
                            A[i] = TOFFSET;
                        }
                        BUFF_OFF = (int)INFO_OFF;
                        for (int i = 0; i < FILES; i++)
                        {
                            long NAME_OFF = getLong(BUFF_OFF);
                            long TSIZE = getLong(BUFF_OFF); //temp size?
                            Console.WriteLine(BUFF_OFF + "     E" + i + " " + TSIZE + " " + NAME_OFF);
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
                        BUFF_OFF = (int)DATA_OFF;
                        long SIZE = getLong(BUFF_OFF);
                        long ZSIZE = getLong(BUFF_OFF);
                        OFFSET = BUFF_OFF;
                        /*hoo boy
                         the clog
                         cloggy boy
                         hmmmmmmmmmmmmmmmm*/
                        intf.Seek(OFFSET, SeekOrigin.Begin);
                        byte[] MEMORY_FILE = new byte[INT_SIZE];
                        intf.Read(MEMORY_FILE, 0, INT_SIZE); //COPY THE ENTIRETY MY BOYO
                        for (int i = 0; i < FILES; i++)
                        {
                            OFFSET = A[i];
                            SIZE = C[i];
                            string NAME = D[i];
                            string FNAME = string.Format(@"\{0}\{1}", folders[FOLDER], NAME);
                            //the big EXPORT TIME
                            Console.WriteLine(extFolder + FNAME);
                            if (!Directory.Exists(extFolder + @"\" + folders[FOLDER]))
                            {
                                Directory.CreateDirectory(extFolder + @"\" + folders[FOLDER]);
                            }
                            Console.WriteLine(OFFSET + " " + SIZE);
                            Stream EXPORT;
                            try
                            {
                                EXPORT = File.Open(extFolder + FNAME, FileMode.CreateNew);
                            }
                            catch (IOException)
                            {
                                EXPORT = File.Open(extFolder + FNAME, FileMode.Truncate);
                            };
                            Console.WriteLine(NAME);
                            EXPORT.Write(MEMORY_FILE, (int)OFFSET, (int)SIZE);
                        }
                    }
                    //Console.WriteLine(getLong(skipZeroes()));

                    Console.WriteLine(FILES + " " + ID + " " + INFO_OFF);
                    break;
                }
            }
        }
    }
}