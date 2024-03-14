using GameController;
using GameObjects;
using GameUtilities;

namespace Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestWordChecker()
    {
        AhoCorasickTrie wordChecker = new(["Hello","Cruel","World","Hellokitty","Helloskitty"]);
        Assert.IsTrue(wordChecker.CheckWord("Hello"));
        Assert.IsTrue(wordChecker.CheckWord("Cruel"));
        Assert.IsTrue(wordChecker.CheckWord("World"));
        Assert.IsTrue(wordChecker.CheckWord("Hellokitty"));

        Assert.IsFalse(wordChecker.CheckWord("Hellos"));
        Assert.IsFalse(wordChecker.CheckWord("Cru"));
        Assert.IsFalse(wordChecker.CheckWord("Dog"));
        Assert.IsFalse(wordChecker.CheckWord("Cat"));
    }
}

