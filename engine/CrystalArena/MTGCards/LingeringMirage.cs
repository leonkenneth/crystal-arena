namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class LingeringMirage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lingering Mirage")
        .ManaCost("{1}{U}")
        .Type("Monster Aura")
        .Text("{Enchant backup}{EOL}Enchanted backup is an Island.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Birds frozen in flight. Sea turned to glass. Tolaria hidden in a mirror.")
        .Cycling("{2}")
        .OverrideScore(p => p.Battlefield = 250)
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new ChangeBasicBackupSubtype("island", replace: true));
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield());
            p.TargetingRule(new EffectBackupMonster(ControlledBy.Opponent));
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}