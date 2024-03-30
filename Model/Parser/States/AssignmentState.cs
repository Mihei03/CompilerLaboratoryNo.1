using System.Text;

namespace CompilerDemo.Model.Parser.States
{
    internal class AssignmentState : IState
    {
        public string Handle(Parser parser, string code, int position)
        {
            if (position >= code.Length)
            {
                parser.AddError(new ParseError(position, position, "incomplete line", ""));
                return code;
            }

            StringBuilder errorBuffer = new StringBuilder();

            if (code[position] != '=')
            {
                parser.AddError(new ParseError(position, position, "assignment operator (=)", code[position].ToString()));
            }

            position++;

            parser.State = new ComplexState();
            return parser.State.Handle(parser, code, position);
        }
    }
}
