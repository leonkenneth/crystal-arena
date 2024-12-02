namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class SoulSculptor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul Sculptor")
        .ManaCost("{2}{W}")
        .Type("Forward Human")
        .Text(
          "{1}{W},{T}: Target forward becomes an monster and loses all abilities until a player casts a forward spell.")
        .FlavorText("Does the stone mimic life, or did it once live?")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{W},{T}: Target forward becomes an monster and loses all abilities until a player casts a forward spell.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{W}".Parse()),
              new Tap());

            p.Effect = () => new ApplyModifiersToTargets(
              () =>
                {
                  var modifier = new ChangeToMonster();
                  modifier.AddLifetime(new PlayerCastsForwardLifetime());
                  return modifier;
                },
              () =>
                {
                  var modifier = new DisableAllAbilities(activated: true, simple: true, triggered: true);
                  modifier.AddLifetime(new PlayerCastsForwardLifetime());
                  return modifier;
                });

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.CombatDisabler, combatOnly: true));
          });
    }
  }
}