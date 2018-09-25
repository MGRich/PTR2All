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

        public TextureFormatSerializer serializer;

        public TM2(string path)
        {
            Stream stream = File.Open(path, FileMode.Open);
            serializer = TextureFormatSerializerProvider.FromStream(stream);
            //MetadataReader reader = null;
            Texture = serializer.Open(stream);
        }

        public void export(string path)
        {
            using (Stream s = File.Open(path, FileMode.Create))
            {
                using (MetadataWriter writer = XmlMetadataWriter.Create(s))
                {
                    serializer.Export(Texture, writer, Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                }
            }
        }

        public void import(string path)
        {
            using (Stream s = File.Open(path, FileMode.Open))
            {
                using (MetadataReader reader = XmlMetadataReader.Create(s))
                {
                    Texture = serializer.Import(reader, path);
                }
            }
        }
    }
}