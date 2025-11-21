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

public class Opus23_026C_Jegran : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-026C")
            .Named("Jegran")
            .Cost(4, "I")
            .Category("DFF · FFCC")
            .Job("Knight")
            .Forward()
            .Power(8000)
            .Text(
                "When Jegran is put from the field into the Break Zone, select 1 of the 2 following actions.\n\"Choose 1 dull Forward. Break it.\"\n\"Your opponent discards 2 cards.\""
            )
            .TriggeredAbility(p =>
            {
                p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone));
                p.Effect = () =>
                    new ModalEffect(
                        1,
                        ceap =>
                        {
                            ceap.Text = "Choose 1 dull Forward. Break it.";
                            ceap.Effect = () => new DestroyTargetPermanents();
                            ceap.TargetSelector.AddEffect(
                                (trg) =>
                                    trg.Is.Card(x => x.Is().Forward && x.IsTapped).On.Battlefield()
                            );
                        },
                        (ceap) =>
                        {
                            ceap.Text = "Your opponent discards 2 cards.";
                            ceap.Effect = () => new DiscardCards(2, P(e => e.Controller.Opponent));
                        }
                    );
            });
    }
}
