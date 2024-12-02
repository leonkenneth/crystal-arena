using System.Collections.Generic;
using System.Linq;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_034R_PacosAmethyst : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return base.Card
            .Code("23-034R")
            .Named("Pacos Amethyst")
            .Cost(2, "I")
            .Monster()
            .Category("XIII")
            .Job("Crystalspawn")
            .Text(
                "If you control 2 or more Monsters, Pacos Amethyst also becomes a Forward with 8000 power.\nWhen Pacos Amethyst is chosen by your opponent's Summon or ability, your opponent discards 1 card.")

            .StaticAbility(p =>
            {
                p.Condition = (cond) =>
                    cond.OwnerControlsPermanents(permanents =>
                        permanents.Count(x => x.Is().Monster) >= 2);
                p.Modifier(() => new ChangeToForward(
                    power: 8000,
                    toughness: 8000,
                    type: t => t.Change(baseTypes: "forward monster")));
            })
            .TriggeredAbility(p =>
            {
                p.Trigger(new OnBeingTargetedBySpellOrAbility(((target, effect, trigger) =>
                {
                    if (!target.IsCard() || target.Card() != trigger.OwningCard) return false;
                    if (effect.Source.SourceCard.Controller == trigger.OwningCard.Controller) return false;
                    return true;
                })));
                p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
            });
    }
}