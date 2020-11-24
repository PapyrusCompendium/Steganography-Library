using Steganography_Library;
using System;
using System.Drawing;
using System.Text;

namespace stegolibtest
{
    class Program
    {
        static void Main()
        {
            //Must be SAVED as a png, can be loaded FROM any format!
            SteganographicImage stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("Image.png")));
            stegoImage.EncodeImage(Encoding.UTF8.GetBytes("Secret Message")).Save("NewImage.png");

            //Decoding, load from png
            stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("NewImage.png")));
            Console.WriteLine(stegoImage.DecodeImage());
        }
    }
}