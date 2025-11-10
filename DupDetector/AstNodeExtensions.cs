namespace DefaultNamespace;

public static class AstNodeExtensions
{

    // Checks if two AST nodes are equivalent ignoring whitespace/comment tokens.

    public static bool IsEquivalentTo(this IAstNode node, IAstNode other)
    {
        if (ReferenceEquals(node, other)) return true;
        if (node is null || other is null) return false;

        using var e1 = EnumerateNonTriviaLeafTexts(node).GetEnumerator();
        using var e2 = EnumerateNonTriviaLeafTexts(other).GetEnumerator();

        while (true)
        {
            var m1 = e1.MoveNext();
            var m2 = e2.MoveNext();
            if (!m1 || !m2) return m1 == m2;
            if (!string.Equals(e1.Current, e2.Current, System.StringComparison.Ordinal))
                return false;
        }
    }

    private static IEnumerable<string> EnumerateNonTriviaLeafTexts(IAstNode root)
    {
        var stack = new Stack<IAstNode>();
        if (root != null) stack.Push(root);

        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n.IsWhitespaceOrComment) continue;

            var first = n.FirstChild;
            if (first == null)
            {
                yield return n.GetText();
                continue;
            }

            // push children right-to-left to traverse left-to-right
            var child = LastChildOf(n);
            while (child != null)
            {
                stack.Push(child);
                child = child.PrevSibling;
            }
        }
    }

    private static IAstNode LastChildOf(IAstNode node)
    {
        var c = node.FirstChild;
        if (c == null) return null;
        while (c.NextSibling != null) c = c.NextSibling;
        return c;
    }
}