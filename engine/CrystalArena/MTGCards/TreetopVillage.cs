namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class TreetopVillage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treetop Village")
        .Type("Backup")
        .Text(
          "Treetop Village enters the battlefield tapped.{EOL}{T}: Add {G} to your mana pool.{EOL}{1}{G}: Treetop Village becomes a 3/3 wind Ape forward with trample until end of turn. It's still a backup.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(Mana.Wind);
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{G}: Treetop Village becomes a 3/3 wind Ape forward with trample until end of turn. It's still a backup.";

            p.Cost = new PayMana("{1}{G}".Parse());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 3,
                toughness: 3,
                colors: L(CardColor.Wind),
                type: t => t.Add(baseTypes: "forward", subTypes: "ape")) {UntilEot = true},
              () => new AddSimpleAbility(Static.Trample) {UntilEot = true});

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}