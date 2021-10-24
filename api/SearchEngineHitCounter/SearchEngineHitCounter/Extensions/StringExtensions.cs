namespace SearchEngineHitCounter.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveNonBreakingSpaces(this string str)
        {
            return str.Replace("&#160;", "");
        }
    }
}
