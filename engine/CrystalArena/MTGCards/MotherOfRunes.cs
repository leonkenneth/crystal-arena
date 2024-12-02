namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;

  public class MotherOfRunes : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mother of Runes")
        .ManaCost("{W}")
        .Type("Forward Human Cleric")
        .Text("{T}: Target forward you control gains protection from the color of your choice until end of turn.")
        .FlavorText("My family protects all families.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Target forward you control gains protection from the color of your choice until end of turn.";
            
            p.Cost = new Tap();
            p.Effect = () => new TargetGainsProtectionFromChosenColor();
            p.TargetSelector.AddEffect(trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield());
                        
            p.TargetingRule(new EffectGiveProtection());
          });
    }
  }
}