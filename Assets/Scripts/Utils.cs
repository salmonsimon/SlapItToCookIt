using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public static class Utils
{
    private static System.Random random = new System.Random();

    public static string FloatToTimeFormat(float timeInput)
    {
        double timePlayedDouble = (double)timeInput;

        System.TimeSpan time = System.TimeSpan.FromSeconds(timePlayedDouble);

        string displayTime = time.ToString("hh':'mm':'ss");

        return displayTime;
    }


    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static Dictionary<string, string> ToDictionary(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        return dictionary;
    }
}
