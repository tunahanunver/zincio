namespace Extensions
{
    public static class StringExtensions
    {
        public static char GetLastLetter(this string word)
        {
            if (string.IsNullOrEmpty(word)) return '\0';
            return word[word.Length - 1];
        }
        public static string ToTurkishUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input
                .Replace('i', 'İ')
                .Replace('ı', 'I')
                .ToUpper();
        }
        public static string ToTurkishLower(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input
                .Replace('İ', 'i')
                .Replace('I', 'ı')
                .ToLower();
        }
        public static string NormalizeTurkish(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.ToTurkishLower().Trim();
        }
    }
}