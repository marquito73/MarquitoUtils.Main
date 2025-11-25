namespace MarquitoUtils.Main.Files.Entities
{
    public class MusicFileProperties
    {
        public string MusicName { get; set; }
        public List<string> Artists { get; set; } = new List<string>();
        public List<string> Feats { get; set; } = new List<string>();
        public List<string> Albums { get; set; } = new List<string>();
        public DateTime? ReleaseDate { get; set; }
        public string ThumbnailUrl { get; set; }

        public MusicFileProperties()
        {

        }
        public MusicFileProperties(string musicName)
        {
            this.MusicName = musicName;
        }
    }
}
