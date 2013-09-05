using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Parsing;

using NConsoler;

using Shakespeare;
using System.IO;
using Irony.Interpreter.Ast;
using Irony.Interpreter;


namespace ShakesCL
{
    class Program
    {
        static void Main(string[] args)
        {
            Consolery.Run(typeof(Program), args);
        }


        [Action]
        public static void Compile(
            [Required(Description="path to Shakespeare source code to compile")]
            string filename,
            [Optional("C","c", Description="Code generator to use ('C' for C code, 'CS' for C#, 'I' for MSIL")]
            string compiler
            )
        {
            var comp = CompilerLoader.Load(compiler);
            var grammar = new ShakespeareGrammar(comp);
            var parser = new Parser(grammar);
            var text = File.ReadAllText(filename);
            var tree = parser.Parse(text, filename);

            var app = new ScriptApp(parser.Language);
            var thread = new ScriptThread(app);
            var param = new CompilerParams()
            {
                OutFolder = @"C:\Users\User\Projects\Shakespeare\Executables\Debug\",
                SrcFileName = filename
            };

            comp.PrepareScope(thread, param);
            var output = (tree.Root.AstNode as AstNode).Evaluate(thread);
            Console.Write(output);

        }
    }
}
