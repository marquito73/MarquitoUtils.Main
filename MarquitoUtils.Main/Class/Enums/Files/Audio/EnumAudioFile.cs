using MarquitoUtils.Main.Class.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarquitoUtils.Main.Class.Enums.Files.Audio
{
    /*public class EnumAudioFile : EnumClass
    {
        public string Extension { get; set; } = "";
        public string Description { get; set; } = "";

        public EnumAudioFile(string extension, string description)
        {
            Extension = extension;
            Description = description;
        }

        public static List<EnumAudioFile> GetAudioFileList()
        {
            //return GetEnumList<EnumAudioFile>();
            // TODO Refaire
            return new List<EnumAudioFile>();
            return GetEnumList
        }

        public static EnumAudioFile Mp3 = new EnumAudioFile("Mp3", "Mp3 audio file");
        public static EnumAudioFile Waw = new EnumAudioFile("Waw", "Waw audio file");
    }*/
    public class EnumAudioFileAttr : EnumClass
    {
        public string Extension { get; set; } = "";
        public string Description { get; set; } = "";

        public EnumAudioFileAttr(string extension, string description)
        {
            Extension = extension;
            Description = description;
        }
    }

    public static class EnumAudioFiles
    {
        public static EnumAudioFileAttr Attr(this EnumAudioFile audioFile)
        {
            return EnumUtils.GetAttr<EnumAudioFileAttr, EnumAudioFile>(audioFile);
        }
    }

    [DataContract]
    public enum EnumAudioFile
    {
        [EnumMember]
        [EnumAudioFileAttr("Mp3", "Mp3 audio file")] Mp3,
        [EnumAudioFileAttr("Waw", "Waw audio file")] Waw
    }
}
