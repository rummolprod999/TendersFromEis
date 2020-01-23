using System.Runtime.CompilerServices;

namespace TendersFromEis.Extensions
{
    public static class StringExtension
    {
        public static string CleanStringXml(this string str)
        {
            var st = str;
            st = st.Replace("ns2:", "");
            st = st.Replace("ns3:", "");
            st = st.Replace("ns4:", "");
            st = st.Replace("ns5:", "");
            st = st.Replace("ns6:", "");
            st = st.Replace("ns7:", "");
            st = st.Replace("ns8:", "");
            st = st.Replace("ns9:", "");
            st = st.Replace("oos:", "");
            st = st.Replace("", "");
            return st;
        }
    }
}