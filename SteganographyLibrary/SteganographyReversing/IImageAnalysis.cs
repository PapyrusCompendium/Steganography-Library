using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SteganographyLibrary.SteganographyReversing {
    public interface IImageAnalysis {
        Rgba32[] AllColors { get; set; }
        double ColorUniqueness { get; }
        Image<Rgba32> Image { get; }
        Rgba32[] UniqueColors { get; set; }
        IVisualizationAid VisualizationAid { get; }

        bool CheckIsSecure();
        IEnumerable<string> ExtractBase64(Encoding encoding, int size = 1);
        IEnumerable<string[]> ExtractStrings(Encoding encoding, ImageAnalysis.StringExtractionOptions stringExtractionOptions);
        string[] ExtractStringsFromRawData(Encoding encoding, byte[] data, ImageAnalysis.StringExtractionOptions stringExtractionOptions);
        IEnumerable<string[]> ExtractStringsFromUniqueColors(Encoding encoding, ImageAnalysis.StringExtractionOptions stringExtractionOptions);
        Rgba32[] GetRepeatedColors(Image<Rgba32> image, int occurrences = 1);
        Image<Rgba32> GetUniqueColorImage(Image<Rgba32> image);
        Rgba32[] GetUniqueColors(Image<Rgba32> image);
    }
}