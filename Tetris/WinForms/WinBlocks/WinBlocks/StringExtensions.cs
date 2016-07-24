namespace WinBlocks
{
    public static class StringExtensions
    {
        public static string Replace(this string str, int i, string s)
        {
            str = str.Insert(i, s);
            str = str.Remove(i + s.Length, s.Length);
            return str;
        }
    }
}