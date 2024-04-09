using System;

namespace CodeBase.Extentions.Time
{
    public static class UnixTimeExtentions
    {
        private static DateTime _epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return (long)(dateTime - _epochStart).TotalSeconds;
        }

        public static DateTime FromUnixTimeSecondsToLocal(this long unixTimeSeconds)
        {
            return _epochStart.AddSeconds(unixTimeSeconds).ToLocalTime();
        }
    }
}