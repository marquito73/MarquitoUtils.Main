using MarquitoUtils.Main.Class.Entities.File;

namespace MarquitoUtils.Main.Class.Service.Files
{
    public interface FileConverterService
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
