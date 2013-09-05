using Irony.Ast;
using Irony.Interpreter;
using Shakespeare.Ast;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shakespeare
{
    public static class CompilerLoader
    {
        public static IShakespeareCompiler  Load(string assemblyCode)
        {
            var file = ConfigurationManager.AppSettings[assemblyCode.ToUpper()];
            var asm = Assembly.LoadFrom(file);
            return new Compiler(asm);
        }
    }


}
