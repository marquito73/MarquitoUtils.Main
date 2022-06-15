using MarquitoUtils.Main.Class.Enums;
using System.Collections.Generic;

namespace MarquitoUtils.Main.Class.Enums.Files.Audio
{
    public class EnumAudioFile : EnumClass
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
            return GetEnumList<EnumAudioFile>();
        }

        public static EnumAudioFile Mp3 = new EnumAudioFile("Mp3", "Mp3 audio file");
        public static EnumAudioFile Waw = new EnumAudioFile("Waw", "Waw audio file");
    }
}
