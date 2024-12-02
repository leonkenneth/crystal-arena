namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class JaceTheLivingGuildpact
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void LookAtTop2Cards()
      {
        var jace = C("Jace, the Living Guildpact");
        var island = C("Island");
        
        Hand(P1, jace);
        MainDeck(P1, island, "Centaur Courser");
        Battlefield(P1, "Forest", "Forest", "Island", "Island", "Island", "Island");

        RunGame(1);
        
        Equal(6, C(jace).Loyality);
        Equal(Zone.BreakZone, C(island).Zone);
      }
    }
  }
}