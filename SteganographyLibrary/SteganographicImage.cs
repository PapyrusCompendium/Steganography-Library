using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SteganographyLibrary.Exceptions;
using SteganographyLibrary.Interfaces;
using SteganographyLibrary.Models;
using SteganographyLibrary.SteganographyReversing;

namespace SteganographyLibrary {
    public class SteganographicImage : ISteganographicImage {
        public Image<Rgba32> Image { get; }

        private readonly ILsbEncoder _lsbEncoder;

        private int ByteCapacity {
            get {
                return Image.Width * Image.Height / 2;
            }
        }

        public SteganographicImage(Image<Rgba32> rawImage) {
            Image = rawImage;
            _lsbEncoder = new LsbEncoder();
        }

        public IImageAnalysis GetImageAnalysis() {
            return new ImageAnalysis(this);
        }

        public byte[] DecodedDataFromImage(string aesKey = default) {
            var encodedData = DecodeRawImageData();

            var dataLength = BitConverter.ToInt32(encodedData, 0);
            var encrypted = BitConverter.ToBoolean(encodedData, 4);
            var data = new byte[dataLength];
            Array.Copy(encodedData, 5, data, 0, data.Length);

            var dataFormatting = new DataFormat(data, encrypted);

            if (dataFormatting.Encrypted && string.IsNullOrEmpty(aesKey)) {
                throw new AesException("Missing Aes Key!");
            }

            if (dataFormatting.Encrypted) {
                var decryptedData = Cryptography.Decrypt(dataFormatting.Data, aesKey);
                dataFormatting = new DataFormat(decryptedData, false);
            }

            return dataFormatting.Data;
        }

        public Image<Rgba32> EncodeDataInImage(byte[] encodedData, string aesKey = default, string encodingOrderMask = "argb") {
            var formattedDataBytes = FormatData(ref encodedData, aesKey);
            return _lsbEncoder.EncodeDataInImage(formattedDataBytes, Image, encodingOrderMask);
        }

        public byte[] DecodeRawImageData(string encodingOrderMask = "argb") {
            return _lsbEncoder.DecodeRawImageData(Image, encodingOrderMask);
        }

        private byte[] FormatData(ref byte[] encodedData, string aesKey) {
            DataFormat dataFormatting;
            if (!string.IsNullOrEmpty(aesKey)) {
                encodedData = Cryptography.Encrypt(encodedData, aesKey);
                dataFormatting = new DataFormat(encodedData, true);
            }
            else {
                dataFormatting = new DataFormat(encodedData, false);
            }

            var formattedDataBytes = dataFormatting.GetBytes();

            return ByteCapacity < formattedDataBytes.Length
                ? throw new CapacityException("Image could not contain the amount of data desired.")
                : formattedDataBytes;
        }
    }
}