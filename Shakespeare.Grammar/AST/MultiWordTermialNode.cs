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

using System.Linq;
using Irony.Ast;
using Irony.Parsing;

namespace Shakespeare.Ast
{
    /// <summary>
    /// For nodes which are really terminals, but must be defined as non-termials because they are made up of more than one word.
    /// </summary>
    public class MultiWordTermialNode : ShakespeareBaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var term = TerminalFactory.CreateCSharpIdentifier("text");
            var tok = new Token(term, Location, "text", System.String.Join("_", treeNode.ChildNodes.Select(cn => cn.Term.Name)));
            treeNode.Token = tok;
        }
    }
}