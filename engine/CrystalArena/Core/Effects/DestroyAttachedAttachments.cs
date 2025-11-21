using System.Collections.Generic;

namespace CrystalArena.Effects
{
    using System.Linq;

    public class DestroyAttachedAttachments : Effect
    {
        private readonly DynParam<IEnumerable<Card>> _permanent;
        private readonly CardSelector _filter;

        private DestroyAttachedAttachments() { }

        public DestroyAttachedAttachments(
            DynParam<IEnumerable<Card>> permanent,
            CardSelector filter = null
        )
        {
            _permanent = permanent;
            _filter =
                filter
                ?? delegate
                {
                    return true;
                };
            RegisterDynamicParameters(permanent);
        }

        protected override void ResolveEffect()
        {
            foreach (var permanent in _permanent.Value)
            {
                var attachments = permanent.Attachments.Where(x => _filter(x, Ctx)).ToList();

                foreach (var attachment in attachments)
                {
                    attachment.Destroy();
                }
            }
        }
    }
}
