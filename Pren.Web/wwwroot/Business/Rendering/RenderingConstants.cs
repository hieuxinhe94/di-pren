namespace Pren.Web.Business.Rendering
{
    public class RenderingConstants
    {
        /// <summary>
        /// Tags to use for the main widths used in the Bootstrap HTML framework
        /// </summary>
        public static class ContentAreaTags
        {
            public const string FullWidth = "col-md-12";
            public const string FullWidthClear = "col-md-12 clearfix";
            public const string TwoThirdsWidth = "col-lg-8 col-md-8 col-sm-8 col-xs-12";
            public const string TwoThirdsWidthClear = "col-lg-8 col-md-8 col-sm-8 col-xs-12 clearfix";
            public const string HalfWidth = "col-lg-6 col-md-6 col-sm-6 col-xs-12";
            public const string HalfWidthClear = "col-lg-6 col-md-6 col-sm-6 col-xs-12 clearfix";
            public const string OneThirdWidth = "col-lg-4 col-md-4 col-sm-6 col-xs-12";
            public const string OneThirdWidthClear = "col-lg-4 col-md-4 col-sm-6 col-xs-12 clearfix";


            public const string QuarterWidth = "col-lg-3 col-md-3 col-sm-4 col-xs-6";
            public const string QuarterWidthClear = "col-lg-3 col-md-3 col-sm-4 col-xs-6 clearfix";
            public const string OneSixthWidth = "col-lg-2 col-md-2 col-sm-4 col-xs-6";
            public const string OneSixthWidthClear = "col-lg-2 col-md-2 col-sm-4 col-xs-6 clearfix";
            public const string NoRenderer = "norenderer";
        }

        ///// <summary>
        ///// Main widths used in the Bootstrap HTML framework
        ///// </summary>
        //public static class ContentAreaWidths
        //{
        //    public const int FullWidth = 12;
        //    public const int TwoThirdsWidth = 8;
        //    public const int HalfWidth = 6;
        //    public const int OneThirdWidth = 4;
        //    public const int OneSixthWidth = 2;
        //}

        //public static Dictionary<string, int> ContentAreaTagWidths = new Dictionary<string, int>
        //    {
        //        { ContentAreaTags.FullWidth, ContentAreaWidths.FullWidth },
        //        { ContentAreaTags.TwoThirdsWidth, ContentAreaWidths.TwoThirdsWidth },
        //        { ContentAreaTags.HalfWidth, ContentAreaWidths.HalfWidth },
        //        { ContentAreaTags.OneThirdWidth, ContentAreaWidths.OneThirdWidth },
        //        { ContentAreaTags.OneSixthWidth, ContentAreaWidths.OneSixthWidth }
        //    };
    }
}
