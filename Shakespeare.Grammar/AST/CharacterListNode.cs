using System.Collections.Generic;

namespace Shakespeare.Ast
{
    public class CharacterListNode : ShakespeareBaseAstNode
    {
        public void Fill(ICollection<CharacterNode> coll)
        {
            foreach (var cn in TreeNode.ChildNodes)
            {
                if (cn != null)
                    coll.Add(cn.AstNode as CharacterNode);
            }
        }
    }
}