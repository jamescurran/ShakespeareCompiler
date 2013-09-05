using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Irony.Ast;
using Irony.Interpreter;
using Shakespeare.Ast;

namespace Shakespeare
{
    public class Compiler : IShakespeareCompiler
    {
        readonly Dictionary<String, Type> mapper;

        readonly IHasPrepareScope scoper;

        public Compiler(Assembly asm)
        {
            mapper = asm.GetExportedTypes().ToDictionary(t => t.Name, t => t);

            var scoperType = mapper.Values.FirstOrDefault(t => typeof(IHasPrepareScope).IsAssignableFrom(t));
            if (scoperType != null)
                scoper = Activator.CreateInstance(scoperType) as IHasPrepareScope;

            ActHeaderNode = MakeCreator("ActHeaderNode");
            BinaryOperatorNode = MakeCreator("BinaryOperatorNode");
            CharacterDeclarationListNode = MakeCreator("CharacterDeclarationListNode");
            CharacterDeclarationNode = MakeCreator("CharacterDeclarationNode");
            CommentNode = MakeCreator("CommentNode");
            ComparativeNode = MakeCreator("ComparativeNode");
            ComparisonNode = MakeCreator("ComparisonNode");
            ConditionalNode = MakeCreator("ConditionalNode");
            ConstantNode = MakeCreator("ConstantNode");
            EnterNode = MakeCreator("EnterNode");
            EqualityNode = MakeCreator("EqualityNode");
            ExitNode = MakeCreator("ExitNode");
            InOutNode = MakeCreator("InOutNode");
            JumpNode = MakeCreator("JumpNode");
            LineNode = MakeCreator("LineNode");
            NegativeComparativeNode = MakeCreator("NegativeComparativeNode");
            NegativeConstantNode = MakeCreator("NegativeConstantNode");
            NonnegatedComparisonNode = MakeCreator("NonnegatedComparisonNode");
            PlayNode = MakeCreator("PlayNode");
            PositiveComparativeNode = MakeCreator("PositiveComparativeNode");
            PositiveConstantNode = MakeCreator("PositiveConstantNode");
            PronounNode = MakeCreator("PronounNode");
            QuestionNode = MakeCreator("QuestionNode");
            RecallNode = MakeCreator("RecallNode");
            RememberNode = MakeCreator("RememberNode");
            SceneHeaderNode = MakeCreator("SceneHeaderNode");
            SentenceNode = MakeCreator("SentenceNode");
            StatementNode = MakeCreator("StatementNode");
            TitleNode = MakeCreator("TitleNode");
            UnaryOperatorNode = MakeCreator("UnaryOperatorNode");
            UnconditionalSentenceNode = MakeCreator("UnconditionalSentenceNode");
            ValueNode = MakeCreator("ValueNode");
        }

        //  AstNodeCreator(AstContext context, ParseTreeNode parseNode);
        private AstNodeCreator MakeCreator(string name)
        {
            Type type;
            if (mapper.TryGetValue(name, out type))
                return MakeCreator(type);
            throw new ArgumentOutOfRangeException(name + " is not an exported type.");
        }

        static private AstNodeCreator MakeCreator(Type type)
        {
            return (context, parseNode) =>
            {
                parseNode.AstNode = Activator.CreateInstance(type) as ShakespeareBaseAstNode;
                //Initialize node
                var iInit = parseNode.AstNode as IAstNodeInit;
                if (iInit != null)
                    iInit.Init(context, parseNode);
            };
        }

        public void PrepareScope(ScriptThread thread, object param)
        {
            if (scoper != null)
                scoper.PrepareScope(thread, param);
        }

        public AstNodeCreator ActHeaderNode { get; private set; }

        public AstNodeCreator BinaryOperatorNode { get; private set; }

        public AstNodeCreator CharacterDeclarationListNode { get; private set; }

        public AstNodeCreator CharacterDeclarationNode { get; private set; }

        public AstNodeCreator CommentNode { get; private set; }

        public AstNodeCreator ComparativeNode { get; private set; }

        public AstNodeCreator ComparisonNode { get; private set; }

        public AstNodeCreator ConditionalNode { get; private set; }

        public AstNodeCreator ConstantNode { get; private set; }

        public AstNodeCreator EnterNode { get; private set; }

        public AstNodeCreator EqualityNode { get; private set; }

        public AstNodeCreator ExitNode { get; private set; }

        public AstNodeCreator InOutNode { get; private set; }

        public AstNodeCreator JumpNode { get; private set; }

        public AstNodeCreator LineNode { get; private set; }

        public AstNodeCreator NegativeComparativeNode { get; private set; }

        public AstNodeCreator NegativeConstantNode { get; private set; }

        public AstNodeCreator NonnegatedComparisonNode { get; private set; }

        public AstNodeCreator PlayNode { get; private set; }

        public AstNodeCreator PositiveComparativeNode { get; private set; }

        public AstNodeCreator PositiveConstantNode { get; private set; }

        public AstNodeCreator PronounNode { get; private set; }

        public AstNodeCreator QuestionNode { get; private set; }

        public AstNodeCreator RecallNode { get; private set; }

        public AstNodeCreator RememberNode { get; private set; }

        public AstNodeCreator SceneHeaderNode { get; private set; }

        public AstNodeCreator SentenceNode { get; private set; }

        public AstNodeCreator StatementNode { get; private set; }

        public AstNodeCreator TitleNode { get; private set; }

        public AstNodeCreator UnaryOperatorNode { get; private set; }

        public AstNodeCreator UnconditionalSentenceNode { get; private set; }

        public AstNodeCreator ValueNode { get; private set; }
    }
}