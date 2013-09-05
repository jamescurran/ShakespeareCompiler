using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Irony.Interpreter;
using Shakespeare;
using Shakespeare.Text;

namespace Shakespeare2C
{
    public class ScopePreparer : IHasPrepareScope
    {
        public void PrepareScope(ScriptThread thread, object compilerParams)
        {
            var param = compilerParams as CompilerParams;

            if (param.OutFileName == null)
            {
                Debug.Assert(param.SrcFileName != null, "Source file undefined");
                var fn = Path.GetFileName(param.SrcFileName);
                param.OutFileName = Path.ChangeExtension(fn, ".c");
            }

            if (param.OutFolder==null)
            {
                param.OutFolder = Path.GetDirectoryName(param.SrcFileName);
            }

            var outPathname = Path.Combine(param.OutFolder, param.OutFileName);

            var scDict = thread.CurrentScope.AsDictionary();
            var tc = new TextContext();
            tc.Writer = new StreamWriter(outPathname);
            scDict.Add(TextContext.Key, tc);
        }
    }
}
