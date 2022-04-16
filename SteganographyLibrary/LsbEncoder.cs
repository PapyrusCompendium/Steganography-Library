using System.Runtime.CompilerServices;

using SteganographyLibrary.Interfaces;

namespace SteganographyLibrary {
    public class LsbEncoder : ILsbEncoder {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetLsb(byte existingByte) {
            return (existingByte & 1) == 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte SetLsb(byte existingByte, bool state) {
            return state
                ? (existingByte |= 1)
                : (byte)((existingByte >> 1) << 1);
        }
    }
}