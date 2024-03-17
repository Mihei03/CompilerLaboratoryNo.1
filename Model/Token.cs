using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerDemo.Model
{
    public enum TokenType 
    {
        Identifier = 1,
        Complex,
        Whitespace,
        Newline,
        Assignment,
        OpenRoundBracket,
        CloseRoundBracket,
        Comma,
        IntegerLiteral,
        ImaginaryWhole,
        DoubleLiteral,
        FractionalImaginary,
        Minus,
        Plus,
        Apostrophe,
        StringLiteral,
        ComplexLiteral,
        ImaginaryLiteral,

        Less,
        Greater,

        Multiply,
        Divide,
        Module,

        GreaterOrEqual,
        LessOrEqual,
        Equal,
        And,
        Or,
        Not,
        True,
        False,
        Number,
        If,
        Else,
        For,
        While,

        Invalid
    }

    public class Token
    {
        private static Dictionary<string, TokenType> DefaultTypes = new Dictionary<string, TokenType>()
        {
            { "complex", TokenType.Complex },
            { "\n", TokenType.Newline },
            { " ", TokenType.Whitespace },
            { ",", TokenType.Comma },
            { "'", TokenType.Apostrophe },
            { "=", TokenType.Assignment },

            { "(", TokenType.OpenRoundBracket },
            { ")", TokenType.CloseRoundBracket },

            { "+", TokenType.Plus },
            { "-", TokenType.Minus },
            { "*", TokenType.Multiply },
            { "/", TokenType.Divide },
            { "%", TokenType.Module },

            { ">", TokenType.Greater},
            { "<", TokenType.Less },
            { "==", TokenType.Equal },
            { ">=", TokenType.GreaterOrEqual },
            { "<=", TokenType.LessOrEqual },

            { "and", TokenType.And },
            { "not", TokenType.Not },
            { "or", TokenType.Or },
            { "true", TokenType.True },
            { "false", TokenType.False },

            { "if", TokenType.If },
            { "else", TokenType.Else },
            { "for", TokenType.For },
            { "while", TokenType.While },
        };
        public TokenType Type { get; }
        public string RawToken { get; }
        public int StartPos { get; }
        public int EndPos { get => StartPos + RawToken.Length; }

        public Token(string rawToken, int startPos)
        {
            if (rawToken.Length == 0)
                throw new ArgumentException("raw token is empty");

            RawToken = rawToken;
            StartPos = startPos;
            Type = GetTokenType(rawToken);
        }

        public static bool DefaultTokenExists(string rawToken)
            => DefaultTypes.ContainsKey(rawToken);
        private static bool IsIdentifier(string rawToken)
        {
            return rawToken.Length != 0 && (char.IsLetter(rawToken.First()) || rawToken.First() == '_') && !DefaultTokenExists(rawToken);
        }
        private static bool IsComplexLiteral(string rawToken)
        {
            return rawToken.StartsWith("complex(") && rawToken.EndsWith(")");
        }
        private static bool IsImaginaryLiteral(string rawToken)
        {
            return rawToken.EndsWith("j");
        }
        private static bool IsIntegerLiteral(string rawToken)
            => int.TryParse(rawToken, out int _) && !rawToken.StartsWith("0.");
        private static bool IsDoubleLiteral(string rawToken)
            => double.TryParse(rawToken, NumberFormatInfo.InvariantInfo, out double _) && (rawToken.StartsWith("0.") || rawToken.Contains('.'));
        private static bool IsStringLiteral(string rawToken)
        {
            return rawToken.StartsWith("'") && rawToken.EndsWith("'") && rawToken.Length > 2;
        }

        public static TokenType GetTokenType(string rawToken)
        {
            if (DefaultTokenExists(rawToken))
            {
                return DefaultTypes[rawToken];
            }

            if (IsComplexLiteral(rawToken))
            {
                return TokenType.ComplexLiteral;
            }

            if (IsImaginaryLiteral(rawToken))
            {
                return TokenType.ImaginaryLiteral;
            }

            if (DefaultTokenExists(rawToken))
            {
                return DefaultTypes[rawToken];
            }

            if (IsStringLiteral(rawToken))
            {
                return TokenType.StringLiteral;
            }

            if (rawToken.EndsWith("j"))
            {
                if (int.TryParse(rawToken.Trim('j'), out _))
                {
                    return TokenType.ImaginaryWhole;
                }
                else if (double.TryParse(rawToken.Trim('j'), NumberFormatInfo.InvariantInfo, out _))
                {
                    return TokenType.FractionalImaginary;
                }
            }
            if (IsIntegerLiteral(rawToken))
            {
                return TokenType.IntegerLiteral;
            }
            if (IsDoubleLiteral(rawToken))
            {
                return TokenType.DoubleLiteral;
            }

            return TokenType.Invalid;
        }
    }
}
