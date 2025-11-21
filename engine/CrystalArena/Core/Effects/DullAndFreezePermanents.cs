using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using CrystalArena.Modifiers;

namespace CrystalArena.Effects
{
    using System.Linq;

    public class DullAndFreezePermanents : Effect
    {
        private Func<Context, IEnumerable<Card>> _filter;

        private DullAndFreezePermanents() { }

        public DullAndFreezePermanents(Func<Context, IEnumerable<Card>> filter)
        {
            _filter = filter;
        }

        protected override void ResolveEffect()
        {
            var p = new ModifierParameters
            {
                SourceEffect = this,
                SourceCard = Source.OwningCard,
                X = X,
            };

            var context = new Context(this, Game);

            foreach (var card in _filter(context))
            {
                card.Tap();
                card.AddModifier(new Freeze(), p);
            }
        }
    }
}
