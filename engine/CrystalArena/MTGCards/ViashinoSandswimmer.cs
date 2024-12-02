namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class ViashinoSandswimmer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Viashino Sandswimmer")
        .ManaCost("{2}{R}{R}")
        .Type("Forward Viashino")
        .Text(
          "{R}: Flip a coin. If you win the flip, return Viashino Sandswimmer to its owner's hand. If you lose the flip, sacrifice Viashino Sandswimmer.")
        .FlavorText("Few swim in a place of such thirst.")
        .Power(3)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}: Flip a coin. If you win the flip, return Viashino Sandswimmer to its owner's hand. If you lose the flip, sacrifice Viashino Sandswimmer.";
            p.Cost = new PayMana(Mana.Fire);
            p.Effect = () => new FlipACoinReturnToHandOrSacrifice();
            p.TimingRule(new WhenOwningCardWillBeDestroyed());
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}