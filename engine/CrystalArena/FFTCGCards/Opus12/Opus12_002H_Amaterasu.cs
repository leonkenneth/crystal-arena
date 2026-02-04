using System.Collections.Generic;
using CrystalArena.Effects;

namespace CrystalArena.FFTCGCards.Opus12;

public class Opus12_002H_Amaterasu : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("12-002H")
            .Named("Amaterasu")
            .Cost(3, "R")
            .Category("FFEX")
            .Summon()
            .Text(
                "Choose 1 auto-ability. Cancel its effect. If the cancelled auto-ability triggered from a Forward, deal that Forward 8000 damage."
            )
            .Cast(p =>
            {
                p.Text =
                    "Choose 1 auto-ability. Cancel its effect. If the cancelled auto-ability triggered from a Forward, deal that Forward 8000 damage.";
                p.Effect = () => new CounterTriggeredAbilityAndDamageSourceForward(8000);
                p.TargetSelector.AddEffect(trg => trg.Is.CounterableTriggeredAbility().On.Stack());
            });
    }
}
