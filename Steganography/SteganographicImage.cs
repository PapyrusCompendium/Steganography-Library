using System;
using System.Collections.Generic;
using System.Drawing;

using Steganography.ImageData;

namespace Steganography {
    public class SteganographicImage : ISteganographicImage {
        private readonly Bitmap _bitmap;

        public ulong ByteCapacity {
            get {
                var pixelArea = (ulong)_bitmap.Width * (ulong)_bitmap.Height;
                return pixelArea / 2;
            }
        }

        public SteganographicImage(Bitmap bitmap) {
            _bitmap = bitmap;
        }

        public void EncodeImageData(ISteganographicData steganographicData) {
            var steganographicDataBytes = EncodedData.GetBytes(steganographicData);
        }

        private IEnumerable<byte> GetEncodedPixles(byte[] encodeData) {
            for (var x = 0; x < _bitmap.Width; x++) {
                for (var y = 0; y < _bitmap.Height; y++) {
                    yield return _bitmap.GetPixel(x, y).R;
                }
            }
        }
    }
}