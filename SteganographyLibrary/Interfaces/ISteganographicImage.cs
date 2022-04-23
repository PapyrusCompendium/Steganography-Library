using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SteganographyLibrary.SteganographyReversing;

namespace SteganographyLibrary {
    public interface ISteganographicImage {
        Image<Rgba32> Image { get; }

        byte[] DecodeRawImageData(string encodingOrderMask = "argb");
        Image<Rgba32> EncodeDataInImage(byte[] encodedData, string aesKey = null, string encodingOrderMask = "argb");
        byte[] DecodedDataFromImage(string aesKey = null);
        IImageAnalysis GetImageAnalysis();
    }
}