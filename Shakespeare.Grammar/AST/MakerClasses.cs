using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shakespeare.AST
{

    public class SelfNode : ShakespeareBaseAstNode
    {
        public override void Init(AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = Term.Name;
        }

        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            return AstNode1.OutputTo(tw);
        }
    }

    public class ListNode : ShakespeareBaseAstNode
    {
        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            foreach (var cn in TreeNode.ChildNodes)
            {
                (cn.AstNode as ShakespeareBaseAstNode).OutputTo(tw);
            }
            return this;
        }
    }

    public class TwoPartNode : ShakespeareBaseAstNode
    {
        public override void Init(AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            AstNode1.OutputTo(tw);
            AstNode2.OutputTo(tw);
            tw.WriteLine();
            return this;
        }
    }

    public class TwoPartWithSpaceNode : ShakespeareBaseAstNode
    {
        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            AstNode1.OutputTo(tw);
            tw.Write(' ');
            AstNode2.OutputTo(tw);
            tw.WriteLine();
            return this;
        }
    }


    public class AsTerminalNode : ShakespeareBaseAstNode
    {
        public override string ToString()
        {
            return Term.Name;
        }
    }

    public class AsValueNode : ShakespeareBaseAstNode
    {
        public override string ToString()
        {
            var tok = this.TreeNode.Token ?? this.Child1.Token;
            return tok.ValueString;
        }
    }

    public class AsTokenNode : ShakespeareBaseAstNode
    {
        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            tw.Write(TreeNode.Token.Text);
            return this;
        }
    }

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



    // Marker class -- this classes have no implementation,
    // and are only used for "if (AstNode is ....)" statements
    
    public class AdjectiveNode : SelfNode { }
    public class FirstPersonNode : MultiWordTermialNode { }
    public class FirstPersonReflexiveNode : MultiWordTermialNode { }
    public class SecondPersonNode : MultiWordTermialNode { }
    public class SecondPersonReflexiveNode : MultiWordTermialNode { }
    public class BeNode : MultiWordTermialNode { }
    public class NothingNode : MultiWordTermialNode { }
    public class UnarticulatedConstantNode : SelfNode { }
    public class PositiveNounNode : ShakespeareBaseAstNode { }
    public class OpenYourNode : ShakespeareBaseAstNode { }
    public class NegativeNounNode : MultiWordTermialNode { }
    public class NegativeComparativeTermNode : MultiWordTermialNode { }
    public class InequalityNode : SelfNode { }
    public class NegativeAdjectiveNode : MultiWordTermialNode { }
    public class EndSymbolNode : SelfNode { }
    public class JumpPhraseBeginninglNode : SelfNode { }
    public class JumpPhraseEndNode : SelfNode { }
    public class JumpPhraseNode : ShakespeareBaseAstNode { }

    public class SceneRomanNode : SelfNode { }
    public class SceneStuffNode : ShakespeareBaseAstNode { }

    public class StatementSymbolNode : SelfNode { }


}
