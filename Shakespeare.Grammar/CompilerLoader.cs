using Irony.Ast;
using Shakespeare.AST;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shakespeare
{
    public static class CompilerLoader
    {
        public static IShakespeareCompiler  Load(string assemblyCode)
        {
            var file = ConfigurationManager.AppSettings[assemblyCode.ToUpper()];
            var asm = Assembly.LoadFrom(file);
            return new Compiler(asm);
        }



    }


    public class Compiler : IShakespeareCompiler
    {
        Dictionary<String, Type> Mapper;
        Assembly assembly;

        public Compiler(Assembly asm)
        {
            assembly = asm;
            Mapper = asm.GetExportedTypes().ToDictionary(t => t.Name, t => t);
            ActHeaderNode = MakeCreator("ActHeaderNode");
            ActNode = MakeCreator("ActNode");
            BinaryOperatorNode = MakeCreator("BinaryOperatorNode");
            CharacterDeclarationListNode = MakeCreator("CharacterDeclarationListNode");
            CharacterDeclarationNode = MakeCreator("CharacterDeclarationNode");
            CharacterListNode = MakeCreator("CharacterListNode");
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
            SceneContentsNode = MakeCreator("SceneContentsNode");
            SceneHeaderNode = MakeCreator("SceneHeaderNode");
            SceneNode = MakeCreator("SceneNode");
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
            if (Mapper.TryGetValue(name, out type))
                return MakeCreator(type);
            throw new ArgumentOutOfRangeException(name + "is not an exported type.");
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

        public AstNodeCreator ActHeaderNode   { get; private set; }
        public AstNodeCreator ActNode   { get; private set; }
        public AstNodeCreator BinaryOperatorNode   { get; private set; }
        public AstNodeCreator CharacterDeclarationListNode   { get; private set; }
        public AstNodeCreator CharacterDeclarationNode   { get; private set; }
        public AstNodeCreator CharacterListNode   { get; private set; }
        public AstNodeCreator CommentNode   { get; private set; }
        public AstNodeCreator ComparativeNode   { get; private set; }
        public AstNodeCreator ComparisonNode   { get; private set; }
        public AstNodeCreator ConditionalNode   { get; private set; }
        public AstNodeCreator ConstantNode   { get; private set; }
        public AstNodeCreator EnterNode   { get; private set; }
        public AstNodeCreator EqualityNode   { get; private set; }
        public AstNodeCreator ExitNode   { get; private set; }
        public AstNodeCreator InOutNode   { get; private set; }
        public AstNodeCreator JumpNode   { get; private set; }
        public AstNodeCreator LineNode   { get; private set; }
        public AstNodeCreator NegativeComparativeNode   { get; private set; }
        public AstNodeCreator NegativeConstantNode   { get; private set; }
        public AstNodeCreator NonnegatedComparisonNode   { get; private set; }
        public AstNodeCreator PlayNode   { get; private set; }
        public AstNodeCreator PositiveComparativeNode   { get; private set; }
        public AstNodeCreator PositiveConstantNode   { get; private set; }
        public AstNodeCreator PronounNode   { get; private set; }
        public AstNodeCreator QuestionNode   { get; private set; }
        public AstNodeCreator RecallNode   { get; private set; }
        public AstNodeCreator RememberNode   { get; private set; }
        public AstNodeCreator SceneContentsNode   { get; private set; }
        public AstNodeCreator SceneHeaderNode   { get; private set; }
        public AstNodeCreator SceneNode   { get; private set; }
        public AstNodeCreator SentenceNode   { get; private set; }
        public AstNodeCreator StatementNode   { get; private set; }
        public AstNodeCreator TitleNode   { get; private set; }
        public AstNodeCreator UnaryOperatorNode   { get; private set; }
        public AstNodeCreator UnconditionalSentenceNode   { get; private set; }
        public AstNodeCreator ValueNode   { get; private set; }


    }

}
