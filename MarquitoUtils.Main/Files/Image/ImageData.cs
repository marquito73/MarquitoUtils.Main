namespace MarquitoUtils.Main.Files.Image
{
    public class ImageData
    {
        public string ImageName { get; set; } = "";
        public byte[] ImageBinaryData { get; set; } = null;
        public string Extension { get; set; } = "";
        public ImageData(byte[] imageBinaryData)
        {
            this.ImageBinaryData = imageBinaryData;
        }

        public ImageData(byte[] imageBinaryData, string imageName) : this(imageBinaryData)
        {
            this.ImageName = imageName;
        }
    }
}
