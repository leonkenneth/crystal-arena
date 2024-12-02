using System;

namespace CrystalArena.Effects
{
  using Modifiers;

  public class ReplaceDamageToTargets : Effect
  {
    private Func<ITarget, ReplaceDamage> _damageReplacementFactory;
    private bool _untilEot;
    
    private ReplaceDamageToTargets() {}
    
    public ReplaceDamageToTargets(Func<ITarget, ReplaceDamage> damageReplacementFactory, bool untilEot)
    {
      _damageReplacementFactory = damageReplacementFactory;
      _untilEot = untilEot;
    }
    
    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
        };

      foreach (var target in ValidEffectTargets)
      {
        var modifier = new AddDamageRedirection((modifier) => _damageReplacementFactory(target)) {UntilEot = _untilEot};
        Game.AddModifier(modifier, mp);
      }
    }
  }
}