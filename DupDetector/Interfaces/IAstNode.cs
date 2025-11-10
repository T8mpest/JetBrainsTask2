namespace DefaultNamespace;

public interface IAstNode
{
    IAstNode Parent { get; }
    IAstNode FirstChild { get; }
    IAstNode NextSibling { get; }
    IAstNode PrevSibling { get; }

    bool IsWhitespaceOrComment { get; }
    
    string GetText();
}