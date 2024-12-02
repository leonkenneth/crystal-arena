namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class GhituEncampment : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ghitu Encampment")
        .Type("Backup")
        .Text(
          "Ghitu Encampment enters the battlefield tapped.{EOL}{T}: Add {R} to your mana pool.{EOL}{1}{R}: Ghitu Encampment becomes a 2/1 fire Warrior forward with first strike until end of turn. It's still a backup.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} to your mana pool.";
            p.ManaAmount(Mana.Fire);
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{R}: Ghitu Encampment becomes a 2/1 fire Warrior forward with first strike until end of turn. It's still a backup.";

            p.Cost = new PayMana("{1}{R}".Parse());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 2,
                toughness: 1,
                colors: L(CardColor.Fire),
                type: t => t.Add(baseTypes: "forward", subTypes: "warrior")) { UntilEot = true },
              () => new AddSimpleAbility(Static.FirstStrike) { UntilEot = true });

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}