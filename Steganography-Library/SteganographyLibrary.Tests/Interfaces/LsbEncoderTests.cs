using SteganographyLibrary.Interfaces;

using Xunit;

namespace SteganographyLibrary.Tests.Interfaces {
    public class LsbEncoderTests {
        private readonly ILsbEncoder _lsbEncoder;
        public LsbEncoderTests() {
            _lsbEncoder = new LsbEncoder();
        }

        [Fact]
        public void Does_Get_Correct_Lsb_When_Empty() {
            var testByte = (byte)0b_10000000;

            var answer = _lsbEncoder.GetLsb(testByte);
            Assert.False(answer);
        }

        [Fact]
        public void Does_Get_Correct_Lsb_When_Not_Empty() {
            var testByte = (byte)0b_00000001;

            var answer = _lsbEncoder.GetLsb(testByte);
            Assert.True(answer);
        }

        [Fact]
        public void Does_Set_Correct_Lsb_When_Empty_SetFalse() {
            var testByte = (byte)0b_00000000;

            var newByte = _lsbEncoder.SetLsb(testByte, false);
            var answer = _lsbEncoder.GetLsb(newByte);
            Assert.False(answer);
        }

        [Fact]
        public void Does_Set_Correct_Lsb_When_Not_Empty_SetFalse() {
            var testByte = (byte)0b_00000001;

            var newByte = _lsbEncoder.SetLsb(testByte, false);
            var answer = _lsbEncoder.GetLsb(newByte);
            Assert.False(answer);
        }

        [Fact]
        public void Does_Set_Correct_Lsb_When_Empty_SetTrue() {
            var testByte = (byte)0b_00000000;

            var newByte = _lsbEncoder.SetLsb(testByte, true);
            var answer = _lsbEncoder.GetLsb(newByte);
            Assert.True(answer);
        }

        [Fact]
        public void Does_Set_Correct_Lsb_When_Not_Empty_SetTrue() {
            var testByte = (byte)0b_00000001;

            var newByte = _lsbEncoder.SetLsb(testByte, true);
            var answer = _lsbEncoder.GetLsb(newByte);
            Assert.True(answer);
        }
    }
}