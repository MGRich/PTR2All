using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ptr2sound
{
	/// <summary>
	/// Good/Cool converter
	/// </summary>
	public partial class Good : Form
	{
		public Good()
		{
			InitializeComponent();
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
		}
		
		byte[] inputData;
		byte[] interData;
		byte[] deinterData;
		string outputDir;
		
		void wavDump()
		{
			FileStream wavfile = new FileStream(outputDir, FileMode.Create);
			BinaryWriter wav = new BinaryWriter(wavfile);
			wav.Write(Encoding.ASCII.GetBytes("RIFF")); //chunkid
			wav.Write(36 + deinterData.Length); //length of all data after this
			wav.Write(Encoding.ASCII.GetBytes("WAVE")); //format
			wav.Write(Encoding.ASCII.GetBytes("fmt ")); //subchunk1id
			wav.Write(16); //subchunk1 size
			wav.Write((ushort)1); //audioformat (pcm = 1)
			wav.Write((ushort)2); //channels
			wav.Write(48000); //samplerate
			wav.Write(192000); //byterate, which is (samplerate * bitspersample * channels) / 8
			wav.Write((ushort)4); //blockalign (channels * bitspersample/8)
			wav.Write((ushort)16); //bitspersample
			wav.Write(Encoding.ASCII.GetBytes("data")); //subchunk2 id
			wav.Write(deinterData.Length); //subchunk2 size
			wav.Write(deinterData); //the data
			wavfile.Flush();
			wav.Close();
			
		}
		
		void wavExtract()
		{
			//byte[] dataLength = new byte[4];
			//Buffer.BlockCopy(inputData, 36, dataLength, 0, 4);
			int dataLength = BitConverter.ToInt32(inputData, 40);
			deinterData = new byte[dataLength];
			Buffer.BlockCopy(inputData, 44, deinterData, 0, dataLength);						
		}
		
		void deinterleave()
		{	
			//int blocks = inputData.Length / 400;
			deinterData = new byte[interData.Length];
			for (int i = 0; i < interData.Length; i+= 1024)
			{
				for (int i2 = 0; i2 < 512; i2+= 2)
				{
					Buffer.BlockCopy(interData, i + i2, deinterData, i + i2 + i2, 2);
					Buffer.BlockCopy(interData, i + i2 + 512, deinterData, i + i2 + i2 + 2, 2);
				}
			}
			//File.WriteAllBytes(outputDir, audioData1);
		}
		
		void interleave()
		{
			byte[] interData1 = new byte[deinterData.Length];
			for (int i = 0; i < deinterData.Length; i+= 1024)
			{
				for (int i2 = 0; i2 < 512; i2+= 2)
				{
					Buffer.BlockCopy(deinterData, i + i2 + i2, interData1, i + i2, 2);
					Buffer.BlockCopy(deinterData, i + i2 + i2 + 2, interData1, i + i2 + 512, 2);
				}
			}
			File.WriteAllBytes(outputDir, interData1);
		}

		void Button3Click(object sender, EventArgs e)
		{
			if (button5.Text == "Select Output (WAV)")
			{
				deinterleave();
				wavDump();
			}
			else
			{
				wavExtract();
				interleave();
			}
			MessageBox.Show("Conversion Done");
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "WP2 or WAV|*.WP2;*.wav";
		    openFileDialog1.Title = "Select a PaRappa 2 WP2 or WAV file";  
		 
		    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)  
		    {  	
		    	if (Path.GetExtension(openFileDialog1.FileName).ToUpper() == ".WP2")
		    	{
		    		interData = File.ReadAllBytes(openFileDialog1.FileName);
		    		button5.Text = "Select Output (WAV)";
		    	}
		    	else
		    	{
		    		inputData = File.ReadAllBytes(openFileDialog1.FileName);
		    		button5.Text = "Select Output (WP2)";
		    	}
		    	button5.Enabled = true;
		    	textBox1.Text = Path.GetFileName(openFileDialog1.FileName);
		    }  
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "WP2|*.WP2";
			if (button5.Text == "Select Output (WAV)")
			{
				saveFileDialog1.Filter = "WAV|*.wav";
			}
			saveFileDialog1.Title = "Save As";
			
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				outputDir = saveFileDialog1.FileName;
				button3.Enabled = true;
				textBox2.Text = Path.GetFileName(saveFileDialog1.FileName);
			}
		}
		
	}
}
