namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BirdsOfParadise
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PlayBearWithBird()
      {
        var bear = C("Grizzly Bears");

        Battlefield(P1, "Mountain", "Birds of Paradise");
        Hand(P1, bear);

        RunGame(maxTurnCount: 2);
        Equal(Zone.Battlefield, C(bear).Zone);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void AddOneAnyManaToPool()
      {
        var bird = C("Birds of Paradise");

        Battlefield(P1, bird);

        Exec(
          At(Step.FirstMain)
            .Activate(bird)
            .Verify(() =>
              {
                True(P1.HasMana(Mana.Light));
                True(P1.HasMana(Mana.Water));
                True(P1.HasMana(Mana.Dark));
                True(P1.HasMana(Mana.Fire));
                True(P1.HasMana(Mana.Wind));
              })
          );
      }
    }
  }
}