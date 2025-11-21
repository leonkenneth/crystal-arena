namespace CrystalArena.Effects
{
    using System;

    public class DealDamageToForwardsAndPlayers : Effect
    {
        private readonly Func<Effect, Card, int> _amountForward;
        private readonly Func<Effect, Player, int> _amountPlayer;
        private readonly Func<Effect, Card, bool> _filterForward;
        private readonly Func<Effect, Player, bool> _filterPlayer;

        private DealDamageToForwardsAndPlayers() { }

        public DealDamageToForwardsAndPlayers(
            int amountForward = 0,
            int amountPlayer = 0,
            Func<Effect, Card, bool> filterForward = null,
            Func<Effect, Player, bool> filterPlayer = null
        )
            : this(
                delegate
                {
                    return amountForward;
                },
                delegate
                {
                    return amountPlayer;
                },
                filterForward,
                filterPlayer
            ) { }

        public DealDamageToForwardsAndPlayers(
            Func<Effect, Card, int> amountForward = null,
            Func<Effect, Player, int> amountPlayer = null,
            Func<Effect, Card, bool> filterForward = null,
            Func<Effect, Player, bool> filterPlayer = null
        )
        {
            _amountForward =
                amountForward
                ?? delegate
                {
                    return 0;
                };
            _amountPlayer =
                amountPlayer
                ?? delegate
                {
                    return 0;
                };
            _filterForward =
                filterForward
                ?? delegate
                {
                    return true;
                };
            _filterPlayer =
                filterPlayer
                ?? delegate
                {
                    return true;
                };
        }

        private bool ShouldDealToForward(Card forward)
        {
            return _amountForward(this, forward) > 0 && _filterForward(this, forward);
        }

        private bool ShouldDealToPlayer(Player player)
        {
            return _amountPlayer(this, player) > 0 && _filterPlayer(this, player);
        }

        public override int CalculatePlayerDamage(Player player)
        {
            return ShouldDealToPlayer(player) ? _amountPlayer(this, player) : 0;
        }

        public override int CalculateForwardDamage(Card forward)
        {
            return ShouldDealToForward(forward) ? _amountForward(this, forward) : 0;
        }

        protected override void ResolveEffect()
        {
            foreach (var player in Players)
            {
                if (ShouldDealToPlayer(player))
                {
                    var amount = _amountPlayer(this, player);
                    Source.OwningCard.DealDamageTo(amount, player, isCombat: false);
                }
            }

            foreach (var player in Players)
            {
                foreach (var forward in player.Battlefield.Forwards)
                {
                    if (ShouldDealToForward(forward))
                    {
                        var amount = _amountForward(this, forward);
                        Source.OwningCard.DealDamageTo(amount, forward, isCombat: false);
                    }
                }
            }
        }
    }
}
