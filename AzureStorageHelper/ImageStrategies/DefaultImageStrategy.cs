using AzureStorageHelper.Interfaces;

namespace AzureStorageHelper.ImageStrategies
{
    public struct DefaultImageStrategy : IImageStrategy
    {
        public int Width => 200;
        public int Height => 0; //Auto AspectRatio
    }
}
