using System;

namespace SteganographyLibrary.Models {
    public class DataFormat {
        public int Length { get; set; }
        public bool Encrypted { get; set; }
        public byte[] Data { get; set; }

        public DataFormat(byte[] rawData, bool encrypted) {
            Data = rawData;
            Length = rawData.Length;
            Encrypted = encrypted;
        }

        public byte[] GetBytes() {
            var bytes = new byte[Data.Length + 4 + 1];

            var lengthBytes = BitConverter.GetBytes(Length);
            Array.Copy(lengthBytes, 0, bytes, 0, 4);

            var encryptedBoolBytes = BitConverter.GetBytes(Encrypted);
            Array.Copy(encryptedBoolBytes, 0, bytes, 4, 1);

            Array.Copy(Data, 0, bytes, 5, Data.Length);

            return bytes;
        }
    }
}