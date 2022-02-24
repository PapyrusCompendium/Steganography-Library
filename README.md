# Steganography
[![Github All Releases](https://img.shields.io/github/downloads/PapyrusCompendium/Steganography-Library/total.svg)]()
[![Github Issues](https://img.shields.io/github/issues/PapyrusCompendium/Steganography-Library.svg)]()
[![Github Stars](https://img.shields.io/github/stars/PapyrusCompendium/Steganography-Library.svg)]()
[![Github License](https://img.shields.io/github/license/PapyrusCompendium/Steganography-Library.svg)]()

# Online Version
Try an online implementation of this lib here:  
https://papyruscompendium.dev/Steganography

This is my own implementation of steganography, using a LSB (Least Significant Bit) method.  
The way this works is by taking the LSB for every byte in a pixel (There are four in this case ARGB)  
and changing the LSB to one bit of the data that you are trying to store. For example if I have  
one byte of data I would like to store, I could store it in two pixels. I can store 4 bits  
in one pixel without modifying the image enough that it is visible to the naked eye. In this  
implementation I am only storing string data, but this can be made to store anything from files  
to other images completely, like QR codes.  

```cs
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using Xunit;

namespace Steganography
{
    class Program
    {
        static void Main(string[] args)
        {
            var encodedMessage = "This is an encoded message";

            var stegoImage = new SteganographicImage(Image.Load<Rgba32>("TestImage.png"));
            var encodedImage = stegoImage.EncodeDataInImage(Encoding.UTF8.GetBytes(encodedMessage));

            stegoImage = new SteganographicImage(encodedImage);
            var encodedData = stegoImage.GetEncodedDataFromImage();
            
            Assert.Equal(encodedMessage, Encoding.UTF8.GetString(encodedData));
        }
    }
}
```

# Bugs and issues
If you encounter and bugs or issues, feel free to open an issue on GitHub. I will address these issues as soon as I can.
