using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.IO;
using Shakespeare.Ast;
using Irony.Interpreter.Ast;
using Irony.Ast;
using Irony.Interpreter;
using System.Resources;
using System.Reflection;

namespace Shakespeare
{
    [Language("Shakespeare", "1.0.*", "A programming language created with the design goal to make the source code resemble Shakespeare plays.")]
    public class ShakespeareGrammar : InterpretedLanguageGrammar
    {
        IShakespeareCompiler compiler;
        public ShakespeareGrammar() : this(CompilerLoader.Load("CS"))
        {
        }

        public ShakespeareGrammar(IShakespeareCompiler compiler)
            : base(false)
        {
            this.compiler = compiler;
            string[] endSymbols = { ".", "?", "!" };
            KeyTerm COLON = ToTerm(":", "colon");
            KeyTerm COMMA = ToTerm(",");

            KeyTerm EXCLAMATION_MARK = ToTerm("!");
            KeyTerm LEFT_BRACKET = ToTerm("[");
            KeyTerm PERIOD = ToTerm(".");
            KeyTerm QUESTION_MARK = ToTerm("?");
            KeyTerm RIGHT_BRACKET = ToTerm("]");


            KeyTerm QuestionSymbol = ToTerm("?", "QuestionMark");


            var Article = BuildTerminal("Article", "article.wordlist");
            var Be = BuildTerminal<BeNode>("Be", "be.wordlist");
            var Character = BuildTerminal("Character", "character.wordlist");
            var FirstPerson = BuildTerminal<FirstPersonNode>("FirstPerson", "first_person.wordlist");
            var FirstPersonPossessive = BuildTerminal("FirstPersonPossessive", "first_person_possessive.wordlist");
            var FirstPersonReflexive = BuildTerminal<FirstPersonReflexiveNode>("FirstPersonReflexive", "first_person_reflexive.wordlist");
            var NegativeAdjective = BuildTerminal<NegativeAdjectiveNode>("NegativeAdjective", "negative_adjective.wordlist");
            var NEGATIVE_COMPARATIVE = BuildTerminal<NegativeComparativeTermNode>("NEGATIVE_COMPARATIVE", "negative_comparative.wordlist");
            var NegativeNoun = BuildTerminal<NegativeNounNode>("NegativeNoun", "negative_noun.wordlist");
            var NeutralAdjective = BuildTerminal("NeutralAdjective", "neutral_adjective.wordlist");
            var NeutralNoun = BuildTerminal("NeutralNoun", "neutral_noun.wordlist");
            var Nothing = BuildTerminal<NothingNode>("Nothing", "nothing.wordlist");
            var PositiveAdjective = BuildTerminal("PositiveAdjective", "Positive_adjective.wordlist");
            var POSITIVE_COMPARATIVE = BuildTerminal("POSITIVE_COMPARATIVE", "Positive_comparative.wordlist");
            var POSITIVE_NOUN = BuildTerminal("POSITIVE_NOUN", "Positive_noun.wordlist");
            var SecondPerson = BuildTerminal<SecondPersonNode>("SecondPerson", "Second_person.wordlist");
            var SecondPersonPossessive = BuildTerminal("SecondPersonPossessive", "Second_person_possessive.wordlist");
            var SecondPersonReflexive = BuildTerminal<SecondPersonReflexiveNode>("SecondPersonReflexive", "Second_person_reflexive.wordlist");
            var ThirdPersonPossessive = BuildTerminal("ThirdPersonPossessive", "Third_person_possessive.wordlist");

            var AND = ToTerm("and");
            var AS = ToTerm("as");
            var ENTER = ToTerm("enter");
            var EXEUNT = ToTerm("exeunt");
            var EXIT = ToTerm("exit");
            var HEART = ToTerm("heart");
            var IF_NOT = ToTerm("if not");
            var IF_SO = ToTerm("if so");
            var LESS = ToTerm("less");
            var LET_US = ToTerm("let us");
            var LISTEN_TO = MultiWordTermial("listen to");
            var MIND = ToTerm("mind");
            var MORE = ToTerm("more");
            var NOT = ToTerm("not");
            var OPEN = ToTerm("open");
            var PROCEED_TO = MultiWordTermial("proceed to");
            var RECALL = ToTerm("recall");
            var REMEMBER = ToTerm("remember");
            var RETURN_TO = MultiWordTermial("return to");
            var SPEAK = ToTerm("speak");
            var THAN = ToTerm("than");
            var THE_CUBE_OF = MultiWordTermial("the cube of");
            var THE_DIFFERENCE_BETWEEN = MultiWordTermial("the difference between");
            var THE_FACTORIAL_OF = MultiWordTermial("the factorial of");
            var THE_PRODUCT_OF = MultiWordTermial("the product of");
            var THE_QUOTIENT_BETWEEN = MultiWordTermial("the quotient between");
            var THE_REMAINDER_OF_THE_QUOTIENT_BETWEEN = MultiWordTermial("the remainder of the quotient between");
            var THE_SQUARE_OF = ToTerm("the square of");
            var THE_SQUARE_ROOT_OF = ToTerm("the square root of");
            var THE_SUM_OF = MultiWordTermial("the sum of");
            var TWICE = ToTerm("twice");
            var WE_MUST = ToTerm("we must");
            var WE_SHALL = ToTerm("we shall");

            var Play = new NonTerminal("Play", compiler.PlayNode);
            var Title = new NonTerminal("Title", compiler.TitleNode);
            var Act = new NonTerminal("Act", typeof(ActNode));
            var ActHeader = new NonTerminal("ActHeader", compiler.ActHeaderNode);
            var ActRoman = new NonTerminal("ActRoman"); 
            var Adjective = new NonTerminal("Adjective", typeof(AdjectiveNode));
            var BinaryOperator = new NonTerminal("BinaryOperator", compiler.BinaryOperatorNode);
            var CharacterDeclaration = new NonTerminal("CharacterDeclaration", compiler.CharacterDeclarationNode);
            var CharacterDeclarationList = new NonTerminal("CharacterDeclarationList", compiler.CharacterDeclarationListNode);
            var CharacterList = new NonTerminal("CharacterList", compiler.CharacterListNode);
            var Comment = new FreeTextLiteral("Comment", FreeTextOptions.ConsumeTerminator, endSymbols);
            var Comparative = new NonTerminal("Comparative", compiler.ComparativeNode);
            var Comparison = new NonTerminal("Comparison", compiler.ComparisonNode);
            var Conditional = new NonTerminal("Conditional", compiler.ConditionalNode);
            var Constant = new NonTerminal("Constant", compiler.ConstantNode);
            var EndSymbol = new NonTerminal("EndSymbol", typeof(EndSymbolNode));
            var Enter = new NonTerminal("Enter", compiler.EnterNode);
            var Equality = new NonTerminal("Equality", compiler.EqualityNode);
            var Exit = new NonTerminal("Exit", compiler.ExitNode);
            var Inequality = new NonTerminal("Inequality", typeof(InequalityNode));
            var InOut = new NonTerminal("InOut", compiler.InOutNode);
            var Jump = new NonTerminal("Jump", compiler.JumpNode);
            var JumpPhraseBeginning = new NonTerminal("JumpPhraseBeginning", typeof(JumpPhraseBeginninglNode));
            var JumpPhraseEnd = new NonTerminal("JumpPhraseEnd", typeof(JumpPhraseEndNode));
            var JumpPhrase = new NonTerminal("JumpPhrase", typeof(JumpPhraseNode));
            var Line = new NonTerminal("Line", compiler.LineNode);
            var NonnegatedComparison = new NonTerminal("NonnegatedComparison", compiler.NonnegatedComparisonNode);
            var NegativeComparative = new NonTerminal("NegativeComparative", compiler.NegativeComparativeNode);
            var NegativeConstant = new NonTerminal("NegativeConstant", compiler.NegativeConstantNode);
            var OpenYour = new NonTerminal("OpenYour", typeof(OpenYourNode));
            var PositiveComparative = new NonTerminal("PositiveComparative", compiler.PositiveComparativeNode);
            var PositiveConstant = new NonTerminal("PositiveConstant", compiler.PositiveConstantNode);
            var PositiveNoun = new NonTerminal("PositiveNoun", typeof(PositiveNounNode));
            var Pronoun = new NonTerminal("Pronoun", compiler.PronounNode);
            var Question = new NonTerminal("Question", compiler.QuestionNode);
            var Recall = new NonTerminal("Recall", compiler.RecallNode);
            var Remember = new NonTerminal("Remember", compiler.RememberNode);
            var RomanNumber = new RegexBasedTerminal("RomanNumer", "[mdclxvi]+");
            var Scene = new NonTerminal("Scene", typeof(SceneNode));
            var Scenes = new NonTerminal("Scenes", typeof(ListNode));
            var SceneContents = new NonTerminal("SceneContents", typeof(SceneContentsNode));
            var SceneHeader = new NonTerminal("SceneHeader", compiler.SceneHeaderNode);
            var SceneRoman = new NonTerminal("SceneRoman", typeof(SceneRomanNode));
            var SceneStuff = new NonTerminal("SceneStuff", typeof(SceneStuffNode));
            var Sentence = new NonTerminal("Sentence", compiler.SentenceNode);
            var SentenceList = new NonTerminal("SentenceList", typeof(ListNode));
            var Statement = new NonTerminal("Statement", compiler.StatementNode);
            var StatementSymbol = new NonTerminal("StatementSymbol", typeof(StatementSymbolNode));
            var Text = new FreeTextLiteral("String", FreeTextOptions.IncludeTerminator | FreeTextOptions.AllowEof, endSymbols);
            var UnarticulatedConstant = new NonTerminal("UnarticulatedConstant", typeof(UnarticulatedConstantNode));
            var UnaryOperator = new NonTerminal("UnaryOperator", compiler.UnaryOperatorNode);
            var UnconditionalSentence = new NonTerminal("UnconditionalSentence", compiler.UnconditionalSentenceNode);
            var Value = new NonTerminal("Value", compiler.ValueNode);
            var Acts = new NonTerminal("Acts", typeof(ListNode));

            Play.Rule = Title + CharacterDeclarationList + Acts;

            Acts.Rule = MakePlusRule(Acts, Act);
            Title.Rule = Text;
            Act.Rule = ActHeader + Scenes;

            Scenes.Rule = MakePlusRule(Scenes, Scene);

            ActHeader.Rule = ActRoman + COLON + Comment;

            ActRoman.Rule = ToTerm("act") + RomanNumber;

            Adjective.Rule =
                PositiveAdjective |
                NeutralAdjective |
                NegativeAdjective;

            BinaryOperator.Rule =
                THE_DIFFERENCE_BETWEEN |
                THE_PRODUCT_OF |
                THE_QUOTIENT_BETWEEN |
                THE_REMAINDER_OF_THE_QUOTIENT_BETWEEN |
                THE_SUM_OF;

            CharacterDeclaration.Rule = Character + COMMA + Comment;
            CharacterDeclarationList.Rule = MakePlusRule(CharacterDeclarationList, CharacterDeclaration);

            CharacterList.Rule = Character + AND + Character
                | Character + COMMA + CharacterList;

            Comparative.Rule = PositiveComparative | NegativeComparative;

            Comparison.Rule = NOT + NonnegatedComparison | NonnegatedComparison;
            Conditional.Rule = IF_SO | IF_NOT;
            Constant.Rule = Article + UnarticulatedConstant |
                FirstPersonPossessive + UnarticulatedConstant |
                SecondPersonPossessive + UnarticulatedConstant |
                ThirdPersonPossessive + UnarticulatedConstant |
                Nothing;

            EndSymbol.Rule = QuestionSymbol | StatementSymbol;

            Enter.Rule =
                  LEFT_BRACKET + ENTER + Character + RIGHT_BRACKET
                | LEFT_BRACKET + ENTER + CharacterList + RIGHT_BRACKET;
            Exit.Rule =
                  LEFT_BRACKET + EXIT + Character + RIGHT_BRACKET
                | LEFT_BRACKET + EXEUNT + CharacterList + RIGHT_BRACKET
                | LEFT_BRACKET + EXEUNT + RIGHT_BRACKET;

            Equality.Rule = AS + Adjective + AS;
            Inequality.Rule = Comparative + THAN;
            OpenYour.Rule =  OPEN + SecondPersonPossessive;


            InOut.Rule = OpenYour + HEART + StatementSymbol |
                SPEAK + SecondPersonPossessive + MIND + StatementSymbol |
                LISTEN_TO + SecondPersonPossessive + HEART + StatementSymbol |
                OpenYour + MIND + StatementSymbol;

            JumpPhraseBeginning.Rule = LET_US | WE_MUST | WE_SHALL;
            JumpPhraseEnd.Rule = PROCEED_TO | RETURN_TO;
            JumpPhrase.Rule = JumpPhraseBeginning + JumpPhraseEnd;

            Jump.Rule = JumpPhrase + ActRoman + StatementSymbol | JumpPhrase + SceneRoman + StatementSymbol;

            Line.Rule = Character + COLON + SentenceList;

            NegativeComparative.Rule =
                NEGATIVE_COMPARATIVE |
                MORE + NegativeAdjective |
                LESS + PositiveAdjective;

            NegativeConstant.Rule = NegativeNoun | NegativeAdjective + NegativeConstant | NeutralAdjective + NegativeConstant;
            NonnegatedComparison.Rule = Equality | Inequality;

            PositiveComparative.Rule = POSITIVE_COMPARATIVE | MORE + PositiveAdjective | LESS + NegativeAdjective;
            PositiveConstant.Rule = PositiveNoun | PositiveAdjective + PositiveConstant | NeutralAdjective + PositiveConstant;
            PositiveNoun.Rule = NeutralNoun | POSITIVE_NOUN;
            Pronoun.Rule = FirstPerson | FirstPersonReflexive | SecondPerson | SecondPersonReflexive;
            Question.Rule = Be + Value + Comparison + Value + QuestionSymbol;
            Recall.Rule = RECALL + Text;
            Remember.Rule = REMEMBER + Value + StatementSymbol;

            Scene.Rule = SceneHeader + SceneContents;
            SceneHeader.Rule = SceneRoman + COLON + Comment;
            SceneRoman.Rule = ToTerm("scene") + RomanNumber;
            SceneStuff.Rule = Enter | Exit | Line;
            SceneContents.Rule = MakeStarRule(SceneContents, SceneStuff);

            SentenceList.Rule = MakePlusRule(SentenceList, Sentence);
            Sentence.Rule = UnconditionalSentence
                | Conditional + COMMA + UnconditionalSentence;

            StatementSymbol.Rule = EXCLAMATION_MARK | PERIOD;

            UnarticulatedConstant.Rule = PositiveConstant | NegativeConstant;

            UnaryOperator.Rule = THE_CUBE_OF | THE_FACTORIAL_OF | THE_SQUARE_OF | THE_SQUARE_ROOT_OF | TWICE;

            UnconditionalSentence.Rule =
                 InOut |
                 Jump |
                 Question |
                 Recall |
                 Remember |
                 Statement;

            Value.Rule = Character |
                Constant |
                Pronoun |
                BinaryOperator + Value + AND + Value |
                UnaryOperator + Value;

            Statement.Rule = SecondPerson + Be + Constant + StatementSymbol |
                SecondPerson + UnarticulatedConstant + StatementSymbol |
                SecondPerson + Be + Equality + Value + StatementSymbol;

            MarkPunctuation(".", "?", "!", "[", "]", ":", ",", "act", "scene", "and", "enter", "exit", "exeunt",
                "as", "than",
                "open");

            this.Root = Play;
            Text.AstConfig = SetNode<AstNode>();
            Comment.AstConfig = SetNode(compiler.CommentNode);
            RomanNumber.AstConfig = SetNode<AsTokenNode>();
            Character.AstConfig = SetNode<CharacterNode>();

            MarkTransient(SceneStuff,ActRoman, SceneRoman,
                StatementSymbol);
            this.LanguageFlags = LanguageFlags.CreateAst;

        }

        NonTerminal BuildTerminal(string name, string filename)
        {
            return BuildTerminal<MultiWordTermialNode>(name, filename);
        }

        NonTerminal BuildTerminal<TNode>(string name, string filename)
            where TNode : MultiWordTermialNode
        {
            const string resourcePrefix = "Shakespeare.include.";
            var termList = new NonTerminal(name, typeof(TNode));
#if false
            var pathname = Path.Combine(include_path, filename);
            var lines = File.ReadAllLines(pathname);
#else
            var strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePrefix + filename.ToLowerInvariant());
            var sw = new StreamReader(strm);
            var block = sw.ReadToEnd();
            var lines = block.Split('\n', '\r');
#endif
            if (lines.Any())
            {
                BnfExpression expr = new BnfExpression(MultiWordTermial(lines[0]));
                foreach (var line in lines.Skip(1))
                {
                    if (!String.IsNullOrWhiteSpace(line))
                        expr |= MultiWordTermial(line);
                }
                termList.Rule = expr;
            }
            return termList;
        }

        BnfTerm MultiWordTermial(string term)
        {
            var parts = term.Split(' ');
            if (parts.Length == 1)
                return ToTerm(term);

            var nonterm = new NonTerminal(term, typeof(MultiWordTermialNode));
            BnfExpression expr = new BnfExpression(ToTerm(parts[0]));
            foreach (var part in parts.Skip(1))
            {
                expr += ToTerm(part);
            }
            nonterm.Rule = expr;
            return nonterm;

        }

        AstNodeConfig SetNode(AstNodeCreator creator)
//            where TNode : AstNode, new()
        {
            return new AstNodeConfig() { NodeCreator = creator };
        }


        AstNodeConfig SetNode<TNode>()
            where TNode : AstNode, new()
        {
            return new AstNodeConfig() { DefaultNodeCreator = () => new TNode() };
        }
    }

}
