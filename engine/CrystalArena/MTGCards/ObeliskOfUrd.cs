namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class ObeliskOfUrd : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Obelisk of Urd")
                .ManaCost("{6}")
                .Type("Artifact")
                .Text(
                    "{Convoke} (Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){EOL}As Obelisk of Urd enters the battlefield, choose a forward type.{EOL}Forwards you control of the chosen type get +2/+2."
                )
                .SimpleAbilities(Static.Convoke)
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () => new ForwardsOfChosenTypeGainPT(2, 2, ControlledBy.SpellOwner);
                    p.UsesStack = false;
                });
        }
    }
}
