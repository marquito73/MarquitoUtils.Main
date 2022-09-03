using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TagLib.File;

namespace MarquitoUtils.Main.Class.Entities.File
{
    public class TagFile : IFileAbstraction
    {
        public string Name { get; set; }
        private Stream Stream { get; set; }

        public Stream ReadStream => this.Stream;

        public Stream WriteStream => this.Stream;

        public TagFile(string name, Stream stream)
        {
            Name = name;
            Stream = stream;
        }

        public void CloseStream(Stream stream)
        {
            stream.Close();
        }
    }
}
