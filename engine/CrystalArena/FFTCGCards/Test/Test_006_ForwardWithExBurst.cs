using System.Collections.Generic;
using CrystalArena.Effects;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Test;

public class Test_006_ForwardWithExBurst : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("0-006X")
            .Named("Test Forward with ExBurst")
            .Cost(2, "R")
            .Forward(multiplayable: true)
            .Power(5000)
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new DealDamageToTargets(20000);
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            });
    }
}
