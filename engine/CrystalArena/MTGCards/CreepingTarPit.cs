namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class CreepingTarPit : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Creeping Tar Pit")
                .Type("Backup")
                .Text(
                    "Creeping Tar Pit enters the battlefield tapped.{EOL}{T}: Add {U} or {B} to your mana pool.{EOL}{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 water and dark Elemental forward and is unblockable. It's still a backup."
                )
                .Cast(p => p.Effect = () => new CastPermanent(tap: true))
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {U} or {B} to your mana pool.";
                    p.ManaAmount(Mana.Colored(isBlue: true, isBlack: true));
                    p.Priority = ManaSourcePriorities.OnlyIfNecessary;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 water and dark Elemental forward and is unblockable. It's still a backup.";

                    p.Cost = new PayMana("{1}{U}{B}".Parse());

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 3,
                                    toughness: 2,
                                    type: t => t.Add(baseTypes: "forward", subTypes: "elemental"),
                                    colors: L(CardColor.Water, CardColor.Dark)
                                )
                                {
                                    UntilEot = true,
                                },
                            () => new AddSimpleAbility(Static.Unblockable) { UntilEot = true }
                        );

                    p.TimingRule(new WhenStackIsEmpty());
                    p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
                    p.TimingRule(new WhenYouHaveMana(4));
                    p.TimingRule(
                        new Any(
                            new BeforeYouDeclareAttackers(),
                            new AfterOpponentDeclaresAttackers()
                        )
                    );
                });
        }
    }
}
