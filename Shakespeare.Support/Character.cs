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

using Shakespeare.Support.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shakespeare.Support
{
    [DebuggerDisplay("{DebugStr}")]
    public class Character : IEquatable<Character>, IKeySearchable<string>
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public bool OnStage { get; set; }
        public Stack<int> Stack { get; set; }


        public Character(string name)
        {
            Name = name;
            OnStage = false;
            Stack = new Stack<int>();
            Value = 0;
        }

        public bool Equals(Character other)
        {
            return this.Name == other.Name;
        }

        public string Key
        {
            get { return Name; }
        }

        public bool ValueIsCharacter
        {
            get { return Char.MinValue <= Value && Value <= Char.MaxValue; }
        }

        public string DebugStr
        {
            get 
            {
                var chr = (char) Value;
                var strChar = "";
                if (Char.IsSymbol(chr) || Char.IsLetterOrDigit(chr))
                {
                    strChar = String.Format(" : '{0}'", chr);
                }
                return string.Format("{0}/{1}{2}{3}", Name, Value, strChar, OnStage ? "/(On Stage)" : ""); 
            }
        }

        internal void Pop(int lineno)
        {
            if (!Stack.Any())
                throw new RuntimeException(lineno, "{0}  is unable to recall anything.", Name);
            Value = Stack.Pop();
        }

        internal void Push(int lineno, int value)
        {
            Stack.Push(value);
        }
    }
}
