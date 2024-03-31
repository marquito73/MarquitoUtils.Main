using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Tools;
using MediaToolkit;
using MediaToolkit.Model;
using System.IO.Compression;
using System.Net;
using System.Text;
using File = System.IO.File;

namespace MarquitoUtils.Main.Class.Service.Files
{
    public class FileConverterServiceImpl : DefaultService, FileConverterService
    {
        public byte[] ExtractMusicFromVideo(CustomFile videoFile, string destExtension, 
            MusicFileProperties fileProperties)
        {
            byte[] audioBytes;

            // The video file complete path
            string videoCompletePath = videoFile.GetCompletePathName();
            // The music file complete path
            string musicCompletePath = videoFile.GetCompletePathName(destExtension);

            // Create the video file
            this.CreateFile(videoFile);

            // Convert video to audio file
            MediaFile inputFile = new MediaFile { Filename = videoCompletePath };
            MediaFile outputFile = new MediaFile { Filename = musicCompletePath };
            using (Engine engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                Thread.Sleep(2);
                engine.Convert(inputFile, outputFile);
            }
            // Delete the video file
            File.Delete(videoCompletePath);

            // Set all musique info needed
            this.SetMusicProperties(musicCompletePath, fileProperties);

            // Get music bytes
            audioBytes = File.ReadAllBytes(musicCompletePath);
            // Delete the video file
            File.Delete(musicCompletePath);
            // Delete the temp directory
            Directory.Delete(videoFile.FilePath);

            // Test
            /*TagFile file = new TagFile(videoFile.FileName + "." + destExtension, 
                Utils.BytesToStream(audioBytes));
            this.SetMusicProperties(file, fileProperties);*/

            return audioBytes;
        }

        public void SetMusicProperties(string filename, MusicFileProperties fileProperties)
        {
            TagLib.Id3v2.Tag.DefaultVersion = 3;
            TagLib.Id3v2.Tag.ForceDefaultVersion = true;


            TagLib.File file = TagLib.File.Create(filename);
            // Music name
            file.Tag.Title = fileProperties.MusicName;
            // Main artist
            file.Tag.AlbumArtists = fileProperties.Artists.ToArray();
            // Others artists
            file.Tag.Artists = fileProperties.Feats.ToArray();
            // Music genres
            file.Tag.Genres = fileProperties.Albums.ToArray();
            if (Utils.IsNotNull(fileProperties.ReleaseDate))
            {
                // Release date
                file.Tag.Year = (uint)fileProperties.ReleaseDate.Value.Year;
            }
            // The thumbnail url for this music
            if (Utils.IsNotEmpty(fileProperties.ThumbnailUrl)) 
            {
                TagLib.Id3v2.AttachmentFrame cover = new TagLib.Id3v2.AttachmentFrame
                {
                    Type = TagLib.PictureType.FrontCover,
                    Description = "Cover",
                    MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                    Data = this.DownloadFileBytesFromUrl(fileProperties.ThumbnailUrl),
                    TextEncoding = TagLib.StringType.UTF16

                };
                file.Tag.Pictures = new TagLib.IPicture[] { cover };
            }

            file.Save();
        }

        public void SetMusicProperties(TagFile tagFile, MusicFileProperties fileProperties)
        {
            TagLib.Id3v2.Tag.DefaultVersion = 3;
            TagLib.Id3v2.Tag.ForceDefaultVersion = true;


            TagLib.File file = TagLib.File.Create(tagFile);
            // Music name
            file.Tag.Title = fileProperties.MusicName;
            // Main artist
            file.Tag.AlbumArtists = fileProperties.Artists.ToArray();
            // Others artists
            file.Tag.Artists = fileProperties.Feats.ToArray();
            // Music genres
            file.Tag.Genres = fileProperties.Albums.ToArray();
            if (Utils.IsNotNull(fileProperties.ReleaseDate))
            {
                // Release date
                file.Tag.Year = (uint)fileProperties.ReleaseDate.Value.Year;
            }
            // The thumbnail url for this music
            if (Utils.IsNotEmpty(fileProperties.ThumbnailUrl))
            {
                TagLib.Id3v2.AttachmentFrame cover = new TagLib.Id3v2.AttachmentFrame
                {
                    Type = TagLib.PictureType.FrontCover,
                    Description = "Cover",
                    MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                    Data = this.DownloadFileBytesFromUrl(fileProperties.ThumbnailUrl),
                    TextEncoding = TagLib.StringType.UTF16

                };
                file.Tag.Pictures = new TagLib.IPicture[] { cover };
            }
            file.Save();
        }

        public void CreateFile(CustomFile file)
        {
            // Create the directory
            Directory.CreateDirectory(file.FilePath);
            // The file complete path
            string fileCompletePath = file.FilePath + file.FileName + "." + file.Extension;
            // Write the file
            File.WriteAllBytes(fileCompletePath, file.BinaryContent);
        }

        public byte[] ZipFiles(string zipName, string zipPath, List<CustomFile> files)
        {
            byte[] zipBytes;

            // Create the directory
            Directory.CreateDirectory(zipPath);
            // Complete zip path filename
            string completePathFile = zipPath + "\\" + zipName + ".zip";
            // Open new zip stream
            using (FileStream zipToOpen = new FileStream(completePathFile, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (CustomFile file in files)
                    {
                        // Create a zip archive entry for this music
                        ZipArchiveEntry readmeEntry = archive
                            .CreateEntry(file.FileName + "." + file.Extension);
                        // We write file as binary
                        using (BinaryWriter binaryWriter 
                            = new BinaryWriter(readmeEntry.Open(), Encoding.Unicode, false))
                        {
                            binaryWriter.Write(file.BinaryContent);
                        }
                    }
                }
            }

            // Get zip bytes
            zipBytes = File.ReadAllBytes(completePathFile);
            // Delete the zip file
            File.Delete(completePathFile);
            // Delete the temp directory
            Directory.Delete(zipPath);

            return zipBytes;
        }

        public string DownloadFileFromUrl(string url, string path, string filename)
        {
            string result = @path + "\\" + filename + ".png";

            using (WebClient client = new WebClient())
            { 
                client.DownloadFile(new Uri(url), @path + "\\" + filename + ".png");
            }

            return result;
        }

        public byte[] DownloadFileBytesFromUrl(string url)
        {
            byte[] result;

            using (WebClient client = new WebClient())
            {
                result = client.DownloadData(new Uri(url));
            }

            return result;
        }
    }
}
