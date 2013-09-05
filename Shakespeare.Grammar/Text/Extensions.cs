using System;
using Irony.Interpreter;

namespace Shakespeare.Text
{
    public static class Extensions
    {
        public static TextContext tc(this ScriptThread thread)
        {
            var scDict = thread.CurrentScope.AsDictionary();
            Object obj;
            if (scDict.TryGetValue(TextContext.Key, out obj))
            {
                return obj as TextContext;
            }

            throw new InvalidOperationException("TextContext not defined.");
        }
    }
}