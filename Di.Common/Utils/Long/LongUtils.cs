namespace Di.Common.Utils.Long
{
    public static class LongUtils
    {
        public static double ConvertBytesToMegabytes(this long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static double ConvertKilobytesToMegabytes(this long kilobytes)
        {
            return kilobytes / 1024f;
        }
    }
}
