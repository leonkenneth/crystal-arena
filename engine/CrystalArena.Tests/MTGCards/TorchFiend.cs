namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TorchFiend
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyCathodion()
      {
        var torchFiend = C("Torch Fiend");
        var cathodion = C("Cathodion");

        Battlefield(P1, torchFiend, "Mountain");
        Battlefield(P2, cathodion);

        RunGame(2);

        Equal(Zone.BreakZone, C(torchFiend).Zone);
        Equal(Zone.BreakZone, C(cathodion).Zone);
      }
    }
  }
}