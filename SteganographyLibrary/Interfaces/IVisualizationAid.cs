using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SteganographyLibrary.SteganographyReversing {
    public interface IVisualizationAid {
        /// <summary>
        /// Draw only colors that are repeated <paramref name="repeatedTimes"/> times.
        /// </summary>
        /// <param name="repeatedTimes"></param>
        /// <returns></returns>
        Image<Rgba32> ShowRepeatedColors(int repeatedTimes);

        /// <summary>
        /// Draw only colors that are unique.
        /// </summary>
        /// <returns></returns>
        Image<Rgba32> ShowUniqueColors();
    }
}