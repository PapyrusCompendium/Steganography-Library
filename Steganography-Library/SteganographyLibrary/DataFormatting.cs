using SteganographyLibrary.Models;

namespace SteganographyLibrary {
    public static class DataFormatting {

        public static byte[] ExtractRawData(byte[] formattedData) {
            return System.Array.Empty<byte>();
        }

        public static byte[] FormatData(byte[] rawData, bool encrypted) {
            var formattedData = new DataFormat(rawData, encrypted);
            return formattedData.GetBytes();
        }
    }
}