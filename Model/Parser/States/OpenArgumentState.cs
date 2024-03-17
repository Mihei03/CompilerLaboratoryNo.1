﻿namespace CompilerDemo.Model.Parser.States
{
    internal class OpenArgumentState : IState
    {
        public void Handle(Parser parser, string code, int position)
        {
            if (code[position] != '(')
            {
                //ОШИБКА
            }

            position++;
            parser.State = new RealPartState();
            parser.State.Handle(parser, code, position);
        }
    }
}
