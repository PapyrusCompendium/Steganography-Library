using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Steganography.ImageData {
    public class StegoImage : ISteganographicData {
        private readonly byte[] _data = null;
        public string AESPassword { get; set; } = null;

        public DataType DataType { get; } = DataType.StegoImage;

        public StegoImage(Bitmap bitmapImage) {
            using (var bitmapStream = new MemoryStream()) {
                bitmapImage.Save(bitmapStream, ImageFormat.Png);
                _data = bitmapStream.ToArray();
            }
        }

        public StegoImage(byte[] data) {
            _data = data;
        }

        public byte[] GetData() {
            return _data ?? Array.Empty<byte>();
        }
    }
}