namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.CostRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class GoblinOffensive : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Offensive")
        .ManaCost("{1}{R}{R}").HasXInCost()
        .Type("Sorcery")
        .Text("Put X 1/1 fire Goblin forward tokens onto the battlefield.")
        .FlavorText("They certainly are.")
        .Cast(p =>
          {
            p.Effect = () => new CreateTokens(
              count: Value.PlusX,
              token: Card
                .Named("Goblin")
                .FlavorText(
                  "When you're a goblin, you don't have to step forward to be a hero—everyone else just has to step back.")
                .Power(1)
                .Toughness(1)
                .Type("Token Forward - Goblin")
                .Colors(CardColor.Fire)
              );

            p.TimingRule(new WhenYouHaveMana(6));
            p.CostRule(new XIsAvailableMana());
          });
    }
  }
}