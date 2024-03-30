using System.Runtime.Versioning;
using System.Text;

namespace CompilerDemo.Model.Parser.States
{
    internal class EndState : IState
    {
        public string Handle(Parser parser, string code, int position)
        {
            StringBuilder errorBuffer = new StringBuilder();
            while (position < code.Length)
            {
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return code;
                }
                char c = code[position];
                if (c != '\n')
                {
                    errorBuffer.Append(c);
                    code = code.Remove(position, 1);
                }
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "newline", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    break;
                }
            }

            position++;

            if (position == code.Length)
            {
                return code;
            }

            parser.State = new IdentifierState();
            return parser.State.Handle(parser, code, position);
        }
    }
}