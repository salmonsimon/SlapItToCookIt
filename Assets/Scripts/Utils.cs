public static class Utils
{
    public static string FloatToTimeFormat(float timeInput)
    {
        double timePlayedDouble = (double)timeInput;

        System.TimeSpan time = System.TimeSpan.FromSeconds(timePlayedDouble);

        string displayTime = time.ToString("hh':'mm':'ss");

        return displayTime;
    }
}
