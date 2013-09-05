using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Irony.Interpreter;
using Shakespeare;
using Shakespeare.Ast;
using TriAxis.RunSharp;

namespace Shakespeare2MSIL
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
                param.OutFileName = Path.ChangeExtension(fn, ".exe");
            }

            if (param.OutFolder==null)
            {
                param.OutFolder = Path.GetDirectoryName(param.SrcFileName);
            }

            var outPathname = Path.Combine(param.OutFolder, param.OutFileName);

            var scDict = thread.CurrentScope.AsDictionary();
            var rs = new RunSharpContext();
            rs.AG = new AssemblyGen(outPathname, true);
            scDict.Add(RunSharpContext.Key, rs);

        }
    }
}
