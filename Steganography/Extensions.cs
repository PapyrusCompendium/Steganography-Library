using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Steganography {
    public static class Extensions {
        public static Color EncodeLSBSet(this Color color, bool[] bits) {
            if (bits.Length != 4) {
                throw new Exception("There should be 4 color bits when encoding LSB.");
            }

            var colorBytes = new byte[4] {
                color.A,
                color.R,
                color.G,
                color.B
            };

            for (var x = 0; x < 4; x++) {
                colorBytes[x].SetLSB(bits[x]);
            }

            return Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2], colorBytes[3]);
        }

        public static bool[] DecodeLSBSet(this Color color) {
            var bits = new bool[4];

            var colorBytes = new byte[4] {
                color.A,
                color.R,
                color.G,
                color.B
            };

            for (var x = 0; x < 4; x++) {
                bits[x] = colorBytes[x].GetLSB();
            }

            return bits;
        }

        private static byte SetLSB(this byte valueByte, bool bit) {
            if (valueByte.GetLSB() && !bit) {
                return (byte)(valueByte ^ 1);
            }
            else if (!valueByte.GetLSB() && bit) {
                return (byte)(valueByte | 1);
            }
            return valueByte;
        }

        private static bool GetLSB(this byte valueByte) {
            return (valueByte & 1) != 0;
        }
    }
}