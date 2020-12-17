using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Steganography.ImageData {
    public class StegoBytes : ISteganographicData {
        private readonly byte[] _data = null;
        public string AESPassword { get; set; } = null;

        public DataType DataType { get; } = DataType.StegoBytes;

        public StegoBytes(byte[] data) {
            _data = data;
        }

        public byte[] GetData() {
            return _data ?? Array.Empty<byte>();
        }
    }
}