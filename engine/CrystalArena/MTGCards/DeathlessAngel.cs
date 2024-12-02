namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.Modifiers;

  public class DeathlessAngel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Deathless Angel")
        .ManaCost("{4}{W}{W}")
        .Type("Forward Angel")
        .Text("{Flying}{EOL}{W}{W}: Target forward is indestructible this turn.")
        .FlavorText(
          "I should have died that day, but I suffered not a scratch. I awoke in a lake of blood, none of it apparently my own.")
        .Power(5)
        .Toughness(7)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{W}{W}: Target forward is indestructible this turn.";
            p.Cost = new PayMana("{W}{W}".Parse());

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddSimpleAbility(Static.Indestructible) {UntilEot = true})
              .SetTags(EffectTag.Indestructible);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectGiveIndestructible());
          });
    }
  }
}