namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;

  public class EphemeralShields : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ephemeral Shields")
        .ManaCost("{1}{W}")
        .Type("Summon")
        .Text("{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Target forward gains indestructible until end of turn.{I}(Damage and effects that say \"destroy\" don't destroy it.){/I}")
        .FlavorText("\"Even your shadow is too foul to tolerate.\"")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
        {
          p.Text = "Target forward gains indestructible until end of turn.";

          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddSimpleAbility(Static.Indestructible) { UntilEot = true }).SetTags(
              EffectTag.Indestructible);

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

          p.TargetingRule(new EffectGiveIndestructible());
        });
    }
  }
}
