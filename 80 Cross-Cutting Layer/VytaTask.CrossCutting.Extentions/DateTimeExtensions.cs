using System;

namespace VytaTask.CrossCutting.Extentions
{
    public static class DateTimeExtensions
    {
        public static string ToInternationalFriendlyDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy MMM dd");
        }
    }
}
