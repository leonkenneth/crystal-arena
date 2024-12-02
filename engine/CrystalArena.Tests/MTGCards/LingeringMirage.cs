namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class LingeringMirage
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void EnchantOpponentsForest()
      {
        Battlefield(P1, "Island", "Island");
        Battlefield(P2, "Forest");
        Hand(P1, "Lingering Mirage");

        RunGame(1);

        Equal(1, P2.Battlefield.Backups.Count(x => x.Is("island")));
      }
    }

    public class Predefine : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void EnchantThenDisenchant()
      {
        var mirage = C("Lingering Mirage");
        var forest = C("Forest");
        var disenchant = C("Disenchant");

        Battlefield(P2, forest);
        Hand(P1, mirage);
        Hand(P2, disenchant);

        Exec(
          At(Step.FirstMain)
            .Cast(mirage, target: forest)
            .Verify(() =>
              {
                True(P2.HasMana(Mana.Water));
                False(P2.HasMana(Mana.Wind));
              }),
          At(Step.SecondMain)
            .Cast(disenchant, target: mirage)
            .Verify(() =>
              {
                False(P2.HasMana(Mana.Water));
                True(P2.HasMana(Mana.Wind));
              })
          );
      }
    }
  }
}