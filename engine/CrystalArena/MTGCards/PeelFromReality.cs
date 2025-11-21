namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class PeelFromReality : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Peel from Reality")
                .ManaCost("{1}{U}")
                .Type("Summon")
                .Text(
                    "Return target forward you control and target forward you don't control to their owners' hands."
                )
                .FlavorText(
                    "\"Soulless demon, you are bound to me. Now we will both dwell in oblivion.\""
                )
                .Cast(p =>
                {
                    p.Text =
                        "Return target forward you control and target forward you don't control to their owners' hands.";
                    p.Effect = () => new ReturnToHand();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                        trg =>
                        {
                            trg.Message = "Select a target forward you control.";
                        }
                    );

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward(ControlledBy.Opponent).On.Battlefield(),
                        trg =>
                        {
                            trg.Message = "Select a target forward your oppenent controls.";
                        }
                    );

                    p.TargetingRule(new EffectBounceOwnAndOpponents());
                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.Bounce,
                            EffectTag.ForwardsOnly
                        )
                    );
                });
        }
    }
}
