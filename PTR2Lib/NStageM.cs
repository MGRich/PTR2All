using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTR2Lib
{
    public class NStageM : WP2
    {
        private byte[] interData;
        private byte[] deinterData1;
        private byte[] deinterData2;
        public string outputDir = null;

        public NStageM(byte[] i, string o)
        {
            interData = i;
            outputDir = o;
        }

        public NStageM(byte[] i)
        {
            interData = i;
        }

        private void dumpBad(FileStream wav)
        {
            deinterleave();
            ManualDump(wav, deinterData1);
            wav.Close();
        }

        private void dumpAwful(FileStream wav)
        {
            deinterleave();
            ManualDump(wav, deinterData2);
            wav.Close();
        }

        public void dump(string path)
        {
            string bad = path.Insert(path.Length - 4, "B");
            string awf = path.Insert(path.Length - 4, "A");
            FileStream wav = File.Open(bad, FileMode.Create);
            dumpBad(wav);
            wav = File.Open(awf, FileMode.Create);
            dumpAwful(wav);
        }

        public void dump()
        {
            string path = outputDir;
            string bad = path.Insert(path.Length - 5, "B");
            string awf = path.Insert(path.Length - 5, "A");
            FileStream wav = File.Open(bad, FileMode.Create);
            dumpBad(wav);
            wav = File.Open(awf, FileMode.Create);
            dumpAwful(wav);
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
            deinterData1 = new byte[interData.Length / 2];
            deinterData2 = new byte[interData.Length / 2];
            for (int i = 0; i < interData.Length; i += 2048)
            {
                for (int i2 = 0; i2 < 512; i2 += 2)
                {
                    Buffer.BlockCopy(interData, i + i2, deinterData1, i / 2 + i2 + i2, 2);
                    Buffer.BlockCopy(interData, i + i2 + 512, deinterData1, i / 2 + i2 + i2 + 2, 2);
                    Buffer.BlockCopy(interData, i + i2 + 1024, deinterData2, i / 2 + i2 + i2, 2);
                    Buffer.BlockCopy(interData, i + i2 + 1536, deinterData2, i / 2 + i2 + i2 + 2, 2);
                }
            }
            //File.WriteAllBytes(outputDir, audioData1);
        }

        public void interleave()
        {
            byte[] interData1 = new byte[deinterData1.Length * 2];
            for (int i = 0; i < interData1.Length; i += 2048)
            {
                for (int i2 = 0; i2 < 512; i2 += 2)
                {
                    Buffer.BlockCopy(deinterData1, i / 2 + i2 + i2, interData1, i + i2, 2);
                    Buffer.BlockCopy(deinterData1, i / 2 + i2 + i2 + 2, interData1, i + i2 + 512, 2);
                    Buffer.BlockCopy(deinterData2, i / 2 + i2 + i2, interData1, i + i2 + 1024, 2);
                    Buffer.BlockCopy(deinterData2, i / 2 + i2 + i2 + 2, interData1, i + i2 + 1536, 2);
                }
            }
            //File.WriteAllBytes(outputDir, interData1);
        }
    }
}