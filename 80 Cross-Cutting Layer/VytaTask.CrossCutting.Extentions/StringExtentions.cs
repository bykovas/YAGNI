using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VytaTask.CrossCutting.Extentions
{
    public static class StringExtentions
    {
        /// <summary>
        /// Truncates given string
        /// </summary>
        /// <param name="input">Original string</param>
        /// <param name="lengts">Lenght of desired truncated string</param>
        /// <param name="addDots">By default will add "..." at the end af truncated string</param>
        /// <returns>string</returns>
        public static string TruncateString(string input, int lengts, bool addDots = true)
        {
            var lenght = lengts >= input.Length ? lengts : input.Length;
            lenght = addDots ? lenght - 3 : lenght;
            return input.Substring(0, lenght) + (addDots ? "..." : string.Empty);
        }
    }
}
