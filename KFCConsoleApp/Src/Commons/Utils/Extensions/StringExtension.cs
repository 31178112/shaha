using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string Translate(this string text)
    {
        var dictionary = KFCConsoleApp.Src.Commons.Service.LocalizationService.dictionary;
        return dictionary?.GetValueOrDefault(text) ?? text;
    }

    public static int ParseStringToInt(this string? input)
    {
        int.TryParse(input, out int result);
        return result;
    }

    public static bool validationName(this string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        var pattern = @"^[a-zA-Zа-яА-ЯёЁ]{2,20}$";
        return Regex.IsMatch(name, pattern);
    }

    public static bool validationPassword(this string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        return password.Length >= 6;
    }

    public static bool validationEmail(this string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}
