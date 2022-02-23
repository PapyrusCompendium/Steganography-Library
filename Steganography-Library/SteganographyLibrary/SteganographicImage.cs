using System.Drawing;

using SteganographyLibrary.Interfaces;
using SteganographyLibrary.Models;

namespace SteganographyLibrary {
    public class SteganographicImage {
        private readonly Bitmap _rawImage;
        private readonly ILsbEncoder _lsbEncoder;
        private int ByteCapacity {
            get {
                return _rawImage.Width * _rawImage.Height / 2;
            }
        }

        public SteganographicImage(Bitmap rawImage) {
            _rawImage = rawImage;
            _lsbEncoder = new LsbEncoder();
        }

        public Bitmap EncodeDataInBitmap(byte[] encodedData, string aesKey = "") {
            var formattedDataBytes = FormatData(ref encodedData, aesKey);

            var byteIndex = 0;
            var bitIndex = 0;

            for (var x = 0; x < _rawImage.Width; x++) {
                for (var y = 0; y < _rawImage.Height; y++) {
                    var color = _rawImage.GetPixel(x, y);
                    var currentByte = formattedDataBytes[byteIndex];

                    _lsbEncoder.SetLsb(color.A, (currentByte & (1 << bitIndex)) == 1);
                    bitIndex++;
                    _lsbEncoder.SetLsb(color.R, (currentByte & (1 << bitIndex)) == 1);
                    bitIndex++;
                    _lsbEncoder.SetLsb(color.G, (currentByte & (1 << bitIndex)) == 1);
                    bitIndex++;
                    _lsbEncoder.SetLsb(color.B, (currentByte & (1 << bitIndex)) == 1);
                    bitIndex++;

                    _rawImage.SetPixel(x, y, color);

                    if (bitIndex >= 8) {
                        bitIndex = 0;
                        bitIndex++;
                    }
                }
            }

            return _rawImage;
        }

        private byte[] FormatData(ref byte[] encodedData, string aesKey) {
            DataFormat dataFormatting;
            if (string.IsNullOrEmpty(aesKey)) {
                encodedData = Cryptography.Encrypt(encodedData, aesKey);
                dataFormatting = new DataFormat(encodedData, true);
            }
            else {
                dataFormatting = new DataFormat(encodedData, false);
            }

            var formattedDataBytes = dataFormatting.GetBytes();

            if (ByteCapacity < formattedDataBytes.Length) {
                throw new System.Exception("Image could not contain the amount of data desired.");
            }

            return formattedDataBytes;
        }

        public byte[] GetEncodedDataFromBitmap(string aesKey = "") {

        }
    }
}