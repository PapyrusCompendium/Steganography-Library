namespace SteganographyLibrary.Models {
    public class StringExtractionOptions {
        public int MaxRepeatedChars { get; set; } = 3;
        public int MinLength { get; set; } = 5;
        public bool IsWord { get; set; }
    }
}
