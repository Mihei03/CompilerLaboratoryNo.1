﻿using System.Text;

namespace CompilerDemo.Model.Parser.States
{
    internal class ImaginaryPartState : IState
    {
        public void Handle(Parser parser, string code, int position)
        {   
            char symbol;
            StringBuilder errorBuffer = new StringBuilder();

            while (position < code.Length)
            {
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return;
                }

                char c = code[position];
                if (!char.IsDigit(c) && c != '+' && c != '-')
                {
                    errorBuffer.Append(c);
                    code.Remove(position);
                }
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "imaginary start", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    position++;
                    break;
                }
                position++;
            }

            errorBuffer.Clear();
            while (position < code.Length)
            {
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return;
                }
                symbol = code[position];

<<<<<<< Updated upstream
                if (!char.IsDigit(symbol) && symbol != '.')
=======
                if (!char.IsDigit(symbol) && symbol != '.' && symbol != ')')
>>>>>>> Stashed changes
                {
                    errorBuffer.Append(symbol);
                    code.Remove(position);
                }
                else if (symbol == '.')
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "real number", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    position++;
                    break;
                }
<<<<<<< Updated upstream
=======
                else if (symbol == ')')
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "real number", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    position++;
                    parser.State = new EndState();
                    parser.State.Handle(parser, code, position);
                    return;
                }
>>>>>>> Stashed changes
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "real number", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }
                }

                position++;
            }

            errorBuffer.Clear();
            while (position < code.Length)
            {
                symbol = code[position];

                if (!char.IsDigit(symbol) && symbol != ')')
                {
                    errorBuffer.Append(symbol);
                    code.Remove(position);
                }
                else if (symbol == ')')
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "real number", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    position++;
                    break;
                }
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "real number", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }
                }

                position++;
            }

            parser.State = new EndState();
            parser.State.Handle(parser, code, position);
        }
    }
}
