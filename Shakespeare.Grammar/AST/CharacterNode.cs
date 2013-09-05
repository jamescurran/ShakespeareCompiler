using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shakespeare.Utility;
using Irony.Ast;
using Irony.Parsing;

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
