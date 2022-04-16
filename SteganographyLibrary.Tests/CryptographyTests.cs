using Xunit;

namespace SteganographyLibrary.Tests {
    public class CryptographyTests {
        private const string TEST_PASSWORD = "password";

        [Fact]
        public void Does_Crypography_Work_Both_Ways() {
            var data = new byte[] { 5, 4, 8, 6, 3, 1, 1, 4, 8, 6, 3, 21, 255, 45, 8, 4, 6, 8, 4, 22 };
            var cipherData = Cryptography.Encrypt(data, TEST_PASSWORD);
            var plainData = Cryptography.Decrypt(cipherData, TEST_PASSWORD);

            Assert.Equal(data, plainData);
        }
    }
}