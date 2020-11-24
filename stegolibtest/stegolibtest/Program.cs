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
			SteganographicImage stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("TestImages/Image.png")));
			var encodedimage = stegoImage.EncodeImage(Encoding.UTF8.GetBytes("Secret Message"));
			encodedimage.Save("TestImages/NewImage.png");

			//Decoding, load from png
			stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("TestImages/NewImage.png")));
			string decoded = Encoding.UTF8.GetString(stegoImage.DecodeImage());
			Console.WriteLine(decoded);
		}
	}
}