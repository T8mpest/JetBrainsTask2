using Xunit;
using DupDetector;

namespace DupDetector.Tests;


public class AstNodeExtensionsTests
{
  [Fact]
  public void Equivalent_WhenOnlyWhitespaceDiffers()
  {
    var left = TestAstNode.Node(
      TestAstNode.Leaf("a"),
      TestAstNode.Leaf(" ", trivia: true),
      TestAstNode.Leaf("+"),
      TestAstNode.Leaf(" ", trivia: true),
      TestAstNode.Leaf("b")
    );
    var right = TestAstNode.Node(
      TestAstNode.Leaf("a"),
      TestAstNode.Leaf("+"),
      TestAstNode.Leaf("b")
    );
    Assert.True(left.IsEquivalentTo(right));
  }

  [Fact]
  public void Equivalent_WhenCommentsDiffer()
  {
    var left = TestAstNode.Node(
      TestAstNode.Leaf("a"),
      TestAstNode.Leaf("/*x*/", trivia: true),
      TestAstNode.Leaf("+"),
      TestAstNode.Leaf("/*y*/", trivia: true),
      TestAstNode.Leaf("b")
    );
    var right = TestAstNode.Node(
      TestAstNode.Leaf("a"),
      TestAstNode.Leaf(" ", trivia: true),
      TestAstNode.Leaf("+"),
      TestAstNode.Leaf(" ", trivia: true),
      TestAstNode.Leaf("b")
    );
    Assert.True(left.IsEquivalentTo(right));
  }

  [Fact]
  public void NotEquivalent_WhenTokensDiffer()
  {
    var left = TestAstNode.Node(TestAstNode.Leaf("a"), TestAstNode.Leaf("+"), TestAstNode.Leaf("b"));
    var right = TestAstNode.Node(TestAstNode.Leaf("a"), TestAstNode.Leaf("-"), TestAstNode.Leaf("b"));
    Assert.False(left.IsEquivalentTo(right));
  }

  [Fact]
  public void Equivalent_ForNestedTrees_IgnoringTrivia()
  {
    var left = TestAstNode.Node(
      TestAstNode.Leaf("("),
      TestAstNode.Node(
        TestAstNode.Leaf("a"),
        TestAstNode.Leaf(" ", trivia: true),
        TestAstNode.Leaf("+"),
        TestAstNode.Leaf(" ", trivia: true),
        TestAstNode.Leaf("("),
        TestAstNode.Node(TestAstNode.Leaf("b"), TestAstNode.Leaf("*"), TestAstNode.Leaf("c")),
        TestAstNode.Leaf(")")
      ),
      TestAstNode.Leaf(")")
    );

    var right = TestAstNode.Node(
      TestAstNode.Leaf("("),
      TestAstNode.Node(
        TestAstNode.Leaf("a"),
        TestAstNode.Leaf("+"),
        TestAstNode.Leaf("("),
        TestAstNode.Node(TestAstNode.Leaf("b"), TestAstNode.Leaf("*"), TestAstNode.Leaf("c")),
        TestAstNode.Leaf(")")
      ),
      TestAstNode.Leaf(")")
    );

    Assert.True(left.IsEquivalentTo(right));
  }

  [Fact]
  public void NotEquivalent_DifferentLengthAfterFiltering()
  {
    var left = TestAstNode.Node(TestAstNode.Leaf("a"), TestAstNode.Leaf("+"), TestAstNode.Leaf("b"));
    var right = TestAstNode.Node(
      TestAstNode.Leaf("a"), TestAstNode.Leaf("+"), TestAstNode.Leaf("b"),
      TestAstNode.Leaf("+"), TestAstNode.Leaf("c"));
    Assert.False(left.IsEquivalentTo(right));
  }
}