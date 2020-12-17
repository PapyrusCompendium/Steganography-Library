using System;

using Steganography.ImageData;

using Xunit;

namespace Steganography_CoreTest {
    public class EncodedDataTests {
        [Fact]
        public void TestDataConvert() {
            var testString = "This is a test String";
            var stegoString = new StegoString(testString);

            var stegoData = EncodedData.GetBytes(stegoString);
            var iStegoData = EncodedData.GetSteganographicData(stegoData);

            Assert.True(iStegoData is StegoString);

            var decodedString = (iStegoData as StegoString).ToString();

            Assert.Equal((ulong)iStegoData.GetData().Length,
                BitConverter.ToUInt64(stegoData, sizeof(DataType)));

            Assert.Equal((int)iStegoData.DataType,
                BitConverter.ToInt32(stegoData, 0));

            Assert.Equal(testString, decodedString);
        }
    }
}