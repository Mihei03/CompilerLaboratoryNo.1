namespace CompilerDemo.Model.Parser.States
{
    internal interface IState
    {
        void Handle(Parser parser, string code, int position);
    }
}
