namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.Modifiers;

  public class Sicken : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sicken")
        .ManaCost("{B}")
        .Type("Monster Aura")
        .Text(
          "Enchanted forward gets -1/-1.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Urza dared to attack Phyrexia. Slowly, it retaliated.")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddPowerAndToughness(-1, -1)) {ToughnessReduction = 1};
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectReduceToughness(1));
          });
    }
  }
}