namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class MobileFort : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mobile Fort")
        .ManaCost("{4}")
        .Type("Artifact Forward Wall")
        .Text(
          "Defender (This forward can't attack.){EOL}{3}: Mobile Fort gets +3/-1 until end of turn and can attack this turn as though it didn't have defender. Activate this ability only once each turn.")
        .Power(0)
        .Toughness(6)
        .SimpleAbilities(Static.Defender)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{3}: Mobile Fort gets +3/-1 until end of turn and can attack this turn as though it didn't have defender. Activate this ability only once each turn.";
            p.Cost = new PayMana(3.Colorless());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(3, -1) {UntilEot = true},
              () => new RemoveAbility(Static.Defender) {UntilEot = true});

            p.ActivateOnlyOnceEachTurn = true;

            p.TimingRule(new BeforeYouDeclareAttackers());
          });
    }
  }
}