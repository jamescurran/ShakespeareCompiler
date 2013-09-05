using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shakespeare.Ast
{

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
        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            return AstNode1.OutputTo(tw);
        }
    }

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
        protected override object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            AstNode1.Evaluate(thread);
            AstNode2.Evaluate(thread);
            return base.ReallyDoEvaluate(thread);
        }

        public override ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            AstNode1.OutputTo(tw);
            AstNode2.OutputTo(tw);
            tw.WriteLine();
            return this;
        }
    }

    public class TwoPartWithSpaceNode : TwoPartNode
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

    public class AsTokenNode : ShakespeareBaseAstNode
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



    // Marker classes -- these classes have no implementation,
    // and are only used for "if (AstNode is ....)" statements
    public class SceneContentsNode : ListNode { }
    public class SceneNode : TwoPartNode { }
    public class ActNode : TwoPartNode { }
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
