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

namespace Shakespeare
{
    public interface IShakespeareCompiler
    {
        void PrepareScope(ScriptThread thread, object param);
        AstNodeCreator ActHeaderNode { get; }
        AstNodeCreator BinaryOperatorNode { get; }
        AstNodeCreator CharacterDeclarationListNode { get; }
        AstNodeCreator CharacterDeclarationNode  { get; }
        AstNodeCreator CommentNode  { get; }
        AstNodeCreator ComparativeNode  { get; }
        AstNodeCreator ComparisonNode  { get; }
        AstNodeCreator ConditionalNode  { get; }
        AstNodeCreator ConstantNode  { get; }
        AstNodeCreator EnterNode  { get; }
        AstNodeCreator EqualityNode  { get; }
        AstNodeCreator ExitNode  { get; }
        AstNodeCreator InOutNode  { get; }
        AstNodeCreator JumpNode  { get; }
        AstNodeCreator LineNode  { get; }
        AstNodeCreator NegativeComparativeNode  { get; }
        AstNodeCreator NegativeConstantNode  { get; }
        AstNodeCreator NonnegatedComparisonNode  { get; }
        AstNodeCreator PlayNode  { get; }
        AstNodeCreator PositiveComparativeNode  { get; }
        AstNodeCreator PositiveConstantNode  { get; }
        AstNodeCreator PronounNode  { get; }
        AstNodeCreator QuestionNode  { get; }
        AstNodeCreator RecallNode  { get; }
        AstNodeCreator RememberNode  { get; }
        AstNodeCreator SceneHeaderNode  { get; }
        AstNodeCreator SentenceNode  { get; }
        AstNodeCreator StatementNode  { get; }
        AstNodeCreator TitleNode  { get; }
        AstNodeCreator UnaryOperatorNode  { get; }
        AstNodeCreator UnconditionalSentenceNode  { get; }
        AstNodeCreator ValueNode  { get; }
    }
                                                                   
}
