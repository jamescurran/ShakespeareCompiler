using System;

namespace Shakespeare.Support.Utilities
{
    public class RuntimeException : Exception
    {
        public RuntimeException(int lineno, string format, params object[] args) : base(string.Format("Runtime error at line {0}:", lineno) + string.Format(format, args))
        {
        }
    }
}