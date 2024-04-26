using System.Collections.Generic;
using System.Linq;

namespace CompilerDemo.Model.Parser.States
{
    internal class ImaginaryPartState : IParserState
    {
        public void Parse(Parser parser, List<Token> tokens, List<IParserState> states)
        {
            if (ParserUtils.TrimWhitespaceTokens(ref tokens) == false || states.Count == 0)
            {
                return;
            }

            List<Token> tail = new List<Token>(tokens);
            List<Token> errorBuffer = new List<Token>();
            Token firstToken = tail.First();
            bool isFound = false;
            foreach (Token token in tail.ToList())
            {
                if (token.Type == TokenType.CloseParenthesis)
                {
                    break;
                }
                if (token.Type != TokenType.DoubleLiteral && token.Type != TokenType.IntegerLiteral)
                {
                    errorBuffer.Add(token);
                    tail.Remove(token);
                }
                else
                {
                    tail.Remove(token);
                    tokens.Remove(token);
                    isFound = true;
                    break;
                }
            }

            states = states.Skip(1).ToList();
            if (tail.Count > 0)
            {
                ParserUtils.CreateErrorFromBuffer(parser, errorBuffer, "Ожидалось мнимое");
                states.FirstOrDefault()?.Parse(parser, tail, states);
                return;
            }
            if (isFound == false)
            {
                ParserUtils.CreateError(parser, firstToken.StartPos, "Пропущено мнимое");
            }

            states.FirstOrDefault()?.Parse(parser, tokens, states);
        }
    }
}
