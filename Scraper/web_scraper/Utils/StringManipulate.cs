using System;
using System.Text.RegularExpressions;

namespace web_scraper.Utils
{
    public class StringManipulate
    {
        public static string OnlyNumbers(string value)
        {
            return String.Join("", Regex.Split(value, @"[^\d]"));
        }
    }
}