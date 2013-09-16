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

using Irony.Interpreter;
using Shakespeare;
using Shakespeare.Text;
using System.Diagnostics;
using System.IO;

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
