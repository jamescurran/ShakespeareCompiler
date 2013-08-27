using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shakespeare.Utility
{
    // Utility classes
    public static class StringExts
    {
        public static string str2varname(this string str)
        {
            var sb = new StringBuilder(str.Length);
            bool lastspace = true;

            foreach (char c in str)
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (!lastspace)
                    {
                        lastspace = true;
                        sb.Append('_');
                    }
                }
                else if (!Char.IsSymbol(c))
                {
                    lastspace = false;
                    sb.Append(Char.ToLower(c));
                }
            }
            return sb.ToString();
        }
    }

}
