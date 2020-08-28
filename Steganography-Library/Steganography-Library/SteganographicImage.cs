using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    public class SteganographicImage
    {
        public Bitmap Stegnograph { get; private set; }
        public SteganographicImage(Bitmap image) =>
            Stegnograph = image;

        public bool FitsIntoImage(byte[] data) =>
            data.Length <= TotalCapacity;

        public int TotalCapacity =>
            ((Stegnograph.Width * Stegnograph.Height) - 8) / 2;

        private bool GetLSB(byte value) =>
            (value & 1) != 0;

        private byte[] GetColourBytes(Color colour) =>
            new byte[4] { colour.A, colour.R, colour.G, colour.B };

        private byte SetLSB(bool bit, byte value)
        {
            if (GetLSB(value) && !bit)
                return (byte)(value ^ 1); //Will flip the LSB to 0 if it is 1
            else if (!GetLSB(value) && bit)
                return (byte)(value | 1); //Will flip the LSB to 1 if it is 0
            return value;
        }

        private byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public byte[] DecodeImage(bool encrypted = false, string key = "")
        {
            bool[] bits = new bool[32];

            //We only want to read 8 pixels to decode the first 4 bytes, thats contains the length of the total data.
            for (int x = 0; x < Math.Ceiling(8d / Stegnograph.Width); x++)
                for (int y = 0; y < (Stegnograph.Height > 8 ? 8 : Stegnograph.Height); y++)
                {
                    byte[] colourBytes = GetColourBytes(Stegnograph.GetPixel(x, y));

                    for (int i = 0; i < 4; i++)
                        bits[(((x * Stegnograph.Width) + y) * 4) + i] = GetLSB(colourBytes[i]);
                }

            byte[] lengthBytes = BitArrayToByteArray(new BitArray(bits));
            int length = BitConverter.ToInt32(lengthBytes, 0);

            if (encrypted)
                return Cryptography.Decrypt(DecodeImageLSB(length), key);
            else
                return DecodeImageLSB(length);
        }

        private byte[] DecodeImageLSB(int length)
        {
            int totalPixelsEncoded = length * 2;
            bool[] bits = new bool[length * 8];

            for (int x = 0; x < Stegnograph.Width; x++)
                for (int y = 0; y < Stegnograph.Height; y++)
                {
                    if ((x * Stegnograph.Width) + y < 8)
                        continue;

                    if ((x * Stegnograph.Width) + y - 8 >= totalPixelsEncoded)
                        return BitArrayToByteArray(new BitArray(bits.ToArray()));

                    byte[] colourBytes = GetColourBytes(Stegnograph.GetPixel(x, y));

                    for (int i = 0; i < 4; i++)
                        bits[(((x * Stegnograph.Width) + y) * 4) + i - 32] = GetLSB(colourBytes[i]);
                }

            return BitArrayToByteArray(new BitArray(bits.ToArray()));
        }

        public Bitmap EncodeImage(byte[] data, bool encrypted = false, string key = "")
        {
            if (encrypted)
                data = Cryptography.Encrypt(data, key);

            byte[] formattedData = new byte[data.Length + 4];
            BitConverter.GetBytes((Int32)data.Length).CopyTo(formattedData, 0);
            data.CopyTo(formattedData, 4);

            Stegnograph = EncodeImageLSB(formattedData);
            return Stegnograph;
        }

        private Bitmap EncodeImageLSB(byte[] data)
        {
            Bitmap bitMap = new Bitmap(Stegnograph.Width, Stegnograph.Height);

            using (Graphics graphics = Graphics.FromImage(bitMap))
                graphics.DrawImage(Stegnograph, 0, 0);

            BitArray bits = new BitArray(data);

            for (int x = 0; x < Stegnograph.Width; x++)
                for (int y = 0; y < Stegnograph.Height; y++)
                {
                    if ((((x * Stegnograph.Width) + y) * 4) >= bits.Length)
                        return bitMap;

                    byte[] colourBytes = GetColourBytes(bitMap.GetPixel(x, y));
                    for (int i = 0; i < 4; i++)
                        colourBytes[i] = SetLSB(bits[(((x * Stegnograph.Width) + y) * 4) + i], colourBytes[i]);

                    bitMap.SetPixel(x, y, Color.FromArgb(colourBytes[0], colourBytes[1], colourBytes[2], colourBytes[3]));
                }

            return bitMap;
        }
    }
}