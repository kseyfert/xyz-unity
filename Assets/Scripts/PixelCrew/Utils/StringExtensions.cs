namespace PixelCrew.Utils
{
    public static class StringExtensions
    {
        public static string Capitalize(this string s)
        {
            var words = s.Split(' ');
            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                words[i] = char.ToUpper(word[0]) + word.Substring(1);
            }
            
            return string.Join(" ", words);
        }
    }
}