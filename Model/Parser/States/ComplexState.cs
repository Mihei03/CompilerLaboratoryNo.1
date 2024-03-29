﻿using System.Text;

namespace CompilerDemo.Model.Parser.States
{
    internal class ComplexState : IState
    {
        public void Handle(Parser parser, string code, int position)
        {
            if (position >= code.Length)
            {
                parser.AddError(new ParseError(position, position, "incomplete line", ""));
                return;
            }

            string whitespaceCharacters = " \n\r\t";
            while (whitespaceCharacters.Contains(code[position]))
            {
                code = code.Remove(position, 1);
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return;
                }

            }


            string expected = "complex(";

            StringBuilder errorBuffer = new StringBuilder();
            for (int expectedPos = 0; expectedPos < expected.Length; expectedPos++)
            {
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return;
                }

                if (expected[expectedPos] != code[position])
                {
                    errorBuffer.Append(code[position]);
                    code = code.Remove(position, 1);
                    expectedPos--;
                }
                else
                {
                    if(errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, expected, errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    position++;
                }
            }

            parser.State = new RealPartState();
            parser.State.Handle(parser, code, position);
        }
    }
}
