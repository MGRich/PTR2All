using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTR2Lib
{
    public class GStageM : WP2
    {
        private byte[] interData;
        private byte[] deinterData;
        public string outputDir = null;

        public GStageM(byte[] i, string o)
        {
            interData = i;
            outputDir = o;
        }

        public GStageM(byte[] i)
        {
            interData = i;
        }

        public void dump(FileStream wav)
        {
            deinterleave();
            ManualDump(wav, deinterData);
            wav.Close();
        }

        public void dump(string path)
        {
            FileStream wav = File.Open(path, FileMode.Create);
            dump(wav);
        }

        public void dump()
        {
            FileStream wav = File.Open(outputDir, FileMode.Create);
            dump(wav);
        }

        /*public void deintCopy()
        { do you REMEMNBRRRRRRRRR
            //byte[] dataLength = new byte[4];
            //Buffer.BlockCopy(inputData, 36, dataLength, 0, 4);
            int dataLength = BitConverter.ToInt32(inputData, 40);
            deinterData = new byte[dataLength];
            Buffer.BlockCopy(inputData, 44, deinterData, 0, dataLength);
        }THE 21St NIGHT OF SEPTEMBRRR R R R */

        public void deinterleave()
        {
            //int blocks = inputData.Length / 400;
            deinterData = new byte[interData.Length];
            for (int i = 0; i < interData.Length; i += 1024)
            {
                for (int i2 = 0; i2 < 512; i2 += 2)
                {
                    Buffer.BlockCopy(interData, i + i2, deinterData, i + i2 + i2, 2);
                    Buffer.BlockCopy(interData, i + i2 + 512, deinterData, i + i2 + i2 + 2, 2);
                }
            }
            //File.WriteAllBytes(outputDir, audioData1);
        }

        public void interleave()
        {
            byte[] interData1 = new byte[deinterData.Length];
            for (int i = 0; i < deinterData.Length; i += 1024)
            {
                for (int i2 = 0; i2 < 512; i2 += 2)
                {
                    Buffer.BlockCopy(deinterData, i + i2 + i2, interData1, i + i2, 2);
                    Buffer.BlockCopy(deinterData, i + i2 + i2 + 2, interData1, i + i2 + 512, 2);
                }
            }
            File.WriteAllBytes(outputDir, interData1);
        }
    }
}