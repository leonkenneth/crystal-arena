using CrystalArena.AI;

namespace CrystalArena.Modifiers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class AddProtectionFromOpponentRemoveFromGameEffects : Modifier, ICardModifier
  {
    private readonly Func<Modifier, IEnumerable<EffectTag>> _effectTags;
    private Protections _protections;
    private AddToList<EffectTag> _modifier;

    public AddProtectionFromOpponentRemoveFromGameEffects()
    {
      _effectTags = (m) => new List<EffectTag>() { EffectTag.RemoveFromPlay };
    }

    public override void Apply(Protections protections)
    {
      _protections = protections;

      var effectTags = _effectTags(this).ToList();

      _modifier = new AddToList<EffectTag>(effectTags);
      _modifier.Initialize(ChangeTracker);
      protections.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _protections.RemoveModfier(_modifier);
    }
  }
}