namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class HarmonicConvergence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Harmonic Convergence")
        .ManaCost("{2}{G}")
        .Type("Summon")
        .Text("Put all monsters on top of their owners' libraries.")
        .FlavorText("When the eternal stars align, can mere mortals resist?")
        .Cast(p =>
          {
            p.Effect = () => new PutAllPermanentsOnTopOfMainDeck(c => c.Is().Monster);
            
            p.TimingRule(new Any(
              new OnOpponentsTurn(Step.DeclareAttackers), 
              new OnYourTurn(Step.DeclareBlocker),
              new OnEndOfOpponentsTurn()));
          });
    }
  }
}