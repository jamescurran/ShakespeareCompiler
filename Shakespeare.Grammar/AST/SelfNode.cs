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

using Irony.Ast;

namespace Shakespeare.Ast
{
    /// <summary>
    /// A class which is essentially just a wrapper around it's child.
    /// FOr node which are "$$ = $1" in the YACC grammar.
    /// </summary>
    public class SelfNode : ShakespeareBaseAstNode
    {
        public override void Init(AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = Term.Name;
        }

        protected override object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            return AstNode1.Evaluate(thread);
        }
    }
}