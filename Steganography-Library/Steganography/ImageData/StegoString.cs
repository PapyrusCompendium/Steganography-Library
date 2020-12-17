using System;
using System.Collections.Generic;
using System.Text;

namespace Steganography.ImageData {
    public class StegoString : ISteganographicData {
        private readonly byte[] _data = null;
        public string AESPassword { get; set; } = null;

        public DataType DataType { get; } = DataType.StegoString;

        public StegoString(string data) {
            _data = Encoding.UTF8.GetBytes(data);
        }

        public StegoString(byte[] data) {
            _data = data;
        }

        public byte[] GetData() {
            return _data ?? Array.Empty<byte>();
        }

        public override string ToString() {
            return Encoding.UTF8.GetString(GetData());
        }
    }
}