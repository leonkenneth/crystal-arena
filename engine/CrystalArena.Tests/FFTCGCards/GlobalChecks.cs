using Xunit;
using Assert = Xunit.Assert;

namespace CrystalArena.Tests.FFTCGCards;

public class GlobalChecks
{
    [Fact]
    public void AllForwardsHavePower()
    {
        var serials = CrystalArena.Cards.ListSerials();
        var failingCards = new List<string>();

        foreach (var serial in serials)
        {
            var card = CrystalArena.Cards.Create(serial);
            if (card.Is().Forward && card.Power == null) {
                failingCards.Add(serial);
            }
        }

        if (failingCards.Any())
        {
            Assert.Fail("These cards are Forwards without Power: " + string.Join(", ", failingCards));
        }
    }
}