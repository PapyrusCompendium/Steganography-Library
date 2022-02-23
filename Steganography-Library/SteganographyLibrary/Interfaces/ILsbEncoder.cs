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
    }
}