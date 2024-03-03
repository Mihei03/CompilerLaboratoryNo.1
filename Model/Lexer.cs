using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerDemo.Model
{
    internal class Lexer
    {
        public List<Token> Scan(string code)
        {
            if (code == null)
            {
                return new List<Token>();
            }

            List<Token> tokens = new List<Token>();
            int position = 0;

            code = code.Replace("\t", "").Replace("\r", "");

            do
            {
                string rawToken = ParseToken(code, position);
                tokens.Add(new Token(rawToken, position));
                position += rawToken.Length;

            } while (position < code.Length);

            return tokens;
        }

        private string ParseToken(string code, int position)
        {
            char symbol = code[position];
            if (char.IsWhiteSpace(symbol))
            {
                return symbol.ToString();
            }

            if (symbol == '\'')
            {
                return ParseStringLiteral(code, position);
            }

            if (char.IsLetter(symbol) || symbol == '_')
            {
                return Parse(code, position, (c) => !char.IsLetterOrDigit(c) && c != '_');
            }

            if (char.IsDigit(symbol))
            {
                return ParseNumeric(code, position);
            }

            return ParseOperator(code, position);
        }

        private string ParseNumeric(string code, int position)
        {
            char symbol;
            StringBuilder buffer = new StringBuilder();
            bool hasDot = false;

            while (position < code.Length)
            {
                symbol = code[position];
                if (char.IsDigit(symbol) || symbol == '.')
                {
                    if (symbol == '.')
                    {
                        if (hasDot) break; // Несколько точек в числе
                        hasDot = true;
                    }
                    buffer.Append(symbol);
                    position++;
                }
                else if (symbol == 'j')
                {
                    buffer.Append(symbol);
                    position++;
                    break;
                }
                else
                {
                    break;
                }
            }

            return buffer.ToString();
        }

        private string ParseStringLiteral(string code, int position)
        {
            char symbol;
            StringBuilder buffer = new StringBuilder();
            int pairCount = 0;

            while (position < code.Length)
            {
                symbol = code[position];
                if (symbol == '\'')
                {
                    pairCount++;
                }
                else if (pairCount == 2 || symbol == '\n')
                {
                    break;
                }

                buffer.Append(symbol);
                position++;
            }

            return buffer.ToString();
        }

        private string Parse(string code, int position, Func<char, bool> stopRule)
        {
            char symbol;
            StringBuilder buffer = new StringBuilder();
            while (position < code.Length)
            {
                symbol = code[position];
                if (stopRule(symbol))
                {
                    break;
                }

                buffer.Append(symbol);
                position++;
            }

            return buffer.ToString();
        }

        private string ParseOperator(string code, int position)
        {
            string symbol = code[position].ToString();

            string firstCharacter = "<>=";
            string secondCharacter = "=";
            if (position < code.Length - 1 && firstCharacter.Contains(symbol) && secondCharacter.Contains(code[position + 1]))
            {
                symbol += code[position + 1];
            }

            return symbol;
        }
    }
}