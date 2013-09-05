using Irony.Ast;
using Irony.Parsing;

namespace Shakespeare.Ast
{
    public class RomanNumberNode : ShakespeareBaseAstNode
    {
        public string Value { get; set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = TreeNode.Token.Text;
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