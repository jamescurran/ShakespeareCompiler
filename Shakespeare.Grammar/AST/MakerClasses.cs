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
