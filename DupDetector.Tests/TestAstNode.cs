using System.Collections.Generic;
using System.Linq;
using DupDetector.Interfaces;

namespace DupDetector.Tests;

public class TestAstNode : IAstNode
{
    private readonly List<TestAstNode> _children = new();

    public IAstNode Parent { get; private set; }
    public IAstNode FirstChild => _children.FirstOrDefault();
    public IAstNode NextSibling { get; private set; }
    public IAstNode PrevSibling { get; private set; }
    public bool IsWhitespaceOrComment { get; set; }
    public string Text { get; set; }

    public string GetText()
    {
        if (FirstChild == null) return Text ?? string.Empty;
        return string.Concat(_children.Select(c => c.GetText()));
    }

    public TestAstNode AddChild(TestAstNode child)
    {
        if (_children.Count > 0)
        {
            var prev = _children[^1];
            prev.NextSibling = child;
            child.PrevSibling = prev;
        }

        child.Parent = this;
        _children.Add(child);
        return this;
    }

    public static TestAstNode Leaf(string text, bool trivia = false)
        => new TestAstNode { Text = text, IsWhitespaceOrComment = trivia };

    public static TestAstNode Node(params TestAstNode[] children)
    {
        var n = new TestAstNode();
        foreach (var c in children) n.AddChild(c);
        return n;
    }
}