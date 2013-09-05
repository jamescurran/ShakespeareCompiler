using Irony.Ast;
using Irony.Parsing;
using System.Linq;

namespace Shakespeare.Ast
{

    // Marker classes -- these classes have no implementation beyond their base class,
    // and are only used for "if (AstNode is ....)" statements
    public class ActNode : TwoPartNode { }
    public class AdjectiveNode : SelfNode { }
    public class BeNode : MultiWordTermialNode { }
    public class EndSymbolNode : SelfNode { }
    public class FirstPersonNode : MultiWordTermialNode { }
    public class FirstPersonReflexiveNode : MultiWordTermialNode { }
    public class InequalityNode : SelfNode { }
    public class JumpPhraseBeginninglNode : SelfNode { }
    public class JumpPhraseEndNode : SelfNode { }
    public class JumpPhraseNode : ShakespeareBaseAstNode { }
    public class NegativeAdjectiveNode : MultiWordTermialNode { }
    public class NegativeComparativeTermNode : MultiWordTermialNode { }
    public class NegativeNounNode : MultiWordTermialNode { }
    public class NothingNode : MultiWordTermialNode { }
    public class OpenYourNode : ShakespeareBaseAstNode { }
    public class PositiveNounNode : ShakespeareBaseAstNode { }
    public class SceneContentsNode : ListNode { }
    public class SceneNode : TwoPartNode { }
    public class SceneRomanNode : SelfNode { }
    public class SceneStuffNode : ShakespeareBaseAstNode { }
    public class SecondPersonNode : MultiWordTermialNode { }
    public class SecondPersonReflexiveNode : MultiWordTermialNode { }
    public class StatementSymbolNode : SelfNode { }
    public class UnarticulatedConstantNode : SelfNode { }


}
