#region Copyright & License summary
/*
 Copyright 2013, James M. Curran, Novel Theory Software

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
#endregion

using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;
using System.Linq;

namespace Shakespeare.Ast
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

        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            base.DoEvaluate(thread);
            thread.CurrentNode = this;  //standard prolog
            var obj = ReallyDoEvaluate(thread);
            thread.CurrentNode = Parent; //standard epilog

            return obj;
        }

        [Obsolete("Do not call ToString().  Use ToString(thread) instead.")]
        public override string ToString()
        {
            throw new NotImplementedException("Do not call ToString().  Use ToString(thread) instead.");
        }

        public string ToString(ScriptThread thread)
        {
            return this.DoEvaluate(thread) as string;
        }
    }
}
