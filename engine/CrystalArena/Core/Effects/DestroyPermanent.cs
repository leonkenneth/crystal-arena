using System;
using System.Collections.Generic;
using System.Linq;

namespace CrystalArena.Effects
{
  using CrystalArena.AI;

  public class DestroyPermanent : Effect
  {
    private readonly DynParam<IEnumerable<Card>> _permanent;

    private DestroyPermanent() {}

    public DestroyPermanent(DynParam<IEnumerable<Card>> permanent)
    {
      _permanent = permanent;
      SetTags(EffectTag.Destroy);

      RegisterDynamicParameters(permanent);
    }
    
    public DestroyPermanent(DynParam<Card> permanent)
    {
      var newGetter = new Func<Effect, Game, IEnumerable<Card>>((effect, game) => new[] { permanent.Getter(effect, game) });
      _permanent = new DynParam<IEnumerable<Card>>(newGetter, permanent.EvaluateAt);
      SetTags(EffectTag.Destroy);

      RegisterDynamicParameters(permanent);
    }


    protected override void ResolveEffect()
    {
      _permanent.Value.ToList().ForEach(p => p.Destroy());
    }
  }
}