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
        public byte[] inputData = null;
        private byte[] interData;
        private byte[] deinterData;
        public string outputDir = null;

        public NStageM(byte[] i, string o)
        {
            inputData = i;
            outputDir = o;
        }

        public void wavExtract()
        {
            //byte[] dataLength = new byte[4];
            //Buffer.BlockCopy(inputData, 36, dataLength, 0, 4);
            int dataLength = BitConverter.ToInt32(inputData, 40);
            deinterData = new byte[dataLength];
            Buffer.BlockCopy(inputData, 44, deinterData, 0, dataLength);
        }

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