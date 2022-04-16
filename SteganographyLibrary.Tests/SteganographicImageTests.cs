using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using Xunit;

namespace SteganographyLibrary.Tests {
    public class SteganographicImageTests {
        private const string TEST_PASSWORD = "Password";

        [Fact]
        public void Does_Work_Both_Ways_No_Encryption() {
            var encodedMessage = "This is an encoded message";

            var stegoImage = new SteganographicImage(Image.Load<Rgba32>("TestImage.png"));
            var encodedImage = stegoImage.EncodeDataInImage(Encoding.UTF8.GetBytes(encodedMessage));

            stegoImage = new SteganographicImage(encodedImage);
            var encodedData = stegoImage.GetEncodedDataFromImage();

            Assert.Equal(encodedMessage, Encoding.UTF8.GetString(encodedData));
        }

        [Fact]
        public void Does_Work_Both_Ways_With_Encryption() {
            var encodedMessage = "This is an encoded message";

            var stegoImage = new SteganographicImage(Image.Load<Rgba32>("TestImage.png"));
            var encodedImage = stegoImage.EncodeDataInImage(Encoding.UTF8.GetBytes(encodedMessage), TEST_PASSWORD);

            stegoImage = new SteganographicImage(encodedImage);
            var encodedData = stegoImage.GetEncodedDataFromImage(TEST_PASSWORD);

            Assert.Equal(encodedMessage, Encoding.UTF8.GetString(encodedData));
        }
    }
}