namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;

  public class Redeem : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Redeem")
        .ManaCost("{1}{W}")
        .Type("Summon")
        .Text("Prevent all damage that would be dealt this turn to up to two target forwards.")
        .FlavorText(
          "That they are saved from death is immaterial. What is important is that they know the source of their benefaction.")
        .Cast(p =>
          {
            p.Text = "Prevent all damage that would be dealt this turn to up to two target forwards.";            
            p.Effect = () => new PreventAllDamageToTargets();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Forward().On.Battlefield(),
              trg => {                
                trg.MinCount = 1;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectPreventNextDamageToTargets());
          });
    }
  }
}