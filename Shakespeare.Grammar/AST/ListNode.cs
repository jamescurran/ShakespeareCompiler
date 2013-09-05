namespace Shakespeare.Ast
{
    /// <summary>
    /// For nodes which are just a list of other nodes, generally created with MakeStarRule or MakePlusRule
    /// </summary>
    public class ListNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            foreach (var cn in TreeNode.ChildNodes)
            {
                (cn.AstNode as ShakespeareBaseAstNode).Evaluate(thread);
            }
            return base.ReallyDoEvaluate(thread);
        }
    }
}