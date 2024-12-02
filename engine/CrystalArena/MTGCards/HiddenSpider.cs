namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class HiddenSpider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Spider")
        .ManaCost("{G}")
        .Type("Monster")
        .Text(
          "When an opponent casts a forward spell with flying, if Hidden Spider is an monster, Hidden Spider becomes a 3/5 Spider forward with reach.")
        .FlavorText("It wants only to dress you in silk.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a forward spell with flying, if Hidden Spider is an monster, Hidden Spider becomes a 3/5 Spider forward with reach.";

            p.Trigger(new OnCastedSpell((c, ctx) =>
                ctx.Opponent == c.Controller && ctx.OwningCard.Is().Monster && c.Is().Forward && c.Has().Flying));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToForward(
              power: 3,
              toughness: 5,
              type: t => t.Change(baseTypes: "forward", subTypes: "spider"),
              colors: L(CardColor.Wind)),
              () => new AddSimpleAbility(Static.Reach));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}