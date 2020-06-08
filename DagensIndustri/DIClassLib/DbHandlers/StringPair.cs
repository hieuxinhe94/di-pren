namespace DIClassLib.DbHandlers
{
    /// <summary>
    /// Class used to populate DDLs
    /// </summary>
    public class StringPair
    {
        public string S1 { get; set; }
        public string S2 { get; set; }

        public StringPair(string s1, string s2)
        {
            S1 = s1;
            S2 = s2;
        }
    }
}