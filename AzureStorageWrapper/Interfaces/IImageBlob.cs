namespace AzureStorageWrapper.Interfaces
{
    public interface IImageBlob : IBlob
    {
        public IImageStrategy? ImageStrategy { get; }

        public new string FormatFileName =>
            $"{Name.Replace(" ", "_")}_{DateTime.UtcNow:yyyymmddHHmmss}.{Extension}";

        public new string ContentType => $"image/{Extension}";

        string IBlob.GetFullBlobName()
        {
            return $"{Folder}/{FormatFileName}.{Extension}";
        }
    }

}