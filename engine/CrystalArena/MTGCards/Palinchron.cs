namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Triggers;
  using ReturnToHand = Effects.ReturnToHand;

  public class Palinchron : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Palinchron")
        .ManaCost("{5}{U}{U}")
        .Type("Forward Illusion")
        .Text(
          "{Flying}{EOL}When Palinchron enters the battlefield, untap up to seven backups.{EOL}{2}{U}{U}: Return Palinchron to its owner's hand.")        
        .Power(4)
        .Toughness(5)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Palinchron enters the battlefield, untap up to seven backups.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 7,
              validator: c => c.Is().Backup,
              text: "Select backups to untap."
              );
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{U}{U}: Return Palinchron to its owner's hand.";
            p.Cost = new PayMana("{2}{U}{U}".Parse());
            p.Effect = () => new ReturnToHand(returnOwningCard: true);

            p.TimingRule(new WhenOwningCardWillBeDestroyed());
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}