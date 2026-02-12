
using NUnit.Framework;

public class HandTest
{
    [Test]
    public void IsWin_RockVsScissors_ReturnsTrue()
    {
        var hand = new Hand(HandType.Rock, HandPosType.LeftUp);
        Assert.IsTrue(hand.IsWin(HandType.Scissors));
    }

    [Test]
    public void IsWin_RockVsPaper_ReturnsFalse()
    {
        var hand = new Hand(HandType.Rock, HandPosType.LeftUp);
        Assert.IsFalse(hand.IsWin(HandType.Paper));
    }

    [Test]
    public void IsWin_RockVsRock_ReturnsFalse()
    {
        var hand = new Hand(HandType.Rock, HandPosType.LeftUp);
        Assert.IsFalse(hand.IsWin(HandType.Rock));
    }

    [Test]
    public void IsWin_ScissorsVsPaper_ReturnsTrue()
    {
        var hand = new Hand(HandType.Scissors, HandPosType.LeftUp);
        Assert.IsTrue(hand.IsWin(HandType.Paper));
    }

    [Test]
    public void IsWin_PaperVsRock_ReturnsTrue()
    {
        var hand = new Hand(HandType.Paper, HandPosType.LeftUp);
        Assert.IsTrue(hand.IsWin(HandType.Rock));
    }
}
