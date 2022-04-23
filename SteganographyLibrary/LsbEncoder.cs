using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SteganographyLibrary.Interfaces;

namespace SteganographyLibrary {
    public class LsbEncoder : ILsbEncoder {

        private readonly Type _colorReflection = typeof(Rgba32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetLsb(byte existingByte) {
            return (existingByte & 1) == 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte SetLsb(byte existingByte, bool state) {
            return state
                ? (existingByte |= 1)
                : (byte)((existingByte >> 1) << 1);
        }

        public byte[] DecodeRawImageData(Image<Rgba32> image, string encodingOrderMask = "argb") {
            CheckOrderMask(encodingOrderMask);
            var encodedData = new byte[image.Width * image.Height / 2];

            var byteIndex = 0;
            var bitIndex = 0;
            byte currentByte = 0;
            for (var x = 0; x < image.Width; x++) {
                for (var y = 0; y < image.Height; y++) {

                    var color = image[x, y];

                    foreach (var encodeField in encodingOrderMask) {
                        var fieldValue = GetColorChannelValue(color, encodeField);
                        PushLeastSignificantBit(ref currentByte, (byte)fieldValue);
                    }

                    if (bitIndex >= 8) {
                        bitIndex = 0;

                        encodedData[byteIndex] = currentByte;
                        currentByte = 0;
                        byteIndex++;
                    }
                }
            }

            return encodedData;

            void PushLeastSignificantBit(ref byte currentByte, byte colorChannel) {
                var lsb = GetLsb(colorChannel) ? 1 : 0;
                currentByte |= (byte)(lsb << bitIndex);
                bitIndex++;
            }
        }

        public Image<Rgba32> EncodeDataInImage(byte[] encodedData, Image<Rgba32> image, string encodingOrderMask = "argb") {
            CheckOrderMask(encodingOrderMask);

            var byteIndex = 0;
            var bitIndex = 0;
            for (var x = 0; x < image.Width; x++) {
                for (var y = 0; y < image.Height; y++) {
                    var color = image[x, y];
                    var currentByte = encodedData[byteIndex];

                    var encodedColorChannels = new Dictionary<char, byte>();
                    foreach (var encodeField in encodingOrderMask) {
                        var fieldValue = GetColorChannelValue(color, encodeField);

                        var encodedColorChannel = SetLsb((byte)fieldValue, (currentByte & (1 << bitIndex)) > 0);
                        encodedColorChannels.Add(encodeField, encodedColorChannel);
                        bitIndex++;
                    }

                    color = Color.FromRgba(encodedColorChannels['r'],
                        encodedColorChannels['g'],
                        encodedColorChannels['b'],
                        encodedColorChannels['a']);

                    image[x, y] = color;

                    if (bitIndex >= 8) {
                        bitIndex = 0;
                        byteIndex++;

                        if (byteIndex >= encodedData.Length) {
                            return image;
                        }
                    }
                }
            }

            return image;
        }

        private object GetColorChannelValue(Rgba32 color, char encodeField) {
            var colorField = _colorReflection.GetField(encodeField.ToString().ToUpper());
            if (colorField is null) {
                throw new Exception("Color channel could not be found from mask.");
            }

            var fieldValue = colorField.GetValue(color);
            return fieldValue is null
                ? throw new Exception("Color channel value was null.")
                : fieldValue;
        }

        private static void CheckOrderMask(string encodingOrder) {
            if (encodingOrder.Length != 4
                || !encodingOrder.Contains("a")
                || !encodingOrder.Contains("r")
                || !encodingOrder.Contains("g")
                || !encodingOrder.Contains("b")) {
                throw new Exception($"[{nameof(encodingOrder)}] '{encodingOrder}' does not contain every color channel.");
            }
        }
    }
}