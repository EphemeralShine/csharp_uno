using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace UIConsole;

public class Prompts
{
    private static bool ValidateInput(string? input, string pattern)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        return Regex.IsMatch(input.Trim(), pattern);
    }

    public static T Prompt<T>(string prompt, string regex)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var playerChoiceStr = Console.ReadLine()?.Trim();
            if (ValidateInput(playerChoiceStr, regex) == false)
            {
                Console.WriteLine("Wrong input style");
            }
            else
            {
                try
                {
                    T convertedValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(playerChoiceStr)!;
                    return convertedValue;
                }
                catch (Exception)
                {
                    Console.WriteLine("Conversion error");
                }
            }
        }
            
    }

    public static T PromptWithDefault<T>(string prompt, string regex, T defaultValue)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var playerChoiceStr = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(playerChoiceStr))
            {
                return defaultValue;
            }

            if (Regex.IsMatch(playerChoiceStr, regex))
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)playerChoiceStr;
                }
                if (typeof(T) == typeof(int))
                {
                    if (int.TryParse(playerChoiceStr, out int result))
                    {
                        return (T)(object)result;
                    }
                }
            }

            Console.WriteLine("Illegal input!");
        }
    }

}