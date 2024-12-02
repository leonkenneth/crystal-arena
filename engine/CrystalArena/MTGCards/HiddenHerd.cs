namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class HiddenHerd : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Herd")
        .ManaCost("{G}")
        .Type("Monster")
        .Text(
          "When an opponent plays a nonbasic backup, if Hidden Herd is an monster, Hidden Herd becomes a 3/3 Beast forward.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent plays a nonbasic backup, if Hidden Herd is an monster, Hidden Herd becomes a 3/3 Beast forward.";

            p.Trigger(new OnBackupPlayed(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Monster &&
                  card.Is().NonBasicBackup));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToForward(
              power: 3,
              toughness: 3,
              type: t => t.Change(baseTypes: "forward", subTypes: "beast"),
              colors: L(CardColor.Wind)
              ));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}