using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SteganographyLibrary.Interfaces;
using SteganographyLibrary.Models;

namespace SteganographyLibrary.SteganographyReversing {
    public class ImageAnalysis : IImageAnalysis {
        public Image<Rgba32> Image {
            get {
                return _steganographicImage.Image;
            }
        }

        public double ColorUniqueness {
            get {
                return (double)UniqueColors.Length / AllColors.Length;
            }
        }


        private Rgba32[] _uniqueColors;
        public Rgba32[] UniqueColors {
            get {
                if (_uniqueColors is null) {
                    _uniqueColors = GetUniqueColors(_steganographicImage.Image).ToArray();
                }

                return _uniqueColors;
            }
            set {
                _uniqueColors = value;
            }
        }

        private Rgba32[] _allColors;
        public Rgba32[] AllColors {
            get {
                if (_allColors is null) {
                    _allColors = GetRepeatedColors(Image).ToArray();
                }

                return _allColors;
            }
            set {
                _allColors = value;
            }
        }

        public IVisualizationAid VisualizationAid { get; }

        private readonly ISteganographicImage _steganographicImage;
        private readonly ILsbEncoder _lsbEncoder = new LsbEncoder();
        private readonly Regex _validText = new Regex(@"[a-zA-Z0-9!@#$%^&*() ]");
        private readonly Regex _validBase64 = new Regex(@"(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?");

        public ImageAnalysis(ISteganographicImage steganographicImage) {
            _steganographicImage = steganographicImage;
            VisualizationAid = new VisualizationAid(this);
        }

        public bool CheckIsSecure() {
            return ColorUniqueness > .45;
        }

        private static IEnumerable<string> Permute(string sourceString, int start, int length) {
            for (var x = start; x <= length; x++) {
                yield return Swap(sourceString, start, x);
                Permute(sourceString, start + 1, length);
                yield return Swap(sourceString, start, x);
            }

            static string Swap(string sourceString, int sourceChar, int swapChar) {
                char temp;
                var charArray = sourceString.ToCharArray();
                temp = charArray[sourceChar];
                charArray[sourceChar] = charArray[swapChar];
                charArray[swapChar] = temp;
                var resultString = new string(charArray);
                return resultString;
            }
        }

        private IEnumerable<byte[]> GetAllDataForms(Image<Rgba32> image) {
            var allEncodingOrders = Permute("argb", 0, 3);
            foreach (var encodingOrder in allEncodingOrders) {
                yield return _lsbEncoder.DecodeRawImageData(image, encodingOrder);
            }
        }

        public IEnumerable<string> ExtractBase64(Encoding encoding, int size = 1) {
            var extractedStrings = ExtractStrings(encoding, new StringExtractionOptions {
                MinLength = 5,
                MaxRepeatedChars = 4
            });

            foreach (var dataString in extractedStrings.SelectMany(i => i)) {
                if (_validBase64.IsMatch(dataString)) {
                    yield return dataString;
                }
            }
        }

        public IEnumerable<string[]> ExtractStrings(Encoding encoding, StringExtractionOptions stringExtractionOptions) {
            var possibleEncodedData = GetAllDataForms(Image);

            foreach (var rawImageData in possibleEncodedData) {
                yield return ExtractStringsFromRawData(encoding, rawImageData, stringExtractionOptions).ToArray();
            }
        }

        public IEnumerable<string[]> ExtractStringsFromUniqueColors(Encoding encoding, StringExtractionOptions stringExtractionOptions) {
            var uniqueColorImage = GetUniqueColorImage(Image);
            var possibleEncodedData = GetAllDataForms(uniqueColorImage);

            foreach (var rawImageData in possibleEncodedData) {
                yield return ExtractStringsFromRawData(encoding, rawImageData, stringExtractionOptions).ToArray();
            }
        }

        public string[] ExtractStringsFromRawData(Encoding encoding, byte[] data, StringExtractionOptions stringExtractionOptions) {
            var dataAsString = encoding.GetString(data);
            var foundStrings = new List<string>();

            var stringBuilder = new StringBuilder();
            for (var x = 0; x < dataAsString.Length; x++) {
                var letter = dataAsString[x];
                var letterString = letter.ToString();

                if (x >= stringExtractionOptions.MaxRepeatedChars
                    && stringBuilder.ToString().Length >= stringExtractionOptions.MaxRepeatedChars) {
                    var currentString = stringBuilder.ToString();
                    var lastRepeatedLetters = currentString
                        .Substring(currentString.Length - stringExtractionOptions.MaxRepeatedChars, stringExtractionOptions.MaxRepeatedChars);

                    if (lastRepeatedLetters == new string(letter, lastRepeatedLetters.Length)) {
                        stringBuilder.Clear();
                        continue;
                    }
                }

                if (_validText.IsMatch(letterString)) {
                    stringBuilder.Append(letterString);
                    continue;
                }

                var extractedString = stringBuilder.ToString();
                stringBuilder.Clear();

                if (extractedString.Length >= stringExtractionOptions.MinLength) {
                    foundStrings.Add(extractedString);
                }
            }

            return foundStrings.ToArray();
        }

        public Rgba32[] GetRepeatedColors(Image<Rgba32> image, int occurrences = 1) {
            var colorSet = new Dictionary<Rgba32, int>();
            var repeatedColors = new HashSet<Rgba32>();

            for (var x = 0; x < image.Width; x++) {
                for (var y = 0; y < image.Height; y++) {
                    var color = image[x, y];

                    if (!colorSet.ContainsKey(color)) {
                        colorSet.Add(color, 1);
                    }

                    if (colorSet[color] <= occurrences) {
                        if (colorSet[color] == occurrences) {
                            repeatedColors.Add(color);
                        }
                        colorSet[color]++;
                    }
                }
            }

            return repeatedColors.ToArray();
        }

        public Rgba32[] GetUniqueColors(Image<Rgba32> image) {
            var colorSet = new HashSet<Rgba32>();
            var nonUnique = new HashSet<Rgba32>();

            for (var x = 0; x < image.Width; x++) {
                for (var y = 0; y < image.Height; y++) {
                    var color = image[x, y];
                    if (nonUnique.Contains(color)) {
                        continue;
                    }

                    if (!colorSet.Add(color)) {
                        colorSet.Remove(color);
                        nonUnique.Add(color);
                    }
                }
            }

            return colorSet.ToArray();
        }

        public Image<Rgba32> GetUniqueColorImage(Image<Rgba32> image) {
            var pixels = GetUniqueColors(image);

            var resultImage = new Image<Rgba32>(pixels.Length, 1);
            for (var x = 0; x < pixels.Length; x++) {
                resultImage[x, 0] = pixels[x];
            }

            return image;
        }
    }
}
