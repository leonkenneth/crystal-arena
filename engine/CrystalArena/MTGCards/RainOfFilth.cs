namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class RainOfFilth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rain of Filth")
        .ManaCost("{B}")
        .Type("Summon")
        .Text("Until end of turn, backups you control gain 'Sacrifice this backup: Add {B} to your mana pool.'")
        .FlavorText("When I say it rained, it was not small drops, but a thick, greasy drool pouring from the heavens.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Backup && ctx.You == c.Controller,              
              modifier: () =>
                {
                  var mp = new ActivatedAbilityParameters
                    {
                      Cost = new Sacrifice(),
                      Text = "Sacrifice this backup: Add {B} to your mana pool.",
                      Effect = () => new AddManaToPool("{B}".Parse()),
                      UsesStack = false,
                    };

                  mp.TimingRule(new WhenYouNeedAdditionalMana());
                  
                  return new AddActivatedAbility(new ActivatedAbility(mp));
                });

            p.TimingRule(new OnYourTurn(Step.Upkeep));
          });
    }
  }
}