using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SteganographyLibrary.Interfaces {
    public interface ILsbEncoder {
        /// <summary>
        /// Sets the least significant bit of a byte to the desired state.
        /// </summary>
        /// <param name="existingByte"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        byte SetLsb(byte existingByte, bool state);

        /// <summary>
        /// Returns the state of the least significant bit.
        /// </summary>
        /// <param name="existingByte"></param>
        /// <returns></returns>
        bool GetLsb(byte existingByte);

        /// <summary>
        /// Deocde an image using a specified color channel sequence.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="encodingOrderMask"></param>
        /// <returns></returns>
        byte[] DecodeRawImageData(Image<Rgba32> image, string encodingOrderMask = "argb");

        /// <summary>
        /// Encode an image using a specified color channel sequence.
        /// </summary>
        /// <param name="encodedData"></param>
        /// <param name="image"></param>
        /// <param name="encodingOrderMask"></param>
        /// <returns></returns>
        Image<Rgba32> EncodeDataInImage(byte[] encodedData, Image<Rgba32> image, string encodingOrderMask = "argb");
    }
}