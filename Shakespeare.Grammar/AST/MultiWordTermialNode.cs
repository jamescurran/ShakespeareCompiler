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