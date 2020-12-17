using System;
using System.Collections.Generic;
using System.Text;

namespace Steganography.ImageData {
    public interface ISteganographicData {
        /// <summary>
        /// The AES password, if the data is not encrypted this will be null.
        /// </summary>
        string AESPassword { get; set; }

        /// <summary>
        /// Returns the data in bytes to be stored.
        /// </summary>
        /// <returns></returns>
        byte[] GetData();

        /// <summary>
        /// Defines the data type that is encoded into an image.
        /// </summary>
        DataType DataType { get; }
    }

    public enum DataType : int {
        StegoString,
        StegoBytes,
        StegoImage
    }
}