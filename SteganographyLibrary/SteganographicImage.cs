﻿using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SteganographyLibrary.Exceptions;
using SteganographyLibrary.Interfaces;
using SteganographyLibrary.Models;

namespace SteganographyLibrary {
    public class SteganographicImage {
        private readonly Image<Rgba32> _rawImage;
        private readonly ILsbEncoder _lsbEncoder;

        private int ByteCapacity {
            get {
                return _rawImage.Width * _rawImage.Height / 2;
            }
        }

        public SteganographicImage(Image<Rgba32> rawImage) {
            _rawImage = rawImage;
            _lsbEncoder = new LsbEncoder();
        }

        public Image<Rgba32> EncodeDataInImage(byte[] encodedData, string aesKey = "") {
            var formattedDataBytes = FormatData(ref encodedData, aesKey);

            var byteIndex = 0;
            var bitIndex = 0;

            for (var x = 0; x < _rawImage.Width; x++) {
                for (var y = 0; y < _rawImage.Height; y++) {
                    var color = _rawImage[x, y];
                    var currentByte = formattedDataBytes[byteIndex];

                    var alpha = _lsbEncoder.SetLsb(color.A, (currentByte & (1 << bitIndex)) > 0);
                    bitIndex++;
                    var red = _lsbEncoder.SetLsb(color.R, (currentByte & (1 << bitIndex)) > 0);
                    bitIndex++;
                    var green = _lsbEncoder.SetLsb(color.G, (currentByte & (1 << bitIndex)) > 0);
                    bitIndex++;
                    var blue = _lsbEncoder.SetLsb(color.B, (currentByte & (1 << bitIndex)) > 0);
                    bitIndex++;

                    color = Color.FromRgba(red, green, blue, alpha);
                    _rawImage[x, y] = color;

                    if (bitIndex >= 8) {
                        bitIndex = 0;
                        byteIndex++;

                        if (byteIndex >= formattedDataBytes.Length) {
                            return _rawImage;
                        }
                    }
                }
            }

            return _rawImage;
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

        public byte[] GetEncodedDataFromImage(string aesKey = "") {
            var encodedData = DecodeImageData();

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

        private byte[] DecodeImageData() {
            var encodedData = new byte[ByteCapacity];

            var byteIndex = 0;
            var bitIndex = 0;

            byte currentByte = 0;
            for (var x = 0; x < _rawImage.Width; x++) {
                for (var y = 0; y < _rawImage.Height; y++) {
                    var color = _rawImage[x, y];

                    var lsb = _lsbEncoder.GetLsb(color.A) ? 1 : 0;
                    currentByte |= (byte)(lsb << bitIndex);
                    bitIndex++;

                    lsb = _lsbEncoder.GetLsb(color.R) ? 1 : 0;
                    currentByte |= (byte)(lsb << bitIndex);
                    bitIndex++;

                    lsb = _lsbEncoder.GetLsb(color.G) ? 1 : 0;
                    currentByte |= (byte)(lsb << bitIndex);
                    bitIndex++;

                    lsb = _lsbEncoder.GetLsb(color.B) ? 1 : 0;
                    currentByte |= (byte)(lsb << bitIndex);
                    bitIndex++;

                    if (bitIndex >= 8) {
                        bitIndex = 0;

                        encodedData[byteIndex] = currentByte;
                        currentByte = 0;
                        byteIndex++;
                        continue;
                    }
                }
            }

            return encodedData;
        }
    }
}