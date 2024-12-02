namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class VeilOfBirds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veil of Birds")
        .ManaCost("{U}")
        .Type("Monster")
        .Text(
          "When an opponent casts a spell, if Veil of Birds is an monster, Veil of Birds becomes a 1/1 Bird forward with flying.")
        .FlavorText("When wind marries sky, even the bride's veil sings her praises.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veil of Birds is an monster, Veil of Birds becomes a 1/1 Bird forward with flying.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Monster));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 1,
                toughness: 1,
                type: t => t.Change(baseTypes: "forward", subTypes: "bird"),
                colors: L(CardColor.Water)),
              () => new AddSimpleAbility(Static.Flying));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}