using System.Collections.Generic;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SteganographyLibrary.SteganographyReversing {
    public class VisualizationAid : IVisualizationAid {
        private readonly ImageAnalysis _imageAnalysis;

        public VisualizationAid(ImageAnalysis imageAnalysis) {
            _imageAnalysis = imageAnalysis;
        }

        public Image<Rgba32> ShowUniqueColors() {
            var image = _imageAnalysis.Image.Clone();

            var uniqueColors = _imageAnalysis.UniqueColors;
            var colorSet = new HashSet<Rgba32>();

            for (var x = 0; x < uniqueColors.Length; x++) {
                colorSet.Add(uniqueColors[x]);
            }

            for (var x = 0; x < image.Width; x++) {
                for (var y = 0; y < image.Height; y++) {
                    if (!colorSet.Contains(image[x, y])) {
                        image[x, y] = new Rgba32(0, 0, 0, 255);
                    }
                }
            }

            return image;
        }

        public Image<Rgba32> ShowRepeatedColors(int repeatedTimes) {
            var image = _imageAnalysis.Image.Clone();

            var colors = _imageAnalysis.GetRepeatedColors(image, repeatedTimes);
            var colorSet = new HashSet<Rgba32>();

            for (var x = 0; x < colors.Length; x++) {
                colorSet.Add(colors[x]);
            }

            for (var x = 0; x < image.Width; x++) {
                for (var y = 0; y < image.Height; y++) {
                    if (!colorSet.Contains(image[x, y])) {
                        image[x, y] = new Rgba32(0, 0, 0, 255);
                    }
                }
            }

            return image;
        }

    }
}
