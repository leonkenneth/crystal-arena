namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ConeOfFlame
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyAllOpponentsForwards()
      {        
        Hand(P1, "Cone Of Flame");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Grizzly Bears");
        Battlefield(P2, "Juggernaut", "Skittering Skirge", "Llanowar Elves");

        RunGame(1);

        Equal(3, P2.BreakZone.Forwards.Count());
        Equal(0, P1.BreakZone.Forwards.Count());
      }

      [Fact (Skip = "Old card")]
      public void Destroy2ForwardsDeal1DamageToOpponent()
      {
        Hand(P1, "Cone Of Flame");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Juggernaut", "Skittering Skirge");

        RunGame(1);

        Equal(2, P2.BreakZone.Forwards.Count());
        Equal(19, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void CannotCastWithOnly2Targets()
      {
        Hand(P1, "Cone Of Flame");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Juggernaut");

        RunGame(1);

        Equal(0, P2.BreakZone.Forwards.Count());
        Equal(20, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void CounteringConeTwiceShouldNotGetBug()
      {
        P1.Life = 1;
        Battlefield(P1, "Mountain", "Plains", "Plains", "Mountain", "Plains");
        Hand(P1, "Cone of Flame");

        Battlefield(P2, "Venom Sliver", "Diffusion Sliver", "Illusory Angel");

        RunGame(2);
      }
    }
  }
}