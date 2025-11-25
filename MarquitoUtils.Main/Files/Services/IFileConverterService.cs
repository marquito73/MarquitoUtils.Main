using MarquitoUtils.Main.Files.Entities;

namespace MarquitoUtils.Main.Files.Services
{
    public interface IFileConverterService
    {
        public byte[] ExtractMusicFromVideo(CustomFile videoFile, string destExtension,
            MusicFileProperties fileProperties);

        public void SetMusicProperties(string filename, MusicFileProperties fileProperties);

        public void CreateFile(CustomFile videoFile);

        public byte[] ZipFiles(string zipName, string zipPath, List<CustomFile> files);

        public string DownloadFileFromUrl(string url, string path, string filename);

        public byte[] DownloadFileBytesFromUrl(string url);
    }
}
