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
using Irony.Interpreter.Ast;
using Irony.Parsing;
using NConsoler;
using Shakespeare;
using System;
using System.IO;


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
            [Optional("C","c", Description="Code generator: 'C' for C code, 'CS' for C#, 'I' for MSIL/EXE")]
            string compiler,
            [Optional(true, Description="Include Debug information in MSIL")]
            bool debug
            )
        {
            Console.WriteLine("ShakesCL - Command-line compiler interface for Shakespeare programming lanaguage");
            Console.WriteLine("Copyright 2013, James M. Curran, Novel Theory Software");

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
                SrcFileName = filename,
                Debug = debug
            };

            comp.PrepareScope(thread, param);
            var output = (tree.Root.AstNode as AstNode).Evaluate(thread);
            Console.Write(output);

        }
    }
}
