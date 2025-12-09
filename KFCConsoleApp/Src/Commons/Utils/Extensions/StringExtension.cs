using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string Translate(this string text)
    {
        return text; // Простая версия
    }

    public static int ParseStringToInt(this string? input)
    {
        if (string.IsNullOrEmpty(input)) return 0;
        int.TryParse(input, out int result);
        return result;
    }
}
