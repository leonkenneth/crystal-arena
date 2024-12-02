namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class ForbiddingWatchtower : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Forbidding Watchtower")
        .Type("Backup")
        .Text(
          "Forbidding Watchtower enters the battlefield tapped.{EOL}{T}: Add {W} to your mana pool.{EOL}{1}{W}: Forbidding Watchtower becomes a 1/5 light Soldier forward until end of turn. It's still a backup.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} to your mana pool.";
            p.ManaAmount(Mana.Light);
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{W}: Forbidding Watchtower becomes a 1/5 light Soldier forward until end of turn. It's still a backup.";

            p.Cost = new PayMana("{1}{W}".Parse());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 1,
                toughness: 5,
                colors: L(CardColor.Light),
                type: t => t.Add(baseTypes: "forward", subTypes: "soldier")) { UntilEot = true });
            
            p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}