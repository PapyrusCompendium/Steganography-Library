using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Steganography;

using Xunit;

namespace Steganography_CoreTest {
    public class EncodedLSBTests {

        [Fact]
        public void TestEncodingLSB() {
            byte binaryLiteral = 0b_0010_1010;

            var colorArray = new Color[2]{
                Color.Red,
                Color.Blue
            };

            var bits = new BitArray(binaryLiteral);

            foreach (var color in colorArray) {
                color.EncodeLSBSet();
            }
        }
    }
}