namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class OpalArchangel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Archangel")
        .ManaCost("{4}{W}")
        .Type("Monster")
        .Text(
          "When an opponent casts a forward spell, if Opal Archangel is an monster, Opal Archangel becomes a 5/5 Angel forward with flying and brave.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a forward spell, if Opal Archangel is an monster, Opal Archangel becomes a 5/5 Angel forward with flying and brave.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Monster && c.Is().Forward));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 5,
                toughness: 5,
                type: t => t.Change(baseTypes: "forward", subTypes: "angel"),
                colors: L(CardColor.Light)),
              () => new AddSimpleAbility(Static.Flying),
              () => new AddSimpleAbility(Static.Brave));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}