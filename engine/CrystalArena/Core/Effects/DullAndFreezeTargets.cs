using CrystalArena.Modifiers;

namespace CrystalArena.Effects
{
  public class DullAndFreezeTargets : Effect
  {
    protected override void ResolveEffect()
    {
      var p = new ModifierParameters()
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };
      foreach (var target in ValidEffectTargets)
      {
        target.Card().Tap();
        target.Card().AddModifier(new Freeze(), p);
      }
    }
  }
}