﻿using System.Drawing;
using System.Text;

using Xunit;

namespace SteganographyLibrary.Tests {
    public class SteganographicImageTests {
        private const string TEST_PASSWORD = "Password";

        [Fact]
        public void Does_Work_Both_Ways_No_Encryption() {
            var encodedMessage = "This is an encoded message";

            var stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("TestImage.png")));
            var encodedImage = stegoImage.EncodeDataInBitmap(Encoding.UTF8.GetBytes(encodedMessage));

            stegoImage = new SteganographicImage(encodedImage);
            var encodedData = stegoImage.GetEncodedDataFromBitmap();

            Assert.Equal(encodedMessage, Encoding.UTF8.GetString(encodedData));
        }

        [Fact]
        public void Does_Work_Both_Ways_With_Encryption() {
            var encodedMessage = "This is an encoded message";

            var stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("TestImage.png")));
            var encodedImage = stegoImage.EncodeDataInBitmap(Encoding.UTF8.GetBytes(encodedMessage), TEST_PASSWORD);

            stegoImage = new SteganographicImage(encodedImage);
            var encodedData = stegoImage.GetEncodedDataFromBitmap(TEST_PASSWORD);

            Assert.Equal(encodedMessage, Encoding.UTF8.GetString(encodedData));
        }
    }
}