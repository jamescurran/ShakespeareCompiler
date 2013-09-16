#region Copyright & License summary
/*
 Copyright 2013, James M. Curran, Novel Theory Software

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
#endregion

using System;
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
