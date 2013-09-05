namespace Shakespeare.Ast
{
    /// <summary>
    /// A variation of ListNode, for node which are basically just a bundling of two other nodes.
    /// </summary>
    public class TwoPartNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            AstNode1.Evaluate(thread);
            AstNode2.Evaluate(thread);
            return base.ReallyDoEvaluate(thread);
        }
    }
}