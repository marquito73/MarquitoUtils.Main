namespace MarquitoUtils.Main.Files.Multimedia.Music
{
    public class SongData
    {
        public string SongName { get; set; } = "";
        public byte[] SongBinaryData { get; set; } = null;
        public string Extension { get; set; } = "";
        public SongData(byte[] songBinaryData)
        {
            this.SongBinaryData = songBinaryData;
        }

        public SongData(byte[] songBinaryData, string songName) : this(songBinaryData)
        {
            this.SongName = songName;
        }
    }
}
