namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class HiddenStag : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hidden Stag")
                .ManaCost("{1}{G}")
                .Type("Monster")
                .Text(
                    "Whenever an opponent plays a backup, if Hidden Stag is an monster, Hidden Stag becomes a 3/2 Elk Beast forward.{EOL}Whenever you play a backup, if Hidden Stag is a forward, Hidden Stag becomes an monster."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever an opponent plays a backup, if Hidden Stag is an monster, Hidden Stag becomes a 3/2 Elk Beast forward.";

                    p.Trigger(
                        new OnBackupPlayed(
                            filter: (ability, card) =>
                                ability.OwningCard.Controller != card.Controller
                                && ability.OwningCard.Is().Monster
                                && card.Is().Backup
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 3,
                                toughness: 2,
                                type: t => t.Change(baseTypes: "forward", subTypes: "elk beast"),
                                colors: L(CardColor.Wind)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever you play a backup, if Hidden Stag is a forward, Hidden Stag becomes an monster.";
                    p.Trigger(
                        new OnBackupPlayed(
                            filter: (ability, card) =>
                                ability.OwningCard.Controller == card.Controller
                                && ability.OwningCard.Is().Forward
                                && card.Is().Backup
                        )
                    );

                    p.Effect = () => new RemoveModifier(typeof(ChangeToForward));

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
