using System.Drawing;

using SteganographyLibrary.Interfaces;
using SteganographyLibrary.Models;

namespace SteganographyLibrary {
    public class SteganographicImage {
        private readonly Bitmap _rawImage;
        private readonly ILsbEncoder _lsbEncoder;

        public SteganographicImage(Bitmap rawImage) {
            _rawImage = rawImage;
            _lsbEncoder = new LsbEncoder();
        }

        public Bitmap EncodeDataInBitmap(byte[] encodedData, string aesKey = "") {
            DataFormat dataFormatting;
            if (string.IsNullOrEmpty(aesKey)) {
                encodedData = Cryptography.Encrypt(encodedData, aesKey);
                dataFormatting = new DataFormat(encodedData, true);
            }
            else {
                dataFormatting = new DataFormat(encodedData, false);
            }

            var formattedDataBytes = dataFormatting.GetBytes();

            for (var x = 0; x < _rawImage.Width; x++) {
                for (var y = 0; y < _rawImage.Height; y++) {
                    
                }
            }
        }

        public byte[] GetEncodedDataFromBitmap(string aesKey = "") {
            throw new System.Exception();
        }
    }
}