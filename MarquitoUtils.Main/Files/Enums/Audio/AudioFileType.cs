using MarquitoUtils.Main.Common.Enums;
using System.Runtime.Serialization;

namespace MarquitoUtils.Main.Files.Enums.Audio
{
    public class AudioFileTypeAttr : EnumClass
    {
        public string Extension { get; set; } = "";
        public string Description { get; set; } = "";

        public AudioFileTypeAttr(string extension, string description)
        {
            Extension = extension;
            Description = description;
        }
    }

    public static class AudioFileTypes
    {
        public static AudioFileTypeAttr Attr(this AudioFileType audioFile)
        {
            return EnumUtils.GetAttr<AudioFileTypeAttr, AudioFileType>(audioFile);
        }
    }

    [DataContract]
    public enum AudioFileType
    {
        [EnumMember]
        [AudioFileTypeAttr("Mp3", "Mp3 audio file")] Mp3,
        [AudioFileTypeAttr("Waw", "Waw audio file")] Waw
    }
}
