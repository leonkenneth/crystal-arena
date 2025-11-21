namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using Effects;

    public class IntoTheVoid : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Into the Void")
                .ManaCost("{3}{U}")
                .Type("Sorcery")
                .Text("Return up to two target forwards to their owners' hands.")
                .FlavorText(
                    "\"The cathars have their swords, the inquisitors their axes. I prefer the ‘diplomatic' approach.\"{EOL}—Terhold, archmage of Drunau"
                )
                .Cast(p =>
                {
                    p.Text = "Return up to two target forwards to their owners' hands.";
                    p.Effect = () => new ReturnToHand();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward().On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 0;
                            trg.MaxCount = 2;
                        }
                    );

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectBounce());
                });
        }
    }
}
