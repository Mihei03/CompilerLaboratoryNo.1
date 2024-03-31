using System.Text;

namespace CompilerDemo.Model.Parser.States
{
    internal class IdentifierState : IState
    {
        public string Handle(Parser parser, string code, int position)
        {
            char symbol;

            StringBuilder errorBuffer = new StringBuilder();
            while (position < code.Length)
            {
                if (position >= code.Length)
                {
                    parser.AddError(new ParseError(position, position, "incomplete line", ""));
                    return code;
                }

                symbol = code[position];
                if (!char.IsLetter(symbol) && symbol != '_')
                {
                    errorBuffer.Append(symbol);
                    code = code.Remove(position, 1);
                }
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "identifier start", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }

                    break;
                }
            }

            errorBuffer.Clear();
            while (position < code.Length)
            {
                symbol = code[position];

                if (symbol == '=')
                {
                    position++;
                    break;
                }
                if (symbol == ' ')
                {
                    position++;
                    break;
                }

                if (!char.IsLetter(symbol) && !char.IsDigit(symbol) && symbol != '_')
                {
                    errorBuffer.Append(symbol);
                    code = code.Remove(position, 1);
                }
                else
                {
                    if (errorBuffer.Length > 0)
                    {
                        parser.AddError(new ParseError(position + 1, position + errorBuffer.Length, "identifier", errorBuffer.ToString()));
                        errorBuffer.Clear();
                    }
                    position++;
                }
            }

            parser.State = new AssignmentState();
            return parser.State.Handle(parser, code, position);
        }
    }
}