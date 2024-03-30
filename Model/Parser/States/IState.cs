namespace CompilerDemo.Model.Parser.States
{
    internal interface IState
    {
        string Handle(Parser parser, string code, int position);
    }
}
