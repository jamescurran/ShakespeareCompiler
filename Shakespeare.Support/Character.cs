using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Shakespeare.Support.Utilities;

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
            get { return string.Format("{0}/{1}{2}", Name, Value, OnStage ? "/(On Stage)" : ""); }
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
