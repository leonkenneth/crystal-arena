namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class Vigor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vigor")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Forward - Elemental Incarnation")
        .Text(
          "{Trample}{EOL}If damage would be dealt to a forward you control other than Vigor, prevent that damage. Put a +1/+1 counter on that forward for each 1 damage prevented this way.{EOL}When Vigor is put into a breakZone from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Trample)
        .StaticAbility(p => p.Modifier(() => new AddDamagePrevention(
          modifier => new ReplaceDamageToPlayersForwardsWithCounters(
            player: modifier.SourceCard.Controller, 
            counter: () => new PowerToughness(1, 1),
            filter: card => card.Name != "Vigor"))))                
        .TriggeredAbility(p =>
          {
            p.Text = "When Vigor is put into a breakZone from anywhere, shuffle it into its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.BreakZone));
            p.Effect = () => new ShuffleOwningCardIntoMainDeck();
          });
    }
  }
}