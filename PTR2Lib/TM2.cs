using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rainbow.ImgLib.Formats;
using Rainbow.ImgLib.Formats.Serialization;
using Rainbow.ImgLib.Formats.Serialization.Metadata;

namespace PTR2Lib
{
    public class TM2
    {
        public TextureFormat Texture = null;

        public TM2(string path)
        {
            Stream stream = File.Open(path, FileMode.Open);
            TextureFormatSerializer serializer = TextureFormatSerializerProvider.FromStream(stream);
            //MetadataReader reader = null;
            Texture = serializer.Open(stream);
        }
    }
}