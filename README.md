# Steganography
[![Github All Releases](https://img.shields.io/github/downloads/PapyrusCompendium/Steganography-Library/total.svg)]()
[![Github Issues](https://img.shields.io/github/issues/PapyrusCompendium/Steganography-Library.svg)]()
[![Github Stars](https://img.shields.io/github/stars/PapyrusCompendium/Steganography-Library.svg)]()
[![Github License](https://img.shields.io/github/license/PapyrusCompendium/Steganography-Library.svg)]()

This is my own implementation of steganography, using a LSB (Least Significant Bit) method.  
The way this works is by taking the LSB for every byte in a pixel (There are four in this case ARGB)  
and changing the LSB to one bit of the data that you are trying to store. For example if I have  
one byte of data I would like to store, I could store it in two pixels. I can store 4 bits  
in one pixel without modifying the image enough that it is visible to the naked eye. In this  
implementation I am only storing string data, but this can be made to store anything from files  
to other images completely, like QR codes.  

# How do I use this?
This version of my implementation is built for developer advantage. It is a consumable code library giving developers ease  
of access to writing their own steganography tools in C#. Simply import this DLL to your code base and you are ready to go!  
```cs
using System.Drawing;

using SteganographyLibrary;

namespace Steganography
{
    class Program
    {
        static void Main(string[] args)
        {
            // Must be SAVED as a png, can be loaded FROM any format!
            var stegoImage = new SteganographicImage(new Bitmap(Image.FromFile("Image.png")));
            var encodedImage = stegoImage.EncodeImage(Encoding.UTF8.GetBytes("Secret Message"));

            // Decoding, load from png
            stegoImage = new SteganographicImage(encodedImage);
            Console.WriteLine(stegoImage.DecodeImage());
        }
    }
}
```

# Bugs and issues
If you encounter and bugs or issues, feel free to open an issue on GitHub. I will address these issues as soon as I can.