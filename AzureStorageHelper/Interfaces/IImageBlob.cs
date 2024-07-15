namespace AzureStorageHelper.Interfaces
{
    public interface IImageBlob : IBlob
    {
        public ImageExtensions ImageExtension { get; }
        public IImageStrategy? ImageStrategy { get; }

        public new string FormatFileName =>
            $"{Name.Replace(" ", "_")}_{DateTime.UtcNow.ToString("yyyymmddHHmmss")}.{Extension}";

        public new string ContentType => $"image/{Extension}";

        string IBlob.GetFullBlobName()
        {
            return $"{Folder}/{FormatFileName}.{Extension}";
        }
    }

    public enum ImageExtensions
    {
        png,
        gif,
        basis
    }
}