using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steganography.ImageData {
    public static class EncodedData {

        public static byte[] GetBytes(ISteganographicData steganographicData) {
            var stegoData = steganographicData.GetData();

            // Should be = to 12. But for best practice we'll define it.
            var preambleLength = sizeof(DataType) + sizeof(ulong);
            var buffer = new byte[preambleLength + stegoData.Length];

            BitConverter.GetBytes((int)steganographicData.DataType).CopyTo(buffer, 0);
            BitConverter.GetBytes(stegoData.Length).CopyTo(buffer, sizeof(DataType));
            Buffer.BlockCopy(stegoData, 0, buffer, preambleLength, stegoData.Length);

            return buffer;
        }

        public static ISteganographicData GetSteganographicData(byte[] data) {
            var dataType = BitConverter.ToInt32(data, 0);
            var stegoData = data.Skip(sizeof(DataType) + sizeof(ulong)).ToArray();

            ISteganographicData steganographicData = (DataType)dataType switch {
                DataType.StegoBytes => new StegoBytes(stegoData),
                DataType.StegoString => new StegoString(stegoData),
                DataType.StegoImage => new StegoImage(stegoData),
                _ => new StegoBytes(stegoData)
            };

            return steganographicData;
        }
    }
}