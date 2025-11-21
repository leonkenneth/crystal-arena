namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class CausticTar : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Caustic Tar")
                .ManaCost("{4}{B}{B}")
                .Type("Monster — Aura")
                .Text(
                    "Enchant backup{EOL}Enchanted backup has \"{T}: Target player loses 3 life.\""
                )
                .FlavorText(
                    "A forest fire can rejuvenate the backup, but the tar's vile consumption leaves the backup forever ruined."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() =>
                        {
                            var ap = new ActivatedAbilityParameters
                            {
                                Text = "{T}: Target player loses 3 life.",
                                Cost = new Tap(),
                                Effect = () => new ChangeLife(-3, P(e => e.Target.Player())),
                            };

                            ap.TargetSelector.AddEffect(trg => trg.Is.Player());

                            ap.TimingRule(
                                new Any(
                                    new OnEndOfOpponentsTurn(),
                                    new WhenOwningCardWillBeDestroyed()
                                )
                            );

                            ap.TargetingRule(new EffectOpponent());

                            return new AddActivatedAbility(new ActivatedAbility(ap));
                        });

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Backup).On.Battlefield()
                    );

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectBackupMonster(ControlledBy.SpellOwner));
                });
        }
    }
}
