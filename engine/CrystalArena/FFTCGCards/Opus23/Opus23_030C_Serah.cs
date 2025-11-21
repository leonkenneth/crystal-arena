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

public class Opus23_030C_Serah : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-030C")
            .Named("Serah")
            .Cost(3, "I")
            .Category("PICTLOGICA · XIII")
            .Job("Ravager")
            .Backup()
            .Text(
                "When Serah enters the field, select 1 of the 2 following actions. If you control a Job Commando, select up to 2 of the 2 following actions instead.\n\"Choose 1 Character. Dull it and Freeze it.\"\n\"Your opponent discards 1 card.\""
            )
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Serah enters the field, select 1 of the 2 following actions. If you control a Job Commando, select up to 2 of the 2 following actions instead.\n\"Choose 1 Character. Dull it and Freeze it.\"\n\"Your opponent discards 1 card.\"";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () =>
                    new ModalEffect(
                        P(e => e.Controller.ControlsAny(c => c.HasJob("Commando")) ? 2 : 1),
                        (ceap) =>
                        {
                            ceap.Text = "Choose 1 Character. Dull it and Freeze it.";
                            ceap.Effect = () => new DullAndFreezeTargets();
                            ceap.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());
                        },
                        ceap =>
                        {
                            ceap.Text = "Your opponent discards 1 card.";
                            ceap.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
                        }
                    );
            });
    }
}
