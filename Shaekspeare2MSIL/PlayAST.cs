using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;
using Shakespeare.Utility;
using TriAxis.RunSharp;

namespace Shakespeare.Ast
{
    internal class RunSharpContext
    {
        public const string Key = "AssemblyGen";
        public AssemblyGen AG { get; set; }
        public TypeGen Script { get; set; }
        public CodeGen Action { get; set; }
        public Dictionary<string, Operand> Cast { get; set; }

        public RunSharpContext()
        {
            Cast = new Dictionary<string, Operand>();
        }

    }


    internal static class Extensions
    {
        public static AssemblyGen AG (this AstContext context)
        {
            Object obj;
            if (context.Values.TryGetValue(RunSharpContext.Key, out obj))
            {
                return obj as AssemblyGen;
            }

            var ag = new AssemblyGen(@"C:\Users\User\Projects\Shakespeare\Executables\Debug\output.exe", true);
            context.Values.Add(RunSharpContext.Key, ag);
            return ag;
        }

        public static RunSharpContext rs(this ScriptThread thread)
        {
            var scDict = thread.CurrentScope.AsDictionary();
            Object obj;
            if (scDict.TryGetValue(RunSharpContext.Key, out obj))
            {
                return obj as RunSharpContext;
            }

            throw new InvalidOperationException("RunSharpContext not defined.");
        }

    }


    public class PlayNode : ShakespeareBaseAstNode
    {

        protected override object ReallyDoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            var rs = thread.rs();
            var ag = rs.AG;
            using (ag.Namespace("Shakespeare.Program"))
            {
                TypeGen scriptClass = ag.Public.Class("Script", typeof(Shakespeare.Support.Dramaturge));
                {
                    rs.Script = scriptClass;

                    var console = typeof(Console);

                    CodeGen ctor = scriptClass.Public.Constructor();
                    ctor.InvokeBase(Static.Property(console, "In"), Static.Property(console, "Out"));
                    { 
                    }

                    CodeGen action = scriptClass.Public.Method(typeof(void), "Action");
                    {
                        rs.Action = action;
                        AstNode1.Evaluate(thread);
                        var cdl = AstNode2 as CharacterDeclarationListNode;
                        foreach (var ch in cdl.Characters)
                            ch.Evaluate(thread);

                        AstNode3.Evaluate(thread);
                    }
                }
                scriptClass.Complete();

                TypeGen MyClass = ag.Public.Class("Program");
                CodeGen Main = MyClass.Public.Static.Method(typeof(void), "Main").Parameter(typeof(string[]), "args");
                {
                    var script = Main.Local(Exp.New(scriptClass.GetCompletedType()));
                    Main.Invoke(script, "Action");
                }
            }
            ag.Save();
            return thread;
        }
        public override string ToString()
        {
            return "Play";
        }
    }

    public class TitleNode : ShakespeareBaseAstNode
    {
    }

    public class CharacterDeclarationListNode : ListNode
    {
        public List<CharacterDeclarationNode> Characters { get; set; }
        public override void Init(AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Characters = treeNode.ChildNodes.Select(cn => cn.AstNode as CharacterDeclarationNode).ToList();
        }
    }

    public class ActHeaderNode : ShakespeareBaseAstNode
    {
        string actnumber;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            actnumber = String1.str2varname();
        }

        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            Context.CurrentAct = actnumber;
            thread.rs().Action.Label(actnumber);           

            return base.ReallyDoEvaluate(thread);
        }
    }

    public class EnterNode : ShakespeareBaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            if (!Exist1)
            {
                context.AddMessage(Irony.ErrorLevel.Error, Location, @"""Enter"" missing character list");
                return;
            }
        }
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var cn = TreeNode.ChildNodes;
            if (Child1.AstNode is CharacterListNode)
                cn = Child1.ChildNodes;
            var action = thread.rs().Action;
            foreach (var ch in cn)
            {
                var name = ch.AstNode.ToString();
                var person = thread.rs().Cast[name];
                action.Invoke(action.Base(), "EnterScene", Location.Line, person);
                Context.ActiveCharacters.Add(ch.AstNode as CharacterNode);
            }
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class ExitNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            var charList = new List<CharacterNode>();
            var cast = thread.rs().Cast;

            if (Exist1)
            {
                if (AstNode1 is CharacterNode)
                    charList.Add(AstNode1 as CharacterNode);
                else
                    (AstNode1 as CharacterListNode).Fill(charList);

                foreach (var chr in charList)
                {
                    var name =  chr.ToString();
                    var person = cast[name];
                    action.Invoke(action.Base(), "ExitScene", Location.Line, person);
                    Context.ActiveCharacters.Remove(chr);
                }
            }
            else
            {
                action.Invoke(action.Base(),"ExitSceneAll", Location.Line);
                Context.ActiveCharacters.Clear();
            }

            return base.ReallyDoEvaluate(thread);
        }
    }



    public class LineNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            var person = thread.rs().Cast[AstNode1.ToString()];
            action.Invoke(action.Base(),"Activate",  Location.Line, person);
            AstNode2.Evaluate(thread);
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class SentenceNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is UnconditionalSentenceNode)
                AstNode1.Evaluate(thread);
            else
            {
                var action = thread.rs().Action;
                action.If(AstNode1.Evaluate(thread) as Operand);
                AstNode2.Evaluate(thread);
                action.End();
            }

            return base.ReallyDoEvaluate(thread);
        }
    }

    public class UnconditionalSentenceNode : SelfNode   {}


    public class InOutNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            if (AstNode1 is OpenYourNode)
            {
                if (String2 == "heart (Keyword)")
                {
                    action.Invoke(action.Base(), "IntOutput", Location.Line);
                }
                else  // Open Your Mind
                {
                    action.Invoke(action.Base(), "CharInput", Location.Line);
                }
            }
            else if (String1 == "speak")
            {
                action.Invoke(action.Base(), "CharOutput", Location.Line);
            }
            else if (String1 == "listen to")
            {
                action.Invoke(action.Base(), "IntInput", Location.Line);
            }
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class JumpNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            string label;
            if (AstNode2 is SceneRomanNode)
                label = Context.CurrentAct + "_" +  AstNode2.ToString().str2varname();
            else
                label = AstNode2.ToString().str2varname();

            var action = thread.rs().Action;
            action.Goto(label);

            return base.ReallyDoEvaluate(thread);
        }
    }

    public class QuestionNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            action.Assign(action.Base().Property("Comp1"), AstNode2.Evaluate(thread) as Operand);
            action.Assign(action.Base().Property("Comp2"), AstNode4.Evaluate(thread) as Operand);
            action.Assign(action.Base().Property("TruthFlag"), AstNode3.Evaluate(thread) as Operand);

            return base.ReallyDoEvaluate(thread);
        }
    }

    public class RecallNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            action.Invoke(action.Base(), "Pop",Location.Line);
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class RememberNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            action.Invoke(action.Base(), "Push", Location.Line, AstNode2.Evaluate(thread) as Operand);
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class StatementNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is SecondPersonNode)
            {
                var action = thread.rs().Action;
                if (AstNode2 is BeNode)
                {
                    if (AstNode3 is ConstantNode)
                        action.Invoke(action.Base(), "Assign", Location.Line, AstNode3.Evaluate(thread) as Operand);
                    else    // SECOND_PERSON BE Equality Value StatementSymbol 
                        action.Invoke(action.Base(), "Assign", Location.Line, AstNode4.Evaluate(thread) as Operand);
  
                }
                else if (AstNode2 is UnarticulatedConstantNode)
                {
                    action.Invoke(action.Base(), "Assign", Location.Line, AstNode2.Evaluate(thread) as Operand);
                }
            }

            return base.ReallyDoEvaluate(thread);
        }

    }

    public class ConstantNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is NothingNode)
                return (Operand)0;
            else  // astNode1 is Article, FirstPErson, SecondPerson, thirdPerson
                return AstNode2.Evaluate(thread);
        }
    }

    public class SceneHeaderNode : ShakespeareBaseAstNode
    {
        string scenenumber;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            scenenumber = String1.str2varname();
        }

        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            Context.CurrentScene = Context.CurrentAct + "_" + scenenumber;

            var action = thread.rs().Action;
            action.Label(Context.CurrentScene);
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class SceneContentsNode : ListNode  {  }

    public class SceneNode : TwoPartNode { }

    public class NegativeConstantNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is NegativeNounNode)
                return ((Operand) 0- 1);
            else // astnode1 is NegativeAdjective or astnode1 is neutralAdjective
                return (AstNode2.Evaluate(thread) as Operand) * 2;
        }
    }

    public class NonnegatedComparisonNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            return AstNode1.Evaluate(thread);
        }
    }



    public class CharacterDeclarationNode : ShakespeareBaseAstNode 
    {
        public string Declaration { get; set; }

        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            var name = AstNode1.ToString();
            var person  = action.Local(typeof(Shakespeare.Support.Character), action.Base().Invoke("InitializeCharacter", Location.Line, name));
            thread.rs().Cast.Add(name, person);
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class CommentNode :ShakespeareBaseAstNode
    {
    }

    public class ActNode : TwoPartNode { }

    public class ComparisonNode : ShakespeareBaseAstNode 
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is NonnegatedComparisonNode)
                return AstNode1.Evaluate(thread);
            else
                return !(AstNode2.Evaluate(thread) as Operand);
        }
    }

    public class ConditionalNode :ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            if (Child1.Term.Name == "if so")
                return action.Base().Property("TruthFlag");
            else// if not
                return !action.Base().Property("TruthFlag");
        }
    }

    public class ValueNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            var cast = thread.rs().Cast;
            if (AstNode1 is CharacterNode)
            {
                return (Operand) cast[String1].Property("Value");
            }
            else if (AstNode1 is ConstantNode)
                return AstNode1.Evaluate(thread);
            else if (AstNode1 is PronounNode)
            {
                return action.Base().Invoke("ValueOf", Location.Line, AstNode1.Evaluate(thread) as Operand);
            }
            else if (AstNode1 is BinaryOperatorNode)
            {
                var operand1 = AstNode2.Evaluate(thread) as Operand;
                var operand2 = AstNode3.Evaluate(thread) as Operand;

                // THis is ugly; I apologize
                var term = Child1.ChildNodes[0].Term.Name;
                if (term == "the difference between")
                    return operand1 - operand2;
                else if (term == "the product of")
                    return operand1 * operand2;
                else if (term == "the quotient between")
                    return operand1 / operand2;
                else if (term == "the remainder of the quotient between")
                    return operand1 % operand2;
                else if (term == "the sum of")
                    return operand1 +operand2;
            }
            else if (AstNode1 is UnaryOperatorNode)
            {
                return action.Base().Invoke(AstNode1.Evaluate(thread) as String, Location.Line, AstNode2.Evaluate(thread) as Operand);
            }
            else
            {
                // error
            }
            return base.ReallyDoEvaluate(thread);
        }

    }

    public class PronounNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;

            if (AstNode1 is FirstPersonNode || AstNode1 is FirstPersonReflexiveNode)
                return  action.Base().Property("FirstPerson");
            else if (AstNode1 is SecondPersonNode || AstNode1 is SecondPersonReflexiveNode)
                return  action.Base().Property("SecondPerson");
            return base.ReallyDoEvaluate(thread);
        }

        public override string ToString()
        {
            return "ERROR";
        }
        
    }

    public class PositiveConstantNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is PositiveNounNode)
                return ((Operand) 1);
            else // astnode1 is NegativeAdjective or astnode1 is neutralAdjective
                return (AstNode2.Evaluate(thread) as Operand) * 2;
        }
    }

    public class EqualityNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            var cond = action.Base().Property("Comp1") == action.Base().Property("Comp2");
            return cond;
        }
    }

    public class ComparativeNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            var action = thread.rs().Action;
            if (AstNode1 is NegativeComparativeNode)
                return action.Base().Property("Comp1") < action.Base().Property("Comp2");
            else  // PositiveComparativeNode
                return action.Base().Property("Comp1") > action.Base().Property("Comp2");
        }
    }

    public class NegativeComparativeNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is NegativeComparativeTermNode)
                return AstNode1.Evaluate(thread) as Operand;
            else
                return AstNode2.Evaluate(thread) as Operand;
        }
    }

    public class PositiveComparativeNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            if (AstNode1 is PositiveComparativeNode)
                return AstNode1.Evaluate(thread) as Operand;
            else
                return AstNode2.Evaluate(thread) as Operand;
        }
    }

    public class BinaryOperatorNode : ShakespeareBaseAstNode
    {
        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            
            return base.ReallyDoEvaluate(thread);
        }
    }

    public class UnaryOperatorNode : ShakespeareBaseAstNode
    {
        static readonly Dictionary<string, string> functionMap = new Dictionary<string, string>
        {
                {"the cube of", "Cube"},
                {"the factorial of", "Factorial"},
                {"the square of", "Square"},
                {"the square root of", "Sqrt"},
                {"twice", "Twice"},
        };

        protected override object ReallyDoEvaluate(ScriptThread thread)
        {
            return functionMap[String1];
        }
    }
}

