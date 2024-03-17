using CompilerDemo.Model.Parser.States;
using System.Collections.Generic;

namespace CompilerDemo.Model.Parser
{
    internal class Parser
    {
        public IState State { get; set; }
        private List<ParseError> Errors { get; set; }
        public Parser()
        {
            Errors = new List<ParseError>();
            State = new ComplexState();
        }

        public List<ParseError> Parse(string code)
        {
            code = code.Replace("\r", string.Empty);
            Errors.Clear();
            State = new IdentifierState();
            State.Handle(this, code, 0);
            return Errors;
        }

        public void AddError(ParseError error)
        {
            Errors.Add(error);
        }
    }
}
