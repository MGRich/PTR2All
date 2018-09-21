using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PTR2Lib
{
    public class WP2
    {
        public FileStream wp2;

        public void dump(FileStream wav, byte[] deIntData)
        {
            BinaryWriter wte = new BinaryWriter(wav);
            wte.Write(Encoding.ASCII.GetBytes("RIFF"));
            wte.Write(36 + deIntData.Length);
            wte.Write(Encoding.ASCII.GetBytes("WAVE"));
            wte.Write(Encoding.ASCII.GetBytes("fmt "));
            wte.Write(16);
            wte.Write((ushort)1);
            wte.Write((ushort)2);
            wte.Write(48000);
            wte.Write(192000);
            wte.Write((ushort)4);
            wte.Write((ushort)16);
            wte.Write(Encoding.ASCII.GetBytes("data"));
            wte.Write(deIntData.Length);
            wte.Write(deIntData);
            wav.Flush();
            wte.Close();
        }
    }
}