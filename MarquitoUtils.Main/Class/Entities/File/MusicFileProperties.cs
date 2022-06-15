using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.File
{
    public class MusicFileProperties
    {
        public string MusicName { get; set; }
        public List<string> Artists { get; set; } = new List<string>();
        public List<string> Feats { get; set; } = new List<string>();
        public List<string> Albums { get; set; } = new List<string>();
        public DateTime? ReleaseDate { get; set; }
        public string ThumbnailUrl { get; set; }

        public MusicFileProperties ()
        {

        }
        public MusicFileProperties(string musicName)
        {
            this.MusicName = musicName;
        }
    }
}
