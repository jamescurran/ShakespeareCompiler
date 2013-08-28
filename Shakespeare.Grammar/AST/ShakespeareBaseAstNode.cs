using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shakespeare.AST
{
    public class ShakespeareBaseAstNode : AstNode
    {
        protected ContextData Context { get; set; }
        protected ParseTreeNode TreeNode { get; set; }
        protected AstContext AstContext { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            const string KeyName = "ShakespeareContext";
            base.Init(context, treeNode);
            AstContext = context;
            TreeNode = treeNode;
            object data;
            if (context.Values.TryGetValue(KeyName, out data))
            {
                Context = data as ContextData;
            }
            else
            {
                Context = new ContextData();
                context.Values.Add(KeyName, Context);
            }
        }

        protected bool Exist1 { get { return TreeNode.ChildNodes.Any(); } }
        protected bool Exist2 { get { return TreeNode.ChildNodes.Count > 1; } }
        protected bool Exist3 { get { return TreeNode.ChildNodes.Count > 2; } }

        protected string String1
        {
            get
            {
                return AstNode1 != null ? AstNode1.ToString() : Child1.Token.ValueString;
                //                return ((object)AstNode1 ?? (object)Child1).ToString();
            }
        }
        protected string String2
        {
            get
            {
                return TreeNode.ChildNodes[1].ToString();
            }
        }
        protected string String3
        {
            get
            {
                return TreeNode.ChildNodes[2].ToString();
            }
        }

        //        protected string String(int i) { return TreeNode.ChildNodes[i - 1].ToString(); }

        protected ParseTreeNode Child1
        {
            get
            {
                return TreeNode.ChildNodes[0];
            }
        }
        protected ParseTreeNode Child2
        {
            get
            {
                return TreeNode.ChildNodes[1];
            }
        }
        protected ParseTreeNode Child3 { get { return TreeNode.ChildNodes[2]; } }
        protected ParseTreeNode Child4 { get { return TreeNode.ChildNodes[3]; } }

        protected ShakespeareBaseAstNode AstNode1 { get { return (Child1.AstNode as ShakespeareBaseAstNode); } }
        protected ShakespeareBaseAstNode AstNode2 { get { return (Child2.AstNode as ShakespeareBaseAstNode); } }
        protected ShakespeareBaseAstNode AstNode3 { get { return (Child3.AstNode as ShakespeareBaseAstNode); } }
        protected ShakespeareBaseAstNode AstNode4 { get { return (Child4.AstNode as ShakespeareBaseAstNode); } }

        protected virtual object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            return null;
        }

        public virtual ShakespeareBaseAstNode OutputTo(TextWriter tw)
        {
            var tok = TreeNode.Token;
            if (TreeNode.Term != null)
                tw.Write(TreeNode.Term.Name);
            else if (TreeNode.Token != null)
                tw.Write(TreeNode.Token.Text);

            return this;
        }

        public override string ToString()
        {
            using (var sw = new StringWriter())
            {
                OutputTo(sw);
                return sw.ToString();
            }
        }
        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            base.DoEvaluate(thread);
            thread.CurrentNode = this;  //standard prolog
            var obj = ReallyDoEvaluate(thread);
            thread.CurrentNode = Parent; //standard epilog

            return obj;
        }
    }
}
