using System;
using System.Drawing;

using Steganography.ImageData;

namespace Steganography {
    public class SteganographicImage : ISteganographicImage {
        private readonly Bitmap _bitmap;
        public SteganographicImage(Bitmap bitmap) {
            _bitmap = bitmap;
        }

        public void EncodeImageData(ISteganographicData steganographicData) {
            var steganographicDataBytes = EncodedData.GetBytes(steganographicData);
            
        }
    }
}