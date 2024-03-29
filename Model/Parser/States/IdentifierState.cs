﻿using System.Text;

namespace CompilerDemo.Model.Parser.States
{
    internal class IdentifierState : IState
    {
        public void Handle(Parser parser, string code, int position)
        {
            char c;

            StringBuilder errorBuffer = new StringBuilder();
            while (position < code.Length)
            {
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return;
                }

                c = code[position];
                if(!char.IsLetter(c) && c != '_')
                {
                    errorBuffer.Append(c);
                    code.Remove(position);
                }
                else
                {
                    if(errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "identifier start", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    break;
                }
                position++;
            }

            errorBuffer.Clear();
            while (position < code.Length)
            {
                c = code[position];

                if(c == '=')
                {
                    position++;
                    break;
                }

                if(!char.IsLetter(c) && !char.IsDigit(c) && c == '_')
                {
                    errorBuffer.Append(c);
                    code.Remove(position, 1);
                }
                else if(c == ' ')
                {
                    position++;
                    break;
                }
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "identifier", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }
                }
                    position++;
            }

            parser.State = new AssignmentState();
            parser.State.Handle(parser, code, position);
        }
    }
}
