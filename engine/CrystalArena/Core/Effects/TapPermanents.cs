using System;
using System.Collections.Generic;

namespace CrystalArena.Effects
{
  using System.Linq;

  public class TapPermanents : Effect
  {
    private Func<Context, IEnumerable<Card>> _filter;
    private TapPermanents()
    {
    }
    
    public TapPermanents(Func<Context, IEnumerable<Card>> filter)
    {
      _filter = filter;
    }

    protected override void ResolveEffect()
    {
      var context = new Context(this, Game);

      foreach (var card in _filter(context))
      {
        card.Tap();
      }
    }
  }
}