namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BarrinMasterWizard
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void BounceDragonSacBackup()
      {
        var dragon1 = C("Shivan Dragon");

        Battlefield(P1, dragon1, "Mountain", "Mountain");
        Battlefield(P2, "Barrin, Master Wizard", "Island", "Island");

        P2.Life = 5;
        RunGame(1);

        Equal(5, P2.Life);
        Equal(Zone.Hand, C(dragon1).Zone);
        Equal(1, P2.BreakZone.Count(x => x.Is().Backup));
      }

      [Fact (Skip = "Old card")]
      public void BounceAttackerSacForward()
      {
        Battlefield(P1, "Shivan Dragon", "Shivan Dragon", "Mountain", "Mountain");
        Battlefield(P2, "Barrin, Master Wizard", "Birds of Paradise", "Island", "Island");

        P2.Life = 5;
        RunGame(1);

        Equal(5, P2.Life);

        Equal(1, P1.Hand.Count(x => x.Is().Forward));
        Equal(2, P2.BreakZone.Count());
      }
    }
  }
}