using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_014H_Nelapa : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-014H")
            .Named("Nelapa")
            .Cost(2, "R")
            .Category("VI")
            .Job("Mage")
            .Forward()
            .Power(5000)
            .Text(
                "When your opponent casts a Summon, select up to 2 of the 2 following actions.\n\"Choose 1 Forward. Deal it 10000 damage.\"\n\"Nelapa deals your opponent 1 point of damage.\"")
            .TriggeredAbility(p =>
            {
                p.TriggerOnlyIfOwningCardIsInPlay = true;
                p.Trigger(new OnCastedSpell((c, ctx) =>
                    ctx.Opponent == c.Owner && c.Is().Summon));
                
                p.Effect = () => new ModalEffect(maxCount: 2, (ceap) =>
                {
                    ceap.Effect = () => new DealDamageToTargets(10000);
                    ceap.TargetSelector.AddEffect((trg) => trg.Is.Forward().On.Battlefield());
                }, (ceap) =>
                {
                    ceap.Effect = () => new DealDamageToPlayer(1, P(e => e.Controller.Opponent));
                });
            });
    }
}