using System;

namespace SteganographyLibrary.Exceptions {
    public class AesException : Exception {
        public AesException(string message) : base(message) {
        }
    }
}
