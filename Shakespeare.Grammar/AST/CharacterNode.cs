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
using Irony.Parsing;
using Shakespeare.Utility;
using System.Collections.Generic;

namespace Shakespeare.Ast
{
    public class CharacterNode : AsValueNode, IEqualityComparer<CharacterNode>
    {
        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Value.str2varname();
        }

        public bool Equals(CharacterNode x, CharacterNode y)
        {
            return x.ToString() == y.ToString();
        }

        public int GetHashCode(CharacterNode obj)
        {
            return obj.ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var cn = obj as CharacterNode;
            if (cn == null)
                return false;
            return Equals(this, cn);
        }
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }

    /// <summary>
    /// These can probably be merged
    /// </summary>
    public class AsValueNode : ShakespeareBaseAstNode
    {
        public string Value { get; set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var tok = this.TreeNode.Token ?? this.Child1.Token;
            Value = tok.ValueString;

        }
        protected override object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            return Value;
        }
        public override string ToString()
        {
            return Value;
        }
    }


}
