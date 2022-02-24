using System;

namespace SteganographyLibrary.Exceptions {
    public class CapacityException : Exception {
        public CapacityException(string message) : base(message) {
        }
    }
}
