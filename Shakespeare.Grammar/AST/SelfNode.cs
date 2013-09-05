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